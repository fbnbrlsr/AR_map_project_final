using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class DatabaseManager
{   

    /*
    *   This class manages the paths and stops data.
    *   Initially, the data is read from the source file and parsed into the correct format.
    *   If the data filters have been changed, the data is adjusted accordingly and a 
    *   new list is returned.
    */

    // Data source
    private static readonly string PathsDataSource = Zurich_RawData.paths_final;
    private static readonly string StopsDataSource = Zurich_RawData.all_stops_final;
    public static DatabaseManager DatabaseManagerInstance;
    private static DataPathVisualizationManager dataPathVisualizationManager;

    // Paths data
    DataTable pathsQueryResult;
    List<DatabaseLegData> initialLegsList;
    List<DatabaseLegData> filteredLegsList;

    // Location stops data
    DataTable locationStopsQueryResult;
    List<DatabaseStopData> initialLocationStopsList;


    public static DatabaseManager GetInstance()
    {   
        if(DatabaseManagerInstance == null)
        {
            DatabaseManagerInstance = new DatabaseManager();
        }
        return DatabaseManagerInstance;
    }

    public static void SetDataPathVisualizationManager(DataPathVisualizationManager dpvm)
    {
        dataPathVisualizationManager = dpvm;
    }

    public void InitializePaths()
    {   
        ReadFromPathsString();
        ApplyPathsFilter(
            new HashSet<TravelMode>(), 
            new HashSet<CustomInterval>(), 
            new HashSet<CustomInterval>(),
            new HashSet<CustomInterval>()
        );
    }

    public void InitializeLocationStops()
    {   
        ReadFromLocationStopsString();
    }

    private void ReadFromPathsString()
    {   
        pathsQueryResult = ConvertPathsStringToDataTable();
        GenerateLegsList();
    }

    private void ReadFromLocationStopsString()
    {
        locationStopsQueryResult = ConvertLocationStopsStringToDataTable();
        GenerateLocationStopsList();
    }

    private void GenerateLegsList()
    {   
        initialLegsList = new List<DatabaseLegData>();

        for(int rowIndex = 0; rowIndex < pathsQueryResult.Rows.Count; rowIndex++)
        {   
            int person_id = pathsQueryResult.Rows[rowIndex]["person_id"]!=DBNull.Value ? (int) pathsQueryResult.Rows[rowIndex]["person_id"] : int.MinValue;
            if( person_id < DatabaseLegData.personIDLowerBound || person_id > DatabaseLegData.personIDUpperBound) continue;
            int trip_id = pathsQueryResult.Rows[rowIndex]["trip_id"]!=DBNull.Value ? (int) pathsQueryResult.Rows[rowIndex]["trip_id"] : int.MinValue;
            int leg_index = pathsQueryResult.Rows[rowIndex]["leg_index"]!=DBNull.Value ? (int) pathsQueryResult.Rows[rowIndex]["leg_index"] : int.MinValue;
            float origin_lon = pathsQueryResult.Rows[rowIndex]["origin_lon"]!=DBNull.Value ? (float) (double) pathsQueryResult.Rows[rowIndex]["origin_lon"] : float.MinValue;
            float origin_lat = pathsQueryResult.Rows[rowIndex]["origin_lat"]!=DBNull.Value ? (float) (double) pathsQueryResult.Rows[rowIndex]["origin_lat"] : float.MinValue;
            float dest_lon = pathsQueryResult.Rows[rowIndex]["destination_lon"]!=DBNull.Value ? (float) (double) pathsQueryResult.Rows[rowIndex]["destination_lon"] : float.MinValue;
            float dest_lat = pathsQueryResult.Rows[rowIndex]["destination_lat"]!=DBNull.Value ? (float) (double) pathsQueryResult.Rows[rowIndex]["destination_lat"] : float.MinValue;
            float departure_time = pathsQueryResult.Rows[rowIndex]["departure_time"]!=DBNull.Value ? (float) (double) pathsQueryResult.Rows[rowIndex]["departure_time"] : float.MinValue;
            float travel_time = pathsQueryResult.Rows[rowIndex]["travel_time"]!=DBNull.Value ? (float) (double) pathsQueryResult.Rows[rowIndex]["travel_time"] : float.MinValue;
            string travel_mode = pathsQueryResult.Rows[rowIndex]["travel_mode"]!=DBNull.Value ? (string) pathsQueryResult.Rows[rowIndex]["travel_mode"] : "";
            DatabaseLegData leg = new DatabaseLegData(
                person_id, trip_id, leg_index, origin_lon, 
                origin_lat, dest_lon, dest_lat, departure_time, travel_time, travel_mode
            );
            initialLegsList.Add(leg);
        }
        Debug.Log("[DatabaseManager] Loaded " + initialLegsList.Count + " paths from database");
        Debug.Log("Leg data: minID=" + DatabaseLegData.minPersonID + ", maxID=" + DatabaseLegData.maxPersonID + ", earliestTime=" + DatabaseLegData.earliestTime + ", latesTime=" + DatabaseLegData.latestTime 
              + ", minDur=" + DatabaseLegData.minDuration + ", maxDur=" + DatabaseLegData.maxDuration);
    }

    private void GenerateLocationStopsList()
    {
        initialLocationStopsList = new List<DatabaseStopData>();

        for(int rowIndex = 0; rowIndex < locationStopsQueryResult.Rows.Count; rowIndex++)
        {
            int person_id = locationStopsQueryResult.Rows[rowIndex]["person_id"]!=DBNull.Value ? (int) locationStopsQueryResult.Rows[rowIndex]["person_id"] : int.MinValue;
            if( person_id < DatabaseLegData.personIDLowerBound || person_id > DatabaseLegData.personIDUpperBound) continue;
            int trip_id = locationStopsQueryResult.Rows[rowIndex]["trip_id"]!=DBNull.Value ? (int) locationStopsQueryResult.Rows[rowIndex]["trip_id"] : int.MinValue;;
            int leg_index = locationStopsQueryResult.Rows[rowIndex]["leg_index"]!=DBNull.Value ? (int) locationStopsQueryResult.Rows[rowIndex]["leg_index"] : int.MinValue;;
            float dest_lon = locationStopsQueryResult.Rows[rowIndex]["destination_lon"]!=DBNull.Value ? (float) (double) locationStopsQueryResult.Rows[rowIndex]["destination_lon"] : float.MinValue;
            float dest_lat = locationStopsQueryResult.Rows[rowIndex]["destination_lat"]!=DBNull.Value ? (float)(double) locationStopsQueryResult.Rows[rowIndex]["destination_lat"] : float.MinValue;
            float stopTime = locationStopsQueryResult.Rows[rowIndex]["stop_time"]!=DBNull.Value ? (float)(double) locationStopsQueryResult.Rows[rowIndex]["stop_time"] : float.MinValue;
            string stopType = locationStopsQueryResult.Rows[rowIndex]["stop_type"]!=DBNull.Value ? (string) locationStopsQueryResult.Rows[rowIndex]["stop_type"] : "";
            DatabaseStopData stop = new DatabaseStopData(dest_lon, dest_lat, stopTime, stopType);
            initialLocationStopsList.Add(stop);
        }
        Debug.Log("[DataBaseEvent] Loaded " + initialLocationStopsList.Count + " stops from database");
    }

    public List<DatabaseLegData> GetFilteredLegsList()
    {
        return filteredLegsList;
    }

    private DataTable ConvertPathsStringToDataTable()
    {
        DataTable dt = new DataTable();
        string[] lines = PathsDataSource.Split('\n');
        string[] headers = lines[0].Split(',');

        foreach (string header in headers)
        {   
            dt.Columns.Add(header);
        }
        dt.Columns[0].DataType = typeof(int);
        dt.Columns[1].DataType = typeof(int);
        dt.Columns[2].DataType = typeof(int);
        dt.Columns[3].DataType = typeof(double);
        dt.Columns[4].DataType = typeof(double);
        dt.Columns[5].DataType = typeof(double);
        dt.Columns[6].DataType = typeof(double);
        dt.Columns[7].DataType = typeof(double);
        dt.Columns[8].DataType = typeof(double);
        dt.Columns[9].DataType = typeof(string);

        for(int k = 1; k < lines.Length; k++)
        {   
            string[] row = lines[k].Split(',');
            DataRow dr = dt.NewRow();
            for (int i = 0; i < headers.Length; i++)
            {
                try{
                    if(row[i].Equals("NULL"))
                    {
                        dr[i] = DBNull.Value;
                    }
                    else if(i == 9)
                    {
                        dr[i] = row[i];
                    }
                    else if(i >= 3)
                    {
                        dr[i] = double.Parse(row[i].Replace("\"", ""));
                    }
                    DataColumn column = dt.Columns[i];
                    object parsedvalue = Convert.ChangeType(row[i], column.DataType);
                    dr[i] = parsedvalue;
                }
                catch(Exception e)
                {
                    Debug.LogError("ERROR in column " + dt.Columns[i] + ", value: " + row[i] + ", error: " + e.Message);
                }
            }
            
            dt.Rows.Add(dr);
        }
        return dt;
    }

    private DataTable ConvertLocationStopsStringToDataTable()
    {
        DataTable dt = new DataTable();
        string[] lines = StopsDataSource.Split('\n');
        string[] headers = lines[0].Split(',');

        foreach (string header in headers)
        {   
            dt.Columns.Add(header);
        }
        dt.Columns[0].DataType = typeof(int);
        dt.Columns[1].DataType = typeof(int);
        dt.Columns[2].DataType = typeof(int);
        dt.Columns[3].DataType = typeof(double);
        dt.Columns[4].DataType = typeof(double);
        dt.Columns[5].DataType = typeof(double);
        dt.Columns[6].DataType = typeof(string);

        for(int k = 1; k < lines.Length; k++)
        {   
            string[] row = lines[k].Split(',');
            DataRow dr = dt.NewRow();
            for (int i = 0; i < headers.Length; i++)
            {
                try{
                    if(row[i].Equals("NULL"))
                    {
                        dr[i] = DBNull.Value;
                    }
                    else if(i <= 2)
                    {
                        dr[i] = int.Parse(row[i].Replace("\"", ""));
                    }
                    else if(i >= 3 && i <= 5)
                    {
                        dr[i] = double.Parse(row[i].Replace("\"", ""));
                    }
                    else if(i == 6)
                    {
                        dr[i] = row[i];
                    }
                    
                    DataColumn column = dt.Columns[i];
                    object parsedvalue = Convert.ChangeType(row[i], column.DataType);
                    dr[i] = parsedvalue;
                }
                catch(Exception e)
                {
                    Debug.LogError("ERROR in column " + dt.Columns[i] + ", value: " + row[i] + ", error: " + e.Message);
                }
                
            }
            dt.Rows.Add(dr);
        }
        return dt;
    }

    public void ApplyPathsFilter(HashSet<TravelMode> travelModes, HashSet<CustomInterval> departureTimes, 
        HashSet<CustomInterval> arrivalTimes, HashSet<CustomInterval> travelDurations)
    {   
        filteredLegsList = new List<DatabaseLegData>();
        List<DatabaseLegData> travelDurationsList = new List<DatabaseLegData>();

        Debug.Log("Applying filter with: #modes=" + travelModes.Count + ", #departureTimes=" + departureTimes.Count + ", #arrivalTimes=" + arrivalTimes.Count
             + ", #travelDuration=" + travelDurations.Count + ")");

        int nofTrips = -1;
        int nofLegs = 0;
        HashSet<int> agentsSet = new HashSet<int>();
        float avgTravelDuration = 0f;
        foreach(DatabaseLegData leg in initialLegsList)
        {   
            if(!travelModes.Contains(leg.travel_mode)) continue;
            if(!DepartureTimeIntervalsContainValue(departureTimes, leg.departure_time)) continue;
            if(!ArrivalTimeIntervalsContainValue(arrivalTimes, leg.arrival_time)) continue;

            travelDurationsList.Add(leg);

            if(!TravelDurationIntervalsContainValue(travelDurations, leg.travel_time)) continue;

            agentsSet.Add(leg.person_id);
            nofLegs += 1;
            avgTravelDuration += leg.travel_time;

            filteredLegsList.Add(leg);
        }

        dataPathVisualizationManager.DestroyAllPaths();
        InformationPanel infoPanel = GameObject.Find("StatisticsPanel").GetComponent<InformationPanel>();
        infoPanel?.SetAllInformation(nofTrips, nofLegs, agentsSet.Count, avgTravelDuration / nofLegs, 0);
        DashboardManager dashboard = GameObject.Find("StatisticsPanel").GetComponent<DashboardManager>();
        dashboard?.UpdateDatasetChart(filteredLegsList);
    }

    public List<DatabaseStopData> GetLocationsStopsList()
    {
        return initialLocationStopsList;
    }

    private bool DepartureTimeIntervalsContainValue(HashSet<CustomInterval> h, float val)
    {
        foreach(CustomInterval i in h)
        {   
            if(i.start*3600 <= val && val < i.end*3600){
                return true;
            }
        }
        return false;
    }

    private bool ArrivalTimeIntervalsContainValue(HashSet<CustomInterval> h, float val)
    {
        foreach(CustomInterval i in h)
        {   
            if(i.start*3600 <= val && val < i.end*3600){
                return true;
            }
        }
        return false;
    }

    private bool TravelDurationIntervalsContainValue(HashSet<CustomInterval> h, float val)
    {
        foreach(CustomInterval i in h)
        {   
            if(i.start*60 <= val && val < i.end*60){
                return true;
            }
        }
        return false;
    }

}

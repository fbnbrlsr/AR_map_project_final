using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering;

public class K_DatabaseManager
{   
    private static readonly string DataSource = K_RawData.all_legs_data;
    public static K_DatabaseManager DatabaseManagerInstance;
    private static K_DataPathVisualizationManager dataPathVisualizationManager;
    private static K_LocationStopVisualizationManager locationStopVisualizationManager;

    // Paths
    DataTable pathsQueryResult;
    List<K_DatabaseLegData> initialLegsList;
    List<K_DatabaseLegData> filteredLegsList;
    private int nextLegIndex;

    // Location stops
    DataTable locationStopsQueryResult;
    List<K_DatabaseStopData> initialLocationStopsList;

    public static K_DatabaseManager GetInstance()
    {   
        if(DatabaseManagerInstance == null)
        {
            DatabaseManagerInstance = new K_DatabaseManager();
        }
        return DatabaseManagerInstance;
    }

    public static void SetDataPathVisualizationManager(K_DataPathVisualizationManager dpvm)
    {
        dataPathVisualizationManager = dpvm;
    }

    public static void SetLocationStopVisualizationManager(K_LocationStopVisualizationManager lsvm)
    {
        locationStopVisualizationManager = lsvm;
    }

    public void InitializePaths(int nof_desired_rows)
    {   
        Debug.Log("[DatabaseManager] Initializing paths...");
        ReadFromPathsString();
        ApplyPathsFilter(new List<TravelMode>(), 0f, 24f, 0f, 1400f, 0f, 172f, 435f, 319827f);
    }

    public void InitializeLocationStops()
    {   
        Debug.Log("[DatabaseManager] Initializing location stops...");
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
        nextLegIndex = 0;
        initialLegsList = new List<K_DatabaseLegData>();

        for(int rowIndex = 0; rowIndex < pathsQueryResult.Rows.Count; rowIndex++)
        {   
            int person_id = pathsQueryResult.Rows[rowIndex]["person_id"]!=DBNull.Value ? (int) pathsQueryResult.Rows[rowIndex]["person_id"] : int.MinValue;
            int trip_id = pathsQueryResult.Rows[rowIndex]["trip_id"]!=DBNull.Value ? (int) pathsQueryResult.Rows[rowIndex]["trip_id"] : int.MinValue;
            int leg_index = pathsQueryResult.Rows[rowIndex]["leg_index"]!=DBNull.Value ? (int) pathsQueryResult.Rows[rowIndex]["leg_index"] : int.MinValue;
            float origin_lon = pathsQueryResult.Rows[rowIndex]["origin_lon"]!=DBNull.Value ? (float) (double) pathsQueryResult.Rows[rowIndex]["origin_lon"] : float.MinValue;
            float origin_lat = pathsQueryResult.Rows[rowIndex]["origin_lat"]!=DBNull.Value ? (float) (double) pathsQueryResult.Rows[rowIndex]["origin_lat"] : float.MinValue;
            float dest_lon = pathsQueryResult.Rows[rowIndex]["destination_lon"]!=DBNull.Value ? (float) (double) pathsQueryResult.Rows[rowIndex]["destination_lon"] : float.MinValue;
            float dest_lat = pathsQueryResult.Rows[rowIndex]["destination_lat"]!=DBNull.Value ? (float) (double) pathsQueryResult.Rows[rowIndex]["destination_lat"] : float.MinValue;
            float departure_time = pathsQueryResult.Rows[rowIndex]["departure_time"]!=DBNull.Value ? (float) (double) pathsQueryResult.Rows[rowIndex]["departure_time"] : float.MinValue;
            float travel_time = pathsQueryResult.Rows[rowIndex]["travel_time"]!=DBNull.Value ? (float) (double) pathsQueryResult.Rows[rowIndex]["travel_time"] : float.MinValue;
            string travel_mode = pathsQueryResult.Rows[rowIndex]["travel_mode"]!=DBNull.Value ? (string) pathsQueryResult.Rows[rowIndex]["travel_mode"] : "";
            K_DatabaseLegData leg = new K_DatabaseLegData(
                person_id, trip_id, leg_index, origin_lon, 
                origin_lat, dest_lon, dest_lat, departure_time, travel_time, travel_mode
            );
            initialLegsList.Add(leg);
        }
        DebugPanel.Log("[DatabaseManager] Loaded " + initialLegsList.Count + " paths from database");
        Debug.Log("Leg data: earliestTime=" + K_DatabaseLegData.earliestTime + ", latesTime=" + K_DatabaseLegData.latestTime 
            + ", maxDur=" + K_DatabaseLegData.maxDuration + ", minDur=" + K_DatabaseLegData.minDuration);
    }

    private void GenerateLocationStopsList()
    {
        initialLocationStopsList = new List<K_DatabaseStopData>();

        for(int rowIndex = 0; rowIndex < locationStopsQueryResult.Rows.Count; rowIndex++)
        {
            int person_id = locationStopsQueryResult.Rows[rowIndex]["person_id"]!=DBNull.Value ? (int) locationStopsQueryResult.Rows[rowIndex]["person_id"] : int.MinValue;
            int trip_id = locationStopsQueryResult.Rows[rowIndex]["trip_id"]!=DBNull.Value ? (int) locationStopsQueryResult.Rows[rowIndex]["trip_id"] : int.MinValue;;
            int leg_index = locationStopsQueryResult.Rows[rowIndex]["leg_index"]!=DBNull.Value ? (int) locationStopsQueryResult.Rows[rowIndex]["leg_index"] : int.MinValue;;
            float dest_lon = locationStopsQueryResult.Rows[rowIndex]["destination_lon"]!=DBNull.Value ? (float) (double) locationStopsQueryResult.Rows[rowIndex]["destination_lon"] : float.MinValue;
            float dest_lat = locationStopsQueryResult.Rows[rowIndex]["destination_lat"]!=DBNull.Value ? (float)(double) locationStopsQueryResult.Rows[rowIndex]["destination_lat"] : float.MinValue;
            float stopTime = locationStopsQueryResult.Rows[rowIndex]["stop_time"]!=DBNull.Value ? (float)(double) locationStopsQueryResult.Rows[rowIndex]["stop_time"] : float.MinValue;
            string stopType = locationStopsQueryResult.Rows[rowIndex]["stop_type"]!=DBNull.Value ? (string) locationStopsQueryResult.Rows[rowIndex]["stop_type"] : "";
            K_DatabaseStopData stop = new K_DatabaseStopData(person_id, trip_id, leg_index, dest_lon, dest_lat, stopTime, stopType);
            initialLocationStopsList.Add(stop);
        }
        Debug.Log("[DataBaseEvent] Loaded " + initialLocationStopsList.Count + " stops from database");
    }

    public K_DatabaseLegData GetNextLeg()
    {   
        K_DatabaseLegData res = filteredLegsList[nextLegIndex];
        nextLegIndex += 1;
        return res;
    }

    public bool HasMorePaths()
    {
        return nextLegIndex < filteredLegsList.Count;
    }

    private DataTable ConvertPathsStringToDataTable()
    {
        DataTable dt = new DataTable();
        string[] lines = DataSource.Split('\n');
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
                    Debug.Log("CAUGHT ERROR in column " + dt.Columns[i] + ", value: " + row[i] + ", error: " + e.Message);
                    DebugPanel.Log("CAUGHT ERROR in column " + dt.Columns[i] + ", value: " + row[i] + ", error: " + e.Message);
                }
                
            }
            dt.Rows.Add(dr);
        }
        
        DebugPanel.Log(" > CSV file reading successful");

        return dt;
    }

    private DataTable ConvertLocationStopsStringToDataTable()
    {
        DataTable dt = new DataTable();
        string[] lines = K_RawData.all_stops_data.Split('\n');
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
                    Debug.Log("CAUGHT ERROR in column " + dt.Columns[i] + ", value: " + row[i] + ", error: " + e.Message);
                    DebugPanel.Log("CAUGHT ERROR in column " + dt.Columns[i] + ", value: " + row[i] + ", error: " + e.Message);
                }
                
            }
            dt.Rows.Add(dr);
        }
        
        DebugPanel.Log(" > CSV file reading successful");

        return dt;
    }
    
    public int GetNofPaths()
    {
        return filteredLegsList.Count;
    }

    public void ApplyPathsFilter(List<TravelMode> modes, float minDepTime, float maxDepTime, float minTrDur, float maxTrDur, float minTrDist, float maxTrDist, float minAgentID, float maxAgentID)
    {   
        filteredLegsList = new List<K_DatabaseLegData>();
        minDepTime *= 3600f;
        maxDepTime *= 3600f;
        minTrDur *= 60f;
        maxTrDur *= 60f;

        string modeString = "";
        foreach(TravelMode m in modes)
        {
            modeString += ", " + TravelModeMap.TravelModeToString(m);
        }

        Debug.Log("Applying filter with: mode=" + modeString + ", depTimes=(" + minDepTime + ", " + maxDepTime + "), travelDurs=("
             + minTrDur + ", " + maxTrDur + "), travelDists=(" + minTrDist + ", " + maxTrDist  + ")");

        int nofTrips = -1;
        int nofLegs = 0;
        HashSet<int> agentsSet = new HashSet<int>();
        float avgTravelTime = 0f;
        foreach(K_DatabaseLegData leg in initialLegsList)
        {
            if(!modes.Contains(leg.travel_mode)) continue;
            if(leg.departure_time < minDepTime || leg.departure_time > maxDepTime) continue;
            if(leg.travel_time < minTrDur || leg.travel_time > maxTrDur) continue;
            if(leg.person_id < minAgentID || leg.person_id > maxAgentID) continue;

            // TODO: filter by travel distance
            agentsSet.Add(leg.person_id);
            nofLegs += 1;
            avgTravelTime += leg.travel_time;

            filteredLegsList.Add(leg);
        }

        K_InformationPanel infoPanel = GameObject.Find("InformationPanel").GetComponent<K_InformationPanel>();
        infoPanel?.SetAllInformation(nofTrips, nofLegs, agentsSet.Count, avgTravelTime / nofLegs, 0);
        dataPathVisualizationManager.DestroyAllPaths();

        DashboardManager dashboard = GameObject.Find("MapDashboardPanel").GetComponent<DashboardManager>();
        dashboard?.UpdateDatasetChart(filteredLegsList);

        Debug.Log("Applied filter -> total paths: " + initialLegsList.Count + ", filtered paths: " + filteredLegsList.Count);
    }

    public void ResetLegIndex()
    {
        nextLegIndex = 0;
    }

    public List<K_DatabaseStopData> GetLocationsStopsList()
    {
        return initialLocationStopsList;
    }

}

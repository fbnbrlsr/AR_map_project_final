
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using Debug = UnityEngine.Debug;

public delegate void DataBaseEvent();

public class DatabaseManager
{
     
    int rowIndex;
    int nof_desired_rows;
    int nof_returned_rows;
    // string connectionString = "Host=localhost;Username=postgres;Password=DB_Password;Database=TestMobisData";
    string connectionString;
    string queryString;
    DataTable queryResult;
    List<DatabaseLegData> legsList;

    private int nextLegIndex;
    public static DataBaseEvent DataBaseInitialized;

    public void Initialize(int nof_desired_rows)
    {
#if UNITY_EDITOR
        //ReadFromDatabase(nof_desired_rows);
        ReadFromString();
#else
        ReadFromString();
#endif

        SetTimeParameters();
        SetDurationParameters();

        Debug.Log("Database: invoking event");
        DataBaseInitialized?.Invoke();
    }

    public void ReadFromDatabase(int nof_desired_rows)
    {   
        connectionString = "Host=localhost:5432;Username=postgres;Password=DB_Password;Database=TestMobisData";
        this.nof_desired_rows = nof_desired_rows;
        queryString = @"SELECT 
                            *
                        FROM 
                            (select 
                                participant_id,
                                leg_id,
                                trip_id,
                                treatment,
                                phase,
                                started_at,
                                finished_at,
                                length,
                                duration,
                                type,
                                mode,
                                in_switzerland,
                                ST_Y(ST_Transform(ST_SetSRID(ST_MakePoint(start_x, start_y), 2056), 4326)) AS start_longitude,
                                ST_X(ST_Transform(ST_SetSRID(ST_MakePoint(start_x, start_y), 2056), 4326)) AS start_latitude,
                                ST_Y(ST_Transform(ST_SetSRID(ST_MakePoint(mid_x, mid_y), 2056), 4326)) AS mid_longitude,
                                ST_X(ST_Transform(ST_SetSRID(ST_MakePoint(mid_x, mid_y), 2056), 4326)) AS mid_latitude,
                                ST_Y(ST_Transform(ST_SetSRID(ST_MakePoint(end_x, end_y), 2056), 4326)) AS end_longitude,
                                ST_X(ST_Transform(ST_SetSRID(ST_MakePoint(end_x, end_y), 2056), 4326)) AS end_latitude
                            from 
                                legs)
                        WHERE
                            sqrt(pow(start_longitude - end_longitude, 2) + pow(start_latitude - end_latitude, 2)) > 2.75 and in_switzerland=true
                        LIMIT
                            50;";

        queryResult = QueryDatabase(queryString);
        GenerateLegsList();
    }

    public void ReadFromCSV(int nof_desired_rows)
    {   
        DebugPanel.Log("Reading data from CSV...");
        // queryResult = ConvertCSVtoDataTable("Assets/MyScripts/DataManagement/example_data_small.csv");
        queryResult = ConvertCSVtoDataTable("Assets/MyScripts/DataManagement/example_data_large.csv");
        GenerateLegsList();
    }

    public void ReadFromString()
    {
        queryResult = ConvertStringToDataTable();
        GenerateLegsList();
    }

    private DataTable QueryDatabase(string queryString)
    {
        DataTable resultTable = new DataTable();
        try
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(queryString, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        resultTable.Load(reader);
                    }
                }
            }
            nof_returned_rows = resultTable.Rows.Count;
            DebugPanel.Log(" > Database connection successful!");
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred: {ex.Message}");
            DebugPanel.Log("Database connection failed:\n");
            DebugPanel.Log(ex.ToString());
        }
        
        return resultTable;
    }

    private void GenerateLegsList()
    {   
        nextLegIndex = 0;
        rowIndex = 0;
        legsList = new List<DatabaseLegData>();

        for(int i = 0; i < nof_returned_rows; i++)
        {   
            string participant_id = queryResult.Rows[rowIndex]["participant_id"]!=DBNull.Value ? (string) queryResult.Rows[rowIndex]["participant_id"] : "";
            int leg_id = queryResult.Rows[rowIndex]["leg_id"]!=DBNull.Value ? (int) queryResult.Rows[rowIndex]["leg_id"] : int.MinValue;
            int trip_id = queryResult.Rows[rowIndex]["trip_id"]!=DBNull.Value ? (int) queryResult.Rows[rowIndex]["trip_id"] : int.MinValue;
            string treatment = queryResult.Rows[rowIndex]["treatment"]!=DBNull.Value ? (string) queryResult.Rows[rowIndex]["treatment"] : "";
            int phase = queryResult.Rows[rowIndex]["treatment"]!=DBNull.Value ? (int) Convert.ToInt16(queryResult.Rows[rowIndex]["phase"]) : int.MinValue;
            DateTime started_at = queryResult.Rows[rowIndex]["started_at"]!=DBNull.Value ? (DateTime) queryResult.Rows[rowIndex]["started_at"] : DateTime.MinValue;
            DateTime finished_at = queryResult.Rows[rowIndex]["finished_at"]!=DBNull.Value ? (DateTime) queryResult.Rows[rowIndex]["finished_at"] : DateTime.MinValue;
            int length = queryResult.Rows[rowIndex]["length"]!=DBNull.Value ? (int) queryResult.Rows[rowIndex]["length"] : int.MinValue;
            float duration = queryResult.Rows[rowIndex]["duration"]!=DBNull.Value ? (float) (double) queryResult.Rows[rowIndex]["duration"] : int.MinValue;
            string mode = queryResult.Rows[rowIndex]["mode"]!=DBNull.Value ? (string) queryResult.Rows[rowIndex]["mode"] : "";
            float start_longitude = queryResult.Rows[rowIndex]["start_longitude"]!=DBNull.Value ? (float) (double) queryResult.Rows[rowIndex]["start_longitude"] : float.MinValue;
            float start_latitude = queryResult.Rows[rowIndex]["start_latitude"]!=DBNull.Value ? (float) (double) queryResult.Rows[rowIndex]["start_latitude"] : float.MinValue;
            float mid_longitude = queryResult.Rows[rowIndex]["mid_longitude"]!=DBNull.Value ? (float) (double) queryResult.Rows[rowIndex]["mid_longitude"] : float.MinValue;
            float mid_latitude = queryResult.Rows[rowIndex]["mid_latitude"]!=DBNull.Value ? (float) (double) queryResult.Rows[rowIndex]["mid_latitude"] : float.MinValue;
            float end_longitude = queryResult.Rows[rowIndex]["end_longitude"]!=DBNull.Value ? (float) (double) queryResult.Rows[rowIndex]["end_longitude"] : float.MinValue;
            float end_latitude = queryResult.Rows[rowIndex]["end_latitude"]!=DBNull.Value ? (float) (double) queryResult.Rows[rowIndex]["end_latitude"] : float.MinValue;
            DatabaseLegData leg = new DatabaseLegData(
                participant_id, leg_id, trip_id, treatment, phase, started_at, finished_at, length, 
                duration, mode, start_longitude, start_latitude, mid_longitude, mid_latitude, end_longitude, end_latitude
            );
            rowIndex += 1;
            legsList.Add(leg);
        }
        DebugPanel.Log("[DatabaseManager] Loaded " + legsList.Count + " paths from database");
    }

    public DatabaseLegData GetNextLeg()
    {   
        DatabaseLegData res = legsList[nextLegIndex];
        nextLegIndex += 1;
        return res;
    }

    public bool HasMorePaths()
    {
        return nextLegIndex < legsList.Count;
    }
    
    private DataTable ConvertCSVtoDataTable(string strFilePath)
    {   
        DebugPanel.Log("Converting data from CSV to DataTable... (NOT IMPLEMENTED)");
        return null;
    }

    private DataTable ConvertStringToDataTable()
    {
        DataTable dt = new DataTable();
        string[] lines = RawData.large_path_data.Split('\n');
        string[] headers = lines[0].Split(',');
        foreach (string header in headers)
        {   
            dt.Columns.Add(header);
        }
        dt.Columns[0].DataType = typeof(string);
        dt.Columns[1].DataType = typeof(int);
        dt.Columns[2].DataType = typeof(int);
        dt.Columns[3].DataType = typeof(string);
        dt.Columns[4].DataType = typeof(int);
        dt.Columns[5].DataType = typeof(DateTime);
        dt.Columns[6].DataType = typeof(DateTime);
        dt.Columns[7].DataType = typeof(int);
        dt.Columns[8].DataType = typeof(double);
        dt.Columns[9].DataType = typeof(string);
        dt.Columns[10].DataType = typeof(string);
        dt.Columns[11].DataType = typeof(bool);
        dt.Columns[12].DataType = typeof(double);
        dt.Columns[13].DataType = typeof(double);
        dt.Columns[14].DataType = typeof(double);
        dt.Columns[15].DataType = typeof(double);
        dt.Columns[16].DataType = typeof(double);
        dt.Columns[17].DataType = typeof(double);
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
                    else if(i == 8 || i >= 12)
                    {
                        dr[i] = double.Parse(row[i].Replace("\"", ""));
                    }
                    else if(i == 5 || i == 6)
                    {
                        dr[i] = DateTime.Parse(row[i].Replace("\"", ""));
                    }
                    if(true)
                    {
                        DataColumn column = dt.Columns[i];
                        object parsedvalue = Convert.ChangeType(row[i], column.DataType);
                        dr[i] = parsedvalue;
                    }
                }
                catch(Exception e)
                {
                    Debug.Log("CAUGHT ERROR in column " + dt.Columns[i] + ", value: " + row[i] + ", error: " + e.Message);
                    DebugPanel.Log("CAUGHT ERROR in column " + dt.Columns[i] + ", value: " + row[i] + ", error: " + e.Message);
                }
                
            }
            dt.Rows.Add(dr);
        }
        
        nof_returned_rows = dt.Rows.Count;
        DebugPanel.Log(" > CSV file reading successful");

        return dt;
    }

    private void SetTimeParameters()
    {   
        
        try{
            DateTime minDate = (DateTime) queryResult.Compute("min([started_at])", "");
            DateTime maxDate = (DateTime) queryResult.Compute("max(finished_at)", "");
            DatabaseLegData.SetTimeParameters(minDate, maxDate, 0f, 1f);
        }
        catch(Exception e)
        {
            DebugPanel.Log("Error while setting database time parameters:");
            DebugPanel.Log(e.Message);
            Debug.LogError("Error while setting database time parameters: " + e.Message);
        }
        
    }

    private void SetDurationParameters()
    {   
        
        try{
            //float minDuration = (float) queryResult.Compute("min([duration])", "");
            float minDuration = 0f;
            float maxDuration = (float) (double) queryResult.Compute("max(duration)", "");
            DatabaseLegData.SetDurationParameters(minDuration, maxDuration, .5f, 1.5f);
        }
        catch(Exception e)
        {
            DebugPanel.Log("Error while setting database duration parameters:");
            DebugPanel.Log(e.Message);
            Debug.LogError("Error while setting database duration parameters: " + e.Message);
        }
        
    }

    public int GetNofPaths()
    {
        return legsList.Count;
    }

}


using System;
using System.Data;
using System.Diagnostics;
using Npgsql;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class rm_CommentBoxCreatorDatabaseManager_old
{
     
    /*static int stringIndex;
    static int limit;
    //static string connectionString = "Host=localhost;Username=postgres;Password=DB_Password;Database=TestMobisData";
    static string connectionString = "Host=192.168.60.171;Username=postgres;Password=DB_Password;Database=TestMobisData";
    static string queryString;
    static DataTable queryResult;

    public static void Initialize(int l)
    {
        stringIndex = 0;
        limit = l;
        queryString = @"SELECT 
                            ST_X(ST_Transform(ST_SetSRID(ST_MakePoint(start_x, start_y), 2056), 4326)) AS start_longitude,
                            ST_Y(ST_Transform(ST_SetSRID(ST_MakePoint(start_x, start_y), 2056), 4326)) AS start_latitude,
                            ST_X(ST_Transform(ST_SetSRID(ST_MakePoint(mid_x, mid_y), 2056), 4326)) AS mid_longitude,
                            ST_Y(ST_Transform(ST_SetSRID(ST_MakePoint(mid_x, mid_y), 2056), 4326)) AS mid_latitude,
                            ST_X(ST_Transform(ST_SetSRID(ST_MakePoint(end_x, end_y), 2056), 4326)) AS end_longitude,
                            ST_Y(ST_Transform(ST_SetSRID(ST_MakePoint(end_x, end_y), 2056), 4326)) AS end_latitude
                        FROM 
                            legs
                        WHERE
                            length > 100000
                        LIMIT
                            " + limit + ";";

        queryResult = QueryDatabase(queryString);
    }

    public static string[] GetNextCoordinateStringArray()
    {
        
        string start_x = queryResult.Rows[stringIndex]["start_longitude"].ToString();
        string start_y = queryResult.Rows[stringIndex]["start_latitude"].ToString();
        string mid_x = queryResult.Rows[stringIndex]["mid_longitude"].ToString();
        string mid_y = queryResult.Rows[stringIndex]["mid_latitude"].ToString();
        string end_x = queryResult.Rows[stringIndex]["end_longitude"].ToString();
        string end_y = queryResult.Rows[stringIndex]["end_latitude"].ToString();
        
        string[] stringArr = new string[3]{
            start_y + ", " + start_x,
            mid_y + ", " + mid_x,
            end_y + ", " + end_x
        };

        Debug.Log("Path: " + stringArr[0] + " | " + stringArr[1] + " | " + stringArr[2]);
        
        stringIndex += 1;
        return stringArr;
    }

    public static DataTable QueryDatabase(string queryString)
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
            DebugPanel.Log("Database connection successful!");
        }
        catch (Exception ex)
        {
            Debug.LogError($"An error occurred: {ex.Message}");
            DebugPanel.Log("Database connection failed:\n");
            DebugPanel.Log(ex.ToString());
        }
        return resultTable;
    }*/
    
}

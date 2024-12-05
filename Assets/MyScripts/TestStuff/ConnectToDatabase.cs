using System;
using Npgsql;
using UnityEngine;

public class ConnectToDatabase : MonoBehaviour
{
    
    [SerializeField] GameObject connectButton;

    void Start()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;
    }

    void EstablishConnection()
    {   
        DebugPanel.Log("Establishing connection...");

        string connectionString = @"Host=192.168.60.252;
                                    Port=5432;
                                    Database=Korsika_test_data;
                                    User Id=postgres;
                                    Password=PW123;";

        try{
            using NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();

            using NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM korsika_legs LIMIT 10;", connection);

            using NpgsqlDataReader reader = cmd.ExecuteReader();

            int row = 1;
            while (reader.Read())
            {   
                string s = "Row" + row + ":";
                for(int i = 0; i < reader.FieldCount; i++)
                {
                    s += "\n\t> " + reader.GetName(i) + ": " + reader[i] + " (type=" + reader.GetFieldType(i) + ")";
                }
                DebugPanel.Log(s);
                row += 1;
            }
        }
        catch(Exception e)
        {
            DebugPanel.Log("ERROR: " + e.Message);
        }
        

        DebugPanel.Log("Database actions finished");

    }

    void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        if(targetObj.transform.IsChildOf(connectButton.transform))
        {
            EstablishConnection();
        }
    }


}

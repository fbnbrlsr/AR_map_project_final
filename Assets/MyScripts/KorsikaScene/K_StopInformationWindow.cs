using System;
using System.Linq;
using TMPro;
using UnityEngine;



public class K_StopInformationWindow
{
    
    private GameObject instance;
    public GameObject Instance => instance;
    public GameObject prefab;
    private Quaternion rotation;

    // information
    private float lat;
    private float lon;
    private int nof_stops;

    public K_StopInformationWindow(float lat, float lon, int nof_stops, GameObject prefab)
    {
        this.lat = lat;
        this.lon = lon;
        this.nof_stops = nof_stops;
        this.prefab = prefab;

        K_DataPathVisualizationManager.GlobalNewWindowSpawnedEvent += this.Hide;
    }

    public void Show(Vector3 worldPos, Vector3 headPos, int nof_stops)
    {   
        this.nof_stops = nof_stops;
        if(instance != null)
        {
            GameObject.Destroy(instance);
            return;
        }

        rotation = Quaternion.LookRotation(worldPos - headPos);

        instance = GameObject.Instantiate(prefab);

        TMP_Text textField = instance.GetComponentInChildren<TMP_Text>();
        if(textField != null) textField.text = this.GenerateText(textField.text);

        instance.transform.position = worldPos;
        instance.transform.rotation = rotation;
    }

    private string GenerateText(string t)
    {
        string s = t.Split("\n")[0];
        s += "\nLatitude: " + lat;
        s += "\nLongitude: " + lon;
        s += "\n#Stops: " + nof_stops;
        return s;
    }

    public void Hide()
    {   
        GameObject.Destroy(instance);
    }

    public static void HideAll()
    {
        K_DataPathVisualizationManager.InvokeGlobalNewWindowSpawnedEvent();
    }

}

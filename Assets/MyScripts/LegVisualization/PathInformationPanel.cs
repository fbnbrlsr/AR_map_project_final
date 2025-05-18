using TMPro;
using UnityEngine;

public class PathInformationPanel
{   

    /*
    *   This class displays information about an instantiated line, namely start and end time, 
    *   travel duration, and travel mode.
    *   The window is enabled by a direct pinch gesture on a line and is spawned at that point
    *   oriented towards the user's head. 
    *   Only one window is shown at a time. A window is disabled again if either another line 
    *   is selected or if the map is being moved.
    */

    private GameObject instance;
    public GameObject Instance => instance;
    public GameObject prefab;
    private Vector3 worldPosition;
    private Quaternion rotation;
    private DatabaseLegData leg;


    public PathInformationPanel(DatabaseLegData leg, GameObject prefab)
    {
        this.leg = leg;
        this.prefab = prefab;
    }
    
    public void Show(Vector3 worldPos, Vector3 headPos)
    {   
        if(instance != null) 
        {
            GameObject.Destroy(instance);
        }

        worldPosition = worldPos;
        rotation = Quaternion.LookRotation(worldPos - headPos);

        instance = GameObject.Instantiate(prefab);

        TMP_Text textField = instance.GetComponentInChildren<TMP_Text>();
        if(textField != null) textField.text = this.GenerateText();
        
        instance.transform.position = worldPos;
        instance.transform.rotation = rotation;
    }

    private string GenerateText()
    {   
        string t = "TRIP ID: " + leg.trip_id + " (leg " + leg.leg_index + ")";
        t += "\nStart time: " + FormatTimeFromSeconds(leg.departure_time);
        t += "\nEnd time: " + FormatTimeFromSeconds(leg.departure_time + leg.travel_time);
        t += "\nDuration: " + FormatTimeFromSeconds(leg.travel_time);
        t += "\nTravel mode: " + leg.travel_mode;
        return t;
    }

    public void Hide()
    {
        GameObject.Destroy(instance);
    }

    public string FormatTimeFromSeconds(float inputf)
    {   
        int input = (int) inputf;
        int seconds = input % 60;
        int minutes = (input - seconds)/60 % 60;
        int hours = (input - seconds - 60*minutes)/3600 % 60;

        if(hours == 0 && minutes == 0)
        {
            return seconds + "s";
        }
        else if(hours == 0)
        {
            return minutes + "m " + seconds + "s";
        }
        else
        {
            return hours + "h " + minutes + "m " + seconds + "s";
        }
    }
}

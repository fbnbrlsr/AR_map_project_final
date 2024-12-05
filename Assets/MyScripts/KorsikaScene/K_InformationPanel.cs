using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class K_InformationPanel : MonoBehaviour
{      
    [SerializeField] TMP_Text nofTrips;
    [SerializeField] TMP_Text nofLegs;
    [SerializeField] TMP_Text nofAgents;
    [SerializeField] TMP_Text avgTravelTime;
    [SerializeField] TMP_Text nofVisiblePaths;
    [SerializeField] TMP_Text nofTransitionalStops;
    [SerializeField] TMP_Text nofActivityStops;

    public void SetAllInformation(int nofTrips, int nofLegs, int nofAgents, float avgTravelTime, int nofVisiblePaths)
    {
        this.nofTrips.text = "#Trips\n" + nofTrips.ToString();
        this.nofLegs.text = "#Legs\n" + nofLegs.ToString();
        this.nofAgents.text = "#Agents\n" + nofAgents.ToString();
        this.avgTravelTime.text = "Average travel duration\n" + SecondsToFormatedTime((int)avgTravelTime);
        this.nofVisiblePaths.text = "Visible paths\n" + nofVisiblePaths; 
    }

    public void SetNofVisiblePaths(int nofVisiblePaths)
    {
        this.nofVisiblePaths.text = "Visible paths\n" + nofVisiblePaths; 
    }

    public void SetNofTransitionalStops(int transitionalStops)
    {
        this.nofTransitionalStops.text = "#Transitional Stops\n" + transitionalStops;
    }

    public void SetNofActivityStops(int activityStops)
    {
        this.nofActivityStops.text = "#Activity Stops\n" + activityStops;

    }

    private string SecondsToFormatedTime(int s)
    {
        int minutes = s/60;
        int seconds = s - (minutes*60);
        return minutes + "min " + seconds + "s";
    }

}

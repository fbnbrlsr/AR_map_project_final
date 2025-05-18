using TMPro;
using UnityEngine;

public class InformationPanel : MonoBehaviour
{      

    /*
    *   This class shows the information about the filtered data in the
    *   upper half of the StatisticsPanel. It consists of six fields showing
    *   various aggregated information about the filtered data.
    */

    [SerializeField] TMP_Text nofLegs;
    [SerializeField] TMP_Text nofAgents;
    [SerializeField] TMP_Text avgTravelTime;
    [SerializeField] TMP_Text nofVisiblePaths;
    [SerializeField] TMP_Text nofTransitionalStops;
    [SerializeField] TMP_Text nofActivityStops;

    public void SetAllInformation(int nofTrips, int nofLegs, int nofAgents, float avgTravelTime, int nofVisiblePaths)
    {
        this.nofLegs.text = "Legs\n" + nofLegs.ToString();
        this.nofAgents.text = "Agents\n" + nofAgents.ToString();
        this.avgTravelTime.text = "Average travel duration\n" + SecondsToFormatedTime((int)avgTravelTime);
        this.nofVisiblePaths.text = "Visible paths\n" + nofVisiblePaths; 
    }

    public void SetNofVisiblePaths(int nofVisiblePaths)
    {
        this.nofVisiblePaths.text = "Visible paths\n" + nofVisiblePaths; 
    }

    public void SetNofTransitionalStops(int transitionalStops)
    {
        this.nofTransitionalStops.text = "Transitional Stops\n" + transitionalStops;
    }

    public void SetNofActivityStops(int activityStops)
    {
        this.nofActivityStops.text = "Activity Stops\n" + activityStops;

    }

    private string SecondsToFormatedTime(int s)
    {
        int minutes = s/60;
        int seconds = s - (minutes*60);
        return minutes + "min " + seconds + "s";
    }

}

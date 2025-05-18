using UnityEngine;

public enum StopType{ TransitionalStop, ActivityStop }

public class DatabaseStopData
{   

    /*
    *   Contains the data about stops at a certain location. The type of stop must be 
    *   either 'transitional' or 'activity'.
    *   The location is a small square area on the map. The size of the area is controlled
    *   by the 'resolution' parameter. The data is the aggregation of stops inside this area.
    */

    // Coordinate bounding box for Zürich data: (47.35, 8.43), (47.47, 8.60)
    public static float minLat = 47.35f;    
    public static float minLon = 8.43f;
    public static float maxLat = 47.47f;
    public static float maxLon = 8.60f;

    // Number of squares per axis, total squares: resolution^2
    public static int resolution = 25;
    public static float squareSize = Mathf.Min(maxLat - minLat, maxLon - minLon) / (resolution-1);
    

    // Data of this stop location
    public float dest_lon;
    public float dest_lat;
    public float stopTime;
    public StopType stopType;

    public DatabaseStopData(float dest_lon, float dest_lat, float stopTime, string stopType)
    {   
        this.dest_lon = dest_lon;
        this.dest_lat = dest_lat;
        this.stopTime = stopTime;

        if(stopType.Equals("transitional")) this.stopType = StopType.TransitionalStop;
        else if(stopType.Equals("activity")) this.stopType = StopType.ActivityStop;
        else Debug.LogError("[DatabaseStopData] 'stopType' argument is invalid (arg=" + stopType + ")");
    }

}
using UnityEngine;

public enum StopType{ TransitionalStop, ActivityStop }

public class K_DatabaseStopData : K_IStopData
{
    private static int id_counter = 0;
    public readonly int id;

    // Universal facts          [Boundaries for Corsica data: (41.36811, 8.594249), (42.991383, 9.55595)]
    public static float minLat = 47.35f;    
    public static float minLon = 8.43f;
    public static float maxLat = 47.47f;
    public static float maxLon = 8.60f;
    public static int resolution = 25;      // number of squares per axis, total squares: resolution^2
    public static float squareSize = Mathf.Min(maxLat - minLat, maxLon - minLon) / (resolution-1);
    

    // Fields present in database
    public int person_id;
    public int trip_id;
    public int leg_index;
    public float dest_lon;
    public float dest_lat;
    public float stopTime;
    public StopType stopType;

    public K_DatabaseStopData(int person_id, int trip_id, int leg_index, float dest_lon, float dest_lat, float stopTime, string stopType)
    {   
        this.id = id_counter++;
        this.person_id = person_id;
        this.trip_id = trip_id;
        this.leg_index = leg_index;
        this.dest_lon = dest_lon;
        this.dest_lat = dest_lat;
        this.stopTime = stopTime;
        if(stopType.Equals("transitional")) this.stopType = StopType.TransitionalStop;
        else if(stopType.Equals("activity")) this.stopType = StopType.ActivityStop;
        else Debug.LogError("[K_DatabaseStopData] 'stopType' argument is invalid (arg=" + stopType + ")");
    }

}
using System;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;

public class DatabaseLegsPath
{   
    // Universal facts
    public static DateTime earliestTime = new DateTime(2019, 9, 2, 13, 57, 37); // 2019-09-02 13:57:37
    public static DateTime latestTime = new DateTime(2023, 1, 3, 2, 46, 8); // 2023-01-03 02:46:08
    public static TimeSpan maxTimeDiff;
    public static float minHeight = 0f;
    public static float maxHeight = 2f;
    static AbstractMap _map;

    // Fields present in the database
    public string participant_id;
    public int leg_id;
    public int trip_id;
    public string treatment;
    public int phase;
    public DateTime started_at;
    public DateTime finished_at;
    public int length;
    public int duration;
    public string mode;
    public int start_x;
    public int start_y;
    public int mid_x;
    public int mid_y;
    public int end_x;
    public int end_y;

    // Fields calculated from the database
    public Vector2d epsg3857_start;
    public Vector2d epsg3857_mid;
    public Vector2d epsg3857_end;
    public Vector3 worldStartPoint;
    public Vector3 worldMidPoint;
    public Vector3 worldEndPoint;

    public static void SetParameters(DateTime earliestTime, DateTime latestTime, float minHeight, float maxHeight, AbstractMap m)
    {
        DatabaseLegsPath.earliestTime = earliestTime;
        DatabaseLegsPath.latestTime = latestTime;
        DatabaseLegsPath.maxTimeDiff = latestTime - earliestTime;
        DatabaseLegsPath.minHeight = minHeight;
        DatabaseLegsPath.maxHeight = maxHeight;
        DatabaseLegsPath._map = m;
    }

    public DatabaseLegsPath(string participant_id, int leg_id, int trip_id, string treatment, int phase, DateTime started_at, 
        DateTime finished_at, int length, int duration, string mode, int start_x, int start_y, int mid_x, int mid_y, int end_x, int end_y)
    {
        if(started_at > finished_at) throw new Exception("[DatabaseLegsPath] Start time cannot be larger than finish time");
        this.participant_id = participant_id;
        this.leg_id = leg_id;
        this.trip_id = trip_id;
        this.treatment = treatment;
        this.phase = phase;
        this.started_at = started_at;
        this.finished_at = finished_at;
        this.length = length;
        this.duration = duration;
        this.mode = mode;
        this.start_x = start_x;
        this.start_y = start_y;
        this.mid_x = mid_x;
        this.mid_y = mid_y;
        this.end_x = end_x;
        this.end_y = end_y;
    }

    public void CalculateWorldCoordinates()
    {
        // Start point
        Vector3 startPlanePos = CalculateWorldPlaneCoordinates(epsg3857_start);
        float startHeight = CalculateWorldHeight(started_at);
        worldStartPoint = startPlanePos + Vector3.up * startHeight;

        // Mid point
        Vector3 midPlanePos = CalculateWorldPlaneCoordinates(epsg3857_mid);
        DateTime midTime = started_at.Add(finished_at - started_at);
        float midHeight = CalculateWorldHeight(midTime);
        worldMidPoint = midPlanePos + Vector3.up * midHeight;

        // End point
        Vector3 endPlanePos = CalculateWorldPlaneCoordinates(epsg3857_end);
        float endHeight = CalculateWorldHeight(finished_at);
        worldEndPoint = endPlanePos + Vector3.up * endHeight;

    }

    public Vector3 CalculateWorldPlaneCoordinates(Vector2d latLonCoords)
    {   
        return _map.GeoToWorldPosition(latLonCoords);
    }

    public float CalculateWorldHeight(DateTime timeStamp)
    {
        TimeSpan timeDiff = timeStamp - earliestTime;
        float frac = 1f * timeDiff.Days / maxTimeDiff.Days;
        return minHeight + (maxHeight - minHeight) * frac;
    }

}

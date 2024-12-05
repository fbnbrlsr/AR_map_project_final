using System;
using System.Data.Common;
using Mapbox.Map;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class DatabaseLegData : ILegData
{   
    private static int id_counter = 0;
    public readonly int id;
    // Universal facts
    public static DateTime earliestTime = new DateTime(2019, 9, 2, 13, 57, 37); // 2019-09-02 13:57:37
    public static DateTime latestTime = new DateTime(2023, 1, 3, 2, 46, 8); // 2023-01-03 02:46:08
    public static TimeSpan maxTimeDiff = new TimeSpan(1218, 12, 48, 31);
    public static float minPointHeight = 0f;
    public static float maxPointHeight = 2f;
    public static float minDuration;
    public static float maxDuration;
    public static float maxDurationDiff;
    public static float minArcHeight;
    public static float maxArcHeight;
    public static AbstractMap _map;

    // Fields present in the database, fields that were null have special values
    public string participant_id;
    public int leg_id;
    public int trip_id;
    public string treatment;
    public int phase;
    public DateTime started_at;
    public DateTime finished_at;
    public int length;
    public float duration;
    public string mode;
    public Vector2d startXY;
    public Vector2d midXY;
    public Vector2d endXY;

    // Fields calculated from the database
    public Vector3 worldStartPoint;
    public Vector3 worldMidPoint;
    public Vector3 worldEndPoint;

    public static void SetMap(AbstractMap m)
    {
        _map = m;
    }

    public static void SetTimeParameters(DateTime earliestTime, DateTime latestTime, float minPointHeight, float maxPointHeight)
    {
        DatabaseLegData.earliestTime = earliestTime;
        DatabaseLegData.latestTime = latestTime;
        DatabaseLegData.maxTimeDiff = latestTime - earliestTime;
        DatabaseLegData.minPointHeight = minPointHeight;
        DatabaseLegData.maxPointHeight = maxPointHeight;
        Debug.Log("DatabaseLegData time parameters changed to: minDate=" + earliestTime + ", maxDate=" + latestTime 
            + ", minPointHeight=" + minPointHeight + ", maxPointHeight=" + maxPointHeight);
    }

    public static void SetDurationParameters(float minDuration, float maxDuration, float minArcHeight, float maxArcHeight)
    {
        DatabaseLegData.minDuration = minDuration;
        DatabaseLegData.maxDuration = maxDuration;
        DatabaseLegData.minArcHeight = minArcHeight;
        DatabaseLegData.maxArcHeight = maxArcHeight;
        DatabaseLegData.maxDurationDiff = maxDuration - minDuration;
        Debug.Log("DatabaseLegData duration parameters changed to: minDuration=" + minDuration + ", maxDuration=" + maxDuration
             + ", minArcHeight=" + minArcHeight + ", maxArcHeight=" + maxArcHeight);
    }

    public DatabaseLegData(string participant_id, int leg_id, int trip_id, string treatment, int phase, DateTime started_at, 
        DateTime finished_at, int length, float duration, string mode, float start_x, float start_y, float mid_x, float mid_y, float end_x, float end_y)
    {
        if(started_at > finished_at) throw new Exception("[DatabaseLeg] Start time cannot be larger than finish time");

        this.id = id_counter++;
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
        this.startXY = new Vector2d(start_x, start_y);
        this.midXY = new Vector2d(mid_x, mid_y);
        this.endXY = new Vector2d(end_x, end_y);
    }

    public void UpdateWorldCoordinates()
    {
        // Start point
        Vector3 startPlanePos = CalculateWorldPlaneCoordinates(startXY);
        float startHeight = CalculateWorldHeight(started_at);
        worldStartPoint = startPlanePos + Vector3.up * startHeight;

        // Mid point
        Vector3 midPlanePos = CalculateWorldPlaneCoordinates(midXY);
        DateTime midTime = started_at.Add(finished_at - started_at);
        float midHeight = CalculateWorldHeight(midTime);
        worldMidPoint = midPlanePos + Vector3.up * midHeight;

        // End point
        Vector3 endPlanePos = CalculateWorldPlaneCoordinates(endXY);
        float endHeight = CalculateWorldHeight(finished_at);
        worldEndPoint = endPlanePos + Vector3.up * endHeight;
    }

    private Vector3 CalculateWorldPlaneCoordinates(Vector2d latLonCoords)
    {   
        return _map.GeoToWorldPosition(latLonCoords, true);
    }

    private float CalculateWorldHeight(DateTime timeStamp)
    {
        TimeSpan timeDiff = timeStamp - earliestTime;
        float frac = 1f * timeDiff.Days / maxTimeDiff.Days;
        return minPointHeight + (maxPointHeight - minPointHeight) * frac;
    }

    public float CalculateArcHeight()
    {
        float durationDiff = duration - minDuration;
        float frac = 1f * durationDiff / maxDurationDiff;
        return minArcHeight + (maxArcHeight - minArcHeight) * frac;
    }

    public static float GetHeightFromTime(DateTime timeStamp)
    {
        TimeSpan timeDiff = timeStamp - earliestTime;
        float frac = 1f * timeDiff.Days / maxTimeDiff.Days;
        return minPointHeight + (maxPointHeight - minPointHeight) * frac;
    }

}

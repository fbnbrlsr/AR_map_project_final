using System.ComponentModel;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;
using Mapbox.Unity.Utilities;
using UnityEngine.SceneManagement;

public class K_DatabaseLegData
{   
    public const int personIDLowerBound = 0;
    public const int personIDUpperBound = 1000000;

    private static int id_counter = 0;
    public readonly int id;
    public static float absoluteDistance;
    public static float timeHeightMultiplier = 1f;
    
    // Universal facts
    public static int minPersonID = int.MaxValue;
    public static int maxPersonID = -1;
    public static float earliestTime = 0f; // maybe change to: float.MaxValue; 
    public static float latestTime = SceneManager.GetActiveScene().name.Equals("DummyScene") ? 88000f : 0f; 
    public static float minPointHeight = 0f;
    public static float maxPointHeight = .1f;
    public static float minDuration = float.MaxValue;
    public static float maxDuration = 0f;
    public static AbstractMap _map;
    public static int nof_timeCategories = 6;
    public static bool projectOnTimePlane = false;
    public static float initAbsoluteDistance;

    // Fields present in the database, fields that were null have special values
    public int person_id;
    public int trip_id;
    public int leg_index;
    public float origin_lon;
    public float origin_lat;
    public float dest_lon;
    public float dest_lat;
    public float departure_time;
    public float travel_time;
    public float arrival_time; 
    public TravelMode travel_mode;
    public float travel_distance;

    // Fields calculated from the database;
    public Vector3 worldStartPoint;
    public Vector3 worldEndPoint;
    public int timeCategory;

    public static void SetMap(AbstractMap m)
    {
        _map = m;
    }

    public K_DatabaseLegData(int person_id, int trip_id, int leg_index, float origin_lon, float origin_lat, 
                                float dest_lon, float dest_lat, float departure_time, float travel_time, string travel_mode)
    {   
        if(departure_time < earliestTime) earliestTime = departure_time;
        if(departure_time+travel_time > latestTime) latestTime = departure_time + travel_time;
        if(travel_time < minDuration) minDuration = travel_time;
        if(travel_time > maxDuration) maxDuration = travel_time;
        if(person_id < minPersonID) minPersonID = person_id;
        if(person_id > maxPersonID) maxPersonID = person_id;

        this.id = id_counter++;
        this.person_id = person_id;
        this.trip_id = trip_id;
        this.leg_index = leg_index;
        this.origin_lon = origin_lon;
        this.origin_lat = origin_lat;
        this.dest_lon = dest_lon;
        this.dest_lat = dest_lat;
        this.departure_time = departure_time;
        this.travel_time = travel_time;
        this.arrival_time = departure_time + travel_time;
        this.travel_mode = TravelModeMap.StringToTravelMode(travel_mode);

        absoluteDistance = CustomReloadMap.GetReferenceDistance();
    }

    public static float[][] GetTimeCategoryIntervals()
    {   
        // Fix intervals to be exactly 4 hours
        float[][] arr = new float[nof_timeCategories][];
        for(int i = 0; i < nof_timeCategories; i++)
        {
            float[] interval = new float[]{ i*4f, (i+1)*4f };
            arr[i] = interval;
        }
        return arr;
        
        /*float[][] arr = new float[nof_timeCategories][];
        float timeDiff = latestTime - earliestTime;
        int categoryInterval = (int) (timeDiff / nof_timeCategories);
        for(int i = 0; i < nof_timeCategories; i++)
        {
            float[] interval = new float[2];
            
            interval[0] = i * categoryInterval;
            interval[1] = (i+1) * categoryInterval;

            arr[i] = interval;
        }
        return arr;*/
    }

    public int GetTimeCategory()
    {   
        //float timeDiff = latestTime - earliestTime;
        //int categoryInterval = (int) (timeDiff / nof_timeCategories);
        int category = (int) (departure_time-1) / 14400;

        if(category < 0)
        {
            Debug.LogError("ERROR: Time category is " + category + " which is smaller than 0, set to 0");
            category = 0;
        }
        if(category > nof_timeCategories-1)
        {
            Debug.LogError("ERROR: Time category is " + category + " which is greater than " + (nof_timeCategories-1) + ", set to " + (nof_timeCategories-1));
            category = nof_timeCategories-1;
        }

        return category;
    }

    public void UpdateWorldCoordinates()
    {   
        absoluteDistance = CustomReloadMap.GetReferenceDistance();

        Vector3 startPlanePos = CalculateWorldPlaneCoordinates(origin_lat, origin_lon);
        float startHeight = CalculateWorldHeight(departure_time);
        worldStartPoint = startPlanePos + Vector3.up * startHeight;

        Vector3 endPlanePos = CalculateWorldPlaneCoordinates(dest_lat, dest_lon);
        float endHeight = CalculateWorldHeight(departure_time + travel_time);
        worldEndPoint = endPlanePos + Vector3.up * endHeight;

        float tiltAngle = MapTilting.tiltAngleRad;
        worldStartPoint = RotateByAngle(worldStartPoint, tiltAngle);
        worldEndPoint = RotateByAngle(worldEndPoint, tiltAngle);
    }

    private Vector3 RotateByAngle(Vector3 v, float angle)
    {
        float y = v.y * Mathf.Cos(angle) - v.z * Mathf.Sin(angle);
        float z = v.y * Mathf.Sin(angle) + v.z * Mathf.Cos(angle);
        return new Vector3(v.x, y,  z);
    }

    private Vector3 CalculateWorldPlaneCoordinates(float lat, float lon)
    {   
        Vector2d latLonCoords = new Vector2d(lat, lon);
        return MyGeoToWorldPosition(latLonCoords);
    }

    private Vector3 MyGeoToWorldPosition(Vector2d latlon)
    {   
        var scaleFactor = Mathf.Pow(2, _map.InitialZoom - _map.AbsoluteZoom);
        //Debug.Log("scaleFactor=" + scaleFactor + ", relativeScale=" + map.WorldRelativeScale + ", product=" + scaleFactor * map.WorldRelativeScale);
        var worldPos = Conversions.GeoToWorldPosition(latlon, _map.CenterMercator, _map.WorldRelativeScale * scaleFactor).ToVector3xz();
        return worldPos * absoluteDistance / initAbsoluteDistance;
    }

    private float CalculateWorldHeight(float seconds)
    {   
        float timeDiff = seconds - earliestTime;
        float frac = 1f * timeDiff / (latestTime - earliestTime);
        float realHeight = (minPointHeight + (maxPointHeight - minPointHeight) * frac) * absoluteDistance / 2;

        if(projectOnTimePlane)
        {   
            return Mathf.Max(DynamicTimePlane.height, realHeight);
        }
        return realHeight;
    }

    public string GetStartPoint()
    {
        return "(" + origin_lat + ", " + origin_lon + ")";
    }

    public string GetEndPoint()
    {
        return "(" + dest_lat + ", " + dest_lon + ")";
    }

    public int GetTravelModeInt()
    {
        if(travel_mode == TravelMode.Car) return 0;
        else if(travel_mode == TravelMode.Bike) return 1;
        else if(travel_mode == TravelMode.Walk) return 2;
        else if(travel_mode == TravelMode.CarPassenger) return 3;
        else if(travel_mode == TravelMode.PublicTr) return 4;
        else return -1;
    }
}


public enum TravelMode
{
    Car,
    Walk,
    Bike,
    CarPassenger,
    PublicTr
}

public struct CustomInterval
{
    public float start;
    public float end;

    public CustomInterval(float start, float end)
    {
        this.start = start;
        this.end = end;
    }

    public override bool Equals(object obj)
    {
        if (obj is CustomInterval other)
        {
            return this.start == other.start && this.end == other.end;
        }
        return false;
    }
}

public class TravelModeMap
{
    public static TravelMode StringToTravelMode(string s)
    {   
        s = s.ToLower();
        if(s.Equals("car")) return TravelMode.Car;
        if(s.Equals("walk")) return TravelMode.Walk;
        if(s.Equals("bike")) return TravelMode.Bike;
        if(s.Equals("car_passenger") || s.Equals("car passenger")) return TravelMode.CarPassenger;
        if(s.Equals("pt")) return TravelMode.PublicTr;
        
        throw new InvalidEnumArgumentException("The provided travel mode '" + s + "' does not exist!");
    }

    public static string TravelModeToString(TravelMode m)
    {
        if(m == TravelMode.Car) return "car";
        if(m == TravelMode.Walk) return "walk";
        if(m == TravelMode.Bike) return "bike";
        if(m == TravelMode.CarPassenger) return "car_passenger";
        if(m == TravelMode.PublicTr) return "pt";
        
        throw new InvalidEnumArgumentException("The provided travel mode does not exist!");
    }
}
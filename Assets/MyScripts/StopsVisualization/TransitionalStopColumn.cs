using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class TransitionalStopColumn : IStopColumnVisualization
{   

    /*
    *   This class manages the visualization of a single transitional stops column
    *   at a certain location on the map.
    *   The location is defined by latitude and longitude. This script takes care
    *   of translating the location to world space in Unity. The column is updated
    *   if the map is moved. The columns can be enabled and disabled through buttons
    *   in the interface.
    */

    public static Transform globalRoot;
    public static float initAbsoluteDistance;
    public static AbstractMap map;
    GameObject informationWindowPrefab;
    StopInformationWindow informationWindow;
    GameObject instance;
    GameObject columnPrefab;
    Vector3 worldPos;

    // Information about this particular column
    int nof_stops;
    float heightQuotient = 25f;
    float lat;
    float lon;

    public TransitionalStopColumn(float lat, float lon, int nof_stops, GameObject columnPrefab, GameObject informationPanelPrefab)
    {
        this.lat = lat + DatabaseStopData.squareSize/2f;
        this.lon = lon + DatabaseStopData.squareSize/2f;
        this.nof_stops = nof_stops;
        this.columnPrefab = columnPrefab;
        this.informationWindowPrefab = informationPanelPrefab;

        informationWindow = new StopInformationWindow(nof_stops, informationWindowPrefab);
#if UNITY_EDITOR
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;
#else
        InputEventsInvoker.InputEventTypes.HandSingleTouchStart += OnInputStart;
#endif
    }

    public void DestroyColumn()
    {
        informationWindow.Hide();
        GameObject.Destroy(instance);
    }

    public void InstantiateColumn()
    {   
        if(nof_stops > 0)
        {
            instance = GameObject.Instantiate(columnPrefab);
        }
    }

    public void SetColumnData(int nof_stops, float lat, float lon)
    {
       this.lat = lat;
       this.lon = lon;
       this.nof_stops = nof_stops;
    }

    public void UpdateVisualization(float width)
    {   
        informationWindow.Hide();

        if(instance == null)
        {
            return;
        }

        float height = nof_stops * width / heightQuotient;
        worldPos = MyGeoToWorldPosition(new Vector2d(lat, lon));
        worldPos = worldPos + (height/2) * Vector3.up;
        
        float tiltAngle = MapTilting.tiltAngleRad;
        instance.transform.position = RotateByAngle(worldPos, tiltAngle) + globalRoot.position;
        instance.transform.rotation = Quaternion.Euler(MapTilting.tiltAngleDeg, 0f, 0f);
        instance.transform.localScale = new Vector3(width, height, width);
    }

    private Vector3 MyGeoToWorldPosition(Vector2d latlon)
    {   
        if(initAbsoluteDistance == 0) initAbsoluteDistance = CustomReloadMap.GetReferenceDistance();
        
        var scaleFactor = Mathf.Pow(2, map.InitialZoom - map.AbsoluteZoom);
        var worldPos = Conversions.GeoToWorldPosition(latlon, map.CenterMercator, map.WorldRelativeScale * scaleFactor).ToVector3xz();
        return worldPos * CustomReloadMap.GetReferenceDistance() / initAbsoluteDistance;
    }

    private Vector3 RotateByAngle(Vector3 v, float angle)
    {
        float y = v.y * Mathf.Cos(angle) - v.z * Mathf.Sin(angle);
        float z = v.y * Mathf.Sin(angle) + v.z * Mathf.Cos(angle);
        return new Vector3(v.x, y,  z);
    }

    public void AddStop()
    {
        nof_stops += 1;
    }

    private void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {
        if(targetObj == instance)
        {   
            StopInformationWindow.HideAll();
            informationWindow.Show(interactionPos, CustomHeadTracking.GetHeadPosition(), nof_stops);
        }
    }

}
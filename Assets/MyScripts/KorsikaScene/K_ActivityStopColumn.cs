using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class K_ActivityStopColumn : K_IStopColumnVisualization
{

    public static Transform globalRoot;
    public static float initAbsoluteDistance;
    public static AbstractMap _map;
    GameObject informationWindowPrefab;
    K_StopInformationWindow informationWindow;
    GameObject instance;
    GameObject columnPrefab;
    Vector3 worldPos;

    AbstractMap map;
    Transform mapRoot;
    int nof_stops;
    float heightQuotient = 25f;
    float lat;
    float lon;

    public K_ActivityStopColumn(float lat, float lon, int nof_stops, GameObject columnPrefab, AbstractMap map, GameObject informationPanelPrefab, Transform mapRoot)
    {
        this.lat = lat + K_DatabaseStopData.squareSize/2f;
        this.lon = lon + K_DatabaseStopData.squareSize/2f;
        this.nof_stops = nof_stops;
        this.columnPrefab = columnPrefab;
        this.map = map;
        this.mapRoot = mapRoot;
        this.informationWindowPrefab = informationPanelPrefab;

        informationWindow = new K_StopInformationWindow(lat, lon, nof_stops, informationWindowPrefab);
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
            instance.transform.SetParent(mapRoot);
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
        
        /*if(!InViewingRange(worldPos)){
            instance.SetActive(false);
            return;
        }
        instance.SetActive(true);*/

        float height = nof_stops * width / heightQuotient;
        
        //Vector2d latLonCoords = new Vector2d(lat, lon);
        worldPos = MyGeoToWorldPosition(new Vector2d(lat, lon));
        worldPos = worldPos + (height/2) * Vector3.up;
        
        float tiltAngle = MapTilting.tiltAngleRad;
        instance.transform.position = RotateByAngle(worldPos, tiltAngle) + globalRoot.position;
        instance.transform.rotation = Quaternion.Euler(MapTilting.tiltAngleDeg, 0f, 0f);
        instance.transform.localScale = new Vector3(width, height, width);
    }

    private Vector3 MyGeoToWorldPosition(Vector2d latlon)
    {   
        var scaleFactor = Mathf.Pow(2, _map.InitialZoom - _map.AbsoluteZoom);
        //Debug.Log("scaleFactor=" + scaleFactor + ", relativeScale=" + map.WorldRelativeScale + ", product=" + scaleFactor * map.WorldRelativeScale);
        var worldPos = Conversions.GeoToWorldPosition(latlon, _map.CenterMercator, _map.WorldRelativeScale * scaleFactor).ToVector3xz();
        return worldPos / 10f * CustomReloadMap.GetReferenceDistance() / initAbsoluteDistance;
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

    public void Print()
    {
        Debug.Log("ActivityStop: lat=" + lat + ", lon=" + lon + ", nof_stops=" + nof_stops);
    }

    private bool InViewingRange(Vector3 p)
    {
        if(p.x < 7f && p.x > -7f && p.z < 7f && p.z > -7f) return true;
        return false;
    }

    private void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {   
        if(targetObj == instance)
        {   
            K_StopInformationWindow.HideAll();
            informationWindow.Show(interactionPos, CustomHeadTracking.GetHeadPosition(), nof_stops);
        }
    }

}
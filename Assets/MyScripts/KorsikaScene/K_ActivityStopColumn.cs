using System.Threading;
using Mapbox.Unity.Map;
using Mapbox.Unity.Map.Strategies;
using Mapbox.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class K_ActivityStopColumn : K_IStopColumnVisualization
{

    GameObject informationWindowPrefab;
    K_StopInformationWindow informationWindow;
    GameObject instance;
    GameObject columnPrefab;
    Vector3 worldPos;

    AbstractMap map;
    Transform mapRoot;
    int nof_stops;
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
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;
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
        
        if(!InViewingRange(worldPos)){
            instance.SetActive(false);
            return;
        }
        instance.SetActive(true);

        float height = nof_stops * width / 100;
        
        Vector2d latLonCoords = new Vector2d(lat, lon);
        worldPos = map.GeoToWorldPosition(latLonCoords, true);

        instance.transform.position = worldPos + (height/2) * Vector3.up;
        instance.transform.localScale = new Vector3(width, height, width);
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

    private void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        if(targetObj == instance)
        {
            informationWindow.Show(interactionPos, CustomHeadTracking.GetHeadPosition(), nof_stops);
        }
    }

}
using System.Collections.Generic;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using UnityEngine;

public class K_LocationStopVisualizationManager : MonoBehaviour
{
    private static K_DatabaseManager _databaseManager;

    // Visualization parameters
    [SerializeField] GameObject transitionalStopColumnPrefab;
    [SerializeField] GameObject activityStopColumnPrefab;
    [SerializeField] AbstractMap abstractMap;
    [SerializeField] GameObject transitionalStopInformationWindowPrefab;
    [SerializeField] GameObject activityStopInformationWindowPrefab;
    [SerializeField] Transform globalRoot;
    [SerializeField] Transform mapRoot;
    [SerializeField] MyBellButton showTransitionalStopsButton;
    [SerializeField] MyBellButton showActivityStopsButton;

    List<K_TransitionalStopColumn> spawnedTransitionalStopColumnsList;
    List<K_ActivityStopColumn> spawnedActivityStopColumnsList;


    void Start()
    {   
        Debug.Log("Started K_LocationStopVisualizationManager");

        _databaseManager = K_DatabaseManager.GetInstance();
        K_DatabaseManager.SetLocationStopVisualizationManager(this);
        _databaseManager.InitializeLocationStops();

        //showStopsTypeDropdown.onValueChanged.AddListener(OnStopsTypeChanged);

        abstractMap.OnUpdated += OnMapUpdated;

        showTransitionalStopsButton.OnToggleButton += ShowTransitionalStopsButtonToggle;
        showActivityStopsButton.OnToggleButton += ShowActivityStopsButtonToggle;

        K_TransitionalStopColumn.globalRoot = globalRoot;
        K_TransitionalStopColumn.initAbsoluteDistance = CustomReloadMap.GetReferenceDistance();
        K_TransitionalStopColumn._map = abstractMap;

        K_ActivityStopColumn.globalRoot = globalRoot;
        K_ActivityStopColumn.initAbsoluteDistance = CustomReloadMap.GetReferenceDistance();
        K_ActivityStopColumn._map = abstractMap;
    }

    private void ShowTransitionalStopsButtonToggle(bool val)
    {
        if(val) OnSpawnTransitionalStops();
        else OnDestroyTransitionalStops();
    }

    private void ShowActivityStopsButtonToggle(bool val)
    {
        if(val) OnSpawnActivityStops();
        else OnDestroyActivityStops();
    }

    void OnMapUpdated()
    {   
        float columnWidth = CustomReloadMap.GetReferenceDistance() * K_DatabaseStopData.squareSize;

        if(spawnedTransitionalStopColumnsList != null)
        {   
            foreach(K_TransitionalStopColumn p in spawnedTransitionalStopColumnsList)
            {
                p.UpdateVisualization(columnWidth);
            }
        }
        if(spawnedActivityStopColumnsList != null)
        {   
            foreach(K_ActivityStopColumn p in spawnedActivityStopColumnsList)
            {
                p.UpdateVisualization(columnWidth);
            }
        }
    }

    private void OnSpawnTransitionalStops()
    {   
        K_StopInformationWindow.HideAll();
        spawnedTransitionalStopColumnsList = new List<K_TransitionalStopColumn>();
        CreateTransitionalStopsGrid();
    }

    private void OnSpawnActivityStops()
    {      
        K_StopInformationWindow.HideAll();
        spawnedActivityStopColumnsList = new List<K_ActivityStopColumn>();
        CreateActivityStopsGrid();
    }

    private void CreateTransitionalStopsGrid()
    {   
        int x_elements = (int) ((K_DatabaseStopData.maxLat - K_DatabaseStopData.minLat) / K_DatabaseStopData.squareSize);
        int y_elements = (int) ((K_DatabaseStopData.maxLon - K_DatabaseStopData.minLon) / K_DatabaseStopData.squareSize);
        K_TransitionalStopColumn[][] transitionalStops = new K_TransitionalStopColumn[x_elements][];

        for(int x = 0; x < x_elements; x++)
        {
            transitionalStops[x] = new K_TransitionalStopColumn[y_elements];
            for(int y = 0; y < y_elements; y++)
            {   
                float lat = K_DatabaseStopData.minLat + x * K_DatabaseStopData.squareSize;
                float lon = K_DatabaseStopData.minLon + y * K_DatabaseStopData.squareSize;
                transitionalStops[x][y] = new K_TransitionalStopColumn(lat, lon, 0, transitionalStopColumnPrefab, abstractMap, transitionalStopInformationWindowPrefab, mapRoot);
                spawnedTransitionalStopColumnsList.Add(transitionalStops[x][y]);
            }
        }

        List<K_DatabaseStopData> stopsFromDatabase = _databaseManager.GetLocationsStopsList();
        int totalTransitionalStops = 0;
        foreach(K_DatabaseStopData stop in stopsFromDatabase)
        {   
            if(!(stop.stopType == StopType.TransitionalStop)) continue;

            int x_idx = (int) ((stop.dest_lat - K_DatabaseStopData.minLat) / K_DatabaseStopData.squareSize);
            int y_idx = (int) ((stop.dest_lon - K_DatabaseStopData.minLon) / K_DatabaseStopData.squareSize);

            if(x_idx >= x_elements || y_idx >= y_elements) continue;

            transitionalStops[x_idx][y_idx].AddStop();
            totalTransitionalStops += 1;
        }

        float columnWidth = CustomReloadMap.GetReferenceDistance() * K_DatabaseStopData.squareSize;
        foreach(K_TransitionalStopColumn stop in spawnedTransitionalStopColumnsList)
        {
            stop.InstantiateColumn();
            stop.UpdateVisualization(columnWidth);
        }

        K_InformationPanel infoPanel = GameObject.Find("StatisticsPanel").GetComponent<K_InformationPanel>();
        infoPanel?.SetNofTransitionalStops(totalTransitionalStops);

    }

    private void CreateActivityStopsGrid()
    {
        int x_elements = (int) ((K_DatabaseStopData.maxLat - K_DatabaseStopData.minLat) / K_DatabaseStopData.squareSize);
        int y_elements = (int) ((K_DatabaseStopData.maxLon - K_DatabaseStopData.minLon) / K_DatabaseStopData.squareSize);
        K_ActivityStopColumn[][] activityStops = new K_ActivityStopColumn[x_elements][];

        for(int x = 0; x < x_elements; x++)
        {
            activityStops[x] = new K_ActivityStopColumn[y_elements];
            for(int y = 0; y < y_elements; y++)
            {   
                float lat = K_DatabaseStopData.minLat + x * K_DatabaseStopData.squareSize;
                float lon = K_DatabaseStopData.minLon + y * K_DatabaseStopData.squareSize;
                activityStops[x][y] = new K_ActivityStopColumn(lat, lon, 0, activityStopColumnPrefab, abstractMap, activityStopInformationWindowPrefab, mapRoot);
                spawnedActivityStopColumnsList.Add(activityStops[x][y]);
            }
        }

        List<K_DatabaseStopData> stopsFromDatabase = _databaseManager.GetLocationsStopsList();
        int totalActivityStops = 0;
        foreach(K_DatabaseStopData stop in stopsFromDatabase)
        {   
            if(!(stop.stopType == StopType.ActivityStop)) continue;

            int x_idx = (int) ((stop.dest_lat - K_DatabaseStopData.minLat) / K_DatabaseStopData.squareSize);
            int y_idx = (int) ((stop.dest_lon - K_DatabaseStopData.minLon) / K_DatabaseStopData.squareSize);

            if(x_idx >= x_elements || y_idx >= y_elements) continue;

            activityStops[x_idx][y_idx].AddStop();
            totalActivityStops += 1;
        }

        float columnWidth = CustomReloadMap.GetReferenceDistance() * K_DatabaseStopData.squareSize;
        foreach(K_ActivityStopColumn stop in spawnedActivityStopColumnsList)
        {
            stop.InstantiateColumn();
            stop.UpdateVisualization(columnWidth);
        }

        K_InformationPanel infoPanel = GameObject.Find("StatisticsPanel").GetComponent<K_InformationPanel>();
        infoPanel?.SetNofActivityStops(totalActivityStops);
    }

    private void OnDestroyTransitionalStops()
    {
        if(spawnedTransitionalStopColumnsList != null)
        {
            foreach(K_TransitionalStopColumn stop in spawnedTransitionalStopColumnsList)
            {
                stop.DestroyColumn();
            }
            spawnedTransitionalStopColumnsList.Clear();
        }
    }

    private void OnDestroyActivityStops()
    {
        if(spawnedActivityStopColumnsList != null)
        {
            foreach(K_ActivityStopColumn stop in spawnedActivityStopColumnsList)
            {
                stop.DestroyColumn();
            }
            spawnedActivityStopColumnsList.Clear();
        }
    }


}
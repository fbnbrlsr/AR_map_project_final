using System.Collections.Generic;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using UnityEngine;

public class LocationStopVisualizationManager : MonoBehaviour
{   

    /*
    *   This class manages the activity and transitional stops columns. 
    *   The initialization is done based on the data in the DatabaseManager when
    *   the application was started.
    *   It handles the button toggles upon which the columns are enabled or 
    *   disabled. Also, when a column is being touch by the user, a window with 
    *   information about that column is spawned at the point of interaction.
    */

    private static DatabaseManager databaseManager;

    [SerializeField] GameObject transitionalStopColumnPrefab;
    [SerializeField] GameObject activityStopColumnPrefab;
    [SerializeField] AbstractMap abstractMap;
    [SerializeField] GameObject transitionalStopInformationWindowPrefab;
    [SerializeField] GameObject activityStopInformationWindowPrefab;
    [SerializeField] Transform globalRoot;
    [SerializeField] MyBellButton showTransitionalStopsButton;
    [SerializeField] MyBellButton showActivityStopsButton;

    List<TransitionalStopColumn> spawnedTransitionalStopColumnsList;
    int totalTransitionalStops;
    List<ActivityStopColumn> spawnedActivityStopColumnsList;
    int totalActivityStops;


    void Start()
    {   
        databaseManager = DatabaseManager.GetInstance();
        databaseManager.InitializeLocationStops();
        CreateTransitionalStopsGrid();
        CreateActivityStopsGrid();

        abstractMap.OnUpdated += OnMapUpdated;

        showTransitionalStopsButton.OnToggleButton += ShowTransitionalStopsButtonToggle;
        showActivityStopsButton.OnToggleButton += ShowActivityStopsButtonToggle;

        TransitionalStopColumn.globalRoot = globalRoot;
        TransitionalStopColumn.initAbsoluteDistance = CustomReloadMap.GetReferenceDistance();
        TransitionalStopColumn.map = abstractMap;

        ActivityStopColumn.globalRoot = globalRoot;
        ActivityStopColumn.initAbsoluteDistance = CustomReloadMap.GetReferenceDistance();
        ActivityStopColumn.map = abstractMap;
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
        float columnWidth = CustomReloadMap.GetReferenceDistance() * DatabaseStopData.squareSize;

        if(spawnedTransitionalStopColumnsList != null)
        {   
            foreach(TransitionalStopColumn p in spawnedTransitionalStopColumnsList)
            {
                p.UpdateVisualization(columnWidth);
            }
        }
        if(spawnedActivityStopColumnsList != null)
        {   
            foreach(ActivityStopColumn p in spawnedActivityStopColumnsList)
            {
                p.UpdateVisualization(columnWidth);
            }
        }
    }

    private void OnSpawnTransitionalStops()
    {   
        StopInformationWindow.HideAll();

        float columnWidth = CustomReloadMap.GetReferenceDistance() * DatabaseStopData.squareSize;
        foreach(TransitionalStopColumn stop in spawnedTransitionalStopColumnsList)
        {
            stop.InstantiateColumn();
            stop.UpdateVisualization(columnWidth);
        }

        InformationPanel infoPanel = GameObject.Find("StatisticsPanel").GetComponent<InformationPanel>();
        infoPanel?.SetNofTransitionalStops(totalTransitionalStops);
    }

    private void OnSpawnActivityStops()
    {      
        StopInformationWindow.HideAll();

        float columnWidth = CustomReloadMap.GetReferenceDistance() * DatabaseStopData.squareSize;
        foreach(ActivityStopColumn stop in spawnedActivityStopColumnsList)
        {
            stop.InstantiateColumn();
            stop.UpdateVisualization(columnWidth);
        }

        InformationPanel infoPanel = GameObject.Find("StatisticsPanel").GetComponent<InformationPanel>();
        infoPanel?.SetNofActivityStops(totalActivityStops);
    }

    private void CreateTransitionalStopsGrid()
    {   
        int x_elements = (int) ((DatabaseStopData.maxLat - DatabaseStopData.minLat) / DatabaseStopData.squareSize);
        int y_elements = (int) ((DatabaseStopData.maxLon - DatabaseStopData.minLon) / DatabaseStopData.squareSize);
        TransitionalStopColumn[][] transitionalStops = new TransitionalStopColumn[x_elements][];

        spawnedTransitionalStopColumnsList = new List<TransitionalStopColumn>();
        for(int x = 0; x < x_elements; x++)
        {
            transitionalStops[x] = new TransitionalStopColumn[y_elements];
            for(int y = 0; y < y_elements; y++)
            {   
                float lat = DatabaseStopData.minLat + x * DatabaseStopData.squareSize;
                float lon = DatabaseStopData.minLon + y * DatabaseStopData.squareSize;
                transitionalStops[x][y] = new TransitionalStopColumn(lat, lon, 0, transitionalStopColumnPrefab, transitionalStopInformationWindowPrefab);
                spawnedTransitionalStopColumnsList.Add(transitionalStops[x][y]);
            }
        }

        List<DatabaseStopData> stopsFromDatabase = databaseManager.GetLocationsStopsList();
        totalTransitionalStops = 0;
        foreach(DatabaseStopData stop in stopsFromDatabase)
        {   
            if(!(stop.stopType == StopType.TransitionalStop)) continue;

            int x_idx = (int) ((stop.dest_lat - DatabaseStopData.minLat) / DatabaseStopData.squareSize);
            int y_idx = (int) ((stop.dest_lon - DatabaseStopData.minLon) / DatabaseStopData.squareSize);

            if(x_idx >= x_elements || y_idx >= y_elements) continue;

            transitionalStops[x_idx][y_idx].AddStop();
            totalTransitionalStops += 1;
        }
    }

    private void CreateActivityStopsGrid()
    {
        int x_elements = (int) ((DatabaseStopData.maxLat - DatabaseStopData.minLat) / DatabaseStopData.squareSize);
        int y_elements = (int) ((DatabaseStopData.maxLon - DatabaseStopData.minLon) / DatabaseStopData.squareSize);
        ActivityStopColumn[][] activityStops = new ActivityStopColumn[x_elements][];

        spawnedActivityStopColumnsList = new List<ActivityStopColumn>();
        for(int x = 0; x < x_elements; x++)
        {
            activityStops[x] = new ActivityStopColumn[y_elements];
            for(int y = 0; y < y_elements; y++)
            {   
                float lat = DatabaseStopData.minLat + x * DatabaseStopData.squareSize;
                float lon = DatabaseStopData.minLon + y * DatabaseStopData.squareSize;
                activityStops[x][y] = new ActivityStopColumn(lat, lon, 0, activityStopColumnPrefab, activityStopInformationWindowPrefab);
                spawnedActivityStopColumnsList.Add(activityStops[x][y]);
            }
        }

        List<DatabaseStopData> stopsFromDatabase = databaseManager.GetLocationsStopsList();
        int totalActivityStops = 0;
        foreach(DatabaseStopData stop in stopsFromDatabase)
        {   
            if(!(stop.stopType == StopType.ActivityStop)) continue;

            int x_idx = (int) ((stop.dest_lat - DatabaseStopData.minLat) / DatabaseStopData.squareSize);
            int y_idx = (int) ((stop.dest_lon - DatabaseStopData.minLon) / DatabaseStopData.squareSize);

            if(x_idx >= x_elements || y_idx >= y_elements) continue;

            activityStops[x_idx][y_idx].AddStop();
            totalActivityStops += 1;
        }
    }

    private void OnDestroyTransitionalStops()
    {
        if(spawnedTransitionalStopColumnsList != null)
        {
            foreach(TransitionalStopColumn stop in spawnedTransitionalStopColumnsList)
            {
                stop.DestroyColumn();
            }
        }
    }

    private void OnDestroyActivityStops()
    {
        if(spawnedActivityStopColumnsList != null)
        {
            foreach(ActivityStopColumn stop in spawnedActivityStopColumnsList)
            {
                stop.DestroyColumn();
            }
        }
    }


}
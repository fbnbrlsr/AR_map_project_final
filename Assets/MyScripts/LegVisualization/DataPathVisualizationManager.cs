using System.Collections.Generic;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void NewWindowSpawned();
public delegate void InitializationFinishedEvent();

/*
    Manages the visualization of the path data from the database
    Uses the DatabaseManager to access the Mobis data from the database
    Creates DatabaseLegData object and passes them to the path classes which then visualize the data
*/
public class DataPathVisualizationManager : MonoBehaviour
{   

    public static event NewWindowSpawned GlobalNewWindowSpawnedEvent;
    public static event InitializationFinishedEvent InitializationFinishedEvent;

#if UNITY_EDITOR
    readonly int SPAWN_LIMIT = 750;
#else
    readonly int SPAWN_LIMIT = 750;
#endif
    private static DatabaseManager _databaseManager;

    // Visualization parameters
    //[SerializeField] List<GameObject> lineTimePrefabs;
    [SerializeField] List<GameObject> lineTravelModePrefabs;
    [SerializeField] GameObject lineInformationPopupPrefab;
    [SerializeField] AbstractMap abstractMap;
    [SerializeField] Transform mapRoot;
    [SerializeField] Transform globalMapRoot;
    [SerializeField] Material selectedLineMaterial;
    [SerializeField] MyBellButton showPathsButton;
    [SerializeField] TimePoleConfiguration timePoleConfiguration;

    // Leg visualization
    List<TwoPointLineVisualizer> spawnedPathsList;

    void Start()
    {   
        if(SceneManager.GetActiveScene().name.Equals("DummyScene"))
        {
            timePoleConfiguration.UpdateConfiguration();
            return;
        }

        DatabaseLegData.initAbsoluteDistance = CustomReloadMap.GetReferenceDistance();

        DatabaseLegData.SetMap(abstractMap);
        //DatabaseLegData.nof_timeCategories = lineTimePrefabs.Count;
        _databaseManager = DatabaseManager.GetInstance();
        DatabaseManager.SetDataPathVisualizationManager(this);
        _databaseManager.InitializePaths();

        spawnedPathsList = new List<TwoPointLineVisualizer>();

        DynamicTimePlane.TimePlaneChanged += OnMapUpdated;
        abstractMap.OnUpdated += OnMapUpdated;
        //lineStyleDropdown.onValueChanged.AddListener(OnLineStyleChanged);
        LegLine.selectedMaterial = selectedLineMaterial;

        showPathsButton.OnToggleButton += OnShowPathsButtonToggled;

        timePoleConfiguration.UpdateConfiguration();

        InitializationFinishedEvent?.Invoke();
    }

    void OnShowPathsButtonToggled(bool v)
    {   
        if(v) OnSpawnAllPaths();
        else OnDestroyPathsButtonPressed();
    }

    void OnMapUpdated()
    {   
        if(spawnedPathsList != null)
        {   
            foreach(TwoPointLineVisualizer p in spawnedPathsList)
            {
                p.UpdateVisualization();
            } 
        }
    }

    void Update()
    {
        TwoPointLineVisualizer.globalMapRoot = globalMapRoot;
    }

    private void OnSpawnAllPaths()
    {   
        //while(SpawnSinglePath());

        List<DatabaseLegData> filteredLegsList = _databaseManager.GetFilteredLegsList();

        if(filteredLegsList.Count > 0) MapLegend.ShowLegend();
        InformationPanel infoPanel = GameObject.Find("StatisticsPanel").GetComponent<InformationPanel>();
        if(infoPanel != null)
        {
            infoPanel.SetNofVisiblePaths(filteredLegsList.Count);
        }
        
        int pathsCount = 0;
        foreach(DatabaseLegData leg in filteredLegsList)
        {
            pathsCount++;
            if(pathsCount >= SPAWN_LIMIT)
            {
                NotificationPopup popup = new NotificationPopup();
                popup.Show("Path spawn limit has been set to " + SPAWN_LIMIT + " to prevent the app from crashing. Deselect some data filters to reduce the number of paths.");
                return;
            }

            int travelModeInt = leg.GetTravelModeInt();
            TwoPointLineVisualizer linePath = new TwoPointLineVisualizer(leg, lineTravelModePrefabs[travelModeInt], lineInformationPopupPrefab, abstractMap);
            linePath.InstantiatePath();
            linePath.UpdateVisualization();
            spawnedPathsList.Add(linePath);

        }
    }

    public void DestroyAllPaths()
    {   
        //showPathsButton.SetState(false);
        if(spawnedPathsList != null)
        {   
            foreach(TwoPointLineVisualizer p in spawnedPathsList)
            {
                p.DestroyPath();
            } 
        }
        spawnedPathsList = new List<TwoPointLineVisualizer>();
        MapLegend.HideLegend();
    }

    void OnDestroyPathsButtonPressed()
    {
        DestroyAllPaths();
    }

    public static void InvokeGlobalNewWindowSpawnedEvent()
    {
        GlobalNewWindowSpawnedEvent?.Invoke();
    }

}
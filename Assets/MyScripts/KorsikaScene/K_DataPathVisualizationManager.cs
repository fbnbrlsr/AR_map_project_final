using System;
using System.Collections.Generic;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public delegate void NewWindowSpawned();
public delegate void InitializationFinishedEvent();

/*
    Manages the visualization of the path data from the database
    Uses the DatabaseManager to access the Mobis data from the database
    Creates DatabaseLegData object and passes them to the path classes which then visualize the data
*/
public class K_DataPathVisualizationManager : MonoBehaviour
{   

    public static event NewWindowSpawned GlobalNewWindowSpawnedEvent;
    public static event InitializationFinishedEvent InitializationFinishedEvent;

#if UNITY_EDITOR
    readonly int SPAWN_LIMIT = 750;
#else
    readonly int SPAWN_LIMIT = 750;
#endif
    private static K_DatabaseManager _databaseManager;

    // Visualization parameters
    [SerializeField] List<GameObject> lineTimePrefabs;
    [SerializeField] List<GameObject> lineTravelModePrefabs;
    [SerializeField] GameObject lineInformationPopupPrefab;
    [SerializeField] AbstractMap abstractMap;
    [SerializeField] Transform mapRoot;
    [SerializeField] Transform globalMapRoot;
    [SerializeField] Material selectedLineMaterial;
    [SerializeField] MyBellButton showPathsButton;
    [SerializeField] TimePoleConfiguration timePoleConfiguration;

    // Leg visualization
    List<K_TwoPointLineVisualizer> spawnedPathsList;

    void Start()
    {   
        if(SceneManager.GetActiveScene().name.Equals("DummyScene"))
        {
            timePoleConfiguration.UpdateConfiguration();
            return;
        }

        K_DatabaseLegData.initAbsoluteDistance = CustomReloadMap.GetReferenceDistance();

        K_DatabaseLegData.SetMap(abstractMap);
        K_DatabaseLegData.nof_timeCategories = lineTimePrefabs.Count;
        _databaseManager = K_DatabaseManager.GetInstance();
        K_DatabaseManager.SetDataPathVisualizationManager(this);
        _databaseManager.InitializePaths(10000);

        spawnedPathsList = new List<K_TwoPointLineVisualizer>();

        DynamicTimePlane.TimePlaneChanged += OnMapUpdated;
        abstractMap.OnUpdated += OnMapUpdated;
        //lineStyleDropdown.onValueChanged.AddListener(OnLineStyleChanged);
        FadeLine.selectedMaterial = selectedLineMaterial;

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
            foreach(ITwoPointVisualization p in spawnedPathsList)
            {
                p.UpdateVisualization();
            } 
        }
    }

    void Update()
    {
        K_TwoPointLineVisualizer.globalMapRoot = globalMapRoot;
    }

    private void OnSpawnAllPaths()
    {   
        //while(SpawnSinglePath());

        List<K_DatabaseLegData> filteredLegsList = _databaseManager.GetFilteredLegsList();

        if(filteredLegsList.Count > 0) MapLegend.ShowLegend();
        K_InformationPanel infoPanel = GameObject.Find("StatisticsPanel").GetComponent<K_InformationPanel>();
        if(infoPanel != null)
        {
            infoPanel.SetNofVisiblePaths(filteredLegsList.Count);
        }
        
        int pathsCount = 0;
        foreach(K_DatabaseLegData leg in filteredLegsList)
        {
            pathsCount++;
            if(pathsCount >= SPAWN_LIMIT)
            {
                NotificationPopup popup = new NotificationPopup();
                popup.Show("Path spawn limit has been set to " + SPAWN_LIMIT + " to prevent the app from crashing. Deselect some data filters to reduce the number of paths.");
                return;
            }

            int travelModeInt = leg.GetTravelModeInt();
            K_TwoPointLineVisualizer linePath = new K_TwoPointLineVisualizer(leg, lineTravelModePrefabs[travelModeInt], lineInformationPopupPrefab, abstractMap, mapRoot);
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
            foreach(ITwoPointVisualization p in spawnedPathsList)
            {
                p.DestroyPath();
            } 
        }
        spawnedPathsList = new List<K_TwoPointLineVisualizer>();
        _databaseManager.ResetLegIndex();
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
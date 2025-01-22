using System;
using System.Collections.Generic;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/*
    Manages the visualization of the path data from the database
    Uses the DatabaseManager to access the Mobis data from the database
    Creates DatabaseLegData object and passes them to the path classes which then visualize the data
*/
public class K_DataPathVisualizationManager : MonoBehaviour
{   
#if UNITY_EDITOR
    readonly int SPAWN_LIMIT = 5000;
#else
    readonly int SPAWN_LIMIT = 1500;
#endif
    private static K_DatabaseManager _databaseManager;
    private InputEventTypes inputEvents;

    // Visualization parameters
    [SerializeField] GameObject startPointPrefab;
    [SerializeField] GameObject endPointPrefab;
    [SerializeField] GameObject greenToRedLinePrefab;
    [SerializeField] List<GameObject> linePrefabs;
    [SerializeField] GameObject lineInformationPopupPrefab;
    [SerializeField] AbstractMap abstractMap;
    [SerializeField] GameObject spawnAllPathsButtonGO;
    [SerializeField] GameObject spawnPathButtonGO;
    [SerializeField] GameObject spawn10PathsButtonGO;
    [SerializeField] GameObject spawn50PathsButtonGO;
    [SerializeField] GameObject destroyPathsButtonGO;
    [SerializeField] TMP_Dropdown lineStyleDropdown;
    [SerializeField] GameObject popupWindowPrefab;
    [SerializeField] Transform mapRoot;
    [SerializeField] Transform globalMapRoot;
    [SerializeField] Material selectedLineMaterial;

    public enum PathType{
        Line,
        Arc
    }
    private PathType pathType;

    // Leg visualization
    List<K_TwoPointLineVisualizer> spawnedTwoPointPathList;

    void Start()
    {   
        Debug.Log("Started K_DataPathVisualizationManager");

        K_DatabaseLegData.initAbsoluteDistance = CustomReloadMap.GetReferenceDistance();

        inputEvents = InputEventsInvoker.InputEventTypes;
        inputEvents.HandSingleIPinchStart += OnInputStart;

        K_DatabaseLegData.SetMap(abstractMap);
        K_DatabaseLegData.nof_timeCategories = linePrefabs.Count;
        _databaseManager = K_DatabaseManager.GetInstance();
        K_DatabaseManager.SetDataPathVisualizationManager(this);
        _databaseManager.InitializePaths(10000);
        
        spawnedTwoPointPathList = new List<K_TwoPointLineVisualizer>();

        DynamicTimePlane.TimePlaneChanged += OnMapUpdated;
        abstractMap.OnUpdated += OnMapUpdated;
        lineStyleDropdown.onValueChanged.AddListener(OnLineStyleChanged);

        FadeLine.selectedMaterial = selectedLineMaterial;
    }

    void OnLineStyleChanged(int index)
    {   
        string type = lineStyleDropdown.options[lineStyleDropdown.value].text;
        if(type.Equals("Straight Line"))
        {
            pathType = PathType.Line;
        }
        else if(type.Equals("Arc Line"))
        {
            pathType = PathType.Arc;
            Debug.LogWarning("Only one line type supported (Straight Line)");
            DebugPanel.Log("Only one line type supported (Straight Line)");
        }
    }

    void OnMapUpdated()
    //void Update()
    {   
        if(spawnedTwoPointPathList != null)
        {   
            foreach(ITwoPointVisualization p in spawnedTwoPointPathList)
            {
                p.UpdateVisualization();
            } 
        }
    }

    void Update()
    {
        K_TwoPointLineVisualizer.globalMapRoot = globalMapRoot;
    }

    void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        /*if(targetObj.transform.IsChildOf(spawnPointButtonGO.transform))
        {
            OnSpawnPointButtonPressed();
        }*/
        if(targetObj.transform.IsChildOf(spawnAllPathsButtonGO.transform))
        {
            OnSpawnAllPaths();
        }
        else if(targetObj.transform.IsChildOf(spawnPathButtonGO.transform))
        {
            OnSpawnPathButtonPressed();
        }
        else if(targetObj.transform.IsChildOf(spawn10PathsButtonGO.transform))
        {
            for(int i = 0; i < 10; i++)
            {
                if(!SpawnSinglePath()) break;
            }
            
        }
        else if(targetObj.transform.IsChildOf(spawn50PathsButtonGO.transform))
        {
            for(int i = 0; i < 50; i++)
            {
                if(!SpawnSinglePath()) break;
            }
        }
        else if(targetObj.transform.IsChildOf(destroyPathsButtonGO.transform))
        {
            OnDestroyPathsButtonPressed();
        }
    }

    private void OnSpawnAllPaths()
    {   
        while(SpawnSinglePath());
    }

    private void OnSpawnPathButtonPressed()
    {   
        SpawnSinglePath();
    }

    private bool SpawnSinglePath()
    {   
        if(spawnedTwoPointPathList.Count > 0) StaticUI.ShowLegend();

        if(spawnedTwoPointPathList.Count >= SPAWN_LIMIT)
        {
            NotificationPopup popup = new NotificationPopup();
            popup.Show("Path spawn limit has been set to " + SPAWN_LIMIT + " to prevent the app from crashing!");
            return false;
        }

        if(_databaseManager.HasMorePaths())
        {   
            K_DatabaseLegData leg = _databaseManager.GetNextLeg();
            switch (pathType)
            {
                case PathType.Line:
                    int timeCategory = leg.GetTimeCategory();
                    K_TwoPointLineVisualizer linePath = new K_TwoPointLineVisualizer(leg, startPointPrefab, endPointPrefab, linePrefabs[timeCategory], lineInformationPopupPrefab, abstractMap, mapRoot);
                    linePath.InstantiatePath();
                    linePath.UpdateVisualization();
                    spawnedTwoPointPathList.Add(linePath);
                    break;
                /*case PathType.Arc:
                    TwoPointArcVisualizer arcPath = new TwoPointArcVisualizer(leg, startPointPrefab, endPointPrefab, arcPrefab, informationPanelPrefab, abstractMap);
                    arcPath.InstantiatePath();
                    arcPath.UpdateVisualization();
                    twoPointPathList.Add(arcPath);
                    spawnedPaths += 1;
                    DebugPanel.Log(" > Spawned arc #" + spawnedPaths + ", from: " + leg.startXY + ", to: " + leg.endXY);
                    break;*/
                default:
                    Debug.LogError("Path type not selected or not supported...");
                    break;
            }

            K_InformationPanel infoPanel = GameObject.Find("InformationPanel").GetComponent<K_InformationPanel>();
            if(infoPanel != null)
            {
                infoPanel.SetNofVisiblePaths(spawnedTwoPointPathList.Count);
            }

            return true;
        }
        else{
            DebugPanel.Log("No more paths to spawn...");
            return false;
        }  
    }

    public void DestroyAllPaths()
    {   
        if(spawnedTwoPointPathList != null)
        {   
            foreach(ITwoPointVisualization p in spawnedTwoPointPathList)
            {
                p.DestroyPath();
            } 
        }
        spawnedTwoPointPathList = new List<K_TwoPointLineVisualizer>();
        _databaseManager.ResetLegIndex();

        StaticUI.HideLegend();
    }

    void OnDestroyPathsButtonPressed()
    {
        DestroyAllPaths();
    }

}
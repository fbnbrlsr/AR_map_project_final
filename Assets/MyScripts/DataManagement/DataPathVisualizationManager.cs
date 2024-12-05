using System.Collections.Generic;
using Mapbox.Unity.Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/*
    Manages the visualization of the path data from the database
    Uses the DatabaseManager to access the Mobis data from the database
    Creates DatabaseLegData object and passes them to the path classes which then visualize the data
*/
public class DataPathVisualizationManager : MonoBehaviour
{   
    private DatabaseManager _databaseManager;
    private InputEventTypes inputEvents;

    // Visualization parameters
    [SerializeField] GameObject startPointPrefab;
    [SerializeField] GameObject midPointPrefab;
    [SerializeField] GameObject endPointPrefab;
    [SerializeField] GameObject linePrefab;
    [SerializeField] GameObject arcPrefab;
    [SerializeField] GameObject informationPanelPrefab;
    [SerializeField] AbstractMap abstractMap;
    [SerializeField] GameObject spawnPointButtonGO;
    [SerializeField] GameObject spawnPathButtonGO;
    [SerializeField] GameObject spawn10PathsButtonGO;
    [SerializeField] GameObject spawn50PathsButtonGO;
    [SerializeField] TMP_Text infoText1;
    [SerializeField] TMP_Text infoText2;
    [SerializeField] TMP_Dropdown lineStyleDropdown;

    public int nof_paths;
    public enum PathType{
        Line,
        Arc
    }
    private PathType pathType;

    // Leg visualization
    List<ITwoPointVisualization> twoPointPathList;
    private int spawnedPaths;

    void Start()
    {   
        Debug.Log("Started DataPathVisualizationManager");

        inputEvents = InputEventsInvoker.InputEventTypes;
        inputEvents.HandSingleIPinchStart += OnInputStart;

        DatabaseLegData.SetMap(abstractMap);
        _databaseManager = new DatabaseManager();
        _databaseManager.Initialize(nof_paths);

        spawnedPaths = 0;
        twoPointPathList = new List<ITwoPointVisualization>();

        infoText1.SetText("Paths in database:\n" + _databaseManager.GetNofPaths());
        infoText2.SetText("Paths spawned\n0");

        abstractMap.OnUpdated += OnMapUpdated;
        lineStyleDropdown.onValueChanged.AddListener(OnLineStyleChanged);
    }

    void OnLineStyleChanged(int index)
    {
        Debug.Log("Dropdown value: " + lineStyleDropdown.options[index].text);
        string lineStyle = lineStyleDropdown.options[index].text;
        if(lineStyle.Equals("Straight Line"))
        {
            pathType = PathType.Line;
        }
        else if(lineStyle.Equals("Arc Line"))
        {
            pathType = PathType.Arc;
        }
    }

    void OnMapUpdated()
    {   
        if(twoPointPathList != null)
        {   
            foreach(ITwoPointVisualization p in twoPointPathList)
            {
                p.UpdateVisualization();
            } 
        }
    }

    void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        if(targetObj.transform.IsChildOf(spawnPointButtonGO.transform))
        {
            OnSpawnPointButtonPressed();
        }
        else if(targetObj.transform.IsChildOf(spawnPathButtonGO.transform))
        {
            OnSpawnPathButtonPressed();
        }
        else if(targetObj.transform.IsChildOf(spawn10PathsButtonGO.transform))
        {
            for(int i = 0; i < 10; i++)
            {
                if(!OnSpawnPathButtonPressed()) break;
            }
            
        }
        else if(targetObj.transform.IsChildOf(spawn50PathsButtonGO.transform))
        {
            for(int i = 0; i < 50; i++)
            {
                if(!OnSpawnPathButtonPressed()) break;
            }
        }
    }

    private void OnSpawnPointButtonPressed()
    {
        DebugPanel.Log("Spawn point functionality is not implemented...");
    }

    private bool OnSpawnPathButtonPressed()
    {   
        if(_databaseManager.HasMorePaths())
        {   
            DatabaseLegData leg = _databaseManager.GetNextLeg();
            switch (pathType)
            {
                case PathType.Line:
                    TwoPointLineVisualizer linePath = new TwoPointLineVisualizer(leg, startPointPrefab, endPointPrefab, linePrefab, informationPanelPrefab, abstractMap);
                    linePath.InstantiatePath();
                    linePath.UpdateVisualization();
                    twoPointPathList.Add(linePath);
                    spawnedPaths += 1;
                    DebugPanel.Log(" > Spawned line #" + spawnedPaths + ", from: " + leg.startXY + ", to: " + leg.endXY);
                    break;
                case PathType.Arc:
                    TwoPointArcVisualizer arcPath = new TwoPointArcVisualizer(leg, startPointPrefab, endPointPrefab, arcPrefab, informationPanelPrefab, abstractMap);
                    arcPath.InstantiatePath();
                    arcPath.UpdateVisualization();
                    twoPointPathList.Add(arcPath);
                    spawnedPaths += 1;
                    DebugPanel.Log(" > Spawned arc #" + spawnedPaths + ", from: " + leg.startXY + ", to: " + leg.endXY);
                    break;
                default:
                    Debug.LogError("Path type not selected...");
                    break;
            }
            infoText1.SetText("Paths in databse:\n" + _databaseManager.GetNofPaths());
            infoText2.SetText("Paths spawned:\n" + spawnedPaths);
            return true;
        }
        else{
            DebugPanel.Log("No more paths to spawn...");
            infoText1.SetText("Paths in databse:\n" + _databaseManager.GetNofPaths());
            infoText2.SetText("Paths spawned:\n" + spawnedPaths);
            return false;
        }
        
    }

}

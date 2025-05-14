using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;

public class rm_DataVisualizationManager_old : MonoBehaviour
{   
    /*[SerializeField] AbstractMap _map;
    [SerializeField] GameObject spawnPointButtonGO;
    [SerializeField] GameObject spawnPathButtonGO;
    [SerializeField] GameObject datapointPrefab;
    [SerializeField] GameObject startPointPrefab;
    [SerializeField] GameObject midPointPrefab;
    [SerializeField] GameObject endPointPrefab;
    [SerializeField] GameObject linePrefab;
    [SerializeField] float spawnScale;

    private InputEventTypes inputEvents;

    // Single points
    private List<CoordinatePoint> spawnedPoints;
    private List<Vector2d> locationCoordinates;

    // Paths with start, mid and end points
    private List<PointPath> spawnedPaths;
    private List<Vector2d[]> pathCoordinates;

    void Start()
    {
        inputEvents = InputEventsInvoker.InputEventTypes;
        if(inputEvents != null)
        {
            inputEvents.HandSingleIPinchStart += OnInputStart;
        }

        DataContainer.ReadCoordinateStrings();
        spawnedPoints = new List<CoordinatePoint>();

        //DataContainer.ReadRandomPaths();
        //DataContainer.ReadDatabasePaths();
        DataContainer.ReadDatabasePathsStatic();
        spawnedPaths = new List<PointPath>();
    }

    void Update()
    {
        for(int i = 0; i < spawnedPoints.Count; i++)
        {
            spawnedPoints[i].UpdatePoint();
        }
        for(int i = 0; i < spawnedPaths.Count; i++)
        {
            spawnedPaths[i].UpdatePath();
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
    }

    void OnSpawnPointButtonPressed()
    {
        try
        {
            Vector2d coordinates = DataContainer.GetNextPoint();
            CoordinatePoint point = new CoordinatePoint(_map, coordinates, new Vector3(spawnScale, spawnScale, spawnScale), datapointPrefab);
            GameObject instance = Instantiate(datapointPrefab);
            point.InstantiatePoint(instance);
            point.UpdatePoint();
            spawnedPoints.Add(point);

            DebugPanel.Log("Spawned point at " + coordinates);
        }
        catch (IndexOutOfRangeException)
        {
            DebugPanel.Log("No more data points to spawn");
        }
        
        Debug.Log("[DataSpawnManager] PointSpawnButton pressed");
    }

    void OnSpawnPathButtonPressed()
    {
        try
        {
            Vector2d[] pathCoordinates = DataContainer.GetNextPath();
            CoordinatePoint startPoint = new CoordinatePoint(_map, pathCoordinates[0], new Vector3(spawnScale, spawnScale, spawnScale), startPointPrefab);
            CoordinatePoint midPoint = new CoordinatePoint(_map, pathCoordinates[1], new Vector3(spawnScale, spawnScale, spawnScale), midPointPrefab);
            CoordinatePoint endPoint = new CoordinatePoint(_map, pathCoordinates[2], new Vector3(spawnScale, spawnScale, spawnScale), endPointPrefab);
            StartEndLine_old line1 = new StartEndLine(_map, startPoint, midPoint, Vector3.one, linePrefab);
            StartEndLine_old line2 = new StartEndLine(_map, midPoint, endPoint, Vector3.one, linePrefab);

            PointPath path = new PointPath(_map, startPoint, midPoint, endPoint, line1, line2);

            GameObject startPointInstance = Instantiate(startPointPrefab);
            GameObject midPointInstance = Instantiate(midPointPrefab);
            GameObject endPointInstance = Instantiate(endPointPrefab);
            GameObject line1Instance = Instantiate(linePrefab);
            GameObject line2Instance = Instantiate(linePrefab);
            path.InstantiatePath(startPointInstance, midPointInstance, endPointInstance, line1Instance, line2Instance);
            //path.SetHeight(spawnedPaths.Count * 0.05f);
            path.SetHeight(0.0f);
            path.UpdatePath();
            spawnedPaths.Add(path);

            DebugPanel.Log("Spawned path #" + spawnedPaths.Count);
        }
        catch (IndexOutOfRangeException)
        {
            DebugPanel.Log("No more data paths to spawn");
        }
        
        Debug.Log("[DataSpawnManager] PathSpawnButton pressed");
    }*/

}

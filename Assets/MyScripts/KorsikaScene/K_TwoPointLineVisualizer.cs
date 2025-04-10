using System;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine;

/*
    Creates a line between a start and end point
    The height of the respective end points represent the timestamp of the data
*/
public class K_TwoPointLineVisualizer : ITwoPointVisualization
{
    // Data reference
    private K_DatabaseLegData _leg;
    

    // Visualization parameters
    private GameObject linePrefab;
    private AbstractMap _map;
    private Transform mapRoot;
    public static Transform globalMapRoot;

    // Instances
    private CustomPoint startCustomPoint;
    private CustomPoint endCustomPoint;
    private FadeLine fadeLine;
    private bool isIntantiated;

    // Interaction parameters
    private bool isSelected;
    private GameObject lineInformationPanelPrefab;
    private K_PathInformationPanel informationPanel;


    public void SetLegData(ILegData leg)
    {
        if((Type) leg != typeof(K_DatabaseLegData))
        {
            Debug.LogError("MY ERROR: Parameter leg has the wrong type!");
            return;
        }
        this._leg = (K_DatabaseLegData) leg;
    }

    public K_TwoPointLineVisualizer(K_DatabaseLegData leg, GameObject linePrefab, GameObject lineInformationPanelPrefab, AbstractMap map, Transform mapRoot)
    {
        this._leg = leg;
        this.linePrefab = linePrefab;
        this.lineInformationPanelPrefab = lineInformationPanelPrefab;
        this._map = map;
        this.mapRoot = mapRoot;
        isIntantiated = false;
        isSelected = false;
#if UNITY_EDITOR
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;
#else
        InputEventsInvoker.InputEventTypes.HandSingleDPinchStart += OnInputStart;
#endif
        K_DataPathVisualizationManager.GlobalNewWindowSpawnedEvent += SetSelectedFalse;
    }

    public void InstantiatePath()
    {   
        if(linePrefab == null)
        {
            throw new Exception("[TwoPointLineVisualizer] Line prefab is null");
        }

        GameObject lineInstance = GameObject.Instantiate(linePrefab);
        fadeLine = new FadeLine(_leg.id, lineInstance, _leg.worldStartPoint, _leg.worldEndPoint);
        Vector2d origin = new Vector2d(_leg.origin_lat, _leg.origin_lon);
        Vector2d dest = new Vector2d(_leg.dest_lat, _leg.dest_lon);
        fadeLine.Update(_map.GeoToWorldPosition(origin, true), _map.GeoToWorldPosition(dest, true));

        informationPanel = new K_PathInformationPanel(_leg, lineInformationPanelPrefab);

        isIntantiated = true;
    }

    public void UpdateVisualization()
    {
       if(isIntantiated){ 
            _leg.UpdateWorldCoordinates();
            fadeLine.Update(_leg.worldStartPoint, _leg.worldEndPoint);

            if(informationPanel.Instance != null) informationPanel.Hide();
            isSelected = false;
            fadeLine.SetSelected(isSelected);
        }
    }

    private void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {
        if(targetObj == fadeLine.Instance)
        {
            isSelected = !isSelected;
            if(isSelected)
            {
                SetSelectedTrue(interactionPos);
            }
            else
            {
                SetSelectedFalse();
            }
        }
    }

    private void SetSelectedTrue(Vector3 interactionPos)
    {   
        K_DataPathVisualizationManager.InvokeGlobalNewWindowSpawnedEvent();

        isSelected = true;

        fadeLine.SetSelected(isSelected);
        informationPanel.Show(interactionPos, CustomHeadTracking.GetHeadPosition());
    }

    private void SetSelectedFalse()
    {
        isSelected = false;

        fadeLine.SetSelected(isSelected);
        informationPanel.Hide();
    }

    public void DestroyPath()
    {
        fadeLine.Destroy();
        if(informationPanel.Instance != null) informationPanel.Hide();
    }

}
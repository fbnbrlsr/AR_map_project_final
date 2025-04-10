using System;
using Mapbox.Unity.Map;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

/*
    Creates an arc between a start and end point
    The height of the respective end points represent the timestamp of the data
*/

public class TwoPointArcVisualizer : ITwoPointVisualization
{
    private InputEventTypes inputEvents;

    // Data reference
    private DatabaseLegData _leg;

    // Visualization parameters
    private GameObject startPointPrefab;
    private GameObject endPointPrefab;
    private GameObject arcPrefab;
    private AbstractMap _map;
    private float arcHeight = 1f;
    private float pointScale = 1f;
    private float lineScale = 1f;

    // Instances
    private CustomPoint startCustomPoint;
    private CustomPoint endCustomPoint;
    private SplineArcLine arcLine;
    private bool isIntantiated;

    // Interaction parameters
    private bool isSelected;
    private GameObject informationPanelPrefab;
    private PathInformationPopup informationPanel;

    public void SetLegData(ILegData leg)
    {   
        if((Type) leg != typeof(DatabaseLegData))
        {
            Debug.LogError("MY ERROR: Parameter leg has the wrong type!");
            return;
        }
        this._leg = (DatabaseLegData) leg;
    }

    public TwoPointArcVisualizer(DatabaseLegData leg, GameObject startPointPrefab, GameObject endPointPrefab, 
        GameObject arcPrefab, GameObject informationPanelPrefab, AbstractMap map)
    {
        this._leg = leg;
        this.startPointPrefab = startPointPrefab;
        this.endPointPrefab = endPointPrefab;
        this.arcPrefab = arcPrefab;
        this.informationPanelPrefab = informationPanelPrefab;
        this._map = map;
        isIntantiated = false;
        isSelected = false;

        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;
    }

    public void InstantiatePath()
    {   
        if(startPointPrefab == null || endPointPrefab == null || arcPrefab == null)
        {
            throw new Exception("[TwoPointArcVisualizer] Some prefab is null");
        }
        GameObject startPointInstance = GameObject.Instantiate(startPointPrefab);
        startCustomPoint = new CustomPoint(startPointInstance, _leg.worldStartPoint);

        GameObject endPointInstance = GameObject.Instantiate(endPointPrefab);
        endCustomPoint = new CustomPoint(endPointInstance, _leg.worldEndPoint);

        GameObject arcInstance = GameObject.Instantiate(arcPrefab);
        arcLine = new SplineArcLine(arcInstance, _leg.worldStartPoint, _leg.worldEndPoint);
        arcLine.SetArcHeight(_leg.CalculateArcHeight());
        
        informationPanel = new PathInformationPopup(_leg, informationPanelPrefab);

        isIntantiated = true;
    }

    public void UpdateVisualization()
    {
       if(isIntantiated){ 
            _leg.UpdateWorldCoordinates();
            startCustomPoint.Update(_leg.worldStartPoint);
            endCustomPoint.Update(_leg.worldEndPoint);

            arcLine.SetMapZoom(_map.Zoom);
            arcLine.Update(_leg.worldStartPoint, _leg.worldEndPoint);

            if(informationPanel.Instance != null) informationPanel.Hide();
            isSelected = false;
        }
    }

    private void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {
        if(targetObj == startCustomPoint.Instance || targetObj == endCustomPoint.Instance || targetObj == arcLine.Instance)
        {   
            isSelected = !isSelected;
            startCustomPoint.SetSelected(isSelected);
            endCustomPoint.SetSelected(isSelected);
            arcLine.SetSelected(isSelected);

            informationPanel.Show(interactionPos, CustomHeadTracking.GetHeadPosition());
        }
    }

    public void DestroyPath()
    {
        startCustomPoint.Destroy();
        endCustomPoint.Destroy();
        arcLine.Destroy();
    }

}

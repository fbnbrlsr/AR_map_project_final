using System;
using Mapbox.Unity.Map;
using UnityEngine;

/*
    Creates a line between a start and end point
    The height of the respective end points represent the timestamp of the data
*/
public class TwoPointLineVisualizer : ITwoPointVisualization
{
    private InputEventTypes inputEvents;

    // Data reference
    private DatabaseLegData _leg;
    

    // Visualization parameters
    private GameObject startPointPrefab;
    private GameObject endPointPrefab;
    private GameObject linePrefab;
    private AbstractMap _map;
    private float pointScale = 1f;
    private float lineScale = 1f;

    // Instances
    private CustomPoint startCustomPoint;
    private CustomPoint endCustomPoint;
    private FadeLine fadeLine;
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

    public TwoPointLineVisualizer(DatabaseLegData leg, GameObject startPointPrefab, GameObject endPointPrefab, 
        GameObject linePrefab, GameObject informationPanelPrefab, AbstractMap map)
    {
        this._leg = leg;
        this.startPointPrefab = startPointPrefab;
        this.endPointPrefab = endPointPrefab;
        this.linePrefab = linePrefab;
        this.informationPanelPrefab = informationPanelPrefab;
        this._map = map;
        isIntantiated = false;
        isSelected = false;

        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;
    }

    public void InstantiatePath()
    {   
        if(startPointPrefab == null || endPointPrefab == null || linePrefab == null)
        {
            throw new Exception("[TwoPointLineVisualizer] Some prefab is null");
        }
        GameObject startPointInstance = GameObject.Instantiate(startPointPrefab);
        startCustomPoint = new CustomPoint(startPointInstance, _leg.worldStartPoint);

        GameObject endPointInstance = GameObject.Instantiate(endPointPrefab);
        endCustomPoint = new CustomPoint(endPointInstance, _leg.worldEndPoint);

        GameObject lineInstance = GameObject.Instantiate(linePrefab);
        fadeLine = new FadeLine(_leg.id, lineInstance, _leg.worldStartPoint, _leg.worldEndPoint);

        informationPanel = new PathInformationPopup(_leg, informationPanelPrefab);

        isIntantiated = true;
    }

    public void UpdateVisualization()
    {
       if(isIntantiated){ 
            _leg.UpdateWorldCoordinates();
            startCustomPoint.Update(_leg.worldStartPoint);
            endCustomPoint.Update(_leg.worldEndPoint);
            fadeLine.Update(_leg.worldStartPoint, _leg.worldEndPoint);

            if(informationPanel.Instance != null) informationPanel.Hide();
            isSelected = false;
        }
    }

    private void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        if(targetObj == startCustomPoint.Instance || targetObj == endCustomPoint.Instance || targetObj == fadeLine.Instance)
        {
            isSelected = !isSelected;
            startCustomPoint.SetSelected(isSelected);
            endCustomPoint.SetSelected(isSelected);
            fadeLine.SetSelected(isSelected);

            informationPanel.Show(interactionPos, CustomHeadTracking.GetHeadPosition());
        }
    }

    public void DestroyPath()
    {
        startCustomPoint.Destroy();
        endCustomPoint.Destroy();
        fadeLine.Destroy();
    }

}

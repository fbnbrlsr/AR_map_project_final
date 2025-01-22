using System;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Npgsql.Replication.PgOutput.Messages;
using NUnit.Framework;
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
    private GameObject startPointPrefab;
    private GameObject endPointPrefab;
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

    public K_TwoPointLineVisualizer(K_DatabaseLegData leg, GameObject startPointPrefab, GameObject endPointPrefab, 
        GameObject linePrefab, GameObject lineInformationPanelPrefab, AbstractMap map, Transform mapRoot)
    {
        this._leg = leg;
        this.startPointPrefab = startPointPrefab;
        this.endPointPrefab = endPointPrefab;
        this.linePrefab = linePrefab;
        this.lineInformationPanelPrefab = lineInformationPanelPrefab;
        this._map = map;
        this.mapRoot = mapRoot;
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
        /*GameObject startPointInstance = GameObject.Instantiate(startPointPrefab);
        startCustomPoint = new CustomPoint(startPointInstance, _leg.worldStartPoint);*/

        /*GameObject endPointInstance = GameObject.Instantiate(endPointPrefab);
        endCustomPoint = new CustomPoint(endPointInstance, _leg.worldEndPoint);*/

        GameObject lineInstance = GameObject.Instantiate(linePrefab);
        //lineInstance.transform.SetParent(mapRoot);
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
            //startCustomPoint.Update(_leg.worldStartPoint);
            //endCustomPoint.Update(_leg.worldEndPoint);
            fadeLine.Update(_leg.worldStartPoint, _leg.worldEndPoint);

            if(informationPanel.Instance != null) informationPanel.Hide();
            isSelected = false;
            fadeLine.SetSelected(isSelected);
        }
    }

    private void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        if(targetObj == fadeLine.Instance)
        //if(targetObj == startCustomPoint.Instance || targetObj == endCustomPoint.Instance || targetObj == fadeLine.Instance)
        {
            isSelected = !isSelected;
            //startCustomPoint.SetSelected(isSelected);
            //endCustomPoint.SetSelected(isSelected);
            fadeLine.SetSelected(isSelected);

            if(isSelected) informationPanel.Show(interactionPos, CustomHeadTracking.GetHeadPosition());
            else informationPanel.Hide();
        }
    }

    public void DestroyPath()
    {
        //startCustomPoint.Destroy();
        //endCustomPoint.Destroy();
        fadeLine.Destroy();
        if(informationPanel.Instance != null) informationPanel.Hide();
    }

}
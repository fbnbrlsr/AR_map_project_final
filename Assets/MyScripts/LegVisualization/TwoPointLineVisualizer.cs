using System;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine;

public class TwoPointLineVisualizer
{

    /*
    *   This class manages the visualization of a single leg as a line.
    *   It updates the transform of the line if the map has been changed.
    *   A line can be selected upon which it is highlighted with a distinct
    *   material. The information window about this leg is displayed.
    */

    // Data reference
    private DatabaseLegData legData;
    
    // Visualization parameters
    private GameObject linePrefab;
    private AbstractMap map;
    public static Transform globalMapRoot;

    // Instances
    private LegLine legLine;
    private bool isIntantiated;

    // Interaction parameters
    private bool isSelected;
    private GameObject lineInformationPanelPrefab;
    private PathInformationPanel informationPanel;

    public TwoPointLineVisualizer(DatabaseLegData leg, GameObject linePrefab, GameObject lineInformationPanelPrefab, AbstractMap map)
    {
        this.legData = leg;
        this.linePrefab = linePrefab;
        this.lineInformationPanelPrefab = lineInformationPanelPrefab;
        this.map = map;
        isIntantiated = false;
        isSelected = false;
#if UNITY_EDITOR
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;
#else
        InputEventsInvoker.InputEventTypes.HandSingleDPinchStart += OnInputStart;
#endif
        DataPathVisualizationManager.GlobalNewWindowSpawnedEvent += SetSelectedFalse;
    }

    public void InstantiatePath()
    {   
        if(linePrefab == null)
        {
            throw new Exception("[TwoPointLineVisualizer] Line prefab is null");
        }

        GameObject lineInstance = GameObject.Instantiate(linePrefab);
        legLine = new LegLine(legData.id, lineInstance, legData.worldStartPoint, legData.worldEndPoint);
        Vector2d origin = new Vector2d(legData.origin_lat, legData.origin_lon);
        Vector2d dest = new Vector2d(legData.dest_lat, legData.dest_lon);
        legLine.Update(map.GeoToWorldPosition(origin, true), map.GeoToWorldPosition(dest, true));

        informationPanel = new PathInformationPanel(legData, lineInformationPanelPrefab);

        isIntantiated = true;
    }

    public void UpdateVisualization()
    {
       if(isIntantiated){ 
            legData.UpdateWorldCoordinates();
            legLine.Update(legData.worldStartPoint, legData.worldEndPoint);

            if(informationPanel.Instance != null) informationPanel.Hide();
            isSelected = false;
            legLine.SetSelected(isSelected);
        }
    }

    private void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {
        if(targetObj == legLine.instance)
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
        DataPathVisualizationManager.InvokeGlobalNewWindowSpawnedEvent();

        isSelected = true;

        legLine?.SetSelected(isSelected);
        informationPanel.Show(interactionPos, CustomHeadTracking.GetHeadPosition());
    }

    private void SetSelectedFalse()
    {
        isSelected = false;

        legLine?.SetSelected(isSelected);
        informationPanel.Hide();
    }

    public void DestroyPath()
    {
        legLine?.Destroy();
        if(informationPanel.Instance != null) informationPanel.Hide();
    }

}
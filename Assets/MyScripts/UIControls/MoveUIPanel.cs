using UnityEngine;
using static Params_PanelStartingPositions;
using UnityEngine.InputSystem.LowLevel;
using Mapbox.Unity.Map;


public class MoveUIPanel : MonoBehaviour
{      

    /*
    *   This script implements the functionality to move the different panels of the
    *   interface to arbitrary positions in the world.
    *   If useVerticalLookAtRotation is true, the panel is always facing the user's head
    *   when being moved around.
    *   The panel always has a pre-defined vertical offset to the map plane, determined 
    *   by the heightOffset value. This prevents the panel from being overlapped by the map.
    */

    public float heightOffset;
    [SerializeField] GameObject mapPanReferenceCube;
    [SerializeField] AbstractMap abstractMap;
    [SerializeField] GameObject UIPanelParentGO;
    [SerializeField] GameObject UIPanelHandle;
    [SerializeField] bool useVerticalLookAtRotation;

    private Transform parentTransform;
    private Vector3 inputStartPos;

    void Start()
    {   
        WorldPositionParameters wpp = Params_PanelStartingPositions.GetWorldPositionParametersByName(UIPanelParentGO.name);
        UIPanelParentGO.transform.position = wpp.position;
        UIPanelParentGO.transform.rotation = wpp.rotation;

        parentTransform = UIPanelParentGO.transform;

        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;
        InputEventsInvoker.InputEventTypes.HandSingleInputCont += OnInputCont;
        abstractMap.OnUpdated += OnMapCubeUpdated;
        DynamicTimePlane.TimePlaneChanged += OnMapCubeUpdated;
    }

    private void OnMapCubeUpdated()
    {   
        SetPosition();
    }

    void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {   
        inputStartPos = interactionPos;
    }

    void OnInputCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion currRot, GameObject targetObj, SpatialPointerKind touchKind)
    {   
        if(targetObj.transform.IsChildOf(UIPanelHandle.transform)){
            Vector3 deltaPosition = interactionPos - inputStartPos;
            parentTransform.position += deltaPosition;
            SetPosition();
            if(useVerticalLookAtRotation)
            {
                parentTransform.rotation = Quaternion.LookRotation(parentTransform.position - CustomHeadTracking.GetHeadPosition());
            }
            else
            {   
                Vector3 mask = new Vector3(1f, 0f, 1f);
                Vector3 pos = Vector3.Scale(parentTransform.position, mask);
                Vector3 target = Vector3.Scale(CustomHeadTracking.GetHeadPosition(), mask);
                parentTransform.rotation = Quaternion.LookRotation(pos - target);
            }
            
            inputStartPos = interactionPos;
        }
    }

    void SetPosition()
    {
        Vector3 panelPosition = UIPanelParentGO.transform.position;
        float zDistance = panelPosition.z - mapPanReferenceCube.transform.position.z;
        float mapPanReferenceCubeY = mapPanReferenceCube.transform.position.y - zDistance * Mathf.Sin(MapTilting.tiltAngleRad);
        float minHeight = mapPanReferenceCubeY + heightOffset - UIPanelParentGO.transform.localScale.y * UIPanelHandle.transform.localPosition.y;

        UIPanelParentGO.transform.position = new Vector3(panelPosition.x, Mathf.Max(panelPosition.y, minHeight), panelPosition.z);
    }

}

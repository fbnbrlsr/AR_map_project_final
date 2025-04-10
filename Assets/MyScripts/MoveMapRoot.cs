using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MoveMapRoot : MonoBehaviour
{      
    [SerializeField] Transform root;
    [SerializeField] GameObject mapRootHandle;
    [SerializeField] AbstractMap _map;

    Vector3 handleInputStartPos;
    GameObject lowerArrow;

    void Start()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;
        InputEventsInvoker.InputEventTypes.HandSingleInputCont += OnInputCont;

        lowerArrow = mapRootHandle.GetNamedChild("LowerArrow");
        if(lowerArrow == null) Debug.LogError("MapRootHanle LowerArrow is null");
    }

    void Update()
    {
        mapRootHandle.transform.rotation = Quaternion.LookRotation(Vector3.left);

        lowerArrow.transform.localPosition = new Vector3(-5*Mathf.Sin(MapTilting.tiltAngleRad), -5*Mathf.Cos(MapTilting.tiltAngleRad), 0f);
    }

    void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {
        if(targetObj.transform.IsChildOf(mapRootHandle.transform))
        {   
            handleInputStartPos = interactionPos;
        }

    }

    void OnInputCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion currRot, GameObject targetObj, SpatialPointerKind touchKind)
    {   
#if UNITY_EDITOR
        if(targetObj.transform.IsChildOf(mapRootHandle.transform))
#else
        if(targetObj.transform.IsChildOf(mapRootHandle.transform) && touchKind == SpatialPointerKind.IndirectPinch)
#endif
        {   
            Vector3 deltaPosition = interactionPos - handleInputStartPos;
            root.position += deltaPosition;
            handleInputStartPos = interactionPos;
            _map.UpdateMap();
        }
    }

}

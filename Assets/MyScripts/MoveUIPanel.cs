using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class MoveUIPanel : MonoBehaviour
{
    [SerializeField] GameObject UIPanelParentGO;
    [SerializeField] GameObject UIPanelHandle;
    private Transform parentTransform;
    private InputEventTypes inEvents;
    private Vector3 inputStartPos;

    void Start()
    {
        parentTransform = UIPanelParentGO.transform;
        inEvents = InputEventsInvoker.InputEventTypes;
        if(inEvents != null)
        {
            inEvents.HandSingleIPinchStart += OnInputStart;
            inEvents.HandSingleInputCont += OnInputCont;
        }
    }

    void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {   
        inputStartPos = interactionPos;
    }

    void OnInputCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion currRot, GameObject targetObj)
    {   
        if(targetObj.transform.IsChildOf(UIPanelHandle.transform)){
            Vector3 deltaPosition = interactionPos - inputStartPos;
            parentTransform.position += deltaPosition;
            parentTransform.rotation = Quaternion.LookRotation(parentTransform.position - CustomHeadTracking.GetHeadPosition());
            inputStartPos = interactionPos;
        }
    }

}

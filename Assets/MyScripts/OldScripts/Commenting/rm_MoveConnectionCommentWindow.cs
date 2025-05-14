using System.Collections;
using System.Collections.Generic;
using System.Net;
using Mapbox.Examples;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class rm_MoveConnectionCommentWindow : MonoBehaviour
{
    /*[SerializeField] GameObject windowParentGO;
    [SerializeField] GameObject windowHandle;
    [SerializeField] GameObject connectionLine;
    [SerializeField] GameObject connectionPoint;
    private Transform parentTransform;
    private InputEventTypes inEvents;
    private Vector3 inputStartPos;

    private Vector3 initPos;

    void Start()
    {
        parentTransform = windowParentGO.transform;
        inEvents = InputEventsInvoker.InputEventTypes;
        if(inEvents != null)
        {
            inEvents.HandSingleIPinchStart += OnInputStart;
            inEvents.HandSingleInputCont += OnInputCont;
        }
        initPos = connectionPoint.transform.position;
    }

    void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {   
        inputStartPos = interactionPos;
    }

    void OnInputCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion currRot, GameObject targetObj)
    {   
        if(targetObj.transform.IsChildOf(windowHandle.transform)){
            Vector3 deltaPosition = interactionPos - inputStartPos;
            parentTransform.position += deltaPosition;
            parentTransform.rotation = Quaternion.LookRotation(parentTransform.position - CustomHeadTracking.GetHeadPosition());
            inputStartPos = interactionPos;
        }
    }*/

}

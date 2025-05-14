using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class rm_MagnifyingGlass : MonoBehaviour
{
    /*[SerializeField] GameObject handleGO;
    [SerializeField] Camera magnifyingCamera;
    [SerializeField] GameObject zoomOutButton;
    [SerializeField] GameObject zoomInButton;

    Vector3 handleInputStartPos;
    float zoomFactor = 2f;

    void Start()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;
        InputEventsInvoker.InputEventTypes.HandSingleInputCont += OnInputCont;
    }

    void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {

        if(targetObj.transform.IsChildOf(zoomOutButton.transform))
        {
            OnZoomOutButtonPressed();
        }

        if(targetObj.transform.IsChildOf(zoomInButton.transform))
        {   
            OnZoomInButtonPressed();
        }

        if(targetObj.transform.IsChildOf(handleGO.transform))
        {   
            handleInputStartPos = interactionPos;
        }

    }

    void OnInputCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion currRot, GameObject targetObj, SpatialPointerKind touchKind)
    {   
        if(targetObj.transform.IsChildOf(handleGO.transform))
        {   
            Vector3 deltaPosition = interactionPos - handleInputStartPos;
            handleGO.transform.parent.position += deltaPosition;
            handleGO.transform.parent.rotation = Quaternion.LookRotation(handleGO.transform.parent.position - CustomHeadTracking.GetHeadPosition());
            handleInputStartPos = interactionPos;
        }
    }

    void OnZoomOutButtonPressed()
    {   
        magnifyingCamera.fieldOfView += zoomFactor;
    }

    void OnZoomInButtonPressed()
    {   
        magnifyingCamera.fieldOfView -= zoomFactor;
    }*/
    
}

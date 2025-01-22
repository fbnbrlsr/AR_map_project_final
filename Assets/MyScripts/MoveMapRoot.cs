using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using UnityEngine;

public class MoveMapRoot : MonoBehaviour
{      
    [SerializeField] Transform root;
    [SerializeField] GameObject handleGO;
    [SerializeField] AbstractMap _map;

    Vector3 handleInputStartPos;

    void Start()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;
        InputEventsInvoker.InputEventTypes.HandSingleInputCont += OnInputCont;
    }

    void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        if(targetObj.transform.IsChildOf(handleGO.transform))
        {   
            handleInputStartPos = interactionPos;
        }

    }

    void OnInputCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion currRot, GameObject targetObj)
    {   
        if(targetObj.transform.IsChildOf(handleGO.transform))
        {   
            Vector3 deltaPosition = interactionPos - handleInputStartPos;
            root.position += deltaPosition;
            handleInputStartPos = interactionPos;
            _map.UpdateMap();
        }
    }

}

using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class TurnGearWheels : MonoBehaviour
{   

    /*
    *   This script enables the user to tilt the map using a GameObject in the 
    *   interface that is shaped like a gear wheel.
    *   The rotation can only be performed around one axis.
    */

    [SerializeField] GameObject mapRoot;
    [SerializeField] GameObject gearWheelsParent;
    [SerializeField] MapTilting mapTilting;

    Vector3 inputStartPos;

    void Start()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;
        InputEventsInvoker.InputEventTypes.HandSingleInputCont += OnInputCont;

        gearWheelsParent.transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }

    void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {   
        inputStartPos = interactionPos;
    }

    void OnInputCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion currRot, GameObject targetObj, SpatialPointerKind touchKind)
    {   
        if(targetObj.transform.IsChildOf(gearWheelsParent.transform)){

            float initAngle = mapRoot.transform.eulerAngles.x;

            float deltaY = (interactionPos.y - inputStartPos.y) + (interactionPos.z - inputStartPos.z);
            mapTilting.SetAngle(initAngle + deltaY * 5);

            inputStartPos = interactionPos;
        }
    }

}

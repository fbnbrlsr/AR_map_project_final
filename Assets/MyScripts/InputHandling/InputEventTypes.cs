using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

// Only one hand makes an input
public delegate void SingleInput(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind);

// Two hands make input
public delegate void DoubleInput(Vector3 startPos1, Quaternion startRot1, Vector3 startPos2, Quaternion startRot2, GameObject targetObj);

// Any input is made
public delegate void AnyInput();

// Input finished
public delegate void InputFinished();

public class InputEventTypes
{   

    /*
    *   This class contains the various different types of input events. Other scripts can subscribe to
    *   these events and the respective functions will be called upon trigger.
    *   The user can use either one or two hands, both with either single or continuous interaction.
    *   Input event are detected and triggered by InputEventsInvoker.
    */

    // Different input events
    public event SingleInput HandSingleTouchStart;
    public event SingleInput HandSingleDPinchStart;
    public event SingleInput HandSingleIPinchStart;
    public event SingleInput HandSingleInputCont;
    public event DoubleInput HandDoubleInputStart;
    public event DoubleInput HandDoubleInputCont;
    public event AnyInput AnyInput;
    public event InputFinished InputFinished;

    public void InvokeHandSingleTouchStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {
        this.HandSingleTouchStart?.Invoke(fingerPos, interactionPos, initRot, targetObj, touchKind);
    }

    public void InvokeHandSingleDPinchStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {
        this.HandSingleDPinchStart?.Invoke(fingerPos, interactionPos, initRot, targetObj, touchKind);
    }

    public void InvokeHandSingleIPinchStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {
        this.HandSingleIPinchStart?.Invoke(fingerPos, interactionPos, initRot, targetObj, touchKind);
    }

    public void InvokeHandSingleInputCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {
        this.HandSingleInputCont?.Invoke(fingerPos, interactionPos, initRot, targetObj, touchKind);
    }

    public void InvokeHandDoubleInputStart(Vector3 pos0, Quaternion rot0, Vector3 pos1, Quaternion rot1, GameObject targetObj)
    {
        this.HandDoubleInputStart?.Invoke(pos0, rot0, pos1, rot1, targetObj);
    }
    public void InvokeHandDoubleInputCont(Vector3 pos0, Quaternion rot0, Vector3 pos1, Quaternion rot1, GameObject targetObj)
    {
        this.HandDoubleInputCont?.Invoke(pos0, rot0, pos1, rot1, targetObj);
    }

    public void InvokeAnyInput()
    {
        this.AnyInput?.Invoke();
    }

    public void InvokeInputFinished()
    {
        this.InputFinished?.Invoke();
    }


}

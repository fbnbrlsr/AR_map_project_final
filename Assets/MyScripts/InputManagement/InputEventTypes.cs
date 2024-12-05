using UnityEngine;
using Unity.PolySpatial.InputDevices;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using UnityEngine.InputSystem.LowLevel;

// Only one hand makes an input
public delegate void SingleInput(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj);

// Two hands make input
public delegate void DoubleInput(Vector3 startPos1, Quaternion startRot1, Vector3 startPos2, Quaternion startRot2, GameObject targetObj);

// Any input is made
public delegate void AnyInput();

// Input finished
public delegate void InputFinished();

public class InputEventTypes
{
    public event SingleInput HandSingleTouchStart;
    public event SingleInput HandSingleDPinchStart;
    public event SingleInput HandSingleIPinchStart;
    public event SingleInput HandSingleInputCont;
    public event DoubleInput HandDoubleInputStart;
    public event DoubleInput HandDoubleInputCont;
    public event AnyInput AnyInput;
    public event InputFinished InputFinished;

    public event SingleInput HandSingleIPinchStartDrawBox;
    public event SingleInput HandSingleInputContDrawBox;

    public InputEventTypes()
    {
        //Debug.Log("InputEventTypes object created...");
    }

    public void InvokeHandSingleTouchStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        this.HandSingleTouchStart?.Invoke(fingerPos, interactionPos, initRot, targetObj);
    }

    public void InvokeHandSingleDPinchStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        this.HandSingleDPinchStart?.Invoke(fingerPos, interactionPos, initRot, targetObj);
    }

    public void InvokeHandSingleIPinchStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        this.HandSingleIPinchStart?.Invoke(fingerPos, interactionPos, initRot, targetObj);
    }

    public void InvokeHandSingleInputCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        this.HandSingleInputCont?.Invoke(fingerPos, interactionPos, initRot, targetObj);
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
    

    // Draw box events
    public void InvokeHandSingleIPinchStartDrawBox(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        this.HandSingleIPinchStartDrawBox?.Invoke(fingerPos, interactionPos, initRot, targetObj);
    }

    public void InvokeHandSingleInputContDrawBox(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        this.HandSingleInputContDrawBox?.Invoke(fingerPos, interactionPos, initRot, targetObj);
    }


}

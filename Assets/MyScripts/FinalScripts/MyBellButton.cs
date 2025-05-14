using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks.Triggers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class MyBellButton : MonoBehaviour, IMyButton
{   
    public bool makeClickable;
    [SerializeField] Material bellOffMaterial;
    [SerializeField] Material bellOnMaterial;
    [SerializeField] GameObject bellInstance;
    [SerializeField] GameObject ropeInstance;
    bool isButtonOn;

    Vector3 ropeInitPos;
    Vector3 inputStartPos;
    float maxDownPullDistance = -0.25f;
    float triggerDistance = 0.05f;

    public event My3DButtonEvent OnToggleButton;

    void Start()
    {
        isButtonOn = false;
        SetState(isButtonOn);

        if(makeClickable)
        {
            InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += Click_OnHandSingleIPinchStart;
            InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += Pull_OnHandSingleIPinchStart;
            InputEventsInvoker.InputEventTypes.HandSingleInputCont += Pull_OnHandSingleCont;
            InputEventsInvoker.InputEventTypes.InputFinished += Pull_OnInputFinished;
        }
        else
        {
            InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += Pull_OnHandSingleIPinchStart;
            InputEventsInvoker.InputEventTypes.HandSingleInputCont += Pull_OnHandSingleCont;
            InputEventsInvoker.InputEventTypes.InputFinished += Pull_OnInputFinished;
        }

        ropeInitPos = ropeInstance.transform.localPosition;
    }

    private void Click_OnHandSingleIPinchStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {
        if(targetObj.transform.IsChildOf(this.transform))
        {      
            isButtonOn = !isButtonOn;
            SetState(isButtonOn);
        }
    }

    private void Pull_OnHandSingleIPinchStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {
        if(targetObj.transform.IsChildOf(ropeInstance.transform))
        {      
            inputStartPos = interactionPos;
        }
    }

    private void Pull_OnHandSingleCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {   
        if(targetObj.transform.IsChildOf(ropeInstance.transform))
        {   
            Vector3 deltaPosition = interactionPos - inputStartPos;
            float yPos = ropeInstance.transform.localPosition.y + deltaPosition.y;

            yPos = Mathf.Min(yPos, ropeInitPos.y);
            yPos = Mathf.Max(yPos, ropeInitPos.y + maxDownPullDistance);
            ropeInstance.transform.localPosition = new Vector3(ropeInitPos.x, yPos, ropeInitPos.z);

            inputStartPos = interactionPos;
        }
    }

    private void Pull_OnInputFinished()
    {
        if(ropeInitPos.y - ropeInstance.transform.localPosition.y > triggerDistance)
        {
            isButtonOn = !isButtonOn;
            SetState(isButtonOn);
        }
    }

    void Update()
    {   
        if (Vector3.Distance(ropeInstance.transform.localPosition, ropeInitPos) > 0.01f)
        {
            ropeInstance.transform.localPosition = Vector3.Lerp(ropeInstance.transform.localPosition, ropeInitPos, 10f * Time.deltaTime);
        }
    }

    public void SetState(bool b)
    {
        isButtonOn = b;
        try{
            if(isButtonOn)
            {
                bellInstance.GetComponent<MeshRenderer>().material = bellOnMaterial;
                if(SceneManager.GetActiveScene().name.Equals("DummyScene")) return;
                OnToggleButton?.Invoke(isButtonOn);
            }
            else
            {
                bellInstance.GetComponent<MeshRenderer>().material = bellOffMaterial;
                if(SceneManager.GetActiveScene().name.Equals("DummyScene")) return;
                OnToggleButton?.Invoke(isButtonOn);
            }
        }
        catch(Exception e)
        {
            Debug.LogError("MyBellButton Exception (target state: " + b + "): " + e.Message);
        }
    }

}

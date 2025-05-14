using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public delegate void CommentWindowMoved();

public class rm_MoveCommentWindow : MonoBehaviour
{
    /*[SerializeField] GameObject UIPanelParentGO;
    [SerializeField] GameObject UIPanelHandle;
    private Transform parentTransform;
    private InputEventTypes inEvents;
    private Vector3 inputStartPos;

    public CommentWindowMoved OnCommentWindowMoved;

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

    void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {   
        inputStartPos = interactionPos;
    }

    void OnInputCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion currRot, GameObject targetObj, SpatialPointerKind touchKind)
    {   
        if(targetObj.transform.IsChildOf(UIPanelHandle.transform)){
            Vector3 deltaPosition = interactionPos - inputStartPos;
            parentTransform.position += deltaPosition;
            parentTransform.rotation = Quaternion.LookRotation(parentTransform.position - CustomHeadTracking.GetHeadPosition());
            inputStartPos = interactionPos;

            OnCommentWindowMoved?.Invoke();
        }
    }*/

}

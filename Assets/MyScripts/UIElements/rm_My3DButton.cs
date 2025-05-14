using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public delegate void My3DButtonEvent(bool val);

public class rm_My3DButton : MonoBehaviour
{   
    /*[SerializeField] Material offMaterial;
    [SerializeField] Material onMaterial;
    bool isButtonOn;
    MeshRenderer meshRenderer;

    public event My3DButtonEvent OnToggleButton;


    void Start()
    {   
        isButtonOn = false;

        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnHandSingleIPinchStart;

        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = offMaterial;
    }

    private void OnHandSingleIPinchStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {   
        if(targetObj.transform.IsChildOf(transform))
        {   
            isButtonOn = !isButtonOn;
            
            if(isButtonOn) meshRenderer.material = onMaterial;
            else meshRenderer.material = offMaterial;

            OnToggleButton?.Invoke(isButtonOn);
        }
    }

    public void SetState(bool b)
    {   
        if(meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();

        isButtonOn = b;
        if(isButtonOn) meshRenderer.material = onMaterial;
        else meshRenderer.material = offMaterial;
    }*/

}

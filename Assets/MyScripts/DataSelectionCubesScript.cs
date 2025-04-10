using UnityEngine.InputSystem.LowLevel;
using UnityEngine;

public class DataSelectionCubesScript : MonoBehaviour
{   

    [SerializeField] K_LocationStopVisualizationManager locationStopsVisualizationManager;
    [SerializeField] GameObject showPathsCube;
    [SerializeField] GameObject showTransitionalStopsCube;
    [SerializeField] GameObject showActivityStopsCube;

    
    void Start()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnHandSingleIPinchStart;
    }

    private void OnHandSingleIPinchStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {
        if(targetObj == showPathsCube)
        {
            Debug.Log("selected showpathscube");
        }
        else if(targetObj == showTransitionalStopsCube)
        {
            Debug.Log("selected showtransitionalstopscube");
        }
        else if(targetObj == showActivityStopsCube)
        {
            Debug.Log("selected showactivitystopscube");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

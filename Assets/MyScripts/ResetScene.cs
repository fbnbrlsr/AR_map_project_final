using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : MonoBehaviour
{
    [SerializeField] GameObject sceneResetButtonGO;

    private InputEventTypes inEvents;

    void Start()
    {   
        inEvents = InputEventsInvoker.InputEventTypes;
        if(inEvents != null)
        {
            inEvents.HandSingleIPinchStart += OnResetSceneButtonPressed;
        }
    }

    void OnResetSceneButtonPressed(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {   
        if(targetObj.transform.IsChildOf(sceneResetButtonGO.transform))
        {
            DebugPanel.ResetText();
            SceneManager.LoadScene("MapScene");
        }
    }
}

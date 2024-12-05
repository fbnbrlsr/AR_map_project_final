using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;

public class NotificationPopup
{
    
    TMP_Text messageText;
    static GameObject windowPrefab;
    GameObject okButton;
    GameObject windowInstance;

    public NotificationPopup()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInput;

        windowInstance = GameObject.Instantiate(windowPrefab);
        messageText = windowInstance.GetNamedChild("NotificationText").GetComponent<TMP_Text>();
        okButton = windowInstance.GetNamedChild("OkayButtonPrefab");

        var components = windowInstance.GetComponents<TMP_Text>();

        foreach(TMP_Text t in components)
        {
            Debug.Log("Component:" + t);
        }

        windowInstance.SetActive(false);
    }

    public static void SetPrefab(GameObject popupWindowPrefab)
    {
        windowPrefab = popupWindowPrefab;
    }

    public void Show(string text)
    {
        Vector3 headPos = CustomHeadTracking.GetHeadPosition();
        windowInstance.transform.position = headPos + 2 * Vector3.forward;
        windowInstance.transform.rotation = Quaternion.LookRotation(windowInstance.transform.position - headPos);

        messageText.text = text;

        windowInstance.SetActive(true);
    }

    private void OnInput(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {   
        if(targetObj.transform.IsChildOf(okButton.transform))
        {
            OnOKButtonPressed();
        }
    }

    private void OnOKButtonPressed()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart -= OnInput;
        GameObject.Destroy(windowInstance);
    }


}

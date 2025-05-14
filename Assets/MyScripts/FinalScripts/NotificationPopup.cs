using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class NotificationPopup
{
    public static bool isActive;
    TMP_Text messageText;
    static GameObject windowPrefab;
    Button okButton;
    GameObject windowInstance;
    bool initializationSuccesful = false;

    public NotificationPopup()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInput;

        windowInstance = GameObject.Instantiate(windowPrefab);
        messageText = windowInstance.GetNamedChild("NotificationText").GetComponent<TMP_Text>();
        if(messageText == null) return;

        okButton = windowInstance.GetComponentInChildren<Button>();
        if(okButton == null) return;
        okButton.onClick.AddListener(OnOKButtonPressed);

        windowInstance.SetActive(false);
        initializationSuccesful = true;
    }

    public static void SetPrefab(GameObject popupWindowPrefab)
    {
        windowPrefab = popupWindowPrefab;
    }

    public void Show(string text)
    {   
        if(isActive) return;
        
        if(!initializationSuccesful)
        {
            Debug.LogError("[PopupWindow] Cannot open popup window because initialization failed...");
            return;
        }

        isActive = true;

        Vector3 headPos = CustomHeadTracking.GetHeadPosition();
        windowInstance.transform.position = headPos + 2 * (CustomHeadTracking.GetHeadRotation() * Vector3.forward);
        windowInstance.transform.rotation = Quaternion.LookRotation(windowInstance.transform.position - headPos);

        messageText.text = text;

        windowInstance.SetActive(true);
    }

    private void OnInput(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {   
        if(targetObj.transform.IsChildOf(okButton.transform))
        {
            OnOKButtonPressed();
        }
    }

    private void OnOKButtonPressed()
    {   
        isActive = false;
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart -= OnInput;
        GameObject.Destroy(windowInstance);
    }


}

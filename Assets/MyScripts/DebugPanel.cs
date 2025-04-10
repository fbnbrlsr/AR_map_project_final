using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{   

    public TMP_Text tmp;
    private static string debugText;

    // DEBUG INPUT EVENTS
    private InputEventTypes inputEvents;
    private string singleStart;
    private string singleCont;
    private string doubleStart;
    private string doubleCont;
    private static string logText;

    
    void Start()
    {
        debugText = "DEBUG TEXT";

        Debug.Log("DebugPanel started");

        // INPUT DEBUGGING
        inputEvents = InputEventsInvoker.InputEventTypes;
        if(inputEvents != null)
        {
            inputEvents.HandSingleDPinchStart += OnAnySingleInputStart;
            inputEvents.HandSingleTouchStart += OnAnySingleInputStart;
            inputEvents.HandSingleIPinchStart += OnAnySingleInputStart;
            inputEvents.HandSingleInputCont += OnSingleInputCont;
            inputEvents.HandDoubleInputStart += OnAnyDoubleInputStart;
            inputEvents.HandDoubleInputCont += OnDoubleInputCont;

            inputEvents.AnyInput += OnAnyInput;
            //inputEvents.InputFinished += OnInputFinished;
        }
    }

    /*void OnInputFinished()
    {
        singleStart = "SINGLE START: None\n\n\n";
        singleCont = "SINGLE CONT: None\n\n\n";
        doubleStart = "DOUBLE START: None\n\n";
        doubleCont = "DOUBLE CONT: None\n\n";

        debugText += "SINGLE START: None\n\n\nSINGLE CONT: None\n\n\nDOUBLE START: None\n\nDOUBLE CONT: None\n\n-----------------------------------------------------\n" + logText;
        debugText += "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
        tmp.text = debugText;
    }*/

    void Update()
    {   
        // INPUT DEBUGGING
        debugText = "INPUT EVENTS DEBUG:\n\n" + singleStart + singleCont + doubleStart + doubleCont;

        // DEVICE POSITION AND ROTATION DEBUGGING
        debugText += "\nHead position:\n";
        debugText += CustomHeadTracking.GetHeadPosition() + "\n";
        debugText += "Head rotation:\n";
        debugText += CustomHeadTracking.GetHeadRotation() + "\n";
        debugText += "-----------------------------------------------------\n" + logText;
        debugText += "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
        //tmp.SetText(debugText);
        tmp.text = debugText;
    }

    void OnAnyInput()
    {
        debugText = "INPUT EVENTS DEBUG:\n\n" + singleStart + singleCont + doubleStart + doubleCont;
        debugText += "-----------------------------------------------------\n" + logText;
        debugText += "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
        tmp.text = debugText;
    }

    public static void Log(string text)
    {
        logText += "\n" + text;
    }

    // DEBUG INPUT EVENTS
    public void OnAnySingleInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion rot, GameObject targetObj, SpatialPointerKind touchKind)
    {
        singleStart = "SINGLE START: fingerPos=" + fingerPos + ", interactionPos=" + interactionPos + ", rot=" + rot + "\n";
        singleStart += " > targetObj=" + targetObj + "\n";
    }

    public void OnSingleInputCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion rot, GameObject targetObj, SpatialPointerKind touchKind)
    {
        singleCont = "SINGLE CONT: pos=" + fingerPos + ", interactionPos=" + interactionPos + ", rot=" + rot + "\n";
        singleCont += " > targetObj=" + targetObj + "\n";
    }

    public void OnAnyDoubleInputStart(Vector3 pos0, Quaternion rot0, Vector3 pos1, Quaternion rot1, GameObject targetObj)
    {
        doubleStart = "DOUBLE START: pos0=" + pos0 + ", rot0=" + rot0 + ", pos1=" + pos1 + ", rot1=" + rot1 + "\n";
        doubleStart += " -> Distance=" + Vector3.Distance(pos0, pos1) + ", targetObj=" + targetObj + "\n";
    }

    public void OnDoubleInputCont(Vector3 pos0, Quaternion rot0, Vector3 pos1, Quaternion rot1, GameObject targetObj)
    {   
        doubleCont = "DOUBLE CONT: pos0=" + pos0 + ", rot0=" + rot0 + ", pos1=" + pos1 + ", rot1=" + rot1 + "\n";
        doubleCont += " -> Distance=" + Vector3.Distance(pos0, pos1) + ", targetObj=" + targetObj + "\n";
    }

    public static void ResetText()
    {
        logText = "";
        debugText = "DEBUG TEXT";
    }


}

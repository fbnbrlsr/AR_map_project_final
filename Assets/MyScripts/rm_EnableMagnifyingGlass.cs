using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class rm_EnableMagnifyingGlass : MonoBehaviour
{
    
    /*[SerializeField] GameObject objectPrefab;
    [SerializeField] GameObject enableButton;

    bool magnifyingGlassEnabled;
    GameObject objectInstance;


    void Start()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;

        objectInstance = GameObject.Instantiate(objectPrefab);
        magnifyingGlassEnabled = false;
        objectInstance.SetActive(false);
    }

    private void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {
        if(targetObj.transform.IsChildOf(enableButton.transform))
        {
            OnEnableButtonPressed();
        }
    }

    private void OnEnableButtonPressed()
    {   
        magnifyingGlassEnabled = !magnifyingGlassEnabled;
        objectInstance.SetActive(magnifyingGlassEnabled);

        TMP_Text textField = enableButton.GetComponentInChildren<TMP_Text>();
        if(textField != null)
        {
            textField.text = objectInstance.activeSelf ? "Disable Magnfying Glass" : "Enable Magnifying Glass";
        }
    }*/

}

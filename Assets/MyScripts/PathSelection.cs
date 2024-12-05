using System;
using System.Collections.Generic;
using UnityEngine;

public class PathSelection : MonoBehaviour
{
    [SerializeField] Material highlightMaterial;
    [SerializeField] GameObject deselectAllPathsButton;

    private Dictionary<GameObject, Material> selectedPaths;


    void Start()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;

        selectedPaths = new Dictionary<GameObject, Material>();
    }

    private void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {   
        if(targetObj.name.Contains("FadeLine"))
        {   
            OnLineSelected(targetObj);
        }
        if(targetObj.transform.IsChildOf(deselectAllPathsButton.transform))
        {
            OnDeselectAllPathsButtonPressed();
        }
    }

    private void OnDeselectAllPathsButtonPressed()
    {
        foreach(KeyValuePair<GameObject, Material> kvp in selectedPaths)
        {
            kvp.Key.GetComponent<MeshRenderer>().material = kvp.Value;
        }
        selectedPaths = new Dictionary<GameObject, Material>();
    }

    private void OnLineSelected(GameObject obj)
    {   
        Debug.Log("Entered OnLineSelected()");
        string objName = obj.name;

        if(selectedPaths.ContainsKey(obj))
        {   
            Debug.Log("Line already selected");
            Material initMat = selectedPaths[obj];
            obj.GetComponent<MeshRenderer>().material = initMat;
            selectedPaths.Remove(obj);
        }
        else
        {   
            Debug.Log("Line not yet selected");
            Material initMat = obj.GetComponent<MeshRenderer>().material;
            selectedPaths.Add(obj, initMat);
            obj.GetComponent<MeshRenderer>().material = highlightMaterial;
        }
    }
}

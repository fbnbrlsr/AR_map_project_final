using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.LowLevel;

public class PathSelection : MonoBehaviour
{
    [SerializeField] Material highlightMaterial;
    [SerializeField] Button deselectAllPathsButton;

    private Dictionary<GameObject, Material> selectedPaths;


    void Start()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;

        deselectAllPathsButton.onClick.AddListener(OnDeselectAllPathsButtonPressed);

        selectedPaths = new Dictionary<GameObject, Material>();
    }

    private void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {   
        if(targetObj.name.Contains("FadeLine"))
        {   
            OnLineSelected(targetObj);
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
            Material initMat = selectedPaths[obj];
            obj.GetComponent<MeshRenderer>().material = initMat;
            selectedPaths.Remove(obj);
        }
        else
        {   
            Material initMat = obj.GetComponent<MeshRenderer>().material;
            selectedPaths.Add(obj, initMat);
            obj.GetComponent<MeshRenderer>().material = highlightMaterial;
        }
    }
}

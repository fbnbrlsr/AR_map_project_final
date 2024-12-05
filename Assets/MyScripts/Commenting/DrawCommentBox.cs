using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void NewCommentBoxCreated(GameObject commentBox);

public class DrawCommentBox : MonoBehaviour
{
    
    [SerializeField] Button enableDrawBoxButton;
    [SerializeField] GameObject commentBoxPrefab;
    
    public NewCommentBoxCreated OnNewCommentBoxCreated;

    private GameObject currentBoxInstance;
    private Vector3 currentBoxStart;

    void Start()
    {   
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStartDrawBox += OnDrawBoxStart;
        InputEventsInvoker.InputEventTypes.HandSingleInputContDrawBox += OnDrawBoxCont;
        InputEventsInvoker.InputEventTypes.InputFinished += OnInputFinished;
        enableDrawBoxButton.onClick.AddListener(OnEnableDrawBoxButtonClicked);
    }

    private void OnInputFinished()
    {
        if(currentBoxInstance != null) OnNewCommentBoxCreated?.Invoke(currentBoxInstance);
        currentBoxInstance = null;
    }

    private void OnEnableDrawBoxButtonClicked()
    {   
        currentBoxInstance = null;
        currentBoxStart = Vector3.zero;
        InputEventsInvoker.isDrawBoxActive = true;
    }

    private void OnDrawBoxStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
#if UNITY_EDITOR
        currentBoxStart = interactionPos;
#else
        currentBoxStart = fingerPos;
#endif

        currentBoxInstance = GameObject.Instantiate(commentBoxPrefab);
    }

    private void OnDrawBoxCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
#if UNITY_EDITOR
        Vector3 diff = interactionPos - currentBoxStart;
#else
        Vector3 diff = fingerPos - currentBoxStart;
#endif

        if(currentBoxInstance != null)
        {
            currentBoxInstance.transform.position = currentBoxStart + diff / 2;
            currentBoxInstance.transform.localScale = diff;
        }
    }

}

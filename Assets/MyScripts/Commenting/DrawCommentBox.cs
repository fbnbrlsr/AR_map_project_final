using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void NewCommentBoxCreated();

public class DrawCommentBox
{
    
    
    /*static GameObject commentBoxPrefab;
    
    public static NewCommentBoxCreated OnNewCommentBoxCreated;

    public static GameObject currentBoxInstance;
    static Vector3 currentBoxStart;

    public static void Initialize()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStartDrawBox += OnDrawBoxStart;
        InputEventsInvoker.InputEventTypes.HandSingleInputContDrawBox += OnDrawBoxCont;
        InputEventsInvoker.InputEventTypes.InputFinished += OnInputFinished;
    }

    private static void OnInputFinished()
    {   
        if(currentBoxInstance != null)
        {   
            OnNewCommentBoxCreated?.Invoke();
        }
    }

    public static void StartDrawingBox(GameObject commentBoxPrefab)
    {   
        DrawCommentBox.commentBoxPrefab = commentBoxPrefab;
        currentBoxInstance = null;
        currentBoxStart = Vector3.zero;
        InputEventsInvoker.isDrawBoxActive = true;
    }

    private static void OnDrawBoxStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
#if UNITY_EDITOR
        currentBoxStart = interactionPos;
#else
        currentBoxStart = fingerPos;
#endif

        currentBoxInstance = GameObject.Instantiate(commentBoxPrefab);
    }

    private static void OnDrawBoxCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
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
    }*/

}

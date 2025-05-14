using System;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Unity.XR.CoreUtils;
using UnityEngine;

public class rm_CommentBoxCreator
{   
    public static Transform root;

    private AbstractMap _map;
    public GameObject commentBox;
    public GameObject commentWindow;
    private bool showCommentWindow;
    private float windowBoxHeightDiff;
    public Vector2d geoPosition;
    public float commentBoxHeight;
    public readonly Vector3 initScale;
    public readonly float initRefDistance;


    /*public static void StartCommentBoxCreation()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnSingleIPStart;




    }

    private static void OnSingleIPStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        if(targetObj == commentBox){
            showCommentWindow = !showCommentWindow;
            commentWindow.SetActive(showCommentWindow);
        }
    }

    public static GameObject GetCreatedCommentBoxInstance()
    {




        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart -= OnSingleIPStart;
    }*/

}
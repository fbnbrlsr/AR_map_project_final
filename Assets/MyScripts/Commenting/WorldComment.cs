using System;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Unity.XR.CoreUtils;
using UnityEngine;

public class WorldComment
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

    public WorldComment(AbstractMap _map, GameObject commentBox, Vector2d geoPosition, float initRefDistance)
    {
        this._map = _map;
        this.commentBox = commentBox;
        this.geoPosition = geoPosition;
        this.initRefDistance = initRefDistance;
        this.commentBoxHeight = commentBox.transform.localPosition.y;
        initScale = commentBox.transform.localScale;

        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnSingleIPStart;
    }

    private void OnSingleIPStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        if(targetObj == commentBox){
            showCommentWindow = !showCommentWindow;
            commentWindow.SetActive(showCommentWindow);
        }
    }

    public void CreateWriteCommentWindow(GameObject prefab)
    {
        commentWindow = GameObject.Instantiate(prefab);
        commentWindow.transform.parent = root;
        commentWindow.transform.position = commentBox.transform.position + Vector3.up * (commentBox.transform.localScale.y + commentWindow.transform.localScale.y) / 2;
        windowBoxHeightDiff = commentWindow.transform.position.y - commentBox.transform.position.y;
    }

    public void CreateConnectionCommentWindow(GameObject prefab)
    {
        commentWindow = GameObject.Instantiate(prefab);
        commentWindow.transform.parent = root;
        commentWindow.transform.position = commentBox.transform.position + Vector3.up * (commentBox.transform.localScale.y + commentWindow.transform.localScale.y) / 2;
        windowBoxHeightDiff = commentWindow.transform.position.y - commentBox.transform.position.y;
    }

    public void Update()
    {   
        float scalingFactor = CustomReloadMap.GetReferenceDistance() / this.initRefDistance;

        Vector3 prevPos = commentBox.transform.position;
        Vector3 worldPos = _map.GeoToWorldPosition(this.geoPosition);
        this.commentBox.transform.position = worldPos + Vector3.up * this.commentBoxHeight * scalingFactor;
        this.commentBox.transform.localScale = this.initScale * scalingFactor;

        commentWindow.transform.position += commentBox.transform.position - prevPos;
        



        GameObject connectionLine = commentWindow.GetNamedChild("ConnectionLine");
        GameObject connectionPoint = commentWindow.GetNamedChild("ConnectionPoint");
        Vector3 initPos = commentBox.transform.position + Vector3.up * commentBox.transform.localScale.y / 2;
        if(connectionLine != null && connectionPoint != null)
        {   
            Debug.Log("found line");
            Vector3 endPoint = connectionPoint.transform.position;
            float dist = Vector3.Distance(initPos, endPoint) * CustomReloadMap.GetReferenceDistance();
            connectionLine.transform.position = initPos/2 + endPoint/2;
            connectionLine.transform.rotation = Quaternion.LookRotation(initPos - endPoint);
            connectionLine.transform.localScale = new Vector3(0.01f, 0.01f, dist);
        }



        
    }

}
using System.Collections.Generic;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;

public class CommentsManager : MonoBehaviour
{   

    [SerializeField] DrawCommentBox commentBoxScript;
    [SerializeField] GameObject writeCommentWindowPrefab;
    [SerializeField] GameObject connectionCommentWindowPrefab;
    [SerializeField] AbstractMap _map;
    [SerializeField] Transform mapRoot;
    [SerializeField] Transform root;
    
    private List<WorldComment> worldCommentsList;

    void Start()
    {
        _map.OnUpdated += OnMapUpdated;
        commentBoxScript.OnNewCommentBoxCreated += OnNewCommentBoxCreated;

        WorldComment.root = root;
        worldCommentsList = new List<WorldComment>();
    }

    private void OnNewCommentBoxCreated(GameObject commentBox)
    {   
        commentBox.transform.parent = mapRoot;
        Vector2d geoPos = _map.WorldToGeoPosition(commentBox.transform.position);
        WorldComment worldComment = new WorldComment(_map, commentBox, geoPos, CustomReloadMap.GetReferenceDistance());

        worldComment.CreateWriteCommentWindow(writeCommentWindowPrefab);
        //worldComment.CreateConnectionCommentWindow(connectionCommentWindowPrefab);

        worldCommentsList.Add(worldComment);
    }

    private void OnMapUpdated()
    {
        foreach(WorldComment wc in worldCommentsList)
        {
            wc.Update();
        }
    }

}

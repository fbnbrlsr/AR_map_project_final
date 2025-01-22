using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Mapbox.Examples;
using Mapbox.Examples.Voxels;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public delegate void CommentWindowUpdate();

public class CommentsManager : MonoBehaviour
{   
    public static CommentWindowUpdate OnCommentWindowUpdated;

    [SerializeField] GameObject commentBoxPrefab;
    [SerializeField] GameObject writeCommentWindowPrefab;
    [SerializeField] GameObject connectionCommentWindowPrefab;
    [SerializeField] AbstractMap _map;
    [SerializeField] Transform mapRoot;
    [SerializeField] Transform root;
    [SerializeField] List<Material> commentClassMaterials;

    private List<ICommentWindow> existingCommentsList;
    private Dictionary<string, Material> existingCommentClasses;

    private bool commentsVisible;

    void Start()
    {
        _map.OnUpdated += OnMapUpdated;

        existingCommentClasses = new Dictionary<string, Material>
        {
            { "Default", commentClassMaterials[0] }
        };

        WriteCommentWindow.root = root;
        WriteCommentWindow.commentClasses = existingCommentClasses;
        VoiceCommentWindow.root = root;
        VoiceCommentWindow.commentClasses = existingCommentClasses;
        DrawCommentBox.Initialize();

        existingCommentsList = new List<ICommentWindow>();

        commentsVisible = true;
    }

    public void HandleAddCommentClass(string input)
    {   
        int matIndex = WriteCommentWindow.commentClasses.Count;
        Material material = new Material(commentClassMaterials[0]);
        material.color = UnityEngine.Random.ColorHSV();
        if(matIndex < commentClassMaterials.Count) material = commentClassMaterials[matIndex];
        WriteCommentWindow.commentClasses.Add(input, material);

        OnCommentWindowUpdated?.Invoke();
    }



    private void OnMapUpdated()
    {   
        foreach(ICommentWindow commentWindow in existingCommentsList)
        {
            commentWindow.UpdatePosition();
        }
    }


    public void HandleCreateWriteComment(GameObject commentBox)
    {   
        GameObject commentWindow = GameObject.Instantiate(writeCommentWindowPrefab);
        if(commentBox != null)
        {
            commentWindow.transform.position = commentBox.transform.position + Vector3.up * commentBox.transform.localScale.y / 2;
            commentWindow.GetComponent<WriteCommentWindow>().SetCommentBoxInstance(commentBox);
        }
        else
        {
            commentWindow.transform.position = CustomHeadTracking.GetHeadPosition() + 0.5f * Vector3.forward;
        }
        
        existingCommentsList.Add(commentWindow.GetComponent<WriteCommentWindow>());
    }

    public void HandleCreateVoiceComment(GameObject commentBox)
    {   
        Debug.LogError("Create voice comment not implemented!");
        /*GameObject commentWindow = GameObject.Instantiate(voiceCommentWindowPrefab);
        commentWindow.transform.position = CustomHeadTracking.GetHeadPosition() + 0.5f * Vector3.forward;

        // TODO
        spawnedCommentWindows.Add(commentWindow);*/
    }

    public void UpdateAllCommentWindows()
    {
        // TODO (is this even neccessary?)
    }

    public void HandleCreateCommentBox()
    {
        DrawCommentBox.StartDrawingBox(commentBoxPrefab);
        DrawCommentBox.OnNewCommentBoxCreated += OnDrawCommentBoxFinished;
    }

    void OnDrawCommentBoxFinished()
    {   
        DrawCommentBox.OnNewCommentBoxCreated -= OnDrawCommentBoxFinished;
        GameObject commentBoxInstance = DrawCommentBox.currentBoxInstance;

        HandleCreateWriteComment(commentBoxInstance);
    }

    internal void HandleVisibleCommentClassesChanged(List<MultiSelectDropdown.OptionData> optionData)
    {   
        IEnumerable<string> selectedClasses = optionData.Select(od=>od.text);

        foreach(ICommentWindow cw in existingCommentsList)
        {   
            cw.SetActive(false);
            if(selectedClasses.Contains(cw.GetCommentClass()))
            {
                cw.SetActive(true);
            }
        }
    }
}

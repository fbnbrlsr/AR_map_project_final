using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateComments : MonoBehaviour
{
    
    [SerializeField] GameObject voiceCommentWindowPrefab;
    [SerializeField] GameObject writeCommentWindowPrefab;
    [SerializeField] Button createVoiceCommentButton;
    [SerializeField] Button createWriteCommentButton;
    [SerializeField] Button toggleCommentWindowsVisibilityButton;
    [SerializeField] TextMeshProUGUI commentsVisibilityButtonText;

    List<GameObject> spawnedCommentWindows;
    bool commentsVisible;

    void Start()
    {   
        createVoiceCommentButton.onClick.AddListener(OnCreateVoiceCommentButtonClicked);
        createWriteCommentButton.onClick.AddListener(OnCreateWriteCommentButtonClicked);
        toggleCommentWindowsVisibilityButton.onClick.AddListener(OnToggleCommentWindowsVisibilityButtonPressed);
        commentsVisibilityButtonText.text = "Hide Comments";

        spawnedCommentWindows = new List<GameObject>();
        commentsVisible = true;
    }

    private void OnToggleCommentWindowsVisibilityButtonPressed()
    {   
        commentsVisible = !commentsVisible;

        foreach(GameObject g in spawnedCommentWindows)
        {
            g.SetActive(commentsVisible);
        }

        commentsVisibilityButtonText.text = commentsVisible ? "Hide Comments" : "Show Comments";
    }

    private void OnCreateVoiceCommentButtonClicked()
    {   
        GameObject commentWindow = GameObject.Instantiate(voiceCommentWindowPrefab);
        commentWindow.transform.position = CustomHeadTracking.GetHeadPosition() + 2 * Vector3.forward;

        spawnedCommentWindows.Add(commentWindow);
    }

    private void OnCreateWriteCommentButtonClicked()
    {   
        GameObject commentWindow = GameObject.Instantiate(writeCommentWindowPrefab);
        commentWindow.transform.position = CustomHeadTracking.GetHeadPosition() + 2 * Vector3.forward;

        spawnedCommentWindows.Add(commentWindow);
    }

}

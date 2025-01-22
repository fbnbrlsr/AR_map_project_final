using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommentsUIInput : MonoBehaviour
{
    [SerializeField] CommentsManager commentsManager;
    [SerializeField] Button createVoiceCommentButton;
    [SerializeField] Button createWriteCommentButton;
    [SerializeField] TMP_InputField createCommentClassInputField;
    [SerializeField] Button enableDrawBoxButton;
    [SerializeField] MultiSelectDropdown visibleCommentClassesDropdown;

    void Start()
    {   
        createVoiceCommentButton.onClick.AddListener(OnCreateVoiceCommentButtonClicked);
        createWriteCommentButton.onClick.AddListener(OnCreateWriteCommentButtonClicked);
        createCommentClassInputField.onSubmit.AddListener(OnCreateCommentClassSubmit);
        enableDrawBoxButton.onClick.AddListener(OnEnableDrawBoxButtonClicked);
        visibleCommentClassesDropdown.onValueChanged.AddListener(OnVisibleCommentClassesChanged);
    }

    private void OnVisibleCommentClassesChanged(uint arg0)
    {
        if(visibleCommentClassesDropdown.GetSelectedOptionsList().Count == 0)
        {
            visibleCommentClassesDropdown.captionText.text = "None";
        }
        else{
            string selectedOptions = "";
            foreach(MultiSelectDropdown.OptionData od in visibleCommentClassesDropdown.GetSelectedOptionsList())
            {
                if(selectedOptions.Length == 0) selectedOptions = od.text;
                else selectedOptions += ", " + od.text;
            }
            visibleCommentClassesDropdown.captionText.text = selectedOptions;
        }

        commentsManager.HandleVisibleCommentClassesChanged(visibleCommentClassesDropdown.GetSelectedOptionsList());
    }

    private void OnCreateCommentClassSubmit(string className)
    {   
        if(className.Length == 0)
        {   
            createCommentClassInputField.text = "";
            return;
        }
        foreach(MultiSelectDropdown.OptionData od in visibleCommentClassesDropdown.options)
        {
            if(className.Equals(od.text))
            {
                createCommentClassInputField.text = "";
                return;
            } 
        }

        visibleCommentClassesDropdown.AddOption(new MultiSelectDropdown.OptionData(className, false));
        createCommentClassInputField.text = "";
        if(visibleCommentClassesDropdown.GetSelectedOptionsList().Count == 0)
        {
            visibleCommentClassesDropdown.captionText.text = "None";
        }
        else{
            string selectedOptions = "";
            foreach(MultiSelectDropdown.OptionData od in visibleCommentClassesDropdown.GetSelectedOptionsList())
            {
                if(selectedOptions.Length == 0) selectedOptions = od.text;
                else selectedOptions += ", " + od.text;
            }
            visibleCommentClassesDropdown.captionText.text = selectedOptions;
        }

        commentsManager.HandleAddCommentClass(className);
    }

    private void OnCreateVoiceCommentButtonClicked()
    {   
        commentsManager.HandleCreateVoiceComment(null);
    }

    private void OnCreateWriteCommentButtonClicked()
    {   
        commentsManager.HandleCreateWriteComment(null);
    }

    private void OnEnableDrawBoxButtonClicked()
    {   
        commentsManager.HandleCreateCommentBox();
    }

}

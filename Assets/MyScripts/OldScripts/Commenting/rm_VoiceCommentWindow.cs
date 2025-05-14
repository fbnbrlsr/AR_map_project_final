using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class rm_VoiceCommentWindow : MonoBehaviour  //, ICommentWindow
{   
    /*static int id = 0;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text contentText;
    [SerializeField] Button toggleRecordButton;
    [SerializeField] TextMeshProUGUI buttonText;

    public static Transform root;
    public static Dictionary<string, Material> commentClasses;

    GameObject commentCubeInstance;

    void Start()
    {   
        id += 1;
        titleText.text = "Voice Comment #" + id; 
        toggleRecordButton.onClick.AddListener(OnToggleRecordButtonPressed);
        buttonText.text = "Start";
    }
    
    void OnToggleRecordButtonPressed()
    {   
        if(!VoiceInput.isRecording)
        {   
            buttonText.text = "Stop";
            VoiceInput.StartVoiceRecording();

            VoiceInput.voiceInputGenerated += ChangeText;
        }
        else
        {
            buttonText.text = "Start";
            VoiceInput.StopVoiceRecording();
        }
    }

    private void ChangeText(string detectedVoiceInput)
    {
        contentText.text = detectedVoiceInput;
        
        VoiceInput.voiceInputGenerated -= ChangeText;
    }

    public void UpdateCommentWindow()
    {   
        // TODO
        throw new NotImplementedException();
    }

    public void UpdatePosition()
    {   
        // TODO
        throw new NotImplementedException();
    }

    public void SetCommentBoxInstance(GameObject commentBoxInstance)
    {   
        // TODO
        throw new NotImplementedException();
    }

    public string GetCommentClass()
    {
        throw new NotImplementedException();
    }

    public void SetActive(bool b)
    {
        throw new NotImplementedException();
    }*/
}

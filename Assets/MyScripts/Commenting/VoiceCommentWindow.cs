using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VoiceCommentWindow : MonoBehaviour
{   
    static int id = 0;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text contentText;
    [SerializeField] Button toggleRecordButton;
    [SerializeField] TextMeshProUGUI buttonText;

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
}

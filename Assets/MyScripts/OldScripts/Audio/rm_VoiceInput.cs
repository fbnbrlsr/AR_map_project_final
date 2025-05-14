using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Whisper;
using Whisper.Utils;

public delegate void VoiceInputGenerated(string detectedVoiceInput);
public class rm_VoiceInput : MonoBehaviour
{

    /*[SerializeField] private WhisperManager whisper;
    [SerializeField] private MicrophoneRecord microphoneRecord;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private WhisperStream _stream;

    public static event VoiceInputGenerated voiceInputGenerated;

    private static string voiceInput;

    async void Start()
    {   
        _stream = await whisper.CreateStream(microphoneRecord);
        _stream.OnResultUpdated += OnResult;
        _stream.OnStreamFinished += OnStreamFinished;
        button.onClick.AddListener(OnButtonPressed);
    }

    private void OnStreamFinished(string finalResult)
    {
        voiceInputGenerated?.Invoke(finalResult);
    }

    void OnButtonPressed()
    {   
        if(!isRecording)
        {
            StartVoiceRecording();
        }
        else
        {
            StopVoiceRecording();
        }

        buttonText.text = microphoneRecord.IsRecording ? "Stop" : "Start";
    }

    public static void StartVoiceRecording()
    {
        if(!isRecording)
        {   
            
            try{
                voiceInput = "";
                instance._stream.StartStream();
                instance.microphoneRecord.StartRecord();
                Debug.Log("No error in StartVoiceRecording");
            }
            catch(Exception e)
            {
                Debug.Log("ERROR IN StartVoiceRecording: " + e.ToString());
            }
        }
        else
        {
            Debug.LogError("Microphone is already recording!");
        }
    }

    public static void StopVoiceRecording()
    {
        if(isRecording)
        {
            instance.microphoneRecord.StopRecord();
        }
        else
        {
            Debug.LogError("Microphone was not recording!");
        }
    }

    void OnResult(string result)
    {   
        voiceInput += result;
        text.text = "VOICE INPUT: (selected microphone: " + microphoneRecord.SelectedMicDevice + ")\n" + voiceInput;
    } */

}
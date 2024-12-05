using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;


public class UIInteractionTest : MonoBehaviour
{   
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text textField;

    public MyLLMManager llm;


    //private string modelPath = @"Assets/StreamingAssets/llama-3.2-1b-instruct-q4_k_m.gguf";
    string llmReply = "";


    void Start()
    {   
        inputField.onSubmit.AddListener(OnTextFieldSubmit);


        llm = new MyLLMManager();
    }

    private void OnTextFieldSubmit(string prompt)
    {
        textField.text = "\nPrompt: " + prompt;
        textField.text += "\nReply: ";


        string reply = llm.HandlePrompt(prompt);

        textField.text += reply;
    }

}

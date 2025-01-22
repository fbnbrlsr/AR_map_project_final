using System;
using TMPro;
using UnityEngine;

public class CommentHistoryManager : MonoBehaviour
{

    [SerializeField] TMP_Text commentHistoryTextField;
    [SerializeField] TMP_InputField commentInputField;


    void Start()
    {
        commentInputField.onSubmit.AddListener(OnCommentInputFieldSubmit);

        commentHistoryTextField.text = "";
    }

    private void OnCommentInputFieldSubmit(string input)
    {   
        string time = DateTime.Now.ToString("h:mm:ss tt");

        commentHistoryTextField.text += "\n[" + time + "] " + input;

        commentInputField.text = "";
    }

}

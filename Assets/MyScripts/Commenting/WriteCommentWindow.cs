using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WriteCommentWindow : MonoBehaviour
{   
    static int id = 0;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text contentText;
    [SerializeField] TMP_InputField inputField;

    void Start()
    {   
        id += 1;
        titleText.text = "Write Comment #" + id; 
        inputField.onSubmit.AddListener(OnSubmitInput);
    }

    private void OnSubmitInput(string inputText)
    {
        contentText.gameObject.SetActive(true);
        contentText.text = inputText;

        inputField.gameObject.SetActive(false);
    }
}

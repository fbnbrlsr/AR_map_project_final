using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class rm_ConfigureHeightPanel : MonoBehaviour
{
    
    /*[SerializeField] List<GameObject> heightLines;
    //[SerializeField] List<TMP_Text> textFields;
    [SerializeField] TMP_Text minDateText;
    [SerializeField] TMP_Text maxDateText;

    DateTime minTime;
    DateTime maxTime;
    float minHeight;
    float maxHeight;
    private bool isConfigured;

    void Awake()
    {      
        isConfigured = false;
        Debug.Log("Height panel configuration started...");
        DatabaseManager.DataBaseInitialized += OnDataBaseInitialized;
    }

    void OnDataBaseInitialized()
    {   
        Debug.Log("Configuring height panel...");
        minTime = DatabaseLegData.earliestTime;
        maxTime = DatabaseLegData.latestTime;
        minHeight = DatabaseLegData.minPointHeight;
        maxHeight = DatabaseLegData.maxPointHeight;

        SetTextFields();
        for(int i = 0; i < heightLines.Count; i++)
        {
            SetLineHeight(i, heightLines[i]);
        }
        Debug.Log("Height panel configured!");
        isConfigured = true;
        
    }

    void SetTextFields()
    {   
        minDateText.SetText(minTime.ToShortDateString());
        maxDateText.SetText(maxTime.ToShortDateString());

        // TODO: set text height
        //minDateText.transform.localPosition = new Vector3(0f, -110f, 0f);
        //minDateText.transform.localPosition = new Vector3(0f, -10f, 0f);
    }

    void SetLineHeight(int index, GameObject line)
    {
        float frac = 1f * index / (heightLines.Count-1);
        float height = minHeight + frac * (maxHeight - minHeight);
        line.transform.localPosition = new Vector3(0f, height, 0f);
    }*/

}

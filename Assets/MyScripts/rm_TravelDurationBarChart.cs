using System;
using System.Collections.Generic;
using UnityEngine;


public class rm_TravelDurationBarChart : MonoBehaviour
{   
    /*public static TravelDurationBarChart instance;

    [SerializeField] RectTransform rect0to2;
    [SerializeField] RectTransform rect2to5;
    [SerializeField] RectTransform rect5to10;
    [SerializeField] RectTransform rect10to30;
    [SerializeField] RectTransform rect30plus;

    int[] histogram;
    
    void Start()
    {
        instance = this;
    }

    
    public void UpdateBarChart(List<K_DatabaseLegData> legsList)
    {
        Debug.Log("[TravelDurationBarChart] Received " + legsList.Count + " legs");

        histogram = new int[5]{0, 0, 0, 0, 0};
        foreach(K_DatabaseLegData leg in legsList)
        {
            if(leg.travel_time < 2*60) histogram[0]++;
            else if(leg.travel_time >= 2*60 && leg.travel_time < 5*60) histogram[1]++;
            else if(leg.travel_time >= 5*60 && leg.travel_time < 10*60) histogram[2]++;
            else if(leg.travel_time >= 10*60 && leg.travel_time < 30*60) histogram[3]++;
            else histogram[4]++;
        }

        Debug.Log("histogram: " + histogram.ToString());

        float maxHistogramCount = -1f;
        for(int i = 0; i < histogram.Length; i++)
        {
            if(histogram[i] > maxHistogramCount && histogram[i] != 0) maxHistogramCount = histogram[i];
        }

        float minRectHeight = 50f;
        float maxRectHeight = 200f;
        rect0to2.sizeDelta = new Vector2(rect0to2.sizeDelta.x, Mathf.Max(minRectHeight, maxRectHeight * histogram[0] / maxHistogramCount));
        rect2to5.sizeDelta = new Vector2(rect2to5.sizeDelta.x, Mathf.Min(minRectHeight, maxRectHeight * histogram[1] / maxHistogramCount));
        rect5to10.sizeDelta = new Vector2(rect5to10.sizeDelta.x, Mathf.Min(minRectHeight, maxRectHeight * histogram[2] / maxHistogramCount));
        rect10to30.sizeDelta = new Vector2(rect10to30.sizeDelta.x, Mathf.Min(minRectHeight, maxRectHeight * histogram[3] / maxHistogramCount));
        rect30plus.sizeDelta = new Vector2(rect30plus.sizeDelta.x, Mathf.Min(minRectHeight, maxRectHeight * histogram[4] / maxHistogramCount));

    }*/


}

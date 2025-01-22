using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Modifiers;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;

public delegate void TimePlaneChangedEvent();

public class DynamicTimePlane : MonoBehaviour
{
    
    [SerializeField] GameObject heightHandlePrefab;
    [SerializeField] Toggle projectOntoTimePlaneToggle;
    [SerializeField] GameObject mapHolder;


    public static TimePlaneChangedEvent TimePlaneChanged;

    GameObject heightHandleInstance;
    Vector3 heightHandleStartPos;

    float minHeight;
    float maxHeight;
    float minTime;
    float maxTime;
    public static float height;


    void Start()
    {   
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;
        InputEventsInvoker.InputEventTypes.HandSingleInputCont += OnInputCont;
        projectOntoTimePlaneToggle.onValueChanged.AddListener(OnProjectOntoTimePlaneToggle);

        heightHandleInstance = GameObject.Instantiate(heightHandlePrefab);
        heightHandleInstance.transform.parent = mapHolder.transform;
        heightHandleInstance.transform.localPosition = new Vector3(0f, 0.2f, 2f);

        minHeight = K_DatabaseLegData.minPointHeight;
        maxHeight = K_DatabaseLegData.maxPointHeight;
        minTime = K_DatabaseLegData.earliestTime;
        maxTime = K_DatabaseLegData.latestTime;
    }

    private void OnProjectOntoTimePlaneToggle(bool b)
    {
        K_DatabaseLegData.projectOnTimePlane = b;

        TimePlaneChanged?.Invoke();
    }

    private void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        if(targetObj.transform.IsChildOf(heightHandleInstance.transform))
        {
            TimePlaneChanged?.Invoke();

            heightHandleStartPos = interactionPos;
        }
    }

    private void OnInputCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        if(targetObj.transform.IsChildOf(heightHandleInstance.transform))
        {   
            Vector3 deltaPos = interactionPos - heightHandleStartPos;
            Vector3 pos = mapHolder.transform.localPosition;
            pos.y += deltaPos.y;
            pos.y = Mathf.Max(minHeight, pos.y);
            pos.y = Mathf.Min(pos.y, 5f);
            mapHolder.transform.localPosition = pos;

            heightHandleStartPos = interactionPos;

            TimePlaneChanged?.Invoke();
        }
    }

    void Update()
    {   
        TMP_Text timeText = heightHandleInstance.GetComponentInChildren<TMP_Text>();
        timeText.text = "Time:\n" + HeightToTime(mapHolder.transform.localPosition.y);
        height = mapHolder.transform.localPosition.y;
    }

    string HeightToTime(float height)
    {   
        if(K_DatabaseLegData.timeHeightMultiplier == 0) return "undefined";

        float absoluteDistance = K_DatabaseLegData.absoluteDistance / 2;
        float scaledHeight = height / K_DatabaseLegData.timeHeightMultiplier;
        float frac = ((scaledHeight / absoluteDistance) - minHeight) / (maxHeight - minHeight);
        float timeDiff = frac * (maxTime - minTime);
        int seconds = (int) (minTime + timeDiff);

        return SecondsToPrettyTime(seconds);
    }

    string SecondsToPrettyTime(int seconds)
    {   
        int hours = seconds / 3600;
        seconds -= hours * 3600;
        int minutes = seconds/60;
        seconds -= minutes * 60;
        return hours + "h " + minutes + "min " + seconds + "s";
    }

}

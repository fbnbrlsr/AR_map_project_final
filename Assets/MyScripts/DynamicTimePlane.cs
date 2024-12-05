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
    
    [SerializeField] GameObject planeObjectPrefab;
    [SerializeField] GameObject enableButton;
    [SerializeField] GameObject projectOntoPlaneButton;
    [SerializeField] GameObject mapRoot;
    [SerializeField] GameObject mapHolder;

    public static TimePlaneChangedEvent TimePlaneChanged;

    bool planeEnabled;
    GameObject planeInstance;
    Slider transparencySlider;
    GameObject heightHandle;
    Vector3 handleStartPos;
    Vector3 initMapHolderPos;

    float minHeight;
    float maxHeight;
    float minTime;
    float maxTime;
    public static float height;


    void Start()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;
        InputEventsInvoker.InputEventTypes.HandSingleInputCont += OnInputCont;

        initMapHolderPos = mapHolder.transform.position;

        planeInstance = GameObject.Instantiate(planeObjectPrefab);
        planeInstance.transform.position = new Vector3(0f, 0f, 3f);
        planeInstance.transform.parent = mapRoot.transform;
        heightHandle = planeInstance.GetNamedChild("TimePlaneHandle");
        transparencySlider = planeInstance.GetComponentInChildren<Slider>();
        transparencySlider.onValueChanged.AddListener(OnSliderValueChanged);

        planeEnabled = false;
        planeInstance.SetActive(false);

        minHeight = K_DatabaseLegData.minPointHeight;
        maxHeight = K_DatabaseLegData.maxPointHeight;
        minTime = K_DatabaseLegData.earliestTime;
        maxTime = K_DatabaseLegData.latestTime;
    }

    private void OnSliderValueChanged(float val)
    {   
        GameObject planeCube = planeInstance.GetNamedChild("PlaneCube");
        float alpha = val / 255f;
        Color initColor = planeCube.GetComponent<MeshRenderer>().material.color;
        initColor.a = alpha;
        planeCube.GetComponent<MeshRenderer>().material.color = initColor;
    }

    private void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        if(targetObj.transform.IsChildOf(enableButton.transform))
        {   
            TimePlaneChanged?.Invoke();

            OnEnableButtonPressed();
        }

        if(targetObj.transform.IsChildOf(projectOntoPlaneButton.transform))
        {   
            TimePlaneChanged?.Invoke();

            OnProjectOntoPlaneButtonPressed();
        }

        if(targetObj.transform.IsChildOf(heightHandle.transform))
        {
            TimePlaneChanged?.Invoke();

            handleStartPos = interactionPos;
        }
    }

    private void OnProjectOntoPlaneButtonPressed()
    {
        K_DatabaseLegData.projectOnTimePlane = !K_DatabaseLegData.projectOnTimePlane;

        if(K_DatabaseLegData.projectOnTimePlane)
        {
            /*Vector3 newPlanePos = planeInstance.transform.localPosition;
            newPlanePos.y = 0f;
            planeInstance.transform.localPosition = newPlanePos;*/
        }
        else
        {   
            mapHolder.transform.position = initMapHolderPos;
        }
        

        TMP_Text textField = projectOntoPlaneButton.GetComponentInChildren<TMP_Text>();
        if(textField != null)
        {
            textField.text = K_DatabaseLegData.projectOnTimePlane ? "Disable Project\nOnto Plane" : "Enable Project\nOnto Plane";
        }

        TimePlaneChanged?.Invoke();
    }

    private void OnInputCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        if(targetObj.transform.IsChildOf(heightHandle.transform))
        {   
            Vector3 deltaPos = interactionPos - handleStartPos;
            Vector3 pos = planeInstance.transform.localPosition;
            pos.y += deltaPos.y;
            pos.y = Mathf.Max(minHeight, pos.y);
            pos.y = Mathf.Min(pos.y, 5f);
            planeInstance.transform.localPosition = pos;

            if(K_DatabaseLegData.projectOnTimePlane)
            {   
                /*Vector3 posMap = mapHolder.transform.localPosition;
                posMap.y = Mathf.Max(0f, posMap.y);
                posMap.y += deltaPos.y;

                mapHolder.transform.localPosition = posMap;*/

                // TODO: THIS DOES NOT WORK
            }
            
            handleStartPos = interactionPos;

            TimePlaneChanged?.Invoke();
        }
    }

    private void OnEnableButtonPressed()
    {   
        planeEnabled = !planeEnabled;
        planeInstance.SetActive(planeEnabled);

        TMP_Text textField = enableButton.GetComponentInChildren<TMP_Text>();
        if(textField != null)
        {
            textField.text = planeInstance.activeSelf ? "Disable Time Plane" : "Enable Time Plane";
        }
    }

    void Update()
    {   
        if(planeInstance.activeSelf)
        {   
            TMP_Text timeText = planeInstance.GetComponentInChildren<TMP_Text>();
            timeText.text = "Time: " + HeightToTime(planeInstance.transform.localPosition.y);

            height = planeInstance.transform.localPosition.y;
        }
        else
        {
            height = -1f;
        }
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

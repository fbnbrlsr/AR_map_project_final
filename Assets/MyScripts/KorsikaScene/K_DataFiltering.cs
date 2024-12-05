using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TS.DoubleSlider;
using TMPro;

public class K_DataFiltering : MonoBehaviour
{
    [SerializeField] GameObject applyButton;
    [SerializeField] DoubleSlider departureTimeRangeSlider;
    [SerializeField] DoubleSlider travelDurationRangeSlider;
    [SerializeField] DoubleSlider travelDistanceRangeSlider;
    [SerializeField] DoubleSlider agentSlider;
    [SerializeField] MultiSelectDropdown travelModeDropdown;

    List<TravelMode> travelModes;
    float[] departureTimes;
    float[] travelDurations;
    float[] travelDistances;
    float[] agentRange;


    private static K_DatabaseManager _databaseManager;

    void Start()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnInputStart;
        _databaseManager = K_DatabaseManager.GetInstance();

        departureTimeRangeSlider.WholeNumbers = true;
        travelDurationRangeSlider.WholeNumbers = true;
        travelDistanceRangeSlider.WholeNumbers = true;
        agentSlider.WholeNumbers = true;

        travelModeDropdown.onValueChanged.AddListener(OnTravelModeChanged);
        departureTimeRangeSlider.OnValueChanged.AddListener(OnDepartureTimeRangeChanged);
        travelDurationRangeSlider.OnValueChanged.AddListener(OnTravelDurationRangeChanged);
        travelDistanceRangeSlider.OnValueChanged.AddListener(OnTravelDistanceRangeChanged);
        agentSlider.OnValueChanged.AddListener(OnAgentSliderChanged);

        travelModes = new List<TravelMode>();
        departureTimes = new float[] { departureTimeRangeSlider.MinValue, departureTimeRangeSlider.MaxValue };
        travelDurations = new float[] { travelDurationRangeSlider.MinValue, travelDurationRangeSlider.MaxValue };
        travelDistances = new float[] { travelDistanceRangeSlider.MinValue, travelDistanceRangeSlider.MaxValue };
        agentRange = new float[] { agentSlider.MinValue, agentSlider.MaxValue };
    }

    private void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {
        if(targetObj.transform.IsChildOf(applyButton.transform))
        {
            OnApplyFilterButtonPressed();
        }
    }

    private void OnApplyFilterButtonPressed()
    {   
        _databaseManager.ApplyPathsFilter(
            travelModes, 
            departureTimes[0], departureTimes[1], 
            travelDurations[0], travelDurations[1], 
            travelDistances[0], travelDistances[1],
            agentRange[0], agentRange[1]
            );
    }

    private void OnTravelModeChanged(uint index)
    {
        travelModes = new List<TravelMode>();
        List<MultiSelectDropdown.OptionData> list = travelModeDropdown.GetSelectedOptionsList();

        string label = "";
        if(list.Count == 0)
        {
            label = "None";
        }
        else
        {
            label = list[0].text;
            travelModes.Add(TravelModeMap.StringToTravelMode(list[0].text));

            for(int i = 1; i < list.Count; i++)
            {
                label += ", " + list[i].text;

                travelModes.Add(TravelModeMap.StringToTravelMode(list[i].text));
            }
        }

        travelModeDropdown.captionText.text = label;
    }

    private void OnDepartureTimeRangeChanged(float min, float max)
    {
        departureTimes[0] = departureTimeRangeSlider.MinValue;
        departureTimes[1] = departureTimeRangeSlider.MaxValue;
    }

    private void OnTravelDurationRangeChanged(float min, float max)
    {
        travelDurations[0] = travelDurationRangeSlider.MinValue;
        travelDurations[1] = travelDurationRangeSlider.MaxValue;
    }

    private void OnTravelDistanceRangeChanged(float min, float max)
    {
        travelDistances[0] = travelDistanceRangeSlider.MinValue;
        travelDistances[1] = travelDistanceRangeSlider.MaxValue;
    }

    private void OnAgentSliderChanged(float min, float max)
    {
        agentRange[0] = agentSlider.MinValue;
        agentRange[1] = agentSlider.MaxValue;
    }


}
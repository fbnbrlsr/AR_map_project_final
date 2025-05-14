using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class K_DataFiltering : MonoBehaviour
{
    //[SerializeField] Button applyFilterButton;

    [SerializeField] Toggle carTravelModeToggle;
    [SerializeField] Toggle walkTravelModeToggle;
    [SerializeField] Toggle bikeTravelModeToggle;
    [SerializeField] Toggle passengerTravelModeToggle;
    [SerializeField] Toggle ptTravelModeToggle;
    [SerializeField] Toggle travelModeSelectAllToggle;
    [SerializeField] Toggle departureTime0to5Toggle;
    [SerializeField] Toggle departureTime5to10Toggle;
    [SerializeField] Toggle departureTime10to15Toggle;
    [SerializeField] Toggle departureTime15to20Toggle;
    [SerializeField] Toggle departureTime20to25Toggle;
    [SerializeField] Toggle departureTime25to30Toggle;
    [SerializeField] Toggle departureTimeSelectAllToggle;
    [SerializeField] Toggle arrivalTime0to5Toggle;
    [SerializeField] Toggle arrivalTime5to10Toggle;
    [SerializeField] Toggle arrivalTime10to15Toggle;
    [SerializeField] Toggle arrivalTime15to20Toggle;
    [SerializeField] Toggle arrivalTime20to25Toggle;
    [SerializeField] Toggle arrivalTime25to30Toggle;
    [SerializeField] Toggle arrivalTimeSelectAllToggle;
    [SerializeField] Toggle duration0to2Toggle;
    [SerializeField] Toggle duration2to5Toggle;
    [SerializeField] Toggle duration5to10Toggle;
    [SerializeField] Toggle duration10to30Toggle;
    [SerializeField] Toggle duration30plusToggle;
    [SerializeField] Toggle durationSelectAllToggle;
    [SerializeField] MyBellButton showPathsBellButton;


    HashSet<TravelMode> travelModes;
    HashSet<CustomInterval> departureTimes;
    HashSet<CustomInterval> arrivalTimes;
    HashSet<CustomInterval> travelDurations;
    //float[] travelDurations;
    float[] agentRange;
    bool isInitialized;

    private static K_DatabaseManager _databaseManager;

    void Start()
    {
        _databaseManager = K_DatabaseManager.GetInstance();

        //if(applyFilterButton != null) applyFilterButton.onClick.AddListener(if(isInitialized) OnApplyFilterButtonPressed);

        carTravelModeToggle.onValueChanged.AddListener(OnCarTravelModeChanged);
        walkTravelModeToggle.onValueChanged.AddListener(OnWalkTravelModeChanged);
        bikeTravelModeToggle.onValueChanged.AddListener(OnBikeTravelModeChanged);
        passengerTravelModeToggle.onValueChanged.AddListener(OnPassengerTravelModeChanged);
        ptTravelModeToggle.onValueChanged.AddListener(OnPTTravelModeChanged);
        travelModeSelectAllToggle.onValueChanged.AddListener(OnTravelModeSelectAllChanged);

        departureTime0to5Toggle.onValueChanged.AddListener(OnDepartureTime0to5ToggleChanged);
        departureTime5to10Toggle.onValueChanged.AddListener(OnDepartureTime5to10ToggleChanged);
        departureTime10to15Toggle.onValueChanged.AddListener(OnDepartureTime10to15ToggleChanged);
        departureTime15to20Toggle.onValueChanged.AddListener(OnDepartureTime15to20ToggleChanged);
        departureTime20to25Toggle.onValueChanged.AddListener(OnDepartureTime20to25ToggleChanged);
        departureTime25to30Toggle.onValueChanged.AddListener(OnDepartureTime25to30ToggleChanged);
        departureTimeSelectAllToggle.onValueChanged.AddListener(OnDepartureTimeSelectAllToggleChanged);

        arrivalTime0to5Toggle.onValueChanged.AddListener(OnArrivalTime0to5ToggleChanged);
        arrivalTime5to10Toggle.onValueChanged.AddListener(OnArrivalTime5to10ToggleChanged);
        arrivalTime10to15Toggle.onValueChanged.AddListener(OnArrivalTime10to15ToggleChanged);
        arrivalTime15to20Toggle.onValueChanged.AddListener(OnArrivalTime15to20ToggleChanged);
        arrivalTime20to25Toggle.onValueChanged.AddListener(OnArrivalTime20to25ToggleChanged);
        arrivalTime25to30Toggle.onValueChanged.AddListener(OnArrivalTime25to30ToggleChanged);
        arrivalTimeSelectAllToggle.onValueChanged.AddListener(OnArrivalTimeSelectAllToggleChanged);

        duration0to2Toggle.onValueChanged.AddListener(OnDuration0to2Changed);
        duration2to5Toggle.onValueChanged.AddListener(OnDuration2to5Changed);
        duration5to10Toggle.onValueChanged.AddListener(OnDuration5to10Changed);
        duration10to30Toggle.onValueChanged.AddListener(OnDuration10to30Changed);
        duration30plusToggle.onValueChanged.AddListener(OnDuration30plusChanged);
        durationSelectAllToggle.onValueChanged.AddListener(OnDurationSelectAllToggleChanged);

        travelModes = new HashSet<TravelMode>();
        departureTimes = new HashSet<CustomInterval>();
        arrivalTimes = new HashSet<CustomInterval>();
        travelDurations = new HashSet<CustomInterval>();
        agentRange = new float[] { K_DatabaseLegData.personIDLowerBound, K_DatabaseLegData.personIDUpperBound };

        isInitialized = true;
        K_DataPathVisualizationManager.InitializationFinishedEvent += InitializeFitlers;
    }

    private void OnDurationSelectAllToggleChanged(bool b)
    {
        if(b) {
            duration0to2Toggle.isOn = true;
            duration2to5Toggle.isOn = true;
            duration5to10Toggle.isOn = true;
            duration10to30Toggle.isOn = true;
            duration30plusToggle.isOn = true;
        }
        else {
            duration0to2Toggle.isOn = false;
            duration2to5Toggle.isOn = false;
            duration5to10Toggle.isOn = false;
            duration10to30Toggle.isOn = false;
            duration30plusToggle.isOn = false;
        }
    }

    private void OnArrivalTimeSelectAllToggleChanged(bool b)
    {
        if(b) {
            arrivalTime0to5Toggle.isOn = true;
            arrivalTime5to10Toggle.isOn = true;
            arrivalTime10to15Toggle.isOn = true;
            arrivalTime15to20Toggle.isOn = true;
            arrivalTime20to25Toggle.isOn = true;
            arrivalTime25to30Toggle.isOn = true;
            arrivalTime25to30Toggle.isOn = true;
        }
        else {
            arrivalTime0to5Toggle.isOn = false;
            arrivalTime5to10Toggle.isOn = false;
            arrivalTime10to15Toggle.isOn = false;
            arrivalTime15to20Toggle.isOn = false;
            arrivalTime20to25Toggle.isOn = false;
            arrivalTime25to30Toggle.isOn = false;
            arrivalTime25to30Toggle.isOn = false;
        }
    }

    private void OnDepartureTimeSelectAllToggleChanged(bool b)
    {
        if(b) {
            departureTime0to5Toggle.isOn = true;
            departureTime5to10Toggle.isOn = true;
            departureTime10to15Toggle.isOn = true;
            departureTime15to20Toggle.isOn = true;
            departureTime20to25Toggle.isOn = true;
            departureTime25to30Toggle.isOn = true;
            departureTime25to30Toggle.isOn = true;
        }
        else {
            departureTime0to5Toggle.isOn = false;
            departureTime5to10Toggle.isOn = false;
            departureTime10to15Toggle.isOn = false;
            departureTime15to20Toggle.isOn = false;
            departureTime20to25Toggle.isOn = false;
            departureTime25to30Toggle.isOn = false;
            departureTime25to30Toggle.isOn = false;
        }
    }

    private void OnTravelModeSelectAllChanged(bool b)
    {
        if(b) {
            carTravelModeToggle.isOn = true;
            bikeTravelModeToggle.isOn = true;
            walkTravelModeToggle.isOn = true;
            passengerTravelModeToggle.isOn = true;
            ptTravelModeToggle.isOn = true;
        }
        else {
            carTravelModeToggle.isOn = false;
            bikeTravelModeToggle.isOn = false;
            walkTravelModeToggle.isOn = false;
            passengerTravelModeToggle.isOn = false;
            ptTravelModeToggle.isOn = false;
        }
    }

    public void InitializeFitlers()
    {   
        /*carTravelModeToggle.isOn = true;
        walkTravelModeToggle.isOn = false;
        bikeTravelModeToggle.isOn = false;
        passengerTravelModeToggle.isOn = false;
        ptTravelModeToggle.isOn = false;
        departureTime0to5Toggle.isOn = true;
        departureTime5to10Toggle.isOn = true;
        departureTime10to15Toggle.isOn = true;
        departureTime15to20Toggle.isOn = true;
        departureTime20to25Toggle.isOn = true;
        departureTime25to30Toggle.isOn = true;
        arrivalTime0to5Toggle.isOn = true;
        arrivalTime5to10Toggle.isOn = true;
        arrivalTime10to15Toggle.isOn = true;
        arrivalTime15to20Toggle.isOn = true;
        arrivalTime20to25Toggle.isOn = true;
        arrivalTime25to30Toggle.isOn = true;
        duration0to2Toggle.isOn = true;
        duration2to5Toggle.isOn = true;
        duration5to10Toggle.isOn = true;
        duration10to30Toggle.isOn = true;
        duration30plusToggle.isOn = true;*/

        //isInitialized = true;
        //OnApplyFilterButtonPressed();
    }

    private void OnApplyFilterButtonPressed()
    {   
        Debug.Log("apply filters: travelModes=" + travelModes.Count + ", departureTimes=" + departureTimes.Count);
        UpdateSelectAllToggles();
        if(SceneManager.GetActiveScene().name.Equals("DummyScene")) return;
        _databaseManager.ApplyPathsFilter(
                travelModes, 
                departureTimes,
                arrivalTimes,
                travelDurations, 
                agentRange
            );
        showPathsBellButton.SetState(false);
        showPathsBellButton.SetState(true);
    }

    private void UpdateSelectAllToggles()
    {
        if(carTravelModeToggle.isOn && bikeTravelModeToggle.isOn && walkTravelModeToggle.isOn && passengerTravelModeToggle.isOn && ptTravelModeToggle.isOn)
        {
            travelModeSelectAllToggle.SetIsOnWithoutNotify(true);
        }
        else
        {
            travelModeSelectAllToggle.SetIsOnWithoutNotify(false);
        }

        if(departureTime0to5Toggle.isOn && departureTime5to10Toggle.isOn && departureTime10to15Toggle.isOn && 
            departureTime15to20Toggle.isOn && departureTime20to25Toggle.isOn && departureTime25to30Toggle.isOn)
        {
            departureTimeSelectAllToggle.SetIsOnWithoutNotify(true);
        }
        else
        {
            departureTimeSelectAllToggle.SetIsOnWithoutNotify(false);
        }

        if(arrivalTime0to5Toggle.isOn && arrivalTime5to10Toggle.isOn && arrivalTime10to15Toggle.isOn && 
            arrivalTime15to20Toggle.isOn && arrivalTime20to25Toggle.isOn && arrivalTime25to30Toggle.isOn)
        {
            arrivalTimeSelectAllToggle.SetIsOnWithoutNotify(true);
        }
        else
        {
            arrivalTimeSelectAllToggle.SetIsOnWithoutNotify(false);
        }

        if(duration0to2Toggle.isOn && duration2to5Toggle.isOn && duration5to10Toggle.isOn && 
            duration10to30Toggle.isOn && duration30plusToggle.isOn)
        {
            durationSelectAllToggle.SetIsOnWithoutNotify(true);
        }
        else
        {
            durationSelectAllToggle.SetIsOnWithoutNotify(false);
        }

    }

    private void OnDuration30plusChanged(bool b)
    {
        if(b) travelDurations.Add(new CustomInterval(30f, 1000f));
        else travelDurations.Remove(new CustomInterval(30f, 1000f));
        if(isInitialized) OnApplyFilterButtonPressed();
    }

    private void OnDuration10to30Changed(bool b)
    {
        if(b) travelDurations.Add(new CustomInterval(10f, 30f));
        else travelDurations.Remove(new CustomInterval(10f, 30f));
        if(isInitialized) OnApplyFilterButtonPressed();
    }

    private void OnDuration5to10Changed(bool b)
    {
        if(b) travelDurations.Add(new CustomInterval(5f, 10f));
        else travelDurations.Remove(new CustomInterval(5f, 10f));
        if(isInitialized) OnApplyFilterButtonPressed();
    }

    private void OnDuration2to5Changed(bool b)
    {
        if(b) travelDurations.Add(new CustomInterval(2f, 5f));
        else travelDurations.Remove(new CustomInterval(2f, 5f));
        if(isInitialized) OnApplyFilterButtonPressed();
    }

    private void OnDuration0to2Changed(bool b)
    {
        if(b) travelDurations.Add(new CustomInterval(0f, 2f));
        else travelDurations.Remove(new CustomInterval(0f, 2f));
        if(isInitialized) OnApplyFilterButtonPressed();
    }

    private void OnCarTravelModeChanged(bool b)
    {
        if(b) travelModes.Add(TravelMode.Car);
        else travelModes.Remove(TravelMode.Car);
        if(isInitialized) OnApplyFilterButtonPressed();
    }
    private void OnWalkTravelModeChanged(bool b)
    {
        if(b) travelModes.Add(TravelMode.Walk);
        else travelModes.Remove(TravelMode.Walk);
        if(isInitialized) OnApplyFilterButtonPressed();
    }
    private void OnBikeTravelModeChanged(bool b)
    {
        if(b) travelModes.Add(TravelMode.Bike);
        else travelModes.Remove(TravelMode.Bike);
        if(isInitialized) OnApplyFilterButtonPressed();
    }
    private void OnPassengerTravelModeChanged(bool b)
    {
        if(b) travelModes.Add(TravelMode.CarPassenger);
        else travelModes.Remove(TravelMode.CarPassenger);
        if(isInitialized) OnApplyFilterButtonPressed();
    }
    private void OnPTTravelModeChanged(bool b)
    {
        if(b) travelModes.Add(TravelMode.PublicTr);
        else travelModes.Remove(TravelMode.PublicTr);
        if(isInitialized) OnApplyFilterButtonPressed();
    }

    private void OnDepartureTime0to5ToggleChanged(bool b)
    {
        if(b) departureTimes.Add(new CustomInterval(0f, 5f));
        else departureTimes.Remove(new CustomInterval(0f, 5f));
        if(isInitialized) OnApplyFilterButtonPressed();
    }
    private void OnDepartureTime5to10ToggleChanged(bool b)
    {
        if(b) departureTimes.Add(new CustomInterval(5f, 10f));
        else departureTimes.Remove(new CustomInterval(5f, 10f));
        if(isInitialized) OnApplyFilterButtonPressed();
    }
    private void OnDepartureTime10to15ToggleChanged(bool b)
    {
        if(b) departureTimes.Add(new CustomInterval(10f, 15f));
        else departureTimes.Remove(new CustomInterval(10f, 15f));
        if(isInitialized) OnApplyFilterButtonPressed();
    }
    private void OnDepartureTime15to20ToggleChanged(bool b)
    {
        if(b) departureTimes.Add(new CustomInterval(15f, 20f));
        else departureTimes.Remove(new CustomInterval(15f, 20f));
        if(isInitialized) OnApplyFilterButtonPressed();
    }
    private void OnDepartureTime20to25ToggleChanged(bool b)
    {
        if(b) departureTimes.Add(new CustomInterval(20f, 25f));
        else departureTimes.Remove(new CustomInterval(20f, 25f));
        if(isInitialized) OnApplyFilterButtonPressed();
    }
    private void OnDepartureTime25to30ToggleChanged(bool b)
    {
        if(b) departureTimes.Add(new CustomInterval(25f, 30f));
        else departureTimes.Remove(new CustomInterval(25f, 30f));
        if(isInitialized) OnApplyFilterButtonPressed();
    }

    private void OnArrivalTime0to5ToggleChanged(bool b)
    {
        if(b) arrivalTimes.Add(new CustomInterval(0f, 5f));
        else arrivalTimes.Remove(new CustomInterval(0f, 5f));
        if(isInitialized) OnApplyFilterButtonPressed();
    }
    private void OnArrivalTime5to10ToggleChanged(bool b)
    {
        if(b) arrivalTimes.Add(new CustomInterval(5f, 10f));
        else arrivalTimes.Remove(new CustomInterval(5f, 10f));
        if(isInitialized) OnApplyFilterButtonPressed();
    }
    private void OnArrivalTime10to15ToggleChanged(bool b)
    {
        if(b) arrivalTimes.Add(new CustomInterval(10f, 15f));
        else arrivalTimes.Remove(new CustomInterval(10f, 15f));
        if(isInitialized) OnApplyFilterButtonPressed();
    }
    private void OnArrivalTime15to20ToggleChanged(bool b)
    {
        if(b) arrivalTimes.Add(new CustomInterval(15f, 20f));
        else arrivalTimes.Remove(new CustomInterval(15f, 20f));
        if(isInitialized) OnApplyFilterButtonPressed();
    }
    private void OnArrivalTime20to25ToggleChanged(bool b)
    {
        if(b) arrivalTimes.Add(new CustomInterval(20f, 25f));
        else arrivalTimes.Remove(new CustomInterval(20f, 25f));
        if(isInitialized) OnApplyFilterButtonPressed();
    }
    private void OnArrivalTime25to30ToggleChanged(bool b)
    {
        if(b) arrivalTimes.Add(new CustomInterval(25f, 30f));
        else arrivalTimes.Remove(new CustomInterval(25f, 30f));
        if(isInitialized) OnApplyFilterButtonPressed();
    }

    /*private void OnTravelDurationRangeChanged(float min, float max)
    {
        travelDurations[0] = travelDurationRangeSlider.MinValue;
        travelDurations[1] = travelDurationRangeSlider.MaxValue;
    }*/

    /*private void OnAgentSliderChanged(float min, float max)
    {   
        agentRange[0] = agentSlider.MinValue;
        agentRange[1] = agentSlider.MaxValue;
    }*/

}

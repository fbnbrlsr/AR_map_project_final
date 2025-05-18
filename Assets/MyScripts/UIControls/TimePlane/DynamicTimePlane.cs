using Mapbox.Examples;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public delegate void TimePlaneChangedEvent();

public class DynamicTimePlane : MonoBehaviour
{   

    /*
    *   This script enables the vertical movement of the map plane.
    *   The user can grab a handle to move the map up and down to inspect
    *   different times (represented by height). This makes data inspection
    *   more convenient.
    *   The time at the current height is displayed on the handle.
    */
    
    [SerializeField] GameObject heightHandleInstance;
    [SerializeField] GameObject mapHolder;

    public static TimePlaneChangedEvent TimePlaneChanged;

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

        minHeight = DatabaseLegData.minPointHeight;
        maxHeight = DatabaseLegData.maxPointHeight;
        minTime = DatabaseLegData.earliestTime;
        maxTime = DatabaseLegData.latestTime;
    }

    private void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {
        if(targetObj.transform.IsChildOf(heightHandleInstance.transform))
        {
            TimePlaneChanged?.Invoke();

            heightHandleStartPos = interactionPos;
        }
    }

    private void OnInputCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
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
        if(SceneManager.GetActiveScene().name.Equals("DummyScene")) return;

        TMP_Text[] timeTexts = heightHandleInstance.GetComponentsInChildren<TMP_Text>();
        foreach(TMP_Text t in timeTexts)
        {
            t.text = "Time:\n" + SecondsToPrettyTime(HeightToTime(mapHolder.transform.localPosition.y));
        }
        height = mapHolder.transform.localPosition.y;
    }

    int HeightToTime(float height)
    {   
        float absoluteDistance = CustomReloadMap.GetReferenceDistance() / 2;
        float frac = ((height / absoluteDistance) - minHeight) / (maxHeight - minHeight);
        float timeDiff = frac * (maxTime - minTime);
        int seconds = (int) (minTime + timeDiff);

        return seconds;
    }

    string SecondsToPrettyTime(int seconds)
    {   
        int hours = seconds / 3600;
        seconds -= hours * 3600;
        int minutes = seconds/60;
        return hours + "h " + minutes + "min";
    }

}
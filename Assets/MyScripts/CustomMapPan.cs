using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class CustomMapPan : MonoBehaviour
{   

    [SerializeField] AbstractMap _map;
    public GameObject panInteractionObject;
    public float panSpeed = 1000f;

    private InputEventTypes inputEvents;
    private Vector3 inputStartPos;

    void Start()
    {
        inputEvents = InputEventsInvoker.InputEventTypes;
        if(inputEvents != null)
        {
            inputEvents.HandSingleIPinchStart += OnInputStart;
            inputEvents.HandSingleInputCont += OnInputCont;
        }
    }

    void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {   
        inputStartPos = fingerPos;
    }

    void OnInputCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion currRot, GameObject targetObj)
    {   
        if(targetObj == panInteractionObject){
            float panMultiplier = panSpeed * Conversions.GetTileScaleInMeters((float)0, _map.AbsoluteZoom) / _map.UnityTileSize;
            Vector3 deltaPosition = inputStartPos - fingerPos;

            if(deltaPosition.magnitude > .1f) return;

            float sinTileAngle = -Mathf.Sin(MapTilting.tiltAngleRad);
            float delta_yz = sinTileAngle * deltaPosition.y + (1 - sinTileAngle) * deltaPosition.z;

            Vector2d deltaPan = Conversions.MetersToLatLon(new Vector2d(deltaPosition.x * panMultiplier, delta_yz * panMultiplier));

            if(deltaPan.magnitude > 0f) _map.UpdateMap(_map.CenterLatitudeLongitude + deltaPan, _map.Zoom);
            inputStartPos = fingerPos;
        }
    }

}

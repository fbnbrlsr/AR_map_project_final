using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class CustomMapPan : MonoBehaviour
{   

    /*
    *   This class implements the support for panning the map using hand gestures.
    */

    [SerializeField] AbstractMap map;
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

    void OnInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj, SpatialPointerKind touchKind)
    {   
        inputStartPos = fingerPos;
    }

    void OnInputCont(Vector3 fingerPos, Vector3 interactionPos, Quaternion currRot, GameObject targetObj, SpatialPointerKind touchKind)
    {   
        if(targetObj == panInteractionObject){
            float panMultiplier = panSpeed * Conversions.GetTileScaleInMeters(0f, map.AbsoluteZoom) / map.UnityTileSize;
            Vector3 deltaPosition = inputStartPos - fingerPos;

            if(deltaPosition.magnitude > .1f) return;

            float sinTileAngle = -Mathf.Sin(MapTilting.tiltAngleRad);
            float delta_yz = sinTileAngle * deltaPosition.y + (1 - sinTileAngle) * deltaPosition.z;

            Vector2d deltaPan = Conversions.MetersToLatLon(new Vector2d(deltaPosition.x * panMultiplier, delta_yz * panMultiplier));

            if(deltaPan.magnitude > 0f) map.UpdateMap(map.CenterLatitudeLongitude + deltaPan, map.Zoom);
            inputStartPos = fingerPos;
        }
    }

}

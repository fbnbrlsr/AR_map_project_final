using UnityEngine;
using Unity.PolySpatial.InputDevices;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.EventSystems;



public class InputEventsInvoker : MonoBehaviour
{   

    /*
    *   This class triggers the various events in InputEventTypes as are detected.
    *   If enabled, visualizes the positions at which input has been detected (currently not implemented correctly).
    */

    public static InputEventTypes InputEventTypes => _inputEventTypes;
    private static InputEventTypes _inputEventTypes;

    [SerializeField] GameObject debugPrefab0;
    [SerializeField] GameObject debugPrefab1;
    [SerializeField] GameObject popupWarningWindowPrefab;
    [SerializeField] GameObject largeCollisionBackgroundPrefab;

    // Input visualization for debugging
    private GameObject debugInstance0;
    private GameObject debugInstance1;
    private GameObject largeCollisionBackgroundInstance;

    private bool hasDoubleInitialValue;
    private bool triggerInputFinishedEvent;


    void Start()
    {   
        if(debugPrefab0 != null)
        {
            debugInstance0 = Instantiate(debugPrefab0);
            debugInstance0.SetActive(false);
        }
        if(debugPrefab1 != null)
        {
            debugInstance1 = Instantiate(debugPrefab1);
            debugInstance1.SetActive(false);
        }
        if(largeCollisionBackgroundPrefab != null)
        {
            largeCollisionBackgroundInstance = Instantiate(largeCollisionBackgroundPrefab);
            largeCollisionBackgroundInstance.SetActive(false);
        }

        _inputEventTypes = new InputEventTypes();

        hasDoubleInitialValue = false;
        triggerInputFinishedEvent = true;

        NotificationPopup.SetPrefab(popupWarningWindowPrefab);
    }

    void Update()
    {   
        EventSystem.current.SetSelectedGameObject(null);
        
        // No hand used for input
        if(Touch.activeTouches.Count == 0)
        {   
            if(triggerInputFinishedEvent)
            {
                _inputEventTypes.InvokeInputFinished();
                triggerInputFinishedEvent = false;
            }

            GameObject.Destroy(debugInstance0);
            GameObject.Destroy(debugInstance1);

            hasDoubleInitialValue = false;
        }
        else triggerInputFinishedEvent = true; 

        // Only one hand used for input
        if(Touch.activeTouches.Count == 1)
        {   
            var touch = Touch.activeTouches[0];
            SpatialPointerState touchData = EnhancedSpatialPointerSupport.GetPointerState(touch);
            
            hasDoubleInitialValue = false;

            _inputEventTypes.InvokeAnyInput();

            if(touchData.targetObject != null &&
             touchData.Kind == SpatialPointerKind.Touch && touch.phase == TouchPhase.Began)
            {   
                _inputEventTypes.InvokeHandSingleTouchStart(touchData.inputDevicePosition, touchData.interactionPosition, touchData.inputDeviceRotation, touchData.targetObject, touchData.Kind);
            }
            else if(touchData.targetObject != null &&
             touchData.Kind == SpatialPointerKind.DirectPinch && touch.phase == TouchPhase.Began)
            {   
                _inputEventTypes.InvokeHandSingleDPinchStart(touchData.inputDevicePosition, touchData.interactionPosition, touchData.inputDeviceRotation, touchData.targetObject, touchData.Kind);
            }
            else if(touchData.targetObject != null &&
             touchData.Kind == SpatialPointerKind.IndirectPinch && touch.phase == TouchPhase.Began)
            {   
                _inputEventTypes.InvokeHandSingleIPinchStart(touchData.inputDevicePosition, touchData.interactionPosition, touchData.inputDeviceRotation, touchData.targetObject, touchData.Kind);
            }
            else if(touchData.targetObject != null)
            {
                _inputEventTypes.InvokeHandSingleInputCont(touchData.inputDevicePosition, touchData.interactionPosition, touchData.inputDeviceRotation, touchData.targetObject, touchData.Kind);
            }
        }
        // Two hands used for input
        else if(Touch.activeTouches.Count == 2)
        {   
            var touch0 = Touch.activeTouches[0];
            var touch1 = Touch.activeTouches[1];
            SpatialPointerState touchData0 = EnhancedSpatialPointerSupport.GetPointerState(touch0);
            SpatialPointerState touchData1 = EnhancedSpatialPointerSupport.GetPointerState(touch1);

            triggerInputFinishedEvent = true;

            _inputEventTypes.InvokeAnyInput();

            if(touchData0.targetObject != null && !hasDoubleInitialValue)
            {
                _inputEventTypes.InvokeHandDoubleInputStart(touchData0.inputDevicePosition, touchData0.inputDeviceRotation,
                                           touchData1.inputDevicePosition, touchData1.inputDeviceRotation, touchData0.targetObject);
                hasDoubleInitialValue = true;
            }
            else if(touchData0.targetObject != null && hasDoubleInitialValue)
            {
                _inputEventTypes.InvokeHandDoubleInputCont(touchData0.inputDevicePosition, touchData0.inputDeviceRotation, 
                                            touchData1.inputDevicePosition, touchData1.inputDeviceRotation, touchData0.targetObject);
            }

        }
    }

}

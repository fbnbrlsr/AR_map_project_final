using UnityEngine;
using Unity.PolySpatial.InputDevices;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using UnityEngine.InputSystem.LowLevel;



public class InputEventsInvoker : MonoBehaviour
{
    public static InputEventTypes InputEventTypes => _inputEventTypes;
    private static InputEventTypes _inputEventTypes;

    [SerializeField] GameObject debugPrefab0;
    [SerializeField] GameObject debugPrefab1;
    [SerializeField] GameObject popupWarningWindowPrefab;
    [SerializeField] GameObject largeCollisionBackgroundPrefab;

    private GameObject debugInstance0;
    private GameObject debugInstance1;
    private GameObject largeCollisionBackgroundInstance;

    private bool hasDoubleInitialValue;
    private bool triggerInputFinishedEvent;

    public static bool isDrawBoxActive;
    private int drawBoxTouchCount;

    public static bool hasInput => Touch.activeTouches.Count > 0;


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
        Debug.Log("InputEventsInvoker started...");

        hasDoubleInitialValue = false;
        triggerInputFinishedEvent = true;

        isDrawBoxActive = false;
        drawBoxTouchCount = 0;

        NotificationPopup.SetPrefab(popupWarningWindowPrefab);
    }

    void Update()
    {   
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

            if(drawBoxTouchCount >= 1)
            {   
                drawBoxTouchCount = 0;
                isDrawBoxActive = false;
                largeCollisionBackgroundInstance?.SetActive(false);
            }
        }
        else triggerInputFinishedEvent = true; 

        if(isDrawBoxActive)
        {   
            // draw collision box
            largeCollisionBackgroundInstance?.SetActive(true);

            if(Touch.activeTouches.Count == 1)
            {   
                var touch = Touch.activeTouches[0];
                SpatialPointerState touchData = EnhancedSpatialPointerSupport.GetPointerState(touch);

                if(touchData.targetObject != null &&
                touchData.Kind == SpatialPointerKind.IndirectPinch && touch.phase == TouchPhase.Began)
                {   
                    drawBoxTouchCount += 1;
                    _inputEventTypes.InvokeHandSingleIPinchStartDrawBox(touchData.inputDevicePosition, touchData.interactionPosition, touchData.inputDeviceRotation, touchData.targetObject);
                }
                else if(touchData.targetObject != null)
                {   
                    _inputEventTypes.InvokeHandSingleInputContDrawBox(touchData.inputDevicePosition, touchData.interactionPosition, touchData.inputDeviceRotation, touchData.targetObject);
                }

            }
            return;
        }

        // Only one hand makes input
        if(Touch.activeTouches.Count == 1)
        {   
            var touch = Touch.activeTouches[0];
            SpatialPointerState touchData = EnhancedSpatialPointerSupport.GetPointerState(touch);
            
            hasDoubleInitialValue = false;

            _inputEventTypes.InvokeAnyInput();

            if(touchData.targetObject != null &&
             touchData.Kind == SpatialPointerKind.Touch && touch.phase == TouchPhase.Began)
            {   
                _inputEventTypes.InvokeHandSingleTouchStart(touchData.inputDevicePosition, touchData.interactionPosition, touchData.inputDeviceRotation, touchData.targetObject);
                //Debug.Log("[InputEventsInvoker] Single hand input: Start (Touch)");
            }
            else if(touchData.targetObject != null &&
             touchData.Kind == SpatialPointerKind.DirectPinch && touch.phase == TouchPhase.Began)
            {   
                _inputEventTypes.InvokeHandSingleDPinchStart(touchData.inputDevicePosition, touchData.interactionPosition, touchData.inputDeviceRotation, touchData.targetObject);
                //Debug.Log("[InputEventsInvoker] Single hand input: Start (Direct Pinch)");
            }
            else if(touchData.targetObject != null &&
             touchData.Kind == SpatialPointerKind.IndirectPinch && touch.phase == TouchPhase.Began)
            {   
                _inputEventTypes.InvokeHandSingleIPinchStart(touchData.inputDevicePosition, touchData.interactionPosition, touchData.inputDeviceRotation, touchData.targetObject);
                //Debug.Log("[InputEventsInvoker] Single hand input: Start (Indirect Pinch)");
            }
            else if(touchData.targetObject != null)
            {
                _inputEventTypes.InvokeHandSingleInputCont(touchData.inputDevicePosition, touchData.interactionPosition, touchData.inputDeviceRotation, touchData.targetObject);
                //Debug.Log("[InputEventsInvoker] Single hand input: Continuous");

                /*if(debugInstance0 == null)
                {
                    debugInstance0 = GameObject.Instantiate(debugPrefab0);
                }
                debugInstance0.transform.position = touchData.inputDevicePosition;*/
            }
        }
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
                //Debug.Log("[InputEventsInvoker] Double hand input: Start");
                hasDoubleInitialValue = true;
            }
            else if(touchData0.targetObject != null && hasDoubleInitialValue)
            {
                _inputEventTypes.InvokeHandDoubleInputCont(touchData0.inputDevicePosition, touchData0.inputDeviceRotation, 
                                            touchData1.inputDevicePosition, touchData1.inputDeviceRotation, touchData0.targetObject);

                /*if(debugInstance0 == null)
                {
                    debugInstance0 = GameObject.Instantiate(debugPrefab0);
                }
                if(debugInstance1 == null)
                {
                    debugInstance1 = GameObject.Instantiate(debugPrefab1);
                }
                Debug.Log("[InputEventsInvoker] Double hand input: Continuous");
                debugInstance0.transform.position = touchData0.inputDevicePosition;
                debugInstance1.transform.position = touchData1.inputDevicePosition;*/
            }

        }


    }
}

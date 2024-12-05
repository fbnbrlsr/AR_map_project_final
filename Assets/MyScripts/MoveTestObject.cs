using UnityEngine;
using Unity.PolySpatial.InputDevices;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using UnityEngine.InputSystem.LowLevel;

public class MoveTestObject : MonoBehaviour
{   

    public GameObject testGameObject;
    private Vector3 lastPosition;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void Update()
    {
        if(Touch.activeTouches.Count >= 1){
            SpatialPointerState touchData = EnhancedSpatialPointerSupport.GetPointerState(Touch.activeTouches[0]);

            if(Touch.activeTouches[0].phase == TouchPhase.Began)
            { 
                lastPosition = touchData.interactionPosition;
            } 
            else if(touchData.targetObject.name.Equals("TestCube") && touchData.Kind != SpatialPointerKind.Touch)
            {
                // Move cube object
                Vector3 deltaPosition = touchData.interactionPosition - lastPosition;
                testGameObject.transform.position += deltaPosition;
                lastPosition = touchData.interactionPosition;
            }
        }
    }
}

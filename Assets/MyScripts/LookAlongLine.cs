using UnityEngine;
using UnityEngine.UI;

public class LookAlongLine : MonoBehaviour
{
    
    [SerializeField] GameObject mapReferenceCube;
    [SerializeField] GameObject touchPointIndicatorPrefab;
    [SerializeField] Transform mapRootTransform;
    [SerializeField] Button jumpToPointButton;
    [SerializeField] Button resetViewPositionButton;

    GameObject touchPointIndicator;
    Vector3 initialMapPosition;
    Vector3 targetPoint;
    bool moveToPoint;


    void Start()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnTouchInputStart;

        jumpToPointButton.onClick.AddListener(OnJumpButtonPressed);
        resetViewPositionButton.onClick.AddListener(OnResetViewPositionButtonPressed);
        
        initialMapPosition = mapRootTransform.position;
        moveToPoint = false;
    }

    void OnTouchInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {   
        if(touchPointIndicator != null)
        {
            GameObject.Destroy(touchPointIndicator);
        }

        // Allow selection of a position if it is a part of a map and spawn the inidcator GO at that position
        if(targetObj.name.Contains("FadeLine") || targetObj.name.Contains("MapPanReferenceCube") || targetObj.name.Contains("AbstractMap"))
        {
            touchPointIndicator = GameObject.Instantiate(touchPointIndicatorPrefab);
            interactionPos.y = Mathf.Max(interactionPos.y, mapReferenceCube.transform.position.y + mapReferenceCube.transform.localScale.y/2);
            touchPointIndicator.transform.position = interactionPos;
        }
    }

    void OnJumpButtonPressed()
    {   
        if(touchPointIndicator == null)
        {
            Debug.LogError("ERROR: Cannot jump to point, no location has been chosen");
            NotificationPopup popupWindow = new NotificationPopup();
            popupWindow.Show("First select a point where you want to jump to.");
            return;
        }

        Vector3 selectedPos = touchPointIndicator.transform.position;
        Vector3 translationVector = CustomHeadTracking.GetHeadPosition() - selectedPos;
        targetPoint = mapRootTransform.position + translationVector;
        moveToPoint = true;

        GameObject.Destroy(touchPointIndicator);
    }

    void OnResetViewPositionButtonPressed()
    {
        targetPoint = initialMapPosition;
        moveToPoint = true;
    }

    void Update()
    {
        if (Vector3.Distance(mapRootTransform.position, targetPoint) > .001f && moveToPoint)
        {
            // Smoothly move the camera towards the target position
            mapRootTransform.position = Vector3.Lerp(mapRootTransform.position, targetPoint, 5f * Time.deltaTime);
        }
        else if(moveToPoint)
        {   
            mapRootTransform.position = targetPoint;
            moveToPoint = false;
        }
        else
        {
            moveToPoint = false;
        }
    }

}

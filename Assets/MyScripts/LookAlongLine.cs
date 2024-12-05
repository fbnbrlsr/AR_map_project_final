using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Unity.VisualScripting;
using UnityEngine;

public class LookAlongLine : MonoBehaviour
{
    
    [SerializeField] GameObject mapReferenceCube;
    [SerializeField] GameObject touchPointIndicatorPrefab;
    [SerializeField] Transform mapRootTransform;
    [SerializeField] GameObject jumpToPointButton;
    [SerializeField] GameObject resetMapPositionButton;

    GameObject touchPointIndicator;
    Vector3 initialMapPosition;
    Vector3 targetPoint;
    bool moveToPoint;


    void Start()
    {
        InputEventsInvoker.InputEventTypes.HandSingleIPinchStart += OnTouchInputStart;
        //InputEventsInvoker.InputEventTypes.HandSingleInputCont += OnTouchInputCont;
        
        initialMapPosition = mapRootTransform.position;
        moveToPoint = false;
    }

    void OnTouchInputStart(Vector3 fingerPos, Vector3 interactionPos, Quaternion initRot, GameObject targetObj)
    {   
        // Jump to point if a position has been selected
        if(targetObj.transform.IsChildOf(jumpToPointButton.transform))
        {   
            if(touchPointIndicator == null)
            {
                Debug.LogError("ERROR: Cannot jump to point, no location has been chosen");
                NotificationPopup popupWindow = new NotificationPopup();
                popupWindow.Show("First select a point where you want to jump to.");
                return;
            }
            
            OnJumpButtonPressed(touchPointIndicator.transform.position);

            GameObject.Destroy(touchPointIndicator);
            return;
        }

        // Restore initial map position
        if(targetObj.transform.IsChildOf(resetMapPositionButton.transform))
        {
            OnResetButtonPressed();
            return;
        }

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

    void OnJumpButtonPressed(Vector3 selectedPos)
    {
        //Vector3 diffVector = CustomHeadTracking.GetHeadPosition() - selectedPos;
        //mapRootTransform.position += diffVector;

        Vector3 translationVector = CustomHeadTracking.GetHeadPosition() - selectedPos;
        targetPoint = mapRootTransform.position + translationVector;
        moveToPoint = true;
    }

    void OnResetButtonPressed()
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

using System.Collections.Generic;
using UnityEngine;

public class TimePoleLookAt : MonoBehaviour
{
    
    /*
    *   This class adjusts the orientation of the time labels such that they are 
    *   always facing the user's head.
    */

    [SerializeField] List<GameObject> timePoleLabels;
    [SerializeField] GameObject heightHandleInstance;

    void Update()
    {   
        Vector3 directionVectorHandle = new Vector3(heightHandleInstance.transform.position.x, heightHandleInstance.transform.position.y, CustomHeadTracking.GetHeadPosition().z);
        heightHandleInstance.transform.rotation = Quaternion.LookRotation(heightHandleInstance.transform.position - directionVectorHandle);

        foreach(GameObject obj in timePoleLabels)
        {   
            Vector3 directionVectorLabel = new Vector3(obj.transform.position.x, obj.transform.position.y, CustomHeadTracking.GetHeadPosition().z);
            obj.transform.rotation = Quaternion.LookRotation(obj.transform.position - directionVectorLabel);
        }
    }
}

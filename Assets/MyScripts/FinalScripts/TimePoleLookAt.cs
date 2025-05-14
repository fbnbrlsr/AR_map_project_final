using System.Collections.Generic;
using UnityEngine;

public class TimePoleLookAt : MonoBehaviour
{
    
    [SerializeField] List<GameObject> timePoleLabels;
    [SerializeField] GameObject heightHandleInstance;

    void Update()
    {   
        //Vector3 directionVectorHandle = new Vector3(CustomHeadTracking.GetHeadPosition().x, heightHandleInstance.transform.position.y, CustomHeadTracking.GetHeadPosition().z);
        Vector3 directionVectorHandle = new Vector3(heightHandleInstance.transform.position.x, heightHandleInstance.transform.position.y, CustomHeadTracking.GetHeadPosition().z);
        heightHandleInstance.transform.rotation = Quaternion.LookRotation(heightHandleInstance.transform.position - directionVectorHandle);

        foreach(GameObject obj in timePoleLabels)
        {   
            //Vector3 directionVectorLabel = new Vector3(CustomHeadTracking.GetHeadPosition().x, obj.transform.position.y, CustomHeadTracking.GetHeadPosition().z);
            Vector3 directionVectorLabel = new Vector3(obj.transform.position.x, obj.transform.position.y, CustomHeadTracking.GetHeadPosition().z);
            obj.transform.rotation = Quaternion.LookRotation(obj.transform.position - directionVectorLabel);
        }
    }
}

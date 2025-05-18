using UnityEngine;
using UnityEngine.XR;

public class CustomHeadTracking
{   
    /*
    *   Returns the position and rotation of the users head in the current frame.
    */

    public static Vector3 GetHeadPosition()
    {
        Vector3 headPos = -2*Vector3.forward;
        if (XRSettings.isDeviceActive)
        {
            InputDevice headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);

            if (headDevice.isValid && headDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 headPosTmp))
            {
                headPos = headPosTmp;
            }
        }
        return headPos;
    }

    public static Quaternion GetHeadRotation()
    {
#if UNITY_EDITOR
        return new Quaternion(0f, 0f, 0f, 1f);
#else
        Quaternion headRot = Quaternion.identity;
        if (XRSettings.isDeviceActive)
        {
            InputDevice headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);

            if (headDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion headRotTmp))
            {
                headRot = headRotTmp;
            }
        }
        return headRot;
#endif
    }
}

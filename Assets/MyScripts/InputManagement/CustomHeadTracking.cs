using UnityEngine;
using UnityEngine.XR;

public class CustomHeadTracking
{   
    public static Vector3 GetHeadPosition()
    {
        Vector3 headPos = Vector3.zero;
        if (XRSettings.isDeviceActive)
        {
            // Get the position and rotation of the XR HMD (headset)
            InputDevice headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);

            // Retrieve head position
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
            // Get the position and rotation of the XR HMD (headset)
            InputDevice headDevice = InputDevices.GetDeviceAtXRNode(XRNode.Head);

            // Retrieve head position
            if (headDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion headRotTmp))
            {
                headRot = headRotTmp;
            }
        }
        return headRot;
#endif
    }
}

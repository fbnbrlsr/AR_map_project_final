using Mapbox.Unity.Map;
using UnityEngine;

public class MapTilting : MonoBehaviour
{   

    /*
    *   This class enables tilting the map around one axis.
    */
    
    [SerializeField] GameObject mapRoot;
    [SerializeField] AbstractMap map;

    public static float tiltAngleDeg;
    public static float tiltAngleRad => tiltAngleDeg * Mathf.PI / 180f;

    private void OnMapTiltSliderChanged(float degrees)
    {   
        tiltAngleDeg = -degrees;
        mapRoot.transform.rotation = Quaternion.Euler(tiltAngleDeg, 0f, 0f);
        map.UpdateMap();
    }

    public void SetAngle(float deg)
    {   
        OnMapTiltSliderChanged(360f-deg);
    }


}

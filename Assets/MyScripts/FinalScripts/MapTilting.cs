using System;
using Mapbox.Unity.Map;
using UnityEngine;
using UnityEngine.UI;

public class MapTilting : MonoBehaviour
{
    
    [SerializeField] GameObject mapRoot;
    [SerializeField] Slider mapTiltSlider;
    [SerializeField] AbstractMap _map;

    public static float tiltAngleDeg;
    public static float tiltAngleRad => tiltAngleDeg * Mathf.PI / 180f;

    void Start()
    {
        mapTiltSlider?.onValueChanged.AddListener(OnMapTiltSliderChanged);
        if(mapTiltSlider != null) tiltAngleDeg = mapTiltSlider.value;
    }

    private void OnMapTiltSliderChanged(float degrees)
    {   
        if(mapTiltSlider != null) mapTiltSlider.value = degrees;
        tiltAngleDeg = -degrees;
        mapRoot.transform.rotation = Quaternion.Euler(tiltAngleDeg, 0f, 0f);
        _map.UpdateMap();
    }

    public void SetAngle(float deg)
    {   
        OnMapTiltSliderChanged(360f-deg);
    }


}

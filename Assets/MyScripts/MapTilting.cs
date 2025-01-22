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
        mapTiltSlider.onValueChanged.AddListener(OnMapTiltSliderChanged);
        tiltAngleDeg = mapTiltSlider.value;
    }

    private void OnMapTiltSliderChanged(float degrees)
    {
        mapRoot.transform.rotation = Quaternion.Euler(-degrees, 0f, 0f);
        tiltAngleDeg = -degrees;
        _map.UpdateMap();
    }


}

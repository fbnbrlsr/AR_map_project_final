using Mapbox.Unity.Map;
using TMPro;
using UnityEngine;

public class rm_ChooseMapStyle : MonoBehaviour
{
    /*[SerializeField] TMP_Dropdown mapStyleDropdown;
    [SerializeField] AbstractMap map;

    ImagerySourceType mapImageSourceType;
    ImageryLayer il;

    void Start()
    {   
        if(mapStyleDropdown != null) mapStyleDropdown.onValueChanged.AddListener(OnMapStyleChanged);

        il = (ImageryLayer) map.ImageLayer;
        mapImageSourceType = il.LayerProperty.sourceType;

        Debug.Log("Map type is " +il.LayerProperty.sourceType);
    }

    private void OnMapStyleChanged(int arg0)
    {   
        string type = mapStyleDropdown.options[mapStyleDropdown.value].text;
        if(type.Equals("Streets"))
        {
            mapImageSourceType = ImagerySourceType.MapboxStreets;
        }
        else if(type.Equals("Outdoors"))
        {
            mapImageSourceType = ImagerySourceType.MapboxOutdoors;
        }
        else if(type.Equals("Dark"))
        {
            mapImageSourceType = ImagerySourceType.MapboxDark;
        }
        else if(type.Equals("Light"))
        {
            mapImageSourceType = ImagerySourceType.MapboxLight;
        }
        else if(type.Equals("Satellite"))
        {
            mapImageSourceType = ImagerySourceType.MapboxSatellite;
        }
        else if(type.Equals("SatelliteStreets"))
        {
            mapImageSourceType = ImagerySourceType.MapboxSatelliteStreet;
        }
        else
        {
            Debug.LogError("[ChooseMapStyle] Image source style does not exist!");
        }

        il.SetLayerSource(mapImageSourceType);
    }*/
}

using System;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;

public class rm_EmbeddedAnnotation : MonoBehaviour
{   
    /*[SerializeField] GameObject instance;
    [SerializeField] AbstractMap map;
    [SerializeField] float lonCoord;
    [SerializeField] float latCoord;
    [SerializeField] float heightCoord;
    Vector3 worldPosition = Vector3.one;
    Quaternion initRotation;
    float absoluteDistance = 1f;

    void Start()
    {   
        initRotation = instance.transform.rotation;
        instance.SetActive(false);
        map.OnUpdated += UpdateWorldCoordinates;
        DynamicTimePlane.TimePlaneChanged += UpdateWorldCoordinates;
    }

    
    public void SetVisibility(bool b)
    {
        instance.SetActive(b);
        if(b) UpdateWorldCoordinates();
    }

    public void UpdateWorldCoordinates()
    {   
        absoluteDistance = CustomReloadMap.GetReferenceDistance();

        Vector3 planePos = CalculateWorldPlaneCoordinates(latCoord, lonCoord);
        float worldHeight = CalculateWorldHeight(heightCoord);
        worldPosition = planePos + Vector3.up * worldHeight;
        float tiltAngle = MapTilting.tiltAngleRad;
        worldPosition = RotateByAngle(worldPosition, tiltAngle);

        if(instance != null && instance.activeSelf)
        {
            instance.transform.position = worldPosition + K_TwoPointLineVisualizer.globalMapRoot.position;
            instance.transform.rotation = initRotation * Quaternion.Euler(new Vector3(-MapTilting.tiltAngleDeg, 0f, 0f));
        }
    }

    private Vector3 CalculateWorldPlaneCoordinates(float lat, float lon)
    {   
        Vector2d latLonCoords = new Vector2d(lat, lon);
        return MyGeoToWorldPosition(latLonCoords);
    }

    private Vector3 MyGeoToWorldPosition(Vector2d latlon)
    {   
        var scaleFactor = Mathf.Pow(2, map.InitialZoom - map.AbsoluteZoom);
        var worldPos = Conversions.GeoToWorldPosition(latlon, map.CenterMercator, map.WorldRelativeScale * scaleFactor).ToVector3xz();
        return worldPos / 10f * absoluteDistance / K_DatabaseLegData.initAbsoluteDistance;
    }

    private float CalculateWorldHeight(float seconds)
    {   
        float timeDiff = seconds - K_DatabaseLegData.earliestTime;
        float frac = 1f * timeDiff / (K_DatabaseLegData.latestTime - K_DatabaseLegData.earliestTime);
        float realHeight = (K_DatabaseLegData.minPointHeight + (K_DatabaseLegData.maxPointHeight - K_DatabaseLegData.minPointHeight) * frac) * absoluteDistance / 2;
        return realHeight;
    }

    private Vector3 RotateByAngle(Vector3 v, float angle)
    {
        float y = v.y * Mathf.Cos(angle) - v.z * Mathf.Sin(angle);
        float z = v.y * Mathf.Sin(angle) + v.z * Mathf.Cos(angle);
        return new Vector3(v.x, y,  z);
    }*/

}

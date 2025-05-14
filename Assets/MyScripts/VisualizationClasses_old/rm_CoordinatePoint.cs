using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using Unity.VisualScripting;
using UnityEngine;

public class CoordinatePoint
{
    private Vector2d coordinates;
    private Vector3 worldPosition;
    private Vector3 dimensionScales;
    private GameObject pointPrefab;
    private Material pointMaterial;
    private AbstractMap _map;

    private GameObject instance;
    private bool isVisible;
    private float height;

    public CoordinatePoint(AbstractMap map, Vector2d coordinates, Vector3 scale, GameObject pointPrefab)
    {   
        this._map = map;
        this.coordinates = coordinates;
        this.worldPosition = _map.GeoToWorldPosition(coordinates, true);
        this.dimensionScales = scale;
        this.pointPrefab = pointPrefab;
        this.pointMaterial = pointPrefab.GetComponent<Material>();
        this.isVisible = true;
    }

    public void InstantiatePoint(GameObject instance)
    {
        this.instance = instance;
    }

    public void UpdatePoint()
    {   
        if(isVisible)
        {   
            worldPosition = _map.GeoToWorldPosition(coordinates, true) + height*Vector3.up;
            instance.transform.localPosition = worldPosition;
            instance.transform.localScale = dimensionScales;
        }
    }

    public void SetScale(Vector3 scale)
    {
        this.dimensionScales = scale;
    }

    public Vector3 GetPosition()
    {
        return worldPosition;
    }

    public void SetHeight(float h)
    {
        this.height = h;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

public class StartEndLine_old
{
    private CoordinatePoint startPoint;
    private CoordinatePoint endPoint;
    private Vector3 dimensionScales;
    private GameObject linePrefab;
    private Material lineMaterial;
    private AbstractMap _map;

    private GameObject instance;
    private float scale;
    private bool isVisible;
    private float height;

    public StartEndLine_old(AbstractMap map, CoordinatePoint s, CoordinatePoint e, Vector3 scale, GameObject linePrefab)
    {
        this._map = map;
        this.startPoint = s;
        this.endPoint = e;
        this.dimensionScales = scale;
        this.linePrefab = linePrefab;
        this.scale = .005f;
        isVisible = true;
    }

    public void InstantiateLine(GameObject instance)
    {
        this.instance = instance;
    }

    public void UdpateLine()
    {
        if(isVisible)
        {   
            Vector3 startWorldPos = startPoint.GetPosition();
            Vector3 endWorldPos = endPoint.GetPosition();

            float dist = Vector3.Distance(startWorldPos, endWorldPos);
            Vector3 linePos = startWorldPos/2 + endWorldPos/2;
            Quaternion lineRot = Quaternion.LookRotation(startWorldPos - endWorldPos);

            instance.transform.position = startWorldPos/2 + endWorldPos/2;
            instance.transform.rotation = Quaternion.LookRotation(startWorldPos - endWorldPos);
            instance.transform.localScale = new Vector3(scale, scale, dist);
        }
    }

    public void SetHeight(float h)
    {
        this.height = h;
    }

}

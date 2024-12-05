using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;
using System.Diagnostics.SymbolStore;
using UnityEngine.Splines;
using Mapbox.Unity.MeshGeneration.Filters;
using System.Linq;

public class ArcLine
{   
    private GameObject instance;
    public GameObject Instance => instance;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 dimensionScales;
    private bool isVisible;
    private float arcHeight;

    public ArcLine(GameObject instance, Vector3 startPoint, Vector3 endPoint, Vector3 dimensionScales)
    {
        this.instance = instance;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.dimensionScales = dimensionScales;
        isVisible = true;
    }

    public ArcLine(GameObject instance, Vector3 startPoint, Vector3 endPoint)
    {
        this.instance = instance;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.dimensionScales = instance.transform.localScale;
        isVisible = true;
    }

    public void Update(Vector3 newStartPoint, Vector3 newEndPoint)
    {   
        this.startPoint = newStartPoint;
        this.endPoint = newEndPoint;

        float dist = Vector3.Distance(startPoint, endPoint);
        instance.transform.localPosition = startPoint/2 + endPoint/2;
        instance.transform.rotation = Quaternion.LookRotation(startPoint - endPoint);
        instance.transform.localScale = new Vector3(dimensionScales.x, arcHeight*dist, dist);

        instance.SetActive(isVisible);
    }

    public void SetVisibility(bool b)
    {
        isVisible = b;
    }

    public void SetArcHeight(float h)
    {
        this.arcHeight = h;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Unity.VisualScripting;
using UnityEngine;

public class PointPath
{   
    private CoordinatePoint startPoint;
    private CoordinatePoint midPoint;
    private CoordinatePoint endPoint;
    private StartEndLine_old line1;
    private StartEndLine_old line2;
    private GameObject linePrefab;
    private Material lineMaterial;
    private AbstractMap _map;
    
    private float scale;
    private float height;
    private bool isVisible;

    public PointPath(AbstractMap map, CoordinatePoint s, CoordinatePoint m, CoordinatePoint e, StartEndLine_old l1, StartEndLine_old l2)
    {
        this.startPoint = s;
        this.midPoint = m;
        this.endPoint = e;
        this.line1 = l1;
        this.line2 = l2;
        isVisible = true;
    }

    public void InstantiatePath(GameObject s, GameObject m, GameObject e, GameObject l1, GameObject l2)
    {
        startPoint.InstantiatePoint(s);
        midPoint.InstantiatePoint(m);
        endPoint.InstantiatePoint(e);
        line1.InstantiateLine(l1);
        line2.InstantiateLine(l2);
    }

    public void UpdatePath()
    {   
        startPoint.UpdatePoint();
        midPoint.UpdatePoint();
        endPoint.UpdatePoint();
        line1.UdpateLine();
        line2.UdpateLine();
    }

    public void SetHeight(float h)
    {
        this.height = h;
        startPoint.SetHeight(h);
        midPoint.SetHeight(h);
        endPoint.SetHeight(h);
        line1.SetHeight(h);
        line2.SetHeight(h);
    }

}

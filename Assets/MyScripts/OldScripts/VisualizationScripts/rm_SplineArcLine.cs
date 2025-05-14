using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SplineArcLine
{   
    private GameObject instance;
    private Spline spline;
    public GameObject Instance => instance;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 dimensionScales;
    private bool isVisible;
    private float arcHeight;
    private float mapZoom;
    private bool isSelected;

    // Line parameters
    private float lineRadius = .005f;
    private int arcSegements = 15;
    private int circumferenceSegments = 3;

    public SplineArcLine(GameObject instance, Vector3 startPoint, Vector3 endPoint, Vector3 dimensionScales)
    {
        this.instance = instance;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.dimensionScales = dimensionScales;
        isVisible = true;
        isSelected = false;

        CreateNewSpline();
    }

    public SplineArcLine(GameObject instance, Vector3 startPoint, Vector3 endPoint)
    {
        this.instance = instance;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.dimensionScales = instance.transform.localScale;
        isVisible = true;

        CreateNewSpline();
    }

    private void CreateNewSpline()
    {   
        List<BezierKnot> knots = new List<BezierKnot>();
        knots.Add(new BezierKnot(startPoint));
        knots.Add(new BezierKnot(Vector3.up));
        knots.Add(new BezierKnot(endPoint));
        spline = new Spline(knots);

        MeshFilter mf = instance.GetComponent<MeshFilter>();
        Mesh m = new Mesh();
        SplineMesh.Extrude(spline, m, lineRadius, circumferenceSegments, arcSegements);

        mf.mesh = m;
    }

    public void Update(Vector3 newStartPoint, Vector3 newEndPoint)
    {   
        this.startPoint = newStartPoint;
        this.endPoint = newEndPoint;
        float dist = Vector3.Distance(startPoint, endPoint);
        Vector3 midPoint = startPoint/2 + endPoint/2 + Vector3.up * arcHeight * dist;

        // Update spline
        Vector3 tang = (endPoint - startPoint) * .2f;
        BezierKnot b0 = new BezierKnot(startPoint, Vector3.zero, Vector3.zero, Quaternion.Euler(90, 0, 0));
        BezierKnot b1 = new BezierKnot(midPoint, -tang, tang);
        BezierKnot b2 = new BezierKnot(endPoint, Vector3.zero, Vector3.zero, Quaternion.Euler(270, 0, 0));
        spline.SetKnot(0, b0);
        spline.SetKnot(1, b1);
        spline.SetKnot(2, b2);

        // Set arc shape and color
        if(instance == null)
        {
            Debug.Log("ERROR: Instance is null!");
            return;
        }
        MeshFilter mf = instance.GetComponent<MeshFilter>();
        MeshRenderer mr = instance.GetComponent<MeshRenderer>();
        MeshCollider mc = instance.GetComponent<MeshCollider>();
        if(mr != null && mf != null && mc != null)
        {      
            Mesh m = new Mesh();
            SplineMesh.Extrude(spline, m, lineRadius, circumferenceSegments, arcSegements);

            Material mat = new Material(mr.material);
            float arclength = 1f / spline.GetLength();
            mat.mainTextureScale = new Vector2(0f, arclength);

            mr.material = mat;
            mf.mesh = m;
            mc.sharedMesh = m;
        }
    }

    public void SetVisibility(bool b)
    {
        isVisible = b;
    }

    public void SetArcHeight(float h)
    {   
        this.arcHeight = h;
    }

    public void SetMapZoom(float z)
    {
        this.mapZoom = z;
    }

    public void SetSelected(bool b)
    {
        isSelected = b;
    }

    public void Destroy()
    {
        GameObject.Destroy(instance);
    }

}

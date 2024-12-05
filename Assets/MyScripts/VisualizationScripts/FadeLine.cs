using UnityEngine;

public class FadeLine
{   
    private GameObject instance;
    public GameObject Instance => instance;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 dimensionScales;
    private bool isVisible;
    private bool isSelected;

    private int id;

    public FadeLine(int id, GameObject instance, Vector3 startPoint, Vector3 endPoint, Vector3 dimensionScales)
    {   
        this.id = id;
        this.instance = instance;
        instance.name = "FadeLine" + id;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.dimensionScales = dimensionScales;
        isVisible = true;
        isSelected = false;
    }

    public FadeLine(int id, GameObject instance, Vector3 startPoint, Vector3 endPoint)
    {   
        this.id = id;
        this.instance = instance;
        instance.name = "FadeLine" + id;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.dimensionScales = instance.transform.localScale;
        isVisible = true;
    }

    public void Update(Vector3 newStartPoint, Vector3 newEndPoint)
    {   
        this.startPoint = newStartPoint;
        this.endPoint = newEndPoint;

        if(!InViewingRange(startPoint) && !InViewingRange(endPoint)){
            instance.SetActive(false);
            return;
        }
        instance.SetActive(true);

        float dist = Vector3.Distance(startPoint, endPoint);
        instance.transform.position = startPoint/2 + endPoint/2;
        Quaternion correctionRot = Quaternion.AngleAxis(-90, Vector3.up);
        instance.transform.rotation = Quaternion.LookRotation(startPoint - endPoint) * correctionRot;
        instance.transform.localScale = new Vector3(dist, dimensionScales.y, dimensionScales.z);
    }

    public void SetVisibility(bool b)
    {
        isVisible = b;
    }

    public void SetSelected(bool b)
    {
        this.isSelected = b;
    }

    private bool InViewingRange(Vector3 p)
    {
        if(p.x < 5f && p.x > -5f && p.z < 5f && p.z > -5f) return true;
        return false;
    }

    public void Destroy()
    {
        GameObject.Destroy(instance);
    }

}

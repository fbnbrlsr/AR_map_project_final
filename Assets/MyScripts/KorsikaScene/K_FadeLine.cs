using UnityEngine;

public class K_FadeLine
{   
    public GameObject instance;
    public GameObject Instance => instance;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 dimensionScales;
    private bool isVisible;
    private bool isSelected;
    Material originalMaterial;
    public static Material selectedMaterial;

    private int id;

    public K_FadeLine(int id, GameObject instance, Vector3 startPoint, Vector3 endPoint, Vector3 dimensionScales)
    {   
        this.id = id;
        this.instance = instance;
        instance.name = "FadeLine" + id;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.dimensionScales = dimensionScales;

        originalMaterial = instance.GetComponent<MeshRenderer>().sharedMaterial;
        isVisible = true;
        isSelected = false;

        Debug.Log("CREATED FADELINE");
    }

    public K_FadeLine(int id, GameObject instance, Vector3 startPoint, Vector3 endPoint)
    {   
        this.id = id;
        this.instance = instance;
        instance.name = "FadeLine" + id;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.dimensionScales = instance.transform.localScale;
        isVisible = true;

        originalMaterial = instance.GetComponent<MeshRenderer>().sharedMaterial;
        isVisible = true;
        isSelected = false;

        Debug.Log("CREATED FADELINE");
    }

    public void Update(Vector3 newStartPoint, Vector3 newEndPoint)
    {   
        this.startPoint = newStartPoint + K_TwoPointLineVisualizer.globalMapRoot.position;
        this.endPoint = newEndPoint + K_TwoPointLineVisualizer.globalMapRoot.position;

        /*if(!InViewingRange(startPoint) && !InViewingRange(endPoint)){
            instance.SetActive(false);
            return;
        }
        instance.SetActive(true);*/

        float dist = Vector3.Distance(startPoint, endPoint);
        instance.transform.localPosition = startPoint/2 + endPoint/2 /* - instance.transform.parent.position*/;
        Quaternion correctionRot = Quaternion.AngleAxis(-90, Vector3.up);
        if(startPoint != endPoint) instance.transform.rotation = Quaternion.LookRotation(startPoint - endPoint) * correctionRot;
        instance.transform.localScale = new Vector3(dist, dimensionScales.y, dimensionScales.z);
    }

    public void SetVisibility(bool b)
    {
        isVisible = b;
    }

    public void SetSelected(bool b)
    {   
        if(this.isSelected == b) return;

        this.isSelected = b;
        if(isSelected)
        {
            instance.GetComponent<MeshRenderer>().material = selectedMaterial;
        }
        else
        {
            instance.GetComponent<MeshRenderer>().material = originalMaterial;
        }
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

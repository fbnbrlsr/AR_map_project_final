using UnityEngine;

public class LegLine
{   

    /*
    *   This class manages the transformation of a line object visualization the
    *   data of a single leg.
    *   The end points of the line represent the origin and desination of the leg. The height of the
    *   endpoints is proportional to the start and end times of the leg.
    *   The line is implemented as a cube object in Unity that is stretched and rotated as desired.
    */
        
    public GameObject instance;
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 dimensionScales;
    private bool isSelected;
    Material originalMaterial;
    public static Material selectedMaterial;

    public LegLine(int id, GameObject instance, Vector3 startPoint, Vector3 endPoint)
    {   
        this.instance = instance;
        instance.name = "FadeLine" + id;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.dimensionScales = instance.transform.localScale;

        originalMaterial = instance.GetComponent<MeshRenderer>().sharedMaterial;
        isSelected = false;
    }

    public void Update(Vector3 newStartPoint, Vector3 newEndPoint)
    {   
        startPoint = newStartPoint + TwoPointLineVisualizer.globalMapRoot.position;
        endPoint = newEndPoint + TwoPointLineVisualizer.globalMapRoot.position;

        float dist = Vector3.Distance(startPoint, endPoint);
        instance.transform.localPosition = startPoint/2 + endPoint/2;
        Quaternion correctionRot = Quaternion.AngleAxis(-90, Vector3.up);
        if(startPoint != endPoint) instance.transform.rotation = Quaternion.LookRotation(startPoint - endPoint) * correctionRot;
        instance.transform.localScale = new Vector3(dist, dimensionScales.y, dimensionScales.z);
    }

    public void SetSelected(bool b)
    {   
        if(isSelected == b) return;

        isSelected = b;
        if(isSelected)
        {
            instance.GetComponent<MeshRenderer>().material = selectedMaterial;
        }
        else
        {
            instance.GetComponent<MeshRenderer>().material = originalMaterial;
        }
    }

    public void Destroy()
    {
        GameObject.Destroy(instance);
    }

}

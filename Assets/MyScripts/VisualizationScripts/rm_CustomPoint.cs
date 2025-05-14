using UnityEngine;

public class CustomPoint
{
    private GameObject instance;
    public GameObject Instance => instance;
    private Vector3 worldPosition;
    private Vector3 dimensionScales;
    private bool isVisible;
    private bool isSelected;

    public CustomPoint(GameObject instance, Vector3 worldPosition, Vector3 dimensionScales)
    {   
        this.instance = instance;
        this.worldPosition = worldPosition;
        this.dimensionScales = dimensionScales;
        isVisible = true;
    }

    public CustomPoint(GameObject instance, Vector3 worldPosition)
    {   
        this.instance = instance;
        this.worldPosition = worldPosition;
        this.dimensionScales = instance.transform.localScale;
        isVisible = true;
        isSelected = false;
    }

    public void Update(Vector3 newWorldPosition)
    {   
        this.worldPosition = newWorldPosition;
        if(instance == null)
        {
            Debug.Log("ERROR: Instance is null!");
            return;
        }
        instance.transform.localPosition = worldPosition;

        instance.SetActive(isVisible);
    }

    public void SetScale(Vector3 scale)
    {
        this.dimensionScales = scale;
    }

    public void SetVisibility(bool b)
    {
        isVisible = b;
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

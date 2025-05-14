using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;

public class PathsContainerObject : MonoBehaviour
{
    
    [SerializeField] AbstractMap _map;

    private float initRefDistance;
    private Vector2d containerGeoPosition;
    private float initContainerHeight;

    void Start()
    {
        containerGeoPosition = _map.WorldToGeoPosition(this.transform.position);
        initContainerHeight = this.transform.localPosition.y;

        initRefDistance = CustomReloadMap.GetReferenceDistance();
    }

    public void UpdateContainer()
    {   
        float scalingFactor = CustomReloadMap.GetReferenceDistance() / this.initRefDistance;

        initContainerHeight = this.transform.position.y - this.transform.parent.position.y;
        Vector3 containerWorldPos = _map.GeoToWorldPosition(containerGeoPosition);
        this.transform.position = containerWorldPos + Vector3.up * initContainerHeight * scalingFactor;
        this.transform.localScale *= scalingFactor;

        initRefDistance = CustomReloadMap.GetReferenceDistance();
    }

    public Transform GetTransform()
    {
        return this.transform;
    }
}

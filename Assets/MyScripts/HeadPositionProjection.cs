using System.Collections;
using System.Collections.Generic;
using Mapbox.Examples.Voxels;
using UnityEngine;

public class HeadPositionProjection : MonoBehaviour
{
    
    [SerializeField] GameObject positionIndicatorPrefab;
    [SerializeField] GameObject mapCubeInstance;

    GameObject indicatorInstance;

    void Start()
    {
        indicatorInstance = GameObject.Instantiate(positionIndicatorPrefab);
        indicatorInstance.SetActive(false);
    }

    void Update()
    {
        Vector3 headPosition = CustomHeadTracking.GetHeadPosition();

        if(IsOverMap(headPosition, mapCubeInstance))
        {   
            float height = mapCubeInstance.transform.position.y + mapCubeInstance.transform.localScale.y + 0.01f;
            indicatorInstance.SetActive(true);
            indicatorInstance.transform.position = new Vector3(headPosition.x, height, headPosition.z);
        }
        else
        {
            indicatorInstance.SetActive(false);
        }

    }

    bool IsOverMap(Vector3 headPos, GameObject cube)
    {
        float cubeSizeX = cube.transform.localScale.x;
        float cubeSizeZ = cube.transform.localScale.z;
        Vector3 cubePos = cube.transform.position;

        bool isInX = headPos.x > cubePos.x - cubeSizeX && headPos.x < cubePos.x + cubeSizeX;
        bool isInZ = headPos.z > cubePos.z - cubeSizeZ && headPos.z < cubePos.z + cubeSizeZ;

        return isInX && isInZ;
    }

}

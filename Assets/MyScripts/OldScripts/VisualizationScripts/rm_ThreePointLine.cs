using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

public class ThreePointLine
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
}

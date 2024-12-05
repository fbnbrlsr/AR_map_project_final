using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Location;
using Mapbox.Unity.Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PathProjection : MonoBehaviour
{
    
    [SerializeField] Slider timeHeightMultiplierSlider;
    [SerializeField] AbstractMap map;
    


    void Start()
    {
        if(timeHeightMultiplierSlider != null)
        {
            timeHeightMultiplierSlider.onValueChanged.AddListener(SetMapHeight);
        }
    }

    private void SetMapHeight(float val)
    {
        K_DatabaseLegData.timeHeightMultiplier = val;
        map.UpdateMap();
    }

}

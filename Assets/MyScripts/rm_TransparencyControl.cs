using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rm_TransparencyControl : MonoBehaviour
{
    
    /*[SerializeField] Slider transparencySlider;

    Renderer[] renderers;
    Hashtable matList = new Hashtable();

    void Start()
    {
        transparencySlider.onValueChanged.AddListener(OnTransparencyChanged);

        renderers = GetComponentsInChildren<Renderer>();
        foreach(Renderer r in renderers){
            matList.Add(r, r.material);
            Debug.Log("[TransparencyControl] Collected material " + r.material + " from renderer " + r);
        }
    }

    private void OnTransparencyChanged(float val)
    {   
        foreach(DictionaryEntry d in matList)
        {
            
            Material m = (Material) d.Value;
            Color c = m.color;
            c.a = val;

            Renderer r = (Renderer) d.Key;
            r.material.color = c;
            
        }
    }*/

}

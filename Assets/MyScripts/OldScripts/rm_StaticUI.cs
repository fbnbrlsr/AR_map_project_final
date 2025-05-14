using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class rm_StaticUI : MonoBehaviour
{   

    /*[SerializeField] GameObject root;
    [SerializeField] GameObject pathColorLegendPrefab;
    [SerializeField] TMP_Text tmpTitleText;
    public string mapTitle;

    GameObject pathColorLegendInstance;
    Vector3 eyeDistance;

    static bool showLegend;

    void Start()
    {
        eyeDistance = new Vector3(0f, 0f, 1f);
        showLegend = false;

        GenerateLegend();
    }
    
    void Update()
    {
        Quaternion headRot = CustomHeadTracking.GetHeadRotation();
        Vector3 headPos = CustomHeadTracking.GetHeadPosition();

        Vector3 pos = headPos + headRot * eyeDistance;
        root.transform.position = pos;
        Quaternion zRot = Quaternion.Euler(0f, 0f, headRot.eulerAngles.z);
        root.transform.rotation = Quaternion.LookRotation(pos - headPos) * zRot;

        pathColorLegendInstance.SetActive(showLegend);
    }

    public static void ShowLegend()
    {   
        showLegend = true;
    }

    public static void HideLegend()
    {   
        showLegend = false;
    }

    void GenerateLegend()
    {
        pathColorLegendInstance = GameObject.Instantiate(pathColorLegendPrefab);
        pathColorLegendInstance.transform.parent = root.transform;
        pathColorLegendInstance.transform.localPosition = new Vector3(0f, -0.4f, 0f);

        TMP_Text[] texts = pathColorLegendInstance.GetComponentsInChildren<TMP_Text>();
        float[][] intervals = K_DatabaseLegData.GetTimeCategoryIntervals();

        for(int i = 0; i < texts.Length; i++)
        {
            TMP_Text text = texts[i];

            for(int j = 0; j < texts.Length; j++)
            {   
                if(text.name.Contains(j.ToString()))
                {
                    text.text = SecondsToPrettyTime((int) intervals[j][0]) + "\nto\n" + SecondsToPrettyTime((int) intervals[j][1]);
                }
            }
        }
        
        tmpTitleText.text = mapTitle;
    }

    string SecondsToPrettyTime(int seconds)
    {   
        int hours = seconds / 3600;
        seconds -= hours * 3600;
        int minutes = seconds/60;
        return hours + "h " + minutes + "min";
    }*/

}

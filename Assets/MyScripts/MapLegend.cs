using TMPro;
using UnityEngine;

public class MapLegend : MonoBehaviour
{   
    [SerializeField] GameObject mapHolder;
    [SerializeField] GameObject pathColorLegendPrefab;
    [SerializeField] TMP_Text titleText;
    public string mapTitle;

    static GameObject pathColorLegendInstance;

    static bool showLegend;


    void Awake()
    {
        showLegend = false;

        SetMapTitle(mapTitle);
        GenerateLegend();
        HideLegend();
    }

    public static void ShowLegend()
    {   
        showLegend = true;

        pathColorLegendInstance.SetActive(showLegend);
    }

    public static void HideLegend()
    {   
        showLegend = false;

        pathColorLegendInstance?.SetActive(showLegend);
    }

    void GenerateLegend()
    {
        pathColorLegendInstance = GameObject.Instantiate(pathColorLegendPrefab);
        pathColorLegendInstance.transform.parent = mapHolder.transform;
        pathColorLegendInstance.transform.localPosition = new Vector3(1.5f, 0.3f, 1f);
        pathColorLegendInstance.transform.localScale = new Vector3(3f, 3f, 3f);
        pathColorLegendInstance.transform.rotation = new Quaternion(0.382683426f,0f,0f,0.923879564f);


        /*TMP_Text[] texts = pathColorLegendInstance.GetComponentsInChildren<TMP_Text>();
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
        }*/
    }

    string SecondsToPrettyTime(int seconds)
    {       
        int hours = seconds / 3600;
        seconds -= hours * 3600;
        int minutes = seconds/60;
        return hours + "h " + minutes + "min";
    }

    void SetMapTitle(string title)
    {
        titleText.text = title;
    } 

}

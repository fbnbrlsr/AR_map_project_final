using TMPro;
using UnityEngine;

public class MapLegend : MonoBehaviour
{   

    /*
    *   This class sets the map title which is shown in on a large panel in the background of
    *   the interface.
    *   It also enables and disables the legends when paths are visualized. The legends describes
    *   the color coding of the path lines. The different colors represent the mode of travel.
    */

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
        pathColorLegendInstance.transform.localPosition = new Vector3(0f, 0.3f, 0.5f);
        pathColorLegendInstance.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        pathColorLegendInstance.transform.rotation = new Quaternion(0.382683426f,0f,0f,0.923879564f);
    }

    void SetMapTitle(string title)
    {
        titleText.text = title;
    } 

}

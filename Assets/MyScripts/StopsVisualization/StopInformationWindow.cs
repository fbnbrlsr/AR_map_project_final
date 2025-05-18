using TMPro;
using UnityEngine;

public class StopInformationWindow
{   

    /*
    *   This class spawns a window showing information about a stop column
    *   that was selected by the user. 
    *   The window is spawned at the touch point and oriented to face the user's head.
    */
    
    private GameObject instance;
    public GameObject prefab;

    // information about a column
    private int nof_stops;

    public StopInformationWindow(int nof_stops, GameObject prefab)
    {
        this.nof_stops = nof_stops;
        this.prefab = prefab;

        DataPathVisualizationManager.GlobalNewWindowSpawnedEvent += this.Hide;
    }

    public void Show(Vector3 worldPos, Vector3 headPos, int nof_stops)
    {   
        this.nof_stops = nof_stops;
        if(instance != null)
        {
            GameObject.Destroy(instance);
            return;
        }

        Quaternion rotation = Quaternion.LookRotation(worldPos - headPos);

        instance = GameObject.Instantiate(prefab);

        TMP_Text textField = instance.GetComponentInChildren<TMP_Text>();
        if(textField != null) textField.text = this.GenerateText(textField.text);

        instance.transform.position = worldPos;
        instance.transform.rotation = rotation;
    }

    private string GenerateText(string t)
    {
        string s = t.Split("\n")[0];
        s += nof_stops == 1 ? "\n" + nof_stops + " stop" : "\n" + nof_stops + " stops";
        return s;
    }

    public void Hide()
    {   
        GameObject.Destroy(instance);
    }

    public static void HideAll()
    {
        DataPathVisualizationManager.InvokeGlobalNewWindowSpawnedEvent();
    }

}

using System;
using TMPro;
using UnityEditor;
using UnityEngine;

public class rm_PathInformationPopup
{   

    /*private GameObject instance;
    public GameObject Instance => instance;
    public GameObject prefab;
    private Vector3 worldPosition;
    private Quaternion rotation;
    private DatabaseLegData leg;
    private static int id_c;
    private int id;

    public PathInformationPopup(DatabaseLegData leg, GameObject prefab)
    {
        this.leg = leg;
        this.prefab = prefab;

        this.id = id_c++;

        Debug.Log("Constructed PathInformationPanel");
    }
    
    public void Show(Vector3 worldPos, Vector3 headPos)
    {   
        if(instance != null) return;

        DebugPanel.Log("Called Show() of InformationPanel...");
        
        this.worldPosition = worldPos;
        this.rotation = Quaternion.LookRotation(worldPos - headPos);

        instance = GameObject.Instantiate(prefab);

        TMP_Text textField = instance.GetComponentInChildren<TMP_Text>();
        if(textField != null) textField.text = this.GenerateText();

        instance.transform.position = worldPos;
        instance.transform.rotation = rotation;

        DebugPanel.Log("Spawned at pos=" + worldPos + " (id=" + id + ")");
    }

    private string GenerateText()
    {   
        string t = "LEG ID: " + leg.leg_id;
        t += "\nStart date: " + leg.started_at;
        t += "\nEnd date: " + leg.finished_at;
        t += "\nDuration: " + leg.duration;
        t += "\nLength: " + leg.length;
        return t;
    }

    public void Hide()
    {
        GameObject.Destroy(instance);
        DebugPanel.Log("InformationPanel destroyed (id=" + id + ")");
    }*/
}

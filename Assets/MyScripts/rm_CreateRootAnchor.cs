using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class rm_CreateRootAnchor : MonoBehaviour
{   
    /*[SerializeField] ARAnchorManager ARAnchorManager;
    [SerializeField] GameObject anchorPrefab;
    ARAnchor anchor;
    bool anchorCreationState;
    void Start()
    {
        ARAnchorManager.trackablesChanged.AddListener(OnTrackablesChanged);

        anchorCreationState = false;
        CreateAnchorAsync(ARAnchorManager);
    }

    async void CreateAnchorAsync(ARAnchorManager manager)
    {
        var pose = new Pose(Vector3.zero, Quaternion.identity);

        if(manager == null) Debug.Log("ANCHOR MANAGER IS NULL");

        var result = await manager.TryAddAnchorAsync(pose);
        if (result.status.IsSuccess())
        {
            this.anchor = result.value;
            anchorCreationState = true;

            this.transform.SetParent(anchor.transform);
            this.transform.localPosition = Vector3.up;
            this.transform.localRotation = Quaternion.identity;

            Instantiate(anchorPrefab, anchor.transform.position, anchor.transform.rotation);
            Debug.Log("ANCHOR CREATION SUCCESSFUL");
        }
        else
        {
            Debug.Log("ANCHOR CREATION FAILED");
        }
    }

    void Update()
    {
        if(!anchorCreationState)
        {
            CreateAnchorAsync(ARAnchorManager);
        }
    }

    private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARAnchor> arg0)
    {
        foreach(var a in arg0.added)
        {
            Debug.Log("[CreateRootAnchor] Created: " + a);
        }
        foreach(var a in arg0.updated)
        {
            Debug.Log("[CreateRootAnchor] Updated: " + a);
        }
        foreach(var a in arg0.removed)
        {
            Debug.Log("[CreateRootAnchor] Removed: " + a);
        }
    }*/
}

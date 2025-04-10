using System.Collections.Generic;
using System.Linq;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using TMPro;
using UnityEngine;

public class WriteCommentWindow : MonoBehaviour, ICommentWindow
{   
    static int id = 0;

    [SerializeField] GameObject instanceObject;
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text contentText;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Dropdown commentClassDropdown;
    [SerializeField] List<GameObject> coloredElements;
    private string commentClass = "Default";
    public string CommentClass => commentClass;

    private AbstractMap _map;
    public static Transform root;
    public static Dictionary<string, Material> commentClasses;

    private float initCommentWindowHeight;
    private Vector2d windowGeoPosition;

    // Comment box information
    GameObject commentBoxInstance;
    private float initRefDistance;
    private Vector2d boxGeoPosition;
    private float initCommentBoxHeight;

    void Awake()
    {     
        this.transform.parent = root;

        _map = GameObject.Find("AbstractMap").GetComponent<AbstractMap>();
        if(_map == null) Debug.LogError("MAP IS NULL");
        
        initRefDistance = CustomReloadMap.GetReferenceDistance();
        windowGeoPosition = _map.WorldToGeoPosition(this.transform.position);
        initCommentWindowHeight = this.transform.position.y - root.position.y;

        //CommentsManager.OnCommentWindowUpdated += UpdateCommentWindow;
        this.GetComponentInChildren<MoveCommentWindow>().OnCommentWindowMoved += OnCommentWindowMoved;

        id += 1;
        titleText.text = "Comment #" + id; 
        inputField.onSubmit.AddListener(OnSubmitInput);

        commentClassDropdown.onValueChanged.AddListener(OnCommentClassChanged);
        UpdateCommentClassesDropdown();

        Material initMat = commentClasses[commentClasses.Keys.ToList()[0]];
        foreach(GameObject g in coloredElements)
        {
            g.GetComponent<MeshRenderer>().material = initMat;
        }
    }

    private void OnCommentWindowMoved()
    {
        windowGeoPosition = _map.WorldToGeoPosition(this.transform.position);
    }

    private void OnSubmitInput(string inputText)
    {
        contentText.gameObject.SetActive(true);
        contentText.text = inputText;

        inputField.gameObject.SetActive(false);
    }

    void OnCommentClassChanged(int index)
    {      
        this.commentClass = commentClassDropdown.options[index].text;
        Material newMat = commentClasses[commentClass];
        foreach(GameObject g in coloredElements)
        {
            g.GetComponent<MeshRenderer>().material = newMat;
        }
        
        // TODO: set comment window visibility
    }

    void UpdateCommentClassesDropdown()
    {   
        int curr_val = commentClassDropdown.value;
        commentClassDropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        foreach(string c in commentClasses.Keys)
        {
            options.Add(new TMP_Dropdown.OptionData(c));
        }
        commentClassDropdown.AddOptions(options);

        commentClassDropdown.SetValueWithoutNotify(curr_val);
    }

    public void UpdateCommentWindow()
    {
        UpdateCommentClassesDropdown();
    }

    public void UpdatePosition()
    {
        float scalingFactor = CustomReloadMap.GetReferenceDistance() / this.initRefDistance;
        initCommentWindowHeight = this.transform.position.y - this.transform.parent.position.y;
        
        Vector3 commentWorldPos = _map.GeoToWorldPosition(this.windowGeoPosition);
        this.transform.position = commentWorldPos + Vector3.up * scalingFactor * initCommentWindowHeight;
        //this.transform.localScale *= scalingFactor;

        if(commentBoxInstance != null){
            initCommentBoxHeight = commentBoxInstance.transform.position.y - commentBoxInstance.transform.parent.position.y;
            Vector3 boxWorldPos = _map.GeoToWorldPosition(this.boxGeoPosition);
            this.commentBoxInstance.transform.position = boxWorldPos + Vector3.up * this.initCommentBoxHeight * scalingFactor;
            this.commentBoxInstance.transform.localScale *= scalingFactor;
        }   
        
        this.initRefDistance = CustomReloadMap.GetReferenceDistance();
    }

    public void SetCommentBoxInstance(GameObject commentBoxInstance)
    {   
        if(commentBoxInstance != null)
        {
            this.commentBoxInstance = commentBoxInstance;
            boxGeoPosition = _map.WorldToGeoPosition(commentBoxInstance.transform.position);
            this.initCommentBoxHeight = commentBoxInstance.transform.localPosition.y;

            commentBoxInstance.transform.parent = root;
            windowGeoPosition = boxGeoPosition;

            // TODO: maybe add functionality that hides comment window upon touching the comment box
        }
    }

    public string GetCommentClass()
    {
        return commentClass;
    }

    public void SetActive(bool b)
    {
        instanceObject.SetActive(b);
    }
}

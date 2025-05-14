using System.Collections.Generic;
using UnityEngine;

public interface rm_ICommentWindow
{
    static int id;
    static Transform root;
    static Dictionary<string, Material> commentClasses;

    public void UpdateCommentWindow();
    public void UpdatePosition();
    public void SetCommentBoxInstance(GameObject commentBoxInstance);
    public string GetCommentClass();
    public void SetActive(bool b);
}

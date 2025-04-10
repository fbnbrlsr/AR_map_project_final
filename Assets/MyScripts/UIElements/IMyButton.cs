using UnityEngine;

public interface IMyButton
{

    public event My3DButtonEvent OnToggleButton;
    
    public void SetState(bool b);

}

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void MultiDropdownValueChanged();

public class rm_StopsMultiSelectDropdown : MonoBehaviour
{
    /*public TMP_Dropdown dropdown;
    private List<string> selectedOptions = new List<string>();
    public MultiDropdownValueChanged MultiDropdownValueChanged;

    void Start()
    {
        // Ensure the dropdown is initialized
        InitializeDropdown();
    }

    private void InitializeDropdown()
    {
        // Set the onValueChanged event to handle selections
        dropdown.onValueChanged.AddListener(OnOptionSelected);
    }

    private void OnOptionSelected(int index)
    {   
        string d = "ALREADY IN LIST: ";
        foreach(string s in selectedOptions)
        {
            d += s + ", ";
        }
        Debug.Log(d);
        Debug.Log("CURRENT OPTION: " + dropdown.options[index].text);

        if(dropdown.options[index].text.Equals("None"))
        {
            selectedOptions.Clear();
        }
        else if(selectedOptions.Contains(dropdown.options[index].text))
        {
            Debug.Log("-> remove");
            selectedOptions.Remove(dropdown.options[index].text); // Deselect if already selected
        }
        else
        {   
            Debug.Log("-> add");
            selectedOptions.Add(dropdown.options[index].text); // Select if not already selected
        }

        UpdateDropdownLabel();
        this.MultiDropdownValueChanged?.Invoke();
    }

    private void UpdateDropdownLabel()
    {
        // Set dropdown label to show all selected options
        string selectedLabel = "";

        if(selectedOptions.Count == 0)
        {
            selectedLabel = "None";
        }
        else
        {   
            selectedLabel = selectedOptions[0];
            for(int i = 1; i < selectedOptions.Count; i++)
            {
                selectedLabel += ", " + selectedOptions[i];
            }
        }
        
        dropdown.captionText.text = selectedLabel;
    }

    public List<string> GetSelectedOptions()
    {   
        List<string> selectedStringOptions = new List<string>();
        foreach(string s in selectedOptions)
        {
            if(s.Equals("None"))
            {
                return new List<string>();
            }
            selectedStringOptions.Add(s);
        }
        return selectedStringOptions;
    }*/

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.CoroutineTween;
using TMPro;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Experimental/DropdownEx")]
    [RequireComponent(typeof(RectTransform))]
    /// <summary>
    ///   A standard dropdown that presents a list of options when clicked, of which one can be chosen.
    /// </summary>
    /// <remarks>
    /// The dropdown component is a Selectable. When an option is chosen, the label and/or image of the control changes to show the chosen option.
    ///
    /// When a dropdown event occurs a callback is sent to any registered listeners of onValueChanged.
    /// </remarks>
    public class MultiSelectDropdown : Selectable, IPointerClickHandler, ISubmitHandler, ICancelHandler
    {        
        /// <summary>
        /// Visual representation of OptionData
        /// </summary>
        protected internal class DropdownItem : MonoBehaviour, IPointerEnterHandler, ICancelHandler
        {
            [SerializeField]
            private TMP_Text m_Text;
            [SerializeField]
            private Image m_Image;
            [SerializeField]
            private RectTransform m_RectTransform;
            [SerializeField]
            private Toggle m_Toggle;

            public TMP_Text text { get { return m_Text; } set { m_Text = value; } }
            public Image image { get { return m_Image; } set { m_Image = value; } }
            public RectTransform rectTransform { get { return m_RectTransform; } set { m_RectTransform = value; } }
            public Toggle toggle { get { return m_Toggle; } set { m_Toggle = value; } }

            public virtual void OnPointerEnter(PointerEventData eventData)
            {
                EventSystem.current.SetSelectedGameObject(gameObject);
            }

            public virtual void OnCancel(BaseEventData eventData)
            {
                Dropdown dropdown = GetComponentInParent<Dropdown>();
                if (dropdown)
                    dropdown.Hide();
            }
        }

        [Serializable]
        /// <summary>
        /// Class to store the text and/or image of a single option in the dropdown list.
        /// </summary>
        public class OptionData
        {
            [SerializeField]
            private string m_Text;
            [SerializeField]
            private Sprite m_Image;
            [SerializeField]
            private bool m_Selected;

            /// <summary>
            /// The text associated with the option.
            /// </summary>
            public string text { get { return m_Text; } internal set { m_Text = value; } }

            /// <summary>
            /// The image associated with the option.
            /// </summary>
            public Sprite image { get { return m_Image; } internal set { m_Image = value; } }

            public bool selected { get { return m_Selected; } internal set { m_Selected = value; } }

            public OptionData() { }

            public OptionData(string text)
            {
                this.text = text;
            }

            public OptionData(Sprite image)
            {
                this.image = image;
            }

            /// <summary>
            /// Create an object representing a single option for the dropdown list.
            /// </summary>
            /// <param name="text">Optional text for the option.</param>
            /// <param name="image">Optional image for the option.</param>
            public OptionData(string text, Sprite image)
            {
                this.text = text;
                this.image = image;
            }

            public OptionData(string text, bool selected)
            {
                this.text = text;
                this.selected = selected;
            }

            public OptionData(string text, Sprite image, bool selected)
            {
                this.text = text;
                this.image = image;
                this.selected = selected;
            }
        }

        // [Serializable]
        // /// <summary>
        // /// Class used internally to store the list of options for the dropdown list.
        // /// </summary>
        // /// <remarks>
        // /// The usage of this class is not exposed in the runtime API. It's only relevant for the PropertyDrawer drawing the list of options.
        // /// </remarks>
        // public class OptionDataList
        // {
        //     [SerializeField]
        //     private List<OptionData> m_Options;

        //     /// <summary>
        //     /// The list of options for the dropdown list.
        //     /// </summary>
        //     public List<OptionData> options { get { return m_Options; } set { m_Options = value; } }

        //     public OptionDataList()
        //     {
        //         options = new List<OptionData>();
        //     }
        // }

        [Serializable]
        /// <summary>
        /// UnityEvent callback for when a dropdown current option is changed.
        /// </summary>
        public class DropdownEvent : UnityEvent<uint> { }

        // Template used to create the dropdown.
        [SerializeField]
        private RectTransform m_Template;

        /// <summary>
        /// The Rect Transform of the template for the dropdown list.
        /// </summary>
        public RectTransform template { get { return m_Template; } set { m_Template = value; RefreshShownValue(); } }

        // Text to be used as a caption for the current value. It's not required, but it's kept here for convenience.
        [SerializeField]
        private TMP_Text m_CaptionText;

        /// <summary>
        /// The Text component to hold the text of the currently selected option.
        /// </summary>
        public TMP_Text captionText { get { return m_CaptionText; } set { m_CaptionText = value; RefreshShownValue(); } }

        [SerializeField]
        private Image m_CaptionImage;

        /// <summary>
        /// The Image component to hold the image of the currently selected option.
        /// </summary>
        public Image captionImage { get { return m_CaptionImage; } set { m_CaptionImage = value; RefreshShownValue(); } }

        [Space]

        [SerializeField]
        private TMP_Text m_ItemText;

        /// <summary>
        /// The Text component to hold the text of the item.
        /// </summary>
        public TMP_Text itemText { get { return m_ItemText; } set { m_ItemText = value; RefreshShownValue(); } }

        [SerializeField]
        private Image m_ItemImage;

        /// <summary>
        /// The Image component to hold the image of the item
        /// </summary>
        public Image itemImage { get { return m_ItemImage; } set { m_ItemImage = value; RefreshShownValue(); } }

        [Space]

        [SerializeField]
        private uint m_Value;


        [Header("Multi-Select Support")]
        [SerializeField]
        private bool _multiSelect = false;
        public string MultipleSelectedText = "Multiple Selected";

        public string NothingSelectedText = "None";

        [SerializeField]
        public bool AllowMultiSelect
        {
            get { return _multiSelect; }
            set
            {
                _multiSelect = value;

                this.value = 0;

                RefreshShownValue();
            }
        }


        [Space]

        // Items that will be visible when the dropdown is shown.
        // We box this into its own class so we can use a Property Drawer for it.
        [SerializeField]
        private List<OptionData> m_Options = new List<OptionData>();

        /// <summary>
        /// The list of possible options. A text string and an image can be specified for each option.
        /// </summary>
        /// <remarks>
        /// This is the list of options within the Dropdown. Each option contains Text and/or image data that you can specify using UI.Dropdown.OptionData before adding to the Dropdown list.
        /// This also unlocks the ability to edit the Dropdown, including the insertion, removal, and finding of options, as well as other useful tools
        /// </remarks>
        /// /// <example>
        /// <code>
        /// //Create a new Dropdown GameObject by going to the Hierarchy and clicking __Create__>__UI__>__Dropdown__. Attach this script to the Dropdown GameObject.
        ///
        /// using UnityEngine;
        /// using UnityEngine.UI;
        /// using System.Collections.Generic;
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///     //Use these for adding options to the Dropdown List
        ///     Dropdown.OptionData m_NewData, m_NewData2;
        ///     //The list of messages for the Dropdown
        ///     List<Dropdown.OptionData> m_Messages = new List<Dropdown.OptionData>();
        ///
        ///
        ///     //This is the Dropdown
        ///     Dropdown m_Dropdown;
        ///     string m_MyString;
        ///     int m_Index;
        ///
        ///     void Start()
        ///     {
        ///         //Fetch the Dropdown GameObject the script is attached to
        ///         m_Dropdown = GetComponent<Dropdown>();
        ///         //Clear the old options of the Dropdown menu
        ///         m_Dropdown.ClearOptions();
        ///
        ///         //Create a new option for the Dropdown menu which reads "Option 1" and add to messages List
        ///         m_NewData = new Dropdown.OptionData();
        ///         m_NewData.text = "Option 1";
        ///         m_Messages.Add(m_NewData);
        ///
        ///         //Create a new option for the Dropdown menu which reads "Option 2" and add to messages List
        ///         m_NewData2 = new Dropdown.OptionData();
        ///         m_NewData2.text = "Option 2";
        ///         m_Messages.Add(m_NewData2);
        ///
        ///         //Take each entry in the message List
        ///         foreach (Dropdown.OptionData message in m_Messages)
        ///         {
        ///             //Add each entry to the Dropdown
        ///             m_Dropdown.options.Add(message);
        ///             //Make the index equal to the total number of entries
        ///             m_Index = m_Messages.Count - 1;
        ///         }
        ///     }
        ///
        ///     //This OnGUI function is used here for a quick demonstration. See the [[wiki:UISystem|UI Section]] for more information about setting up your own UI.
        ///     void OnGUI()
        ///     {
        ///         //TextField for user to type new entry to add to Dropdown
        ///         m_MyString = GUI.TextField(new Rect(0, 40, 100, 40), m_MyString);
        ///
        ///         //Press the "Add" Button to add a new entry to the Dropdown
        ///         if (GUI.Button(new Rect(0, 0, 100, 40), "Add"))
        ///         {
        ///             //Make the index the last number of entries
        ///             m_Index = m_Messages.Count;
        ///             //Create a temporary option
        ///             Dropdown.OptionData temp = new Dropdown.OptionData();
        ///             //Make the option the data from the TextField
        ///             temp.text = m_MyString;
        ///
        ///             //Update the messages list with the TextField data
        ///             m_Messages.Add(temp);
        ///
        ///             //Add the Textfield data to the Dropdown
        ///             m_Dropdown.options.Insert(m_Index, temp);
        ///         }
        ///
        ///         //Press the "Remove" button to delete the selected option
        ///         if (GUI.Button(new Rect(110, 0, 100, 40), "Remove"))
        ///         {
        ///             //Remove the current selected item from the Dropdown from the messages List
        ///             m_Messages.RemoveAt(m_Dropdown.value);
        ///             //Remove the current selection from the Dropdown
        ///             m_Dropdown.options.RemoveAt(m_Dropdown.value);
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        public IReadOnlyList<OptionData> options
        {
            get { return m_Options; }
            // protected set { m_Options = value; RefreshShownValue(); }
        }

        // Notification triggered when the dropdown changes.
        [Space]
        [SerializeField]
        private DropdownEvent m_OnValueChanged = new DropdownEvent();

        [SerializeField]
        private DropdownEvent m_OnItemSelected = new DropdownEvent();
        [SerializeField]
        private DropdownEvent m_OnItemDeselected = new DropdownEvent();

        /// <summary>
        /// A UnityEvent that is invoked when when a user has clicked one of the options in the dropdown list.
        /// </summary>
        /// <remarks>
        /// Use this to detect when a user selects one or more options in the Dropdown. Add a listener to perform an action when this UnityEvent detects a selection by the user. See https://unity3d.com/learn/tutorials/topics/scripting/delegates for more information on delegates.
        /// </remarks>
        /// <example>
        ///  <code>
        /// //Create a new Dropdown GameObject by going to the Hierarchy and clicking Create>UI>Dropdown. Attach this script to the Dropdown GameObject.
        /// //Set your own Text in the Inspector window
        ///
        /// using UnityEngine;
        /// using UnityEngine.UI;
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///     Dropdown m_Dropdown;
        ///     public Text m_Text;
        ///
        ///     void Start()
        ///     {
        ///         //Fetch the Dropdown GameObject
        ///         m_Dropdown = GetComponent<Dropdown>();
        ///         //Add listener for when the value of the Dropdown changes, to take action
        ///         m_Dropdown.onValueChanged.AddListener(delegate {
        ///                 DropdownValueChanged(m_Dropdown);
        ///             });
        ///
        ///         //Initialise the Text to say the first value of the Dropdown
        ///         m_Text.text = "First Value : " + m_Dropdown.value;
        ///     }
        ///
        ///     //Ouput the new value of the Dropdown into Text
        ///     void DropdownValueChanged(Dropdown change)
        ///     {
        ///         m_Text.text =  "New Value : " + change.value;
        ///     }
        /// }
        /// </code>
        /// </example>
        public DropdownEvent onValueChanged { get { return m_OnValueChanged; } set { m_OnValueChanged = value; } }
        public DropdownEvent onItemSelected { get { return m_OnItemSelected; } set { m_OnItemSelected = value; } }
        public DropdownEvent onItemDeselected { get { return m_OnItemDeselected; } set { m_OnItemDeselected = value; } }

        private GameObject m_Dropdown;
        private GameObject m_Blocker;
        private List<DropdownItem> m_Items = new List<DropdownItem>();
        private TweenRunner<FloatTween> m_AlphaTweenRunner;
        private bool validTemplate = false;

        private static OptionData s_NoOptionData = new OptionData();

        /// <summary>
        /// The Value is the index number of the current selection in the Dropdown. 0 is the first option in the Dropdown, 1 is the second, and so on.
        /// 
        /// In Multi-Select mode, value will contain a bitmask of the selected items.  
        /// A value of zero  means nothing is selected.
        /// A value of 1 means the first item is selected.
        /// A value of 3 means the first AND second items are selected.
        /// </summary>
        /// <example>
        /// <code>
        /// //Create a new Dropdown GameObject by going to the Hierarchy and clicking __Create__>__UI__>__Dropdown__. Attach this script to the Dropdown GameObject.
        /// //Set your own Text in the Inspector window
        ///
        /// using UnityEngine;
        /// using UnityEngine.UI;
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///     //Attach this script to a Dropdown GameObject
        ///     Dropdown m_Dropdown;
        ///     //This is the string that stores the current selection m_Text of the Dropdown
        ///     string m_Message;
        ///     //This Text outputs the current selection to the screen
        ///     public Text m_Text;
        ///     //This is the index value of the Dropdown
        ///     int m_DropdownValue;
        ///
        ///     void Start()
        ///     {
        ///         //Fetch the DropDown component from the GameObject
        ///         m_Dropdown = GetComponent<Dropdown>();
        ///         //Output the first Dropdown index value
        ///         Debug.Log("Starting Dropdown Value : " + m_Dropdown.value);
        ///     }
        ///
        ///     void Update()
        ///     {
        ///         //Keep the current index of the Dropdown in a variable
        ///         m_DropdownValue = m_Dropdown.value;
        ///         //Change the message to say the name of the current Dropdown selection using the value
        ///         m_Message = m_Dropdown.options[m_DropdownValue].text;
        ///         //Change the onscreen Text to reflect the current Dropdown selection
        ///         m_Text.text = m_Message;
        ///     }
        /// }
        /// </code>
        /// </example>
        public uint value
        {
            get
            {
                return m_Value;
            }

            set
            {
                if (Application.isPlaying && (value == m_Value /*|| options.Count == 0*/))
                    return;

                if (AllowMultiSelect)
                {
                    // If we invert m_Value and mask it with 'value'
                    // we will have the bits that have changed
                    //
                    // Here's how this works:
                    //
                    // so lets say m_Value = 5  00000101
                    // and lets say value is 6  00000110
                    // so options[1] is added and options[0] is removed

                    // ~m_Value & value == 11111010 &
                    //                     00000110
                    //                     --------
                    //                     00000010 <-- index 1 (option #2)

                    // m_Value & ~value == 00000101
                    //                     11111001
                    //                     --------
                    //                     00000001 <-- index 0 (option #1)

                    uint added_mask = ~m_Value & value;
                    uint removed_mask = m_Value & ~value;
                    updateOptionsState(added_mask, removed_mask);

                    if (m_Dropdown != null)
                    {
                        // setting the value will close an open dropdown
                        // this way when it is opened back up, the toggles
                        // will reflect the new state.
                        //
                        // If we set the toggle state here, OnSelected()
                        // will be called and we will be here all over again.
                        //
                        // I guess we could remove the event listener temporarily
                        // and then set the toggle values to avoid closing an already
                        // open box but I think this is an edge case. 
                        Hide();
                    }
                }
                else
                {
                    m_OnItemDeselected.Invoke(m_Value);
                    m_OnItemSelected.Invoke(value);
                }

                m_Value = value;
                RefreshShownValue();

                // Notify all listeners
                UISystemProfilerApi.AddMarker("DropdownEx.value", this);
                m_OnValueChanged.Invoke(m_Value);
            }
        }

        protected virtual void updateOptionsState(uint added, uint removed)
        {
            uint index = 0;
            while (added > 0)
            {
                if ((added & 0x01) == 0x01)
                {
                    options[(int)index].selected = true;
                    m_OnItemSelected.Invoke(index);
                }

                index++;
                added >>= 0x01;
            }

            index = 0;
            while (removed > 0)
            {
                if ((removed & 0x01) == 0x01)
                {
                    options[(int)index].selected = false;
                    m_OnItemDeselected.Invoke(index);
                }

                index++;
                removed >>= 0x01;
            }
        }

        public List<OptionData> GetSelectedOptionsList()
        { 
            List<OptionData> options = new List<OptionData>();
            foreach (var option in m_Options)
            {   
                if (option.selected)
                {
                    options.Add(option);
                }
            }

            return options;
        }

        public IEnumerable<OptionData> SelectedOptions
        {
            get
            {
                foreach (var option in options)
                    if (option.selected)
                        yield return option;
            }
        }

        public uint SelectedCount
        {
            get
            {
                // If in single selection mode, there is always one item selected
                if (!AllowMultiSelect)
                    return 1;

                return countBits(m_Value);
            }
        }

        private uint IndexOfBit(uint src)
        {
            var i = 0u;
            while (src > 1)
            {
                src >>= 1;
                i++;
            }
            return i;
        }

        private uint countBits(uint v)
        {
            uint c; // c accumulates the total bits set in v
            for (c = 0; v > 0; c++)
            {
                v &= v - 1; // clear the least significant bit set
            }
            return c;
        }

        protected Toggle getToggleForIndex(int i)
        {
            Show();
            string caption = string.IsNullOrEmpty(options[i].text) ? "" : options[i].text;

            var go = GameObject.Find(string.Format("Item {0}: {1}", i, caption));
            return go.GetComponent<Toggle>();
        }

        protected MultiSelectDropdown() { }

        protected override void Awake()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return;
#endif

            m_AlphaTweenRunner = new TweenRunner<FloatTween>();
            m_AlphaTweenRunner.Init(this);

            if (m_CaptionImage)
                m_CaptionImage.enabled = (m_CaptionImage.sprite != null);

            if (m_Template)
                m_Template.gameObject.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();

            RefreshShownValue();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (!IsActive())
                return;

            // clamp at zero in case there are no options in the list
            uint maxValue = Math.Max(0, ((uint)options.Count) - 1);
            if (AllowMultiSelect)
            {
                maxValue = (1u << options.Count) - 1;
            }

            m_Value = m_Value > maxValue ? maxValue : m_Value;

            RefreshShownValue();
        }

#endif

        /// <summary>
        /// Refreshes the text and image (if available) of the currently selected option.
        /// </summary>
        /// <remarks>
        /// If you have modified the list of options, you should call this method afterwards to ensure that the visual state of the dropdown corresponds to the updated options.
        /// </remarks>
        public void RefreshShownValue()
        {
            if (0 == options.Count)
            {
                if (m_CaptionText != null)
                    m_CaptionText.text = !string.IsNullOrEmpty(s_NoOptionData.text) ? s_NoOptionData.text : "";

                if (m_CaptionImage != null)
                {
                    m_CaptionImage.sprite = s_NoOptionData.image;
                    m_CaptionImage.enabled = (m_CaptionImage.sprite != null);
                }
            }
            else
            {
                OptionData data = null;

                uint itemCount = 1;

                // clear out selections
                for (int i = 0; i < options.Count; i++)
                    options[i].selected = false;

                if (AllowMultiSelect)
                {
                    // bitcount is the number of bits set in 'value', which
                    // represents the number of options selected
                    itemCount = this.SelectedCount;

                    //
                    // If a single option is selected, figure out the index
                    // in the options array it is associated with
                    //
                    if (1 == itemCount) // only one option selected
                    {
                        // Shift the single bit over to determine the option index
                        var i = (int)IndexOfBit(m_Value);

                        data = options[i];
                        data.selected = true;
                    }
                    else
                    {
                        for (int i = 0; i < options.Count; i++)
                        {
                            // In case you aren't familiar ...
                            // The mask has one bit set, moving from least to most significant.
                            // i == 0,  mask == 00000001
                            // i == 1,  mask == 00000010
                            // i == 2,  mask == 00000100
                            //
                            // Logical AND m_Value with mask will equal the mask value if that
                            // bit is set.  So if:
                            // m_Value == 5      00000101
                            // and mask == 4     00000100
                            // Then ANDing them: 00000100
                            // Notice: mask == (m_Value & mask) 
                            int mask = 1 << i;
                            options[i].selected = ((m_Value & mask) == mask);
                        }
                    }
                }
                else
                {
                    data = options[(int)m_Value];
                    data.selected = true;
                }

                //
                // Depending on the number of options selected, set the
                // displayed caption text and image
                //
                if (m_CaptionText != null)
                {
                    if (0 == itemCount)
                    {
                        m_CaptionText.text = !string.IsNullOrEmpty(NothingSelectedText) ? NothingSelectedText : "";
                    }
                    else if (1 == itemCount)
                    {
                        m_CaptionText.text = !string.IsNullOrEmpty(data.text) ? data.text : "";
                    }
                    else
                    {
                        // The only way bitcount can be > 1 is if AllowMultipleSelections is true
                        m_CaptionText.text = !string.IsNullOrEmpty(MultipleSelectedText) ? MultipleSelectedText : "";
                    }
                }

                if (m_CaptionImage != null)
                {
                    if (0 == itemCount)
                    {
                        m_CaptionImage.sprite = null;
                        m_CaptionImage.enabled = false;
                    }
                    else if (1 == itemCount)
                    {
                        m_CaptionImage.sprite = data.image;
                        m_CaptionImage.enabled = (m_CaptionImage.sprite != null);
                    }
                    else
                    {
                        m_CaptionImage.sprite = null;
                        m_CaptionImage.enabled = false;
                    }
                }
            }
        }

        /// <summary>
        /// Add multiple options to the options of the Dropdown based on a list of OptionData objects.
        /// </summary>
        /// <param name="options">The list of OptionData to add.</param>
        /// /// <remarks>
        /// See AddOptions(List<string> options) for code example of usages.
        /// </remarks>
        public void AddOptions(IEnumerable<OptionData> options)
        {
            this.m_Options.AddRange(options);
            RefreshShownValue();
        }

        public void AddOption(OptionData option)
        {
            this.m_Options.Add(option);
            RefreshShownValue();
        }

        /// <summary>
        /// Add multiple text-only options to the options of the Dropdown based on a list of strings.
        /// </summary>
        /// <remarks>
        /// Add a List of string messages to the Dropdown. The Dropdown shows each member of the list as a separate option.
        /// </remarks>
        /// <param name="options">The list of text strings to add.</param>
        /// <example>
        /// <code>
        /// //Create a new Dropdown GameObject by going to the Hierarchy and clicking Create>UI>Dropdown. Attach this script to the Dropdown GameObject.
        ///
        /// using System.Collections.Generic;
        /// using UnityEngine;
        /// using UnityEngine.UI;
        ///
        /// public class Example : MonoBehaviour
        /// {
        ///     //Create a List of new Dropdown options
        ///     List<string> m_DropOptions = new List<string> { "Option 1", "Option 2"};
        ///     //This is the Dropdown
        ///     Dropdown m_Dropdown;
        ///
        ///     void Start()
        ///     {
        ///         //Fetch the Dropdown GameObject the script is attached to
        ///         m_Dropdown = GetComponent<Dropdown>();
        ///         //Clear the old options of the Dropdown menu
        ///         m_Dropdown.ClearOptions();
        ///         //Add the options created in the List above
        ///         m_Dropdown.AddOptions(m_DropOptions);
        ///     }
        /// }
        /// </code>
        /// </example>
        public void AddOptions(List<string> options)
        {
            for (int i = 0; i < options.Count; i++)
                this.m_Options.Add(new OptionData(options[i]));
            RefreshShownValue();
        }

        /// <summary>
        /// Add multiple image-only options to the options of the Dropdown based on a list of Sprites.
        /// </summary>
        /// <param name="options">The list of Sprites to add.</param>
        /// <remarks>
        /// See AddOptions(List<string> options) for code example of usages.
        /// </remarks>
        public void AddOptions(List<Sprite> options)
        {
            for (int i = 0; i < options.Count; i++)
                this.m_Options.Add(new OptionData(options[i]));
            RefreshShownValue();
        }

        /// <summary>
        /// Clear the list of options in the Dropdown.
        /// </summary>
        public void ClearOptions()
        {
            m_Options.Clear();
            RefreshShownValue();
        }

        private void SetupTemplate()
        {
            validTemplate = false;

            if (!m_Template)
            {
                Debug.LogError("The dropdown template is not assigned. The template needs to be assigned and must have a child GameObject with a Toggle component serving as the item.", this);
                return;
            }

            GameObject templateGo = m_Template.gameObject;
            templateGo.SetActive(true);
            Toggle itemToggle = m_Template.GetComponentInChildren<Toggle>();

            validTemplate = true;
            if (!itemToggle || itemToggle.transform == template)
            {
                validTemplate = false;
                Debug.LogError("The dropdown template is not valid. The template must have a child GameObject with a Toggle component serving as the item.", template);
            }
            else if (!(itemToggle.transform.parent is RectTransform))
            {
                validTemplate = false;
                Debug.LogError("The dropdown template is not valid. The child GameObject with a Toggle component (the item) must have a RectTransform on its parent.", template);
            }
            else if (itemText != null && !itemText.transform.IsChildOf(itemToggle.transform))
            {
                validTemplate = false;
                Debug.LogError("The dropdown template is not valid. The Item Text must be on the item GameObject or children of it.", template);
            }
            else if (itemImage != null && !itemImage.transform.IsChildOf(itemToggle.transform))
            {
                validTemplate = false;
                Debug.LogError("The dropdown template is not valid. The Item Image must be on the item GameObject or children of it.", template);
            }

            if (!validTemplate)
            {
                templateGo.SetActive(false);
                return;
            }

            DropdownItem item = itemToggle.gameObject.AddComponent<DropdownItem>();
            item.text = m_ItemText;
            item.image = m_ItemImage;
            item.toggle = itemToggle;
            item.rectTransform = (RectTransform)itemToggle.transform;

            Canvas popupCanvas = GetOrAddComponent<Canvas>(templateGo);
            popupCanvas.overrideSorting = true;
            popupCanvas.sortingOrder = 30000;

            GetOrAddComponent<GraphicRaycaster>(templateGo);
            GetOrAddComponent<CanvasGroup>(templateGo);
            templateGo.SetActive(false);

            validTemplate = true;
        }

        private static T GetOrAddComponent<T>(GameObject go) where T : Component
        {
            T comp = go.GetComponent<T>();
            if (!comp)
                comp = go.AddComponent<T>();
            return comp;
        }

        /// <summary>
        /// Handling for when the dropdown is initially 'clicked'. Typically shows the dropdown
        /// </summary>
        /// <param name="eventData">The asocciated event data.</param>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Show();
        }

        /// <summary>
        /// Handling for when the dropdown is selected and a submit event is processed. Typically shows the dropdown
        /// </summary>
        /// <param name="eventData">The asocciated event data.</param>
        public virtual void OnSubmit(BaseEventData eventData)
        {
            Show();
        }

        /// <summary>
        /// This will hide the dropdown list.
        /// </summary>
        /// <remarks>
        /// Called by a BaseInputModule when a Cancel event occurs.
        /// </remarks>
        /// <param name="eventData">The asocciated event data.</param>
        public virtual void OnCancel(BaseEventData eventData)
        {
            Hide();
        }

        /// <summary>
        /// Show the dropdown.
        ///
        /// Plan for dropdown scrolling to ensure dropdown is contained within screen.
        ///
        /// We assume the Canvas is the screen that the dropdown must be kept inside.
        /// This is always valid for screen space canvas modes.
        /// For world space canvases we don't know how it's used, but it could be e.g. for an in-game monitor.
        /// We consider it a fair constraint that the canvas must be big enough to contain dropdowns.
        /// </summary>
        public void Show()
        {
            if (!IsActive() || !IsInteractable() || m_Dropdown != null)
                return;

            if (!validTemplate)
            {
                SetupTemplate();
                if (!validTemplate)
                    return;
            }

            // Get root Canvas.
            var list = ListPool<Canvas>.Get();
            gameObject.GetComponentsInParent(false, list);
            if (list.Count == 0)
                return;
            Canvas rootCanvas = list[0];
            ListPool<Canvas>.Release(list);

            m_Template.gameObject.SetActive(true);

            // Instantiate the drop-down template
            m_Dropdown = CreateDropdownList(m_Template.gameObject);
            m_Dropdown.name = "Dropdown List";
            m_Dropdown.SetActive(true);

            // Make drop-down RectTransform have same values as original.
            RectTransform dropdownRectTransform = m_Dropdown.transform as RectTransform;
            dropdownRectTransform.SetParent(m_Template.transform.parent, false);

            // Instantiate the drop-down list items

            // Find the dropdown item and disable it.
            DropdownItem itemTemplate = m_Dropdown.GetComponentInChildren<DropdownItem>();

            GameObject content = itemTemplate.rectTransform.parent.gameObject;
            RectTransform contentRectTransform = content.transform as RectTransform;
            itemTemplate.rectTransform.gameObject.SetActive(true);

            // Get the rects of the dropdown and item
            Rect dropdownContentRect = contentRectTransform.rect;
            Rect itemTemplateRect = itemTemplate.rectTransform.rect;

            // Calculate the visual offset between the item's edges and the background's edges
            Vector2 offsetMin = itemTemplateRect.min - dropdownContentRect.min + (Vector2)itemTemplate.rectTransform.localPosition;
            Vector2 offsetMax = itemTemplateRect.max - dropdownContentRect.max + (Vector2)itemTemplate.rectTransform.localPosition;
            Vector2 itemSize = itemTemplateRect.size;

            m_Items.Clear();

            Toggle prev = null;
            for (int i = 0; i < options.Count; ++i)
            {
                OptionData data = options[i];
                // var selected = (this.value & (1 << i)) == (1 << i);
                DropdownItem item = AddItem(data, itemTemplate, m_Items);
                if (item == null)
                    continue;

                // Automatically set up a toggle state change listener
                item.toggle.isOn = data.selected;
                item.toggle.onValueChanged.AddListener(x => OnSelectItem(item.toggle));

                // Select current option
                if (item.toggle.isOn)
                    item.toggle.Select();

                // Automatically set up explicit navigation
                if (prev != null)
                {
                    Navigation prevNav = prev.navigation;
                    Navigation toggleNav = item.toggle.navigation;
                    prevNav.mode = Navigation.Mode.Explicit;
                    toggleNav.mode = Navigation.Mode.Explicit;

                    prevNav.selectOnDown = item.toggle;
                    prevNav.selectOnRight = item.toggle;
                    toggleNav.selectOnLeft = prev;
                    toggleNav.selectOnUp = prev;

                    prev.navigation = prevNav;
                    item.toggle.navigation = toggleNav;
                }
                prev = item.toggle;
            }

            // Reposition all items now that all of them have been added
            Vector2 sizeDelta = contentRectTransform.sizeDelta;
            sizeDelta.y = itemSize.y * m_Items.Count + offsetMin.y - offsetMax.y;
            contentRectTransform.sizeDelta = sizeDelta;

            float extraSpace = dropdownRectTransform.rect.height - contentRectTransform.rect.height;
            if (extraSpace > 0)
                dropdownRectTransform.sizeDelta = new Vector2(dropdownRectTransform.sizeDelta.x, dropdownRectTransform.sizeDelta.y - extraSpace);

            // Invert anchoring and position if dropdown is partially or fully outside of canvas rect.
            // Typically this will have the effect of placing the dropdown above the button instead of below,
            // but it works as inversion regardless of initial setup.
            Vector3[] corners = new Vector3[4];
            dropdownRectTransform.GetWorldCorners(corners);

            RectTransform rootCanvasRectTransform = rootCanvas.transform as RectTransform;
            Rect rootCanvasRect = rootCanvasRectTransform.rect;
            for (int axis = 0; axis < 2; axis++)
            {
                bool outside = false;
                for (int i = 0; i < 4; i++)
                {
                    Vector3 corner = rootCanvasRectTransform.InverseTransformPoint(corners[i]);
                    if (corner[axis] < rootCanvasRect.min[axis] || corner[axis] > rootCanvasRect.max[axis])
                    {
                        outside = true;
                        break;
                    }
                }
                if (outside)
                    RectTransformUtility.FlipLayoutOnAxis(dropdownRectTransform, axis, false, false);
            }

            for (int i = 0; i < m_Items.Count; i++)
            {
                RectTransform itemRect = m_Items[i].rectTransform;
                itemRect.anchorMin = new Vector2(itemRect.anchorMin.x, 0);
                itemRect.anchorMax = new Vector2(itemRect.anchorMax.x, 0);
                itemRect.anchoredPosition = new Vector2(itemRect.anchoredPosition.x, offsetMin.y + itemSize.y * (m_Items.Count - 1 - i) + itemSize.y * itemRect.pivot.y);
                itemRect.sizeDelta = new Vector2(itemRect.sizeDelta.x, itemSize.y);
            }

            // Fade in the popup
            AlphaFadeList(0.15f, 0f, 1f);

            // Make drop-down template and item template inactive
            m_Template.gameObject.SetActive(false);
            itemTemplate.gameObject.SetActive(false);

            m_Blocker = CreateBlocker(rootCanvas);
        }

        /// <summary>
        /// Create a blocker that blocks clicks to other controls while the dropdown list is open.
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to obtain a blocker GameObject.
        /// </remarks>
        /// <param name="rootCanvas">The root canvas the dropdown is under.</param>
        /// <returns>The created blocker object</returns>
        protected virtual GameObject CreateBlocker(Canvas rootCanvas)
        {
            // Create blocker GameObject.
            GameObject blocker = new GameObject("Blocker");

            // Setup blocker RectTransform to cover entire root canvas area.
            RectTransform blockerRect = blocker.AddComponent<RectTransform>();
            blockerRect.SetParent(rootCanvas.transform, false);
            blockerRect.anchorMin = Vector3.zero;
            blockerRect.anchorMax = Vector3.one;
            blockerRect.sizeDelta = Vector2.zero;

            // Make blocker be in separate canvas in same layer as dropdown and in layer just below it.
            Canvas blockerCanvas = blocker.AddComponent<Canvas>();
            blockerCanvas.overrideSorting = true;
            Canvas dropdownCanvas = m_Dropdown.GetComponent<Canvas>();
            blockerCanvas.sortingLayerID = dropdownCanvas.sortingLayerID;
            blockerCanvas.sortingOrder = dropdownCanvas.sortingOrder - 1;

            // Add raycaster since it's needed to block.
            blocker.AddComponent<GraphicRaycaster>();

            // Add image since it's needed to block, but make it clear.
            Image blockerImage = blocker.AddComponent<Image>();
            blockerImage.color = Color.clear;

            // Add button since it's needed to block, and to close the dropdown when blocking area is clicked.
            Button blockerButton = blocker.AddComponent<Button>();
            blockerButton.onClick.AddListener(Hide);

            return blocker;
        }

        /// <summary>
        /// Convenience method to explicitly destroy the previously generated blocker object
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to dispose of a blocker GameObject that blocks clicks to other controls while the dropdown list is open.
        /// </remarks>
        /// <param name="blocker">The blocker object to destroy.</param>
        protected virtual void DestroyBlocker(GameObject blocker)
        {
            Destroy(blocker);
        }

        /// <summary>
        /// Create the dropdown list to be shown when the dropdown is clicked. 
        /// The dropdown list should correspond to the provided template GameObject, 
        /// equivalent to instantiating a copy of it.
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to obtain a dropdown 
        /// list GameObject.
        /// </remarks>
        /// <param name="template">The template to create the dropdown list from.</param>
        /// <returns>The created drop down list gameobject.</returns>
        protected virtual GameObject CreateDropdownList(GameObject template)
        {
            return (GameObject)Instantiate(template);
        }

        /// <summary>
        /// Convenience method to explicitly destroy the previously generated dropdown list
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to dispose of a dropdown list GameObject.
        /// </remarks>
        /// <param name="dropdownList">The dropdown list GameObject to destroy</param>
        protected virtual void DestroyDropdownList(GameObject dropdownList)
        {
            Destroy(dropdownList);
        }

        /// <summary>
        /// Create a dropdown item based upon the item template.
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to obtain an option item.
        /// The option item should correspond to the provided template DropdownItem and its GameObject, equivalent to instantiating a copy of it.
        /// </remarks>
        /// <param name="itemTemplate">e template to create the option item from.</param>
        /// <returns>The created dropdown item component</returns>
        protected virtual DropdownItem CreateItem(DropdownItem itemTemplate)
        {
            return (DropdownItem)Instantiate(itemTemplate);
        }

        /// <summary>
        ///  Convenience method to explicitly destroy the previously generated Items.
        /// </summary>
        /// <remarks>
        /// Override this method to implement a different way to dispose of an option item.
        /// Likely no action needed since destroying the dropdown list destroys all contained items as well.
        /// </remarks>
        /// <param name="item">The Item to destroy.</param>
        protected virtual void DestroyItem(DropdownItem item) { }

        // Add a new drop-down list item with the specified values.
        private DropdownItem AddItem(OptionData data, DropdownItem itemTemplate, List<DropdownItem> items)
        {
            // Add a new item to the dropdown.
            DropdownItem item = CreateItem(itemTemplate);
            item.rectTransform.SetParent(itemTemplate.rectTransform.parent, false);

            item.gameObject.SetActive(true);
            item.gameObject.name = "Item " + items.Count + (data.text != null ? ": " + data.text : "");

            if (item.toggle != null)
            {
                item.toggle.isOn = data.selected;
            }

            // Set the item's data
            //Debug.Log("data.text: " + data.text);
            if ((bool)item.text)
            {
                item.text.text = data.text;
            }
            
            if (item.image)
            {
                item.image.sprite = data.image;
                item.image.enabled = (item.image.sprite != null);
            }

            items.Add(item);
            return item;
        }

        private void AlphaFadeList(float duration, float alpha)
        {
            CanvasGroup group = m_Dropdown.GetComponent<CanvasGroup>();
            AlphaFadeList(duration, group.alpha, alpha);
        }

        private void AlphaFadeList(float duration, float start, float end)
        {
            if (end.Equals(start))
                return;

            FloatTween tween = new FloatTween { duration = duration, startValue = start, targetValue = end };
            tween.AddOnChangedCallback(SetAlpha);
            tween.ignoreTimeScale = true;
            m_AlphaTweenRunner.StartTween(tween);
        }

        private void SetAlpha(float alpha)
        {
            if (!m_Dropdown)
                return;
            CanvasGroup group = m_Dropdown.GetComponent<CanvasGroup>();
            group.alpha = alpha;
        }

        /// <summary>
        /// Hide the dropdown list. I.e. close it.
        /// </summary>
        public void Hide()
        {
            if (m_Dropdown != null)
            {
                AlphaFadeList(0.15f, 0f);

                // User could have disabled the dropdown during the OnValueChanged call.
                if (IsActive())
                    StartCoroutine(DelayedDestroyDropdownList(0.15f));
            }
            if (m_Blocker != null)
                DestroyBlocker(m_Blocker);
            m_Blocker = null;
            Select();
        }

        private IEnumerator DelayedDestroyDropdownList(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            for (int i = 0; i < m_Items.Count; i++)
            {
                if (m_Items[i] != null)
                    DestroyItem(m_Items[i]);
            }
            m_Items.Clear();
            if (m_Dropdown != null)
                DestroyDropdownList(m_Dropdown);
            m_Dropdown = null;
        }

        // Change the value and hide the dropdown.
        private void OnSelectItem(Toggle toggle)
        {
            if (!toggle.isOn && !AllowMultiSelect)
                toggle.isOn = true;

            int selectedIndex = -1;
            Transform tr = toggle.transform;
            Transform parent = tr.parent;
            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i) == tr)
                {
                    // Subtract one to account for template child.
                    selectedIndex = i - 1;
                    break;
                }
            }

            if (selectedIndex < 0)
                return;

            // options[selectedIndex].selected = toggle.isOn;

            if (toggle.isOn)
            {
                if (AllowMultiSelect)
                    value |= 1u << selectedIndex;
                else
                    value = (uint)selectedIndex;
            }
            else
            {
                if (AllowMultiSelect)
                    value &= ~(1u << selectedIndex);
                else
                    value = (uint)selectedIndex;
            }

            Hide();
        }

        public void DeselectAll()
        {
            // Deselecting everything doesn't apply to single selection dropdowns
            if (!AllowMultiSelect)
                return;

            value = 0;
        }
    }
}

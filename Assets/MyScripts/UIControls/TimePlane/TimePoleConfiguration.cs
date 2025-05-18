using Mapbox.Examples;
using Mapbox.Unity.Map;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimePoleConfiguration : MonoBehaviour
{

    /*
    *   This class manages the configuration of the time pole.
    *   It updates the time labels and time planes to be at the correct height
    *   depending on the map state.
    *   It also takes care of enabling and disabling the time planes upon
    *   toggling the label buttons.
    */

    public float materialAlphaValue = 0.25f;
    [SerializeField] AbstractMap map; 
    [SerializeField] GameObject timePoleCylinder;
    [SerializeField] GameObject timePoleLabel5h;
    [SerializeField] GameObject timePoleLabel10h;
    [SerializeField] GameObject timePoleLabel15h;
    [SerializeField] GameObject timePoleLabel20h;
    [SerializeField] GameObject timePoleLabel25h;
    [SerializeField] GameObject timePoleLabel30h;
    
    [SerializeField] GameObject timePlane5h;
    [SerializeField] GameObject timePlane10h;
    [SerializeField] GameObject timePlane15h;
    [SerializeField] GameObject timePlane20h;
    [SerializeField] GameObject timePlane25h;
    [SerializeField] GameObject timePlane30h;

    [SerializeField] Toggle timePlane5hToggle;
    [SerializeField] Toggle timePlane10hToggle;
    [SerializeField] Toggle timePlane15hToggle;
    [SerializeField] Toggle timePlane20hToggle;
    [SerializeField] Toggle timePlane25hToggle;
    [SerializeField] Toggle timePlane30hToggle;

    float height0h;
    float height5h;
    float height10h;
    float height15h;
    float height20h;
    float height25h;
    float height30h;
    float height35h;
    Vector3 timePoleLocalPosition;
    Vector3 timePoleLocalScale;

    void Start()
    {   
        map.OnUpdated += UpdateConfiguration;
        timePoleLocalPosition = timePoleCylinder.transform.localPosition;
        timePoleLocalScale = timePoleCylinder.transform.localScale;

        if(SceneManager.GetActiveScene().name.Equals("DummyScene")) return;
        
        timePlane5hToggle.onValueChanged.AddListener(OnTimePlane5hTogglePressed);
        timePlane10hToggle.onValueChanged.AddListener(OnTimePlane10hTogglePressed);
        timePlane15hToggle.onValueChanged.AddListener(OnTimePlane15hTogglePressed);
        timePlane20hToggle.onValueChanged.AddListener(OnTimePlane20hTogglePressed);
        timePlane25hToggle.onValueChanged.AddListener(OnTimePlane25hTogglePressed);
        timePlane30hToggle.onValueChanged.AddListener(OnTimePlane30hTogglePressed);

        SetMaterialAlpha();
    }

    public void UpdateConfiguration()
    {   
        height0h = SecondsToRealHeight(0f);
        height5h = SecondsToRealHeight(5*60*60f);
        height10h = SecondsToRealHeight(10*60*60f);
        height15h = SecondsToRealHeight(15*60*60f);
        height20h = SecondsToRealHeight(20*60*60f);
        height25h = SecondsToRealHeight(25*60*60f);
        height30h = SecondsToRealHeight(30*60*60f);
        height35h = SecondsToRealHeight(35*60*60f);

        timePoleCylinder.transform.localScale = new Vector3(timePoleLocalScale.x, (height35h-height0h)/2, timePoleLocalScale.x);
        timePoleCylinder.transform.localPosition = new Vector3(timePoleLocalPosition.x, (height35h-height0h)/2, timePoleLocalPosition.z);

        timePoleLabel5h.transform.localPosition = new Vector3(timePoleLabel5h.transform.localPosition.x, height5h, timePoleLabel5h.transform.localPosition.z);
        timePoleLabel10h.transform.localPosition = new Vector3(timePoleLabel10h.transform.localPosition.x, height10h, timePoleLabel10h.transform.localPosition.z);
        timePoleLabel15h.transform.localPosition = new Vector3(timePoleLabel15h.transform.localPosition.x, height15h, timePoleLabel15h.transform.localPosition.z);
        timePoleLabel20h.transform.localPosition = new Vector3(timePoleLabel20h.transform.localPosition.x, height20h, timePoleLabel20h.transform.localPosition.z);
        timePoleLabel25h.transform.localPosition = new Vector3(timePoleLabel25h.transform.localPosition.x, height25h, timePoleLabel25h.transform.localPosition.z);
        timePoleLabel30h.transform.localPosition = new Vector3(timePoleLabel30h.transform.localPosition.x, height30h, timePoleLabel30h.transform.localPosition.z);

        timePlane5h.transform.localPosition = new Vector3(timePlane5h.transform.localPosition.x, height5h, timePlane5h.transform.localPosition.z);
        timePlane5h.SetActive(timePlane5hToggle.isOn);
        timePlane10h.transform.localPosition = new Vector3(timePlane10h.transform.localPosition.x, height10h, timePlane10h.transform.localPosition.z);
        timePlane10h.SetActive(timePlane10hToggle.isOn);
        timePlane15h.transform.localPosition = new Vector3(timePlane15h.transform.localPosition.x, height15h, timePlane15h.transform.localPosition.z);
        timePlane15h.SetActive(timePlane15hToggle.isOn);
        timePlane20h.transform.localPosition = new Vector3(timePlane20h.transform.localPosition.x, height20h, timePlane20h.transform.localPosition.z);
        timePlane20h.SetActive(timePlane20hToggle.isOn);
        timePlane25h.transform.localPosition = new Vector3(timePlane25h.transform.localPosition.x, height25h, timePlane25h.transform.localPosition.z);
        timePlane25h.SetActive(timePlane25hToggle.isOn);
        timePlane30h.transform.localPosition = new Vector3(timePlane30h.transform.localPosition.x, height30h, timePlane30h.transform.localPosition.z);
        timePlane30h.SetActive(timePlane30hToggle.isOn);
    }

    private float SecondsToRealHeight(float seconds)
    {   
        float frac = 1f * seconds / DatabaseLegData.latestTime;
        return DatabaseLegData.maxPointHeight * frac * CustomReloadMap.GetReferenceDistance() / 2;
    }

    private void OnTimePlane5hTogglePressed(bool b)
    {
        timePlane5h.SetActive(b);
    }

    private void OnTimePlane10hTogglePressed(bool b)
    {
        timePlane10h.SetActive(b);
    }

    private void OnTimePlane15hTogglePressed(bool b)
    {
        timePlane15h.SetActive(b);
    }

    private void OnTimePlane20hTogglePressed(bool b)
    {
        timePlane20h.SetActive(b);
    }

    private void OnTimePlane25hTogglePressed(bool b)
    {
        timePlane25h.SetActive(b);
    }

    private void OnTimePlane30hTogglePressed(bool b)
    {
        timePlane30h.SetActive(b);
    }

    private void SetMaterialAlpha()
    {
        GameObject[] objs = new GameObject[6] 
        {
            timePlane5h,
            timePlane10h,
            timePlane15h,
            timePlane20h,
            timePlane25h,
            timePlane30h
        };

        foreach(GameObject obj in objs)
        {
            MeshRenderer r = obj.GetNamedChild("TimePlane").GetComponent<MeshRenderer>();
            Material mat = r.material;
            Color col = mat.color;
            col.a = materialAlphaValue;
            mat.color = col;
            r.material = mat;
        }
    }

}

using HoloToolkit.Unity.SpatialMapping;
using UnityEngine;

public class GlowScriptLocal : MonoBehaviour {
    
    private SphereCollider m_sc;
    private Light m_light;
    private TapToPlace m_ttp;
    private float DefaultIntensity = 1.4f;
    private float MinLightRange = 0.0f;
    private float MaxLightRange = 0.3f;
    private float GlobalSmoothTime = 0.1f;
    private float GlobalDeltaPerChange = 0.1f;
    private string GlowGroup;
    private string GlowTag;

    private Color HighColor = Color.green;
    //private Color LowColor = Color.red;
    private Color PrivateColor = Color.white;

    [SerializeField]
    private float targetLightRange = 0.1f; // Starting value
    private float lightRangeDelta;
    private float currColorScale = 1.0f; // Starting value, from 0 to 1
    private Color currColor;
    private float targetColorScale;

    // Use this for initialization
    void Start () {
        GlowGroup = gameObject.transform.parent.tag;
        GlowTag = gameObject.tag;
        m_ttp = gameObject.GetComponent<TapToPlace>();
        m_sc = GetComponent<SphereCollider>();
        m_light = GetComponent<Light>();
        m_light.range = targetLightRange;
        m_light.intensity = DefaultIntensity;
        m_light.color = GlowTag == "AllPublic" ? HighColor : PrivateColor;
        currColor = m_light.color;
        targetColorScale = currColorScale;
        lightRangeDelta = (MaxLightRange - MinLightRange) * GlobalDeltaPerChange; // Default each press moves 10% of range
    }

    // Update is called once per frame
    void Update() {
        // Only listen to input if this object is being placed
        //if (!m_ttp.IsBeingPlaced) return;

        // Clones the current object
        //if (Input.GetKeyDown("d")) // Virtual Input -> Hololens for some reason doesn't recognize KeyCode.D
        //{
        //    GameObject glowClone = Instantiate(gameObject);
        //    glowClone.GetComponent<TapToPlace>().SavedAnchorFriendlyName = "GlowClone_" + System.DateTime.Now.ToString("yyyyMMddhhmmss");
        //    glowClone.GetComponent<TapToPlace>().IsBeingPlaced = false;
        //}

        switch (GlowGroup)
        {
            case "1Glow":
                keyListeners("down", "up", "left", "right"); break;
            case "2Glow":
                keyListeners("z", "x", "c", "v"); break;
            case "3Glow":
                keyListeners("a", "s", "d", "f"); break;
            case "4Glow":
                keyListeners("q", "w", "e", "r"); break;
            case "5Glow":
                keyListeners("1", "2", "3", "4"); break;
            default:
                Debug.LogError("Invalid GlowGroup!"); break;
        }
    }

    void keyListeners(string downArousal, string upArousal, string downValence, string upValence)
    {
        if (GlowTag != "AllPrivate")
        {
            if (Input.GetKeyDown(upArousal)) targetLightRange = Mathf.Min(MaxLightRange, m_light.range + lightRangeDelta);
            if (Input.GetKeyDown(downArousal)) targetLightRange = Mathf.Max(MinLightRange, m_light.range - lightRangeDelta);

            float v1 = 0.0f;
            m_light.range = Mathf.SmoothDamp(m_light.range, targetLightRange, ref v1, GlobalSmoothTime);
            m_sc.radius = m_light.range / 2.0f;
        }

        if (GlowTag == "AllPublic")
        {
            if (Input.GetKeyDown(upValence)) targetColorScale = Mathf.Min(1, currColorScale + GlobalDeltaPerChange);
            if (Input.GetKeyDown(downValence)) targetColorScale = Mathf.Max(0, currColorScale - GlobalDeltaPerChange);

            float v2 = 0.0f;
            currColorScale = Mathf.SmoothDamp(currColorScale, targetColorScale, ref v2, GlobalSmoothTime);

            float relativeScale = currColorScale;
            if (currColorScale > 0.5f)
            {
                relativeScale = (1.0f - relativeScale) / 0.5f;
                currColor.r = relativeScale;
            }
            else
            {
                relativeScale /= 0.5f;
                currColor.g = relativeScale;
            }
            m_light.color = currColor;
        }
    }
}

using HoloToolkit.Unity.SpatialMapping;
using UnityEngine;

public class GlowScriptLocal : MonoBehaviour {
    
    private SphereCollider m_sc;
    private Light m_light;
    private TapToPlace m_ttp;
    private float DefaultIntensity = 1.4f; // Seems like the light halos (glows) max out at 1.4 intensity
    private float MinLightRange = 0.0f; // Minimum size of halo
    private float MaxLightRange = 0.3f; // Maximunm size of halo
    private float GlobalSmoothTime = 0.1f;
    private float GlobalDeltaPerChange = 0.1f; // 0.1f means 10% change for every keypress, both for size and color
    private string GlowGroup; // All glows in a group respond together to the same keypresses (it's set to be the tag of the parent object)
    private string GlowTag; // Can be "AllPublic", "ValencePrivate", or "AllPrivate"

    private Color HighColor = Color.green; // Default high valence color, is also default starting color for "AllPublic" glows
    //private Color LowColor = Color.red;
    private Color PrivateColor = Color.white; // Default color for glows that DON'T show valence, so currently both the "ValencePrivate" and "AllPrivate" glows

    [SerializeField]
    private float targetLightRange = 0.1f; // Keeps track of how large (or small) the light range is smoothly trying to get (value here specifies starting value)
    private float lightRangeDelta; // Will be set to whatever value is X% between the min and max light ranges (see Start() below)
    private float currColorScale = 1.0f; // A value from 0 to 1 that represents current valence
    private Color currColor; // A Color that represents the current color
    private float targetColorScale; // A value from 0 to 1 that represents valence that we're trying to smoothly arrive at

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
            // Sets targetLightRange to be X% higher or lower than the current light range, bounded by max and min
            if (Input.GetKeyDown(upArousal)) targetLightRange = Mathf.Min(MaxLightRange, m_light.range + lightRangeDelta);
            if (Input.GetKeyDown(downArousal)) targetLightRange = Mathf.Max(MinLightRange, m_light.range - lightRangeDelta);

            // Smoothly change light range toward the targetLightRange
            float v1 = 0.0f;
            m_light.range = Mathf.SmoothDamp(m_light.range, targetLightRange, ref v1, GlobalSmoothTime);

            // Keep the SphereCollider radius of the halo (glow) half of the range
            m_sc.radius = m_light.range / 2.0f;
        }

        if (GlowTag == "AllPublic")
        {
            // Sets targetColorScale to be X% higher or lower than the current color scale, bounded by max and min
            if (Input.GetKeyDown(upValence)) targetColorScale = Mathf.Min(1, currColorScale + GlobalDeltaPerChange);
            if (Input.GetKeyDown(downValence)) targetColorScale = Mathf.Max(0, currColorScale - GlobalDeltaPerChange);

            // Smoothly change the color scale toward the targetColorScale
            float v2 = 0.0f;
            currColorScale = Mathf.SmoothDamp(currColorScale, targetColorScale, ref v2, GlobalSmoothTime);

            // Translate color scale (basically valence value from 0 to 1) into an actual color
            float relativeScale = currColorScale;
            if (currColorScale > 0.5f) // Assuming red to green (0 to 1), as color scale moves over 0.5, we inversely scale back red value
            {
                relativeScale = (1.0f - relativeScale) / 0.5f; // Assuming red to green, 1.0 color scale should mean 0.0 red, and anything less than 0.5 scale should mean 1.0 red
                currColor.r = relativeScale;
            }
            else // Assuming red to green (0 to 1), as color scale moves under 0.5, we linearly scale back green value
            {
                relativeScale /= 0.5f; // Assuming red to green, 0.0 color scale should mean 0.0 green, and anything 0.5 and above should mean 1.0 green
                currColor.g = relativeScale;
            }

            // Set the light color to the current color that was updated above
            m_light.color = currColor;
        }
    }
}

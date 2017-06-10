﻿using HoloToolkit.Unity.SpatialMapping;
using UnityEngine;

public class GlowScriptLocal : MonoBehaviour {
    //protected HoloToolkit.Unity.WorldAnchorManager anchorManager; // ???

    private SphereCollider m_sc;
    private Light m_light;
    private TapToPlace m_ttp;
    private float DefaultIntensity = 1.4f;
    private float MinLightRange = 0.0f;
    private float MaxLightRange = 0.3f;
    private float GlobalSmoothTime = 0.1f;
    private float GlobalDeltaPerChange = 0.1f;
    private string GlowTag;

    private Color HighColor = Color.green;
    private Color LowColor = Color.blue;
    private Color PrivateColor = Color.white;

    [SerializeField]
    private float targetLightRange = 0.1f; // Starting value
    private float lightRangeDelta;
    private float currColorScale = 1.0f; // Starting value, from 0 to 1
    private Color currColor;
    private float targetColorScale;

    // Use this for initialization
    void Start () {
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
        if (!m_ttp.IsBeingPlaced) return;

        // Clones the current object
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameObject glowClone = Instantiate(gameObject);
            m_ttp.SavedAnchorFriendlyName = "GlowClone_" + System.DateTime.Now.ToString("yyyyMMddhhmmss");
            glowClone.GetComponent<TapToPlace>().IsBeingPlaced = false;
            HoloToolkit.Unity.WorldAnchorManager.Instance.AttachAnchor(glowClone, m_ttp.SavedAnchorFriendlyName); /// ???
        }

        if (GlowTag != "AllPrivate")
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) targetLightRange = Mathf.Min(MaxLightRange, m_light.range + lightRangeDelta);
            if (Input.GetKeyDown(KeyCode.DownArrow)) targetLightRange = Mathf.Max(MinLightRange, m_light.range - lightRangeDelta);

            float v1 = 0.0f;
            m_light.range = Mathf.SmoothDamp(m_light.range, targetLightRange, ref v1, GlobalSmoothTime);
            m_sc.radius = m_light.range / 2.0f;
        }

        if (GlowTag == "AllPublic")
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) targetColorScale = Mathf.Min(1, currColorScale + GlobalDeltaPerChange);
            if (Input.GetKeyDown(KeyCode.LeftArrow)) targetColorScale = Mathf.Max(0, currColorScale - GlobalDeltaPerChange);

            float v2 = 0.0f;
            currColorScale = Mathf.SmoothDamp(currColorScale, targetColorScale, ref v2, GlobalSmoothTime);
            float relativeScale = currColorScale;
            if (currColorScale > 0.5f)
            {
                relativeScale = (1.0f - relativeScale) / 0.5f;
                currColor.b = relativeScale;
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

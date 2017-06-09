using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowScriptLocal : MonoBehaviour {
    private SphereCollider m_sc;
    private Light m_light;
    private float MinLightRange = 0.0f;
    private float MaxLightRange = 0.3f;
    private float LightSmoothTime = 0.1f;

    [SerializeField]
    private float targetLightRange = 0.1f; // Default
    private float lightRangeDelta;

    // Use this for initialization
    void Start () {
        m_sc = GetComponent<SphereCollider>();
        m_light = GetComponent<Light>();
        m_light.range = targetLightRange;
        lightRangeDelta = (MaxLightRange - MinLightRange) / 10.0f; // Default each press moves 10% of range
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            targetLightRange = Mathf.Min(MaxLightRange, m_light.range + lightRangeDelta);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            targetLightRange = Mathf.Max(MinLightRange, m_light.range - lightRangeDelta);

        float v = 0.0f;
        m_light.range = Mathf.SmoothDamp(m_light.range, targetLightRange , ref v, LightSmoothTime);
        m_sc.radius = m_light.range / 2.0f;
	}
}

/*
* Create by William (c)
* https://github.com/Long18
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{

    #region Variables

    public float CurrentTimeInDay;

    [SerializeField] Light m_DirectionalLight;
    [SerializeField] LightingPreset m_Preset;

    [SerializeField, Range(0, 24)] float m_TimeOfDay;

    float m_TimeInRealLife = 24f;
    
    [Tooltip("The time in real life, disable if you want to use the time of day in game")]
    [SerializeField] bool m_IsReal = true;


    #endregion

    #region Unity Methods

    void Update()
    {
        if (m_Preset == null)
            return;

        if (Application.isPlaying)
        {
            if (m_IsReal)
                m_TimeOfDay += Time.deltaTime / m_TimeInRealLife;
            else
                m_TimeOfDay += Time.deltaTime;
            m_TimeOfDay %= 24;
            UpdateLighting(m_TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(m_TimeOfDay / 24f);
        }

        CurrentTimeInDay = m_TimeOfDay;
    }

    void OnValidate()
    {
        if (m_DirectionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            m_DirectionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] _lights = FindObjectsOfType<Light>();

            foreach (Light _light in _lights)
            {
                if (_light.type == LightType.Directional)
                {
                    m_DirectionalLight = _light;
                    return;
                }
            }
        }
    }

    #endregion

    #region Class

    private void UpdateLighting(float _timePercent)
    {
        RenderSettings.ambientLight = m_Preset.m_AmbientColor.Evaluate(_timePercent);
        RenderSettings.fogColor = m_Preset.m_FogColor.Evaluate(_timePercent);

        if (m_DirectionalLight != null)
        {
            m_DirectionalLight.color = m_Preset.m_DirectionColor.Evaluate(_timePercent);
            m_DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((_timePercent * 360f) - 90f, 170f, 0f));
        }
    }

    #endregion
}

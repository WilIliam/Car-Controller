/*
* Create by William (c)
* https://github.com/Long18
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "LightingPreset", menuName = "Scriptable/Lighting Preset", order = 1)]
public class LightingPreset : ScriptableObject
{

    #region Variables

    public Gradient m_AmbientColor;
    public Gradient m_DirectionColor;
    public Gradient m_FogColor;
    #endregion
}

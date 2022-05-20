/*
* Create by William (c)
* https://github.com/Long18
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleLight : MonoBehaviour
{

    #region Variables

    public enum Side
    {
        Front,
        Back
    }

    [System.Serializable]
    public struct Light
    {
        public GameObject m_LightObject;
        public Side m_Side;
    }

    public List<Light> m_Lights;

    public bool m_IsFrontLightOn;
    public bool m_IsBackLightOn;
    #endregion

    #region Unity Methods

    // Start is called before the first frame update
    void Start()
    {
        m_IsBackLightOn = false;
    }


    #endregion

    #region Class

    public void OperateFrontLights()
    {
        m_IsFrontLightOn = !m_IsFrontLightOn;

        if (m_IsFrontLightOn)
        {
            // Turn the lights on
            foreach (Light light in m_Lights)
            {
                if (light.m_Side == Side.Front && !light.m_LightObject.activeInHierarchy)
                {
                    light.m_LightObject.SetActive(true);
                }
            }
        }
        else
        {
            //Turn the lights off
            foreach (Light light in m_Lights)
            {
                if (light.m_Side == Side.Front && light.m_LightObject.activeInHierarchy)
                {
                    light.m_LightObject.SetActive(false);
                }
            }
        }
    }

    public void OperateBackLights()
    {
        if (m_IsBackLightOn)
        {
            // Turn the lights on
            foreach (Light light in m_Lights)
            {
                if (light.m_Side == Side.Back && !light.m_LightObject.activeInHierarchy)
                {
                    light.m_LightObject.SetActive(true);
                }
            }
        }
        else
        {
            //Turn the lights off
            foreach (Light light in m_Lights)
            {
                if (light.m_Side == Side.Back && light.m_LightObject.activeInHierarchy)
                {
                    light.m_LightObject.SetActive(false);
                }
            }
        }
    }
    #endregion
}

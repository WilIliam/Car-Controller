using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{


    public GameObject m_Player;
    VehicleController m_VehicleController;
    private const float MAX_SPEED_ANGLE = -20;
    private const float ZERO_SPEED_ANGLE = 230;

    Transform m_NeedleTranform;
    Transform m_SpeedLabelTemplateTransform;

    private const float m_SpeedMax = 200f;
    private float m_Speed;

    private void Awake()
    {
        m_NeedleTranform = transform.Find("needle");
        m_SpeedLabelTemplateTransform = transform.Find("speedLabelTemplate");
        m_SpeedLabelTemplateTransform.gameObject.SetActive(false);

        m_VehicleController = m_Player.GetComponent<VehicleController>();
        CreateSpeedLabels();
    }

    private void FixedUpdate()
    {
        HandlePlayerInput();

        m_NeedleTranform.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
    }

    private void HandlePlayerInput()
    {

        m_Speed = m_VehicleController.m_Speed;

        // m_Speed = Mathf.Clamp(m_Speed, 0f, m_SpeedMax);
    }

    private void CreateSpeedLabels()
    {
        int labelAmount = 10;
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        for (int i = 0; i <= labelAmount; i++)
        {
            Transform speedLabelTransform = Instantiate(m_SpeedLabelTemplateTransform, transform);
            float labelSpeedNormalized = (float)i / labelAmount;
            float speedLabelAngle = ZERO_SPEED_ANGLE - labelSpeedNormalized * totalAngleSize;
            speedLabelTransform.eulerAngles = new Vector3(0, 0, speedLabelAngle);
            speedLabelTransform.Find("speedText").GetComponent<Text>().text = Mathf.RoundToInt(labelSpeedNormalized * m_SpeedMax).ToString();
            speedLabelTransform.Find("speedText").eulerAngles = Vector3.zero;
            speedLabelTransform.gameObject.SetActive(true);
        }

        m_NeedleTranform.SetAsLastSibling();
    }

    private float GetSpeedRotation()
    {
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;

        float speedNormalized = m_Speed / m_SpeedMax;

        return ZERO_SPEED_ANGLE - speedNormalized * totalAngleSize;
    }
}

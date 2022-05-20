/*
* Create by William (c)
* https://github.com/Long18
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{

    #region Variables

    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject m_WheelObject;
        public GameObject m_WheelEffect;
        public WheelCollider m_WheelCollider;
        public ParticleSystem m_SmokeParticle;
        public Axel m_Axel;
    }

    public float m_MaxAcceleration = 30.0f;
    public float m_BreakAcceleration = 50.0f;

    public float m_TurnSensitivity = 1f;
    public float m_MaxSteerAngle = 30.0f;
    public Vector3 m_CenterOfMass;

    public List<Wheel> m_Wheels;

    public float m_Speed;

    const string HORIZONTAL = "Horizontal";
    const string VERTICAL = "Vertical";

    float m_HorizontalInput;
    float m_VerticalInput;
    float m_TimeOfDay;
    bool m_IsBreaked;
    bool m_IsBoosted;

    Rigidbody m_Rigidbody;

    VehicleLight m_VehicleLight;
    [SerializeField] LightingManager m_LightingManager;

    Vector3 lastPosition;

    #endregion

    #region Unity Methods

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.centerOfMass = m_CenterOfMass;
        m_VehicleLight = GetComponent<VehicleLight>();
    }

    void Update()
    {
        GetInput();
        AnimationOfWheels();
        WheelEffect();
        LightControl();

    }
    void FixedUpdate()
    {
        Move();
        Steer();
        Brake();
        ControlSpeed();
    }


    #endregion

    #region Class

    void GetInput()
    {
        m_VerticalInput = Input.GetAxis(VERTICAL);
        m_HorizontalInput = Input.GetAxis(HORIZONTAL);
        m_IsBreaked = Input.GetKey(KeyCode.Space);
        m_IsBoosted = Input.GetKey(KeyCode.LeftShift);
    }

    void Move()
    {
        foreach (var _Wheel in m_Wheels)
        {

            _Wheel.m_WheelCollider.motorTorque = m_VerticalInput * 600 * m_MaxAcceleration * Time.deltaTime;
        }

        if (m_IsBoosted)
        {
            //Boost Speed
            m_Rigidbody.AddForce(0.5f * transform.forward, ForceMode.VelocityChange);
        }
        // Debug.Log(m_VerticalInput);
    }

    void Steer()
    {
        foreach (var _Wheel in m_Wheels)
        {
            if (_Wheel.m_Axel == Axel.Front)
            {
                var _SteerAngle = m_HorizontalInput * m_MaxAcceleration * m_TurnSensitivity;
                _Wheel.m_WheelCollider.steerAngle = Mathf.Lerp(_Wheel.m_WheelCollider.steerAngle, _SteerAngle, 0.6f);
            }
        }
    }

    void Brake()
    {
        if (m_IsBreaked)
        {
            foreach (var _Wheel in m_Wheels)
            {
                _Wheel.m_WheelCollider.brakeTorque = 300 * m_BreakAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach (var _Wheel in m_Wheels)
            {
                _Wheel.m_WheelCollider.brakeTorque = 0;
            }
        }
    }

    void AnimationOfWheels()
    {
        foreach (var _Wheel in m_Wheels)
        {
            Quaternion _rot;
            Vector3 _pos;

            _Wheel.m_WheelCollider.GetWorldPose(out _pos, out _rot);
            _Wheel.m_WheelObject.transform.position = _pos;
            _Wheel.m_WheelObject.transform.rotation = _rot;
        }

    }

    void WheelEffect()
    {
        foreach (var _Wheel in m_Wheels)
        {
            if (m_IsBreaked && _Wheel.m_WheelCollider.isGrounded && _Wheel.m_Axel == Axel.Rear && m_Rigidbody.velocity.magnitude >= 10.0f)
            {
                _Wheel.m_WheelEffect.GetComponentInChildren<TrailRenderer>().emitting = true;
                _Wheel.m_SmokeParticle.Emit(1);
            }
            else
            {
                _Wheel.m_WheelEffect.GetComponentInChildren<TrailRenderer>().emitting = false;
            }
        }
    }

    void ControlSpeed()
    {
        m_Speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;
        // Debug.Log(m_Speed);
    }

    void LightControl()
    {
        m_TimeOfDay = m_LightingManager.CurrentTimeInDay;
        Debug.Log("Time of day: " + m_TimeOfDay);

        if (m_TimeOfDay >= 5 && m_TimeOfDay <= 18)
        {
            m_VehicleLight.m_IsFrontLightOn = true;
            m_VehicleLight.OperateFrontLights();
        }
        else
        {
            m_VehicleLight.m_IsFrontLightOn = false;
            m_VehicleLight.OperateFrontLights();
        }

        if (m_VerticalInput < 0 || m_IsBreaked)
        {
            m_VehicleLight.m_IsBackLightOn = true;
            m_VehicleLight.OperateBackLights();
        }
        else
        {
            m_VehicleLight.m_IsBackLightOn = false;
            m_VehicleLight.OperateBackLights();
        }
    }

    #endregion
}

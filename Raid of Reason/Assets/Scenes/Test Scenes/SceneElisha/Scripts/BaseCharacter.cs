//*
// @Brief: This class will be a base character class used to create our 3 main protagonists 
// Author: Elisha Anagnostakis
// Date: 14/05/19 
//*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class BaseCharacter : MonoBehaviour {

    public enum PlayerState
    {
        ALIVE,
        REVIVE,
        DEAD
    }

    [SerializeField]
    private PlayerState playerState;
    [SerializeField]
    private int m_maxHealth;
    [SerializeField]
    private float m_speed;
    [SerializeField]
    private float m_maxSpeed;
    [SerializeField]
    private int m_damage;
    [SerializeField]
    private float m_controlSpeed = 10.0f;
    [SerializeField]

    public XboxController controller;
    private float m_rotationSpeed = 250.0f;
    private Vector3 direction;
    private Vector3 prevRotDirection = Vector3.forward;
    private int m_health;

    void Start () {
        m_maxHealth = m_health;
  	}

    virtual public void FixedUpdate(){}

    virtual public void CharacterMovement()
    {
        /// <summary> 
        /// Handles the forward and backwards movement of the character via the xbox controller layout
        /// </summary>
        float axisX = XCI.GetAxisRaw(XboxAxis.LeftStickX, controller) * m_controlSpeed;
        float axisZ = XCI.GetAxisRaw(XboxAxis.LeftStickY, controller) * m_controlSpeed;
        axisX *= Time.deltaTime;
        axisZ *= Time.deltaTime;
        transform.Translate(axisX, 0, axisZ);

        /// <summary>
        /// Handles the rotation of the character via the xbox controller layout
        /// </summary>
        float rotAxisX = XCI.GetAxisRaw(XboxAxis.RightStickX, controller) * m_rotationSpeed;
        float rotAxisZ = XCI.GetAxisRaw(XboxAxis.RightStickY, controller) * m_rotationSpeed;
        direction = new Vector3(rotAxisX, 0, rotAxisZ);

        if (direction.magnitude < 0.1f)
        {
            direction = prevRotDirection;
        }
        direction = direction.normalized;
        prevRotDirection = direction;
        transform.localRotation = Quaternion.LookRotation(direction);

        //*

        // CAMERA RELATIVE CODE INSERT HERE

        //*
    }

    virtual public void SetDamage(int damage)
    {
        m_damage = damage;
    }

    virtual public void SetHealth(int health)
    {
        m_health = health;
    }

    virtual public void SetSpeed(float speed)
    {
        m_speed = speed;
    }

    virtual public void SetMaxSpeed(float maxSpeed)
    {
        m_maxSpeed = maxSpeed;
    }

    virtual public int GetDamage()
    {
        return m_damage;
    }

    virtual public int GetHealth()
    {
        return m_health;
    }

    virtual public float GetSpeed()
    {
        return m_speed;
    }

    virtual public float GetMaxSpeed()
    {
        return m_maxSpeed;
    }

    public void Player()
    {
        switch(playerState)
        {
            case PlayerState.ALIVE:
                CharacterMovement();
                break;
            case PlayerState.REVIVE:
                break;
            case PlayerState.DEAD:
                break;

            default:
                break;
        }
    }
}
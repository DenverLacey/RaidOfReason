//*
// @Brief: This class will be a base character class used to create our 3 main protagonists 
// Author: Elisha Anagnostakis
// Date: 14/05/19 
//*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public abstract class BaseCharacter : MonoBehaviour {

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
    private int m_currentHealth;
    [SerializeField]
    private int m_damage;
    private int m_armor;
    [SerializeField]
    private float m_controlSpeed;

    protected XboxController controller;
    private float m_rotationSpeed = 250.0f;
    private Vector3 direction;
    private Vector3 prevRotDirection = Vector3.forward;

    void Start () {
        m_currentHealth = m_maxHealth;
  	}

    protected virtual void FixedUpdate() {   }

    protected virtual void Update() { }

    virtual protected void CharacterMovement()
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

    public virtual void TakeDamage(int damage)
    {
        m_currentHealth -= damage;

        if (m_currentHealth <= 0.0f)
        {
            playerState = PlayerState.REVIVE;
        }
    }

    virtual public void SetDamage(int damage)
    {
        m_damage = damage;
    }

    virtual public void SetArmor(int armor) {
        m_armor = armor;
    }

    virtual public int GetArmor()
    {
        return m_armor;
    }

     public void SetHealth(int health)
    {
        health = m_maxHealth;
    }

    virtual public float GetSpeed()
    {
        return m_controlSpeed;
    }

    public void SetSpeed(float speed)
    {
        speed = m_controlSpeed;
    }

    virtual public int GetDamage()
    {
        return m_damage;
    }

     public int GetHealth()
    {
        return m_maxHealth;
    }

    virtual protected void Player()
    {
        switch (playerState)
        {
            case PlayerState.ALIVE:
                //Debug.Log("alive state activated");
                CharacterMovement();
                break;
            case PlayerState.REVIVE:
                Debug.Log("revive state activated");
                break;
            case PlayerState.DEAD:
                Debug.Log("dead state activated");
                break;

            default:
                break;
        }
    }
}
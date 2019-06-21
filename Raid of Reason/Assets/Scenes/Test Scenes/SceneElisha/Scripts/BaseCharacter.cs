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

    public XboxController controller;
    [SerializeField] private PlayerState playerState;
    [SerializeField] protected float m_damage;
    [SerializeField] private float m_controlSpeed;
    public float m_maxHealth;

    [HideInInspector]
    public float m_currentHealth;
    public int m_playerSkillPoints;

    private float m_rotationSpeed = 250.0f;
    private Vector3 direction;
    private Vector3 prevRotDirection = Vector3.forward;

    protected bool m_bActive;

    public MultiTargetCamera m_camera;

    protected virtual void Awake () {
        m_currentHealth = m_maxHealth;
        m_bActive = false;
        m_camera = FindObjectOfType<MultiTargetCamera>();
        m_playerSkillPoints = 0;
}

    protected virtual void FixedUpdate()
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

    protected virtual void Update() {
	}

    virtual protected void CharacterMovement() {

        ///<summary> 
        /// Handles the forward and backwards movement of the character via the xbox controller layout
        /// </summary>
        float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller) * m_controlSpeed;
        float axisZ = XCI.GetAxis(XboxAxis.LeftStickY, controller) * m_controlSpeed;

        Vector3 movement = m_camera.transform.TransformDirection(axisX, 0, axisZ);
        transform.position += new Vector3(movement.x, 0, movement.z) * Time.deltaTime;

        /// <summary>
        /// Handles the rotation of the character via the xbox controller layout
        /// </summary>
        float rotAxisX = XCI.GetAxis(XboxAxis.RightStickX, controller) * m_rotationSpeed;
        float rotAxisZ = XCI.GetAxis(XboxAxis.RightStickY, controller) * m_rotationSpeed;

        Vector3 rawDir = m_camera.transform.TransformDirection(rotAxisX, 0, rotAxisZ);
        Vector3 direction = new Vector3(rawDir.x, 0, rawDir.z);

        if (direction.magnitude < 0.1f)
        {
            direction = prevRotDirection;
        }
        direction = direction.normalized;
        prevRotDirection = direction;
        transform.localRotation = Quaternion.LookRotation(direction);
    }

    public virtual void TakeDamage(float damage)
    {
        m_currentHealth -= damage;

        if (m_currentHealth <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    virtual public void SetDamage(float damage)
    {
        m_damage = damage;
    }

    public void SetHealth(float health)
    {
        m_currentHealth = health;
    }
     
    virtual public float GetSpeed()
    {
        return m_controlSpeed;
    }

    public void SetSpeed(float speed)
    {
        speed = m_controlSpeed;
    }

    virtual public float GetDamage()
    {
        return m_damage;
    }

    public float GetHealth()
    {
        return m_currentHealth;
    }

    public void SetMaxHealth(float maxhealth) {
        maxhealth = m_maxHealth;
    }

    public float GetMaxHealth()
    {
        return m_maxHealth;
    }

  
}
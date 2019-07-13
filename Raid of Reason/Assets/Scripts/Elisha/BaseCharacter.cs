//*
// @Brief: This class will be a base character class used to create our 3 main protagonists 
// Author: Elisha Anagnostakis
// Date: 14/05/19 
//*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using TMPro;

[RequireComponent(typeof(Animator))]
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
    protected float m_damage;
    [SerializeField]
    private float m_movementSpeed;

    public float m_maxHealth;
    public XboxController controller;

    //[HideInInspector]
    public float m_currentHealth;
    [SerializeField]
    public int m_playerSkillPoints;

    public List<SkillsAbilities> playerSkills = new List<SkillsAbilities>();
    public Dictionary<string, SkillsAbilities> dicSkills = new Dictionary<string, SkillsAbilities>();

    private float m_rotationSpeed = 250.0f;
    private Vector3 direction;
    private Vector3 prevRotDirection = Vector3.forward;
   
    protected bool m_bActive;

    public MultiTargetCamera m_camera;
    public TextMeshProUGUI m_SkillDisplay;
    public TextMeshProUGUI m_SkillMaxed;

    // a multiplier for damage taken
    protected float m_vulnerability;

    public int SkillPoints {
        get { return m_playerSkillPoints;  }
        set {
            m_playerSkillPoints = value;
        }
    }

    protected Animator m_animator;

    protected virtual void Awake () {
        m_currentHealth = m_maxHealth;
        m_vulnerability = 1.0f;
        m_bActive = false;
        m_camera = FindObjectOfType<MultiTargetCamera>();
        m_animator = GetComponent<Animator>();
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
        m_SkillDisplay.text = m_playerSkillPoints.ToString();
	}

    protected virtual void Update() {
        if (playerSkills.Count == 4)
        {
            m_SkillDisplay.gameObject.SetActive(false);
            m_SkillMaxed.gameObject.SetActive(true);
        }
    }

    virtual protected void CharacterMovement() {
        ///<summary> 
        /// Handles the forward and backwards movement of the character via the xbox controller layout
        /// </summary>
        float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller) * m_movementSpeed;
        float axisZ = XCI.GetAxis(XboxAxis.LeftStickY, controller) * m_movementSpeed;
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
		direction.Normalize();
        prevRotDirection = direction;
        transform.localRotation = Quaternion.LookRotation(direction);

        if (m_animator)
        {
            // calculate angle between character's direction and forward
            float angle = Vector3.SignedAngle(direction, Vector3.forward, Vector3.up);

            // rotate movement into world space to get animation movement
            Vector3 animationMovement = Quaternion.AngleAxis(angle, Vector3.up) * movement.normalized;

            // set animator's movement floats
            m_animator.SetFloat("MovX", animationMovement.x);
            m_animator.SetFloat("MovZ", animationMovement.z);
            m_animator.SetFloat("Speed", movement.magnitude);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        m_currentHealth -= damage * m_vulnerability;
        GetComponent<MeshRenderer>().material.color = Color.red;
        StartCoroutine(ResetMaterialColour(this.gameObject, .2f));

        if (m_currentHealth <= 0.0f)
        {
            OnDeath();
            Destroy(gameObject);
        }
    }

    protected virtual void OnDeath()
    {
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
        return m_movementSpeed;
    }

    public void SetSpeed(float speed)
    {
        speed = m_movementSpeed;
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

    public float GetVulnerability() {
        return m_vulnerability;
    }

    public void SetVulnerability(float vulnerability) {
        m_vulnerability = vulnerability;
    }

    public void ResetVulernability() {
        m_vulnerability = 1.0f;
    }

    public delegate void OnPointChange();
    public event OnPointChange onPointChange;

    public void UpdateSkillPont(int amount) {
        m_playerSkillPoints += amount;
    }

    IEnumerator ResetMaterialColour(GameObject player, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (player)
        {
            player.GetComponent<MeshRenderer>().material.color = Color.clear;
        }
    }
}
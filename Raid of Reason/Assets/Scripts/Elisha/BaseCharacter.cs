﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

/* 
 * Author: Elisha, Denver, Afridi
 * Description: Abstract base class that will be derived by character classes Kenron, Nashorn and Thea. Handles everything to do 
 *              with the characters movements and states.
 */


public abstract class BaseCharacter : MonoBehaviour 
{
    public enum PlayerState
    {
        ALIVE,
        REVIVE,
        DEAD
    }

    public PlayerState playerState;

    public float m_maxHealth;
    public float m_currentHealth;

    [SerializeField]
    [Tooltip("How long can a player be in revive state before they die?")]
    private float m_deathTimer;

    [SerializeField]
    [Tooltip("How much damage will the player deal?")]
    protected float m_damage;

    [SerializeField]
    [Tooltip("Hpw fast will the player move?")]
    protected float m_movementSpeed;

    [Tooltip("Pick what controller this player is.")]
    public XboxController controller;

    // Player Colliders
    private CapsuleCollider m_playerCollider;

    [SerializeField]
    [Tooltip("How long will it take the player to revive his teammate?")]
    private float m_reviveTimer;

    [SerializeField]
    [Tooltip("How big is the radius of the revive?")]
    private float m_reviveRadius;

    [SerializeField]
    [Tooltip("How much health will the player get back when revived?")]
    private float m_healthUponRevive;

    public List<Image> m_skillPopups = new List<Image>();

    [Tooltip("The Skill Manager that manages the skills of the players")]
    public SkillManager skillManager;

    [Tooltip("A List of How Many Skill Upgrades the Players Have")]
    public List<SkillsAbilities> m_skillUpgrades = new List<SkillsAbilities>();

    [HideInInspector]
    public bool m_controllerOn;

	[HideInInspector]
    public MultiTargetCamera m_camera;

    protected Animator m_animator;
    public SphereCollider m_reviveColliderRadius;

    [HideInInspector]
    public bool IsBeingRevived { get; private set; }
   
    protected float m_vulnerability;
    protected bool m_bActive;
    protected float m_rotationSpeed = 250.0f;
    private Vector3 m_direction;
    protected Vector3 m_prevRotDirection = Vector3.forward;
    private bool m_isRevived = false;
    private MeshRenderer m_renderer;
    private Color m_originalColour;


    /// <summary>
    /// This will be called first.
    /// </summary>
    protected virtual void Awake () {

        m_renderer = GetComponentInChildren<MeshRenderer>();
        // Gets the original colour of the player.
        m_originalColour = m_renderer.sharedMaterial.color;
        m_camera = FindObjectOfType<MultiTargetCamera>();
        m_playerCollider = GetComponent<CapsuleCollider>();
        m_animator = GetComponentInChildren<Animator>();
        m_currentHealth = m_maxHealth;
        m_vulnerability = 1.0f;
        m_reviveTimer = 5f;
        m_controllerOn = true;
        m_bActive = false;
        m_reviveColliderRadius.enabled = false;
    }

    /// <summary>
    /// Physics update.
    /// </summary>
    protected virtual void FixedUpdate()
    {
        // Checks the states of all derived players of this class.
		switch (playerState)
		{
			case PlayerState.ALIVE:
                // Call this.
                CharacterMovement();
                m_playerCollider.enabled = true;
                break;

			case PlayerState.REVIVE:
                m_deathTimer -= Time.deltaTime;
                break;

			case PlayerState.DEAD:
                // Player gets removed from camera array.
                if (m_camera.targets.Count > 0)
                {
                    m_camera.targets.Remove(this.gameObject.transform);
                }
                m_playerCollider.enabled = false;
                break;

			default:
                Debug.Log("default");
				break;
		}
	}

    /// <summary>
    /// Updates every frame.
    /// </summary>
    protected virtual void Update() {

        if (XCI.GetButton(XboxButton.B))
        {
            ReviveTeammates();
        }
        else
        {
            IsBeingRevived = false;
            m_reviveTimer = 5f;
        }

        // Makes sure health doesnt exceed limit.
        if (m_currentHealth >= m_maxHealth) 
        {
            m_currentHealth = m_maxHealth;
        }
    }

    /// <summary>
    /// Handles the main movement of each derived character using Xinput.
    /// </summary>
    virtual protected void CharacterMovement() 
    {
        // Checks if player is active.
        if (m_controllerOn)
        {
            // Calculates the x axis of the left stick (left and right movement).
            float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller) * m_movementSpeed;
            // Calculates the z axis of the left stick (forward and backward movement).
            float axisZ = XCI.GetAxis(XboxAxis.LeftStickY, controller) * m_movementSpeed;
            // Makes sure Player movement is relative to the direction of the cameras forward.
            Vector3 movement = m_camera.transform.TransformDirection(axisX, 0, axisZ);
            transform.position += new Vector3(movement.x, 0, movement.z) * Time.deltaTime;

            // Calculates the rotation on the x axis of the right stick.
            float rotAxisX = XCI.GetAxis(XboxAxis.RightStickX, controller) * m_rotationSpeed;
            // Calculates the rotation on the z axis of the right stick.
            float rotAxisZ = XCI.GetAxis(XboxAxis.RightStickY, controller) * m_rotationSpeed;
            // Makes sure Player rotation is relative to the direction of the cameras forward.
            Vector3 rawDir = m_camera.transform.TransformDirection(rotAxisX, 0, rotAxisZ);
            Vector3 direction = new Vector3(rawDir.x, 0, rawDir.z); 
            // Checks if the magnitude of the vector is less than 0.1
            if (direction.magnitude < 0.1f)
            {
                // Change direction of the vector.
                direction = m_prevRotDirection;
            }
            // Normalise the vector to 1.
            direction.Normalize();
            // New direction
            m_prevRotDirection = direction;
            // Rotate the player to that direction.
            transform.localRotation = Quaternion.LookRotation(direction);

            if (m_animator)
            {
                // Calculate angle between character's direction and forward
                float angle = Vector3.SignedAngle(direction, Vector3.forward, Vector3.up);

                // Rotate movement into world space to get animation movement
                Vector3 animationMovement = Quaternion.AngleAxis(angle, Vector3.up) * movement.normalized;

                // flip x movement if walking backwards
                animationMovement.x *= animationMovement.z >= 0 ? 1 : -1;

                // Set animator's movement floats
                m_animator.SetFloat("MovX", animationMovement.x);
                m_animator.SetFloat("MovZ", animationMovement.z);
            }
        }
    }

    /// <summary>
    /// This allows the player to take damage.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage)
    {
        // Take an amount of damage from the players current health.
        m_currentHealth -= damage * m_vulnerability;
        // Player damage indicator.
        IndicateHit();

        // If player has no health.
        if (m_currentHealth <= 0.0f)
        {
            // Player revive state.
            OnDeath();
        }
    } 

    void ReviveTeammates()
    {
        // checks if a player needs to be healed
        foreach (var player in GameManager.Instance.Players)
        {
            // don't check self
            if (player == this) { continue; }

            // check if player outside revive range
            float sqrDistance = (player.transform.position - transform.position).sqrMagnitude;
            if (sqrDistance > m_reviveRadius * m_reviveRadius) { continue; }
            
            player.IsBeingRevived = true;
            
            // check if player doesn't need to be revived
            if (player.playerState != PlayerState.REVIVE) { continue; }

            // Start the revive timer.
            player.m_reviveTimer -= Time.deltaTime;

            // TODO: Start revive particle effect.
            // If the revive timer hits 0.
            if (player.m_reviveTimer <= 0)
            {
                // Player revived.
                player.m_isRevived = true;
                player.IsBeingRevived = false;
                player.m_controllerOn = true;
                // Disable the revive collider.
                player.m_reviveColliderRadius.enabled = false;
                GameManager.Instance.Thea.m_aimCursor.SetActive(true);

                // Change them to the ALIVE state.
                player.playerState = PlayerState.ALIVE;
                // Give the player some health when they get back up.
                player.m_currentHealth = m_healthUponRevive;
                // TODO: Stop revive particle effect.
                // Reset timer.
                player.m_reviveTimer = 5f;
                player.m_deathTimer = 20f;
                m_playerCollider.enabled = true;
            }
            
            if(player.m_deathTimer <= 0)
            {
                // Kill the player 
                player.playerState = PlayerState.DEAD;
                player.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// This gets called when player has no health. Allows the player to be revived by an ally.
    /// </summary>
    protected virtual void OnDeath()
    {
        playerState = PlayerState.REVIVE;
        if (GameManager.Instance.Thea.playerState == PlayerState.REVIVE)
        {
            GameManager.Instance.Thea.m_aimCursor.SetActive(false);
        }
        
        m_controllerOn = false;
        // Enable revive collider.
        m_reviveColliderRadius.enabled = true;
        // Have the colliders radius equal the AOE float radius.
        m_reviveColliderRadius.radius = m_reviveRadius;
    }

    /// <summary>
    /// Sets damage to a float value.
    /// </summary>
    /// <param name="damage"></param>
    virtual public void SetDamage(float damage)
    {
        m_damage = damage;
    }

    /// <summary>
    /// Sets health to a float value.
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(float health)
    {
        m_currentHealth = health;
    }

    /// <summary>
    /// Returns the players movement speed.
    /// </summary>
    /// <returns> float value. </returns>
    virtual public float GetSpeed()
    {
        return m_movementSpeed;
    }

    /// <summary>
    /// Sets the players speed to a float value.
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        m_movementSpeed = speed;
    }

    /// <summary>
    /// Returns the damage taken.
    /// </summary>
    /// <returns> float value. </returns>
    virtual public float GetDamage()
    {
        return m_damage;
    }

    /// <summary>
    /// Returns the current health of the player.
    /// </summary>
    /// <returns> float value. </returns>
    public float GetHealth()
    {
        return m_currentHealth;
    }

    /// <summary>
    /// Sets the max health of the player.
    /// </summary>
    /// <param name="maxhealth"></param>
    public void SetMaxHealth(float maxhealth) {
        maxhealth = m_maxHealth;
    }

    /// <summary>
    /// Gets the player max's health.
    /// </summary>
    /// <returns> float value. </returns>
    public float GetMaxHealth()
    {
        return m_maxHealth;
    }

    /// <summary>
    /// Gets Nashorns vulnerability status.
    /// </summary>
    /// <returns> float value. </returns>
    public float GetVulnerability() {
        return m_vulnerability;
    }

    /// <summary>
    /// Sets Nashorns vulnerability status.
    /// </summary>
    /// <param name="vulnerability"></param>
    public void SetVulnerability(float vulnerability) {
        m_vulnerability = vulnerability;
    }

    /// <summary>
    /// Resets Nashorns vulnerability to 1.
    /// </summary>
    public void ResetVulernability() {
        m_vulnerability = 1.0f;
    }

    /// <summary>
    /// A corotine that resets the players mesh colour back to normal when called.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="delay"></param>
    /// <returns> Gameobject and a float value. </returns>
    IEnumerator ResetMaterialColour(GameObject player, float delay)
    {
        // Suspends the coroutine execution for the given amount of seconds.
        yield return new WaitForSeconds(delay);

        // If player gets returned.
        if (player)
        {
            // Change players mesh colour back to the original colour.
            player.GetComponent<MeshRenderer>().material.color = Color.clear;
        }
    }

    void IndicateHit()
    {
        m_renderer.material.color = Color.red;
        StartCoroutine(ResetColour(.2f));
    }

    IEnumerator ResetColour(float duration)
    {
        yield return new WaitForSeconds(duration);
        m_renderer.material.color = m_originalColour;
    }
}
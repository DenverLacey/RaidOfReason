﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

/* 
 * Author: Elisha_Anagnostakis
 * Description: Thea's main class, which handles her basic attacks and abilities.
 */

public class Theá : BaseCharacter
{
    [SerializeField]
    private GameObject m_waterPrefab;

    [SerializeField]
    [Tooltip("Insert Theas projectile object.")]
    private GameObject m_projectile;

    [SerializeField]
    [Tooltip("How long of a delay will it take for her next projectile to instiantiate?")]
    private float m_projectileDelay;

    [SerializeField]
    private SkillManager m_skillManager;

    [SerializeField]
    [Tooltip("AOE collider that grows over time.")]
    private SphereCollider m_AOEParticleCollider;

    [SerializeField]
    [Tooltip("The AOE particle used for visual effect.")]
    private ParticleSystem m_AOEParticle;

    [SerializeField]
    [Tooltip("How big can Thea's AOE get?")]
    private float m_AOEMax;

    [SerializeField]
    [Tooltip("What is Thea's minimum radius of her AOE.")]
    private float m_AOEMin;

    [SerializeField]
    [Tooltip("How fast will Thea's AOE grow?")]
    private float m_AOEGrowTime;

    [SerializeField]
    [Tooltip("How much the Gift of Poseidon heals by?")]
    private float m_GOPEffect;

    [SerializeField]
    [Tooltip("How fast will Thea's cursor move?")]
    private float aimCursorSpeed;

    [Tooltip("How much health thea passively heals with her first skill")]
    public float healthRegenerated;

    [Tooltip("The delay between how much health thea passively with her first skill")]
    public float regenEachFrame;

    [SerializeField]
    private List<EnemyData> m_nearbyEnemies = new List<EnemyData>();

    [Tooltip("hpw much the enemies are knockbacked by theas third skill")]
    public float knockbackForce;

    // Runs once check
    private bool neverDone;

    public float m_aimCursorRadius;

    private Vector3 newLocation;

    private bool m_isHealthRegen;

    private Kenron m_kenron;
    private Nashorn m_nashorn;
    private BaseCharacter m_playerController;
    private Vector3 m_hitLocation;
    private LayerMask m_layerMask;
    private GameObject m_temp;
    public GameObject m_aimCursor;
    private float m_shotCounter;
    private float m_counter;
    private bool m_isActive;
    public bool m_skillActive;
    private float m_AOERadius;
    private float m_AOETimer;
    private float m_particleRadius;
    public bool nashornBuffGiven = false;

    ParticleSystem.ShapeModule aoe;

	private void Start()
	{
		GameManager.Instance.GiveCharacterReference(this);
	}

	/// <summary>
	/// Gets called before Start.
	/// </summary>
	protected override void Awake()
    {
        base.Awake();
        m_isActive = false;
        neverDone = true;
        m_isHealthRegen = false;
        m_nashorn = FindObjectOfType<Nashorn>();
		m_kenron = FindObjectOfType<Kenron>();
        m_playerController = GetComponent<BaseCharacter>();
        m_AOETimer = 0f;
        m_AOEParticle = FindObjectOfType<ParticleSystem>();
        m_AOEParticleCollider.transform.position = this.gameObject.transform.position;
        m_AOEParticle.transform.position = this.gameObject.transform.position;
        m_AOEParticleCollider.enabled = false;
        aoe = m_AOEParticle.shape;
        m_aimCursor = GameObject.FindGameObjectWithTag("AimCursor");
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        // If the player is alive.
        if(this.gameObject != null)
        {
			base.FixedUpdate();
			Projectile();
            SkillChecker();
            CharacterMovement();
        }
    }

    protected override void CharacterMovement()
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

            // Calculates the x axis of the right stick (left and right movement).
            float cursorAxisX = XCI.GetAxis(XboxAxis.RightStickX, controller) * aimCursorSpeed;
            // Calculates the z axis of the right stick (forward and backward movement).
            float cursorAxisZ = XCI.GetAxis(XboxAxis.RightStickY, controller) * aimCursorSpeed;

            Vector3 cursorMovement = m_camera.transform.TransformDirection(cursorAxisX, 0, cursorAxisZ);
            cursorMovement.y = 0f;

            m_aimCursor.transform.position += cursorMovement * Time.deltaTime;

            Vector3 direction = (m_aimCursor.transform.position - transform.position).normalized;
            direction.y = 0f;

            transform.rotation = Quaternion.LookRotation(direction);

            //Vector3 clampedPosition = m_aimCursor.transform.position;
            //clampedPosition.x = Mathf.Clamp(clampedPosition.x, minCursorPosition.position.x, maxCursorPosition.position.x);
            //clampedPosition.z = Mathf.Clamp(clampedPosition.z, minCursorPosition.position.z, maxCursorPosition.position.z);
            //m_aimCursor.transform.position = clampedPosition;

            // Thea is the centre position of the cursor radius.
            Vector3 centerPosition = transform.position;
			centerPosition.y = m_aimCursor.transform.position.y;
            // Find the distance between Thea and the cursor.
            float distance = Vector3.Distance(m_aimCursor.transform.position, centerPosition);

            // Radius 
            if(distance > m_aimCursorRadius)
            {
                Vector3 fromOriginToObject = m_aimCursor.transform.position - centerPosition; 
                fromOriginToObject *= m_aimCursorRadius / distance;
                m_aimCursor.transform.position = centerPosition + fromOriginToObject;
            } 


            if (m_animator)
            {
                // Calculate angle between character's direction and forward
                float angle = Vector3.SignedAngle(Vector3.forward, Vector3.forward, Vector3.up);

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
    /// Theas projectile instantiation and damage when pressing the trigger button.
    /// </summary>
    public void Projectile()
    {
        m_counter += Time.deltaTime;

        // If the player presses the right trigger button.
        if (XCI.GetAxis(XboxAxis.RightTrigger, this.controller) > 0.1)
        {
            // FindObjectOfType<AudioManager>().PlaySound("TheaProjectile");
            // Start the shot counter.
            m_shotCounter += Time.deltaTime;

            if (m_counter > m_projectileDelay)
            {
                // Instantiate projectile object.
                GameObject temp = Instantiate(m_projectile, transform.position + transform.forward * 2 + Vector3.up * transform.lossyScale.y * 2, transform.rotation);
                // Set projectile damage and move projectile.
				temp.GetComponent<ProjectileMove>().SetDamage(m_damage);
                // Reset counter.
                m_counter = 0f;
            }
        }
        // If player releases the right trigger button.
        else if (XCI.GetAxis(XboxAxis.RightTrigger, this.controller) < 0.1)
        {
            // Reset the counter.
            m_shotCounter = 0f;
        }
    }

    void SkillChecker()
    {
        if (m_playerSkills.Find(skill => skill.name == "Settling Tide"))
        {
            if (neverDone == true)
            {
                m_skillManager.m_Skills[2].m_coolDown = m_skillManager.m_Skills[2].m_coolDown / 2;
                neverDone = false;
            }
            if (m_currentHealth != m_maxHealth && !m_isHealthRegen)
            {
                StartCoroutine(HealthOverTime());
            }
        }
        if (m_playerSkills.Find(skill => skill.name == "Oceans Ally"))
        {
            float healthcomparison = m_kenron.m_currentHealth + m_nashorn.m_currentHealth;

            if (healthcomparison <= 150)
            {
                m_projectileDelay = 0.7f;
                m_damage = 13.0f;
            }
            if (healthcomparison <= 130)
            {
                m_projectileDelay = 0.5f;
                m_damage = 18.0f;
            }
            if (healthcomparison <= 60)
            {
                m_projectileDelay = 0.3f;
                m_damage = 24.0f;
            }
            if (healthcomparison <= 25)
            {
                m_projectileDelay = 0.1f;
                m_damage = 35.0f;
            }
        }
        if (m_isActive == true && m_playerSkills.Find(skill => skill.name == "Hydro Pressure"))
        {
            m_nearbyEnemies = new List<EnemyData>(FindObjectsOfType<EnemyData>());
            foreach (EnemyData enemy in m_nearbyEnemies) {
                float sqrDistance = (this.transform.position - enemy.transform.position).sqrMagnitude;
                if (sqrDistance <= m_AOERadius * m_AOERadius) {
                    enemy.Stun(0.5f);
                    enemy.Rigidbody.AddExplosionForce(knockbackForce, transform.position, m_AOERadius, 0, ForceMode.Impulse);
                    enemy.TakeDamage(-5);
                }
            }
        }
    }

    /// <summary>
    /// Thea has the ability to do a charge up time attack with scaling AOE and heal.
    /// The longer you hold the charge up, the bigger the AOE is which means 
    /// the more health is healed when released. If players are in the AOE when 
    /// the charge up is released the players will be healed the right amount.
    /// </summary>
    public void GiftOfPoseidon()
    {
        int coolDown = 20;
        m_isActive = true;
        // Enables collider for the ability.
        m_AOEParticleCollider.enabled = true;

        // Checks if player can use the ability.
        if (m_isActive == true && coolDown == 20)
        {
            
            // Start AOE timer.
            m_AOETimer += Time.deltaTime;
            // Disable player movement and rotation.
            m_playerController.m_controllerOn = false;

			// turn AOE effect on
			m_AOEParticle.gameObject.SetActive(true);
			//m_AOEParticle.transform.position = transform.position;
			
			// Grow AOE radius.
            m_AOERadius = Mathf.Lerp(m_AOERadius, m_AOEMax, m_AOETimer / m_AOEGrowTime);

            // AOE collider radius grows in conjuction to the lerp.
            m_AOEParticleCollider.radius = m_AOERadius;
            Debug.Log("Collider radius: " + m_AOEParticleCollider.radius + "Float radius: " + m_AOERadius);

            // Temporary way of doing the particles.
            aoe.radius = m_AOEParticleCollider.radius;
            m_skillActive = true;
        }

        // Checks if ability has been used.
        if (m_isActive == false && coolDown <= 0)
        {
            return;
        }
    }

    /// <summary>
    /// checks the distance from both characters to thea and if they are within 
    /// range when the player releases the trigger, it will heal them according 
    /// to how long the player charges the heal.
    /// </summary>
    public void GiveHealth()
    {
        // Calculates the magnitude.
        float sqrDistanceNash = (m_nashorn.transform.position - this.transform.position).sqrMagnitude;
        // Calculates the magnitude.
        float sqrDistanceKen = (m_kenron.transform.position - this.transform.position).sqrMagnitude;

        // Checks if Nashorn is in correct distance of the AOE to heal.
        if (sqrDistanceNash <= m_AOERadius * m_AOERadius && GameManager.Instance.Nashorn.m_controllerOn)
        {
            m_nashorn.SetHealth(m_nashorn.m_currentHealth + m_AOETimer * m_GOPEffect);
  
            if (m_nashorn != null)
            {
                m_temp = Instantiate(m_waterPrefab, m_nashorn.transform.position + Vector3.down * (m_nashorn.transform.localScale.y / 2), Quaternion.Euler(90, 0, 0), m_nashorn.transform);
            }
            Destroy(m_temp, 2f);
        }

        // Checks if Kenron is in correct distance of the AOE to heal.
        if (sqrDistanceKen <= m_AOERadius * m_AOERadius && GameManager.Instance.Kenron.m_controllerOn)
        {
            m_kenron.SetHealth(m_kenron.m_currentHealth +  m_AOETimer * m_GOPEffect);
            if (m_kenron != null)
            {
                m_temp = Instantiate(m_waterPrefab, m_kenron.transform.position + Vector3.down * (m_kenron.transform.localScale.y / 2), Quaternion.Euler(90, 0, 0), m_kenron.transform);
            }
            Destroy(m_temp, 2f);
        }

        // Heal Thea.
        SetHealth(m_currentHealth + m_AOETimer * m_GOPEffect);
        

        if (m_skillActive = true & m_playerSkills.Find(skill => skill.name == "Serenade Of Water"))
        {
            m_kenron.SetHealth(m_kenron.m_currentHealth * 1.5f);
            m_nashorn.SetHealth(m_nashorn.m_currentHealth * 1.5f);
            SetHealth(m_currentHealth * 1.5f);
            m_skillActive = false;
        }
    }

    /// <summary>
    /// Resets Theas skill and changes the radius of the AOE to min.
    /// </summary>
    public void ResetGiftOfPoseidon()
    {
        m_AOETimer = 0f;
        m_isActive = false;
        m_AOERadius = m_AOEMin;
        m_AOEParticleCollider.enabled = false;
        m_playerController.m_controllerOn = true;
        aoe.radius = 1;

		// turn AOE effect off
		m_AOEParticle.gameObject.SetActive(false);
	}

    private IEnumerator HealthOverTime() {
        m_isHealthRegen = true;
        while (m_currentHealth < m_maxHealth) {
            Regenerate();
            yield return new WaitForSeconds(regenEachFrame);
        }
        m_isHealthRegen = false;
    }

    public void Regenerate() {
        m_currentHealth += healthRegenerated;
    }
}
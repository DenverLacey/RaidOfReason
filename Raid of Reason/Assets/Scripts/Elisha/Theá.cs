using System.Collections;
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

    private Kenron m_kenron;
    private Nashorn m_nashorn;
    private BaseCharacter m_playerController;
    private Vector3 m_hitLocation;
    private LayerMask m_layerMask;
    private GameObject m_temp;
    private float m_shotCounter;
    private float m_counter;
    private bool m_isActive;
    private bool m_skillActive = false;
    private float m_AOERadius;
    private float m_AOETimer;
    private float m_particleRadius;

    ParticleSystem.ShapeModule aoe;

    public bool nashornBuffGiven = false;

    /// <summary>
    /// Gets called before Start.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        m_isActive = false;
        m_nashorn = FindObjectOfType<Nashorn>();
		m_kenron = FindObjectOfType<Kenron>();
        m_playerController = GetComponent<BaseCharacter>();
        m_AOETimer = 0f;
        m_AOEParticle = GetComponentInChildren<ParticleSystem>();
        // Sets AOE particle transform to spawn on Thea.
        m_AOEParticleCollider.transform.position = this.gameObject.transform.position;
        m_AOEParticle.transform.position = this.gameObject.transform.position;
        m_AOEParticleCollider.enabled = false;
        aoe = m_AOEParticle.shape;
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
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Theas projectile instantiation and damage when pressing the trigger button.
    /// </summary>
    public void Projectile()
    {
        m_counter += Time.deltaTime;

        // If the player presses the right trigger button.
        if (XCI.GetAxis(XboxAxis.RightTrigger, this.m_controller) > 0.1)
        {
            // Start the shot counter.
            m_shotCounter += Time.deltaTime;

            if (m_counter > m_projectileDelay)
            {
                // Instantiate projectile object.
                GameObject temp = Instantiate(m_projectile, transform.position + transform.forward * 2, transform.rotation);
                // Set projectile damage and move projectile.
				temp.GetComponent<ProjectileMove>().SetDamage(m_damage);
                // Reset counter.
                m_counter = 0f;
            }
        }
        // If player releases the right trigger button.
        else if (XCI.GetAxis(XboxAxis.RightTrigger, this.m_controller) < 0.1)
        {
            // Reset the counter.
            m_shotCounter = 0f;
        }
    }

    void SkillChecker() {
        if (m_skillActive == true && m_playerSkills.Find(skill => skill.name == "Settling Tide")) {
            m_skillActive = false;
            m_skillManager.m_Skills[2].m_coolDown = m_skillManager.m_Skills[2].m_coolDown / 2;
        }
        if (m_playerSkills.Find(skill => skill.name == "Hydro Pressure"))
        {
            float healthcomparison = m_kenron.m_currentHealth + m_nashorn.m_currentHealth;

            if (healthcomparison <= 150) {
                m_projectileDelay = 0.7f;
                m_damage = 13.0f;
            }
            if (healthcomparison <= 130) {
                m_projectileDelay = 0.5f;
                m_damage = 18.0f;
            }
            if (healthcomparison <= 60) {
                m_projectileDelay = 0.3f;
                m_damage = 24.0f;
            }
            if (healthcomparison <= 25) {
                m_projectileDelay = 0.1f;
                m_damage = 35.0f;
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
        m_skillActive = true;

        // Checks if player can use the ability.
        if(m_isActive == true && coolDown == 20)
        {
            // Enables collider for the ability.
            m_AOEParticleCollider.enabled = true;
            
            // Start AOE timer.
            m_AOETimer += Time.deltaTime;
            // Disable player movement and rotation.
            m_playerController.m_controllerOn = false;
            // Grow AOE radius.
            m_AOERadius = Mathf.Lerp(m_AOERadius, m_AOEMax, m_AOETimer / m_AOEGrowTime);

            // AOE collider radius grows in conjuction to the lerp.
            m_AOEParticleCollider.radius = m_AOERadius;
            Debug.Log("Collider radius: " + m_AOEParticleCollider.radius + "Float radius: " + m_AOERadius);

            // Temporary way of doing the particles.
            aoe.radius = m_AOEParticleCollider.radius;
        }

        // Checks if ability has been used.
        if(m_isActive == false && coolDown <= 0)
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
        if (sqrDistanceNash <= m_AOERadius * m_AOERadius)
        {
            m_nashorn.SetHealth(m_nashorn.m_currentHealth + m_AOETimer * m_GOPEffect);
  
            if (m_nashorn != null)
            {
                m_temp = Instantiate(m_waterPrefab, m_nashorn.transform.position + Vector3.down * (m_nashorn.transform.localScale.y / 2), Quaternion.Euler(90, 0, 0), m_nashorn.transform);
            }
            Destroy(m_temp, 2f);
        }

        // Checks if Kenron is in correct distance of the AOE to heal.
        if (sqrDistanceKen <= m_AOERadius * m_AOERadius)
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
    }
}
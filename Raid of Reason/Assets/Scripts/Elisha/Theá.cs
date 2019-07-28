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
    private GameObject m_projectile;
    [SerializeField]
    private Kenron m_kenron;
    [SerializeField]
    private Nashorn m_nashorn;
    [SerializeField]
    private SkillManager m_skillManager;
    [SerializeField]
    private float m_projectileDelay;
    [SerializeField]
    private float m_AOETimer;
    [SerializeField]
    private float m_AOEMax;
    [SerializeField]
    private float m_AOEGrowTime;
    [SerializeField]
    private float m_AOEMin;
    [SerializeField]
    private float m_GOPEffect;

    private BaseCharacter m_playerController;
    private Vector3 m_hitLocation;
    private LayerMask m_layerMask;
    private GameObject m_temp;
    private float m_shotCounter;
    private float m_counter;
    private bool m_isActive;
    private bool m_skillActive = false;
    private float m_AOERadius;

    public SphereCollider m_AOEParticle;
    public GameObject m_AOEParticleObject;

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
        // Sets AOE particle transform to spawn on Thea.
        m_AOEParticle.transform.position = this.gameObject.transform.position;
        m_AOEParticleObject.transform.position = this.gameObject.transform.position;
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
        if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.Third) > 0.1)
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
        else if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.Third) < 0.1)
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
            // Disable player movement and rotation.
            m_playerController.m_controllerOn = false;
            // Start AOE timer.
            m_AOETimer += Time.deltaTime;
            // Grow AOE radius.
            m_AOERadius = Mathf.Lerp(m_AOERadius, m_AOEMax, m_AOETimer / m_AOEGrowTime);
            // AOE visual particle radius grows in conjuction to the lerp.
            m_AOEParticle.radius = m_AOERadius;
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
        float sqrDistance = (m_kenron.transform.position - m_nashorn.transform.position).sqrMagnitude;

        // Checks if players are in correct distance of the AOE to heal.
        if(sqrDistance <= m_AOERadius * m_AOERadius)
        {
            // Heal all players based on how long the charge up was.
            m_kenron.SetHealth(m_kenron.m_currentHealth + m_AOETimer * m_GOPEffect);
            m_nashorn.SetHealth(m_nashorn.m_currentHealth + m_AOETimer * m_GOPEffect);
            SetHealth(m_currentHealth + m_AOETimer * m_GOPEffect);

            // Instantiate water particle on all players positions.
            m_temp = Instantiate(m_waterPrefab, transform.position + Vector3.down * (transform.localScale.y / 2), Quaternion.Euler(90, 0, 0));
            if (m_kenron != null)
            {
                m_temp = Instantiate(m_waterPrefab, m_kenron.transform.position + Vector3.down * (m_kenron.transform.localScale.y / 2), Quaternion.Euler(90, 0, 0), m_kenron.transform);
            }
            if (m_nashorn != null)
            {
                m_temp = Instantiate(m_waterPrefab, m_nashorn.transform.position + Vector3.down * (m_nashorn.transform.localScale.y / 2), Quaternion.Euler(90, 0, 0), m_nashorn.transform);
            }
            // Destroy particle effect.
            Destroy(m_temp);
        }
    }

    /// <summary>
    /// Resets Theas skill and changes the radius of the AOE to min.
    /// </summary>
    public void ResetGiftOfPoseidon()
    {
        m_isActive = false;
        m_AOERadius = m_AOEMin;
        // Enables player controls.
        m_playerController.m_controllerOn = true;
    }
}
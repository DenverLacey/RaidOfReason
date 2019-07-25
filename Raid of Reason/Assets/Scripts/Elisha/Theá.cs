//*
// Summary: Thea's main class, which handles her basic attacks, abilities and her ult which heals all players in the game to max health
// Author: Elisha Anagnostakis
// Date: 14/05/19 
//*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Theá : BaseCharacter
{
    [SerializeField] private GameObject waterPrefab;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Kenron m_Kenron;
    [SerializeField] private Nashorn m_Nashorn;
    [SerializeField] private float delay;
    [SerializeField] private SkillManager manager;
    [SerializeField] private float m_aoeTimer;
    [SerializeField] private float m_aoeMax;
    [SerializeField] private float m_aoeGrowTime;
    [SerializeField] private float m_aoeMin;
    [SerializeField] private float m_GOPEffect;

    private BaseCharacter m_playerController;
    private Vector3 hitLocation;
    private LayerMask layerMask;
    private GameObject m_temp;
    private float shotCounter;
    private float counter;
    private bool isActive;
    private bool skillActive = false;
    private float m_aoeRadius;

    protected override void Awake()
    {
        base.Awake();
        isActive = false;
        m_Nashorn = FindObjectOfType<Nashorn>();
		m_Kenron = FindObjectOfType<Kenron>();
        m_playerController = GetComponent<BaseCharacter>();
        m_aoeTimer = 0f;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
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
    /// - Theas projectile instantiation and damage when pressing the trigger button.
    /// </summary>
    public void Projectile()
    {
        counter += Time.deltaTime;

        if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.Third) > 0.1)
        {
            shotCounter += Time.deltaTime;

            if (counter > delay)
            {
                GameObject temp = Instantiate(projectile, transform.position + transform.forward * 2, transform.rotation);
				temp.GetComponent<ProjectileMove>().SetDamage(m_damage);
                counter = 0f;
            }
        }
        else if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.Third) < 0.1)
        {
            shotCounter = 0f;
        }
    }

    void SkillChecker() {
        if (skillActive == true && playerSkills.Find(skill => skill.name == "Settling Tide")) {
            skillActive = false;
            manager.m_Skills[2].m_coolDown = manager.m_Skills[2].m_coolDown / 2;
        }
        if (playerSkills.Find(skill => skill.name == "Hydro Pressure"))
        {
            float healthcomparison = m_Kenron.m_currentHealth + m_Nashorn.m_currentHealth;

            if (healthcomparison <= 150) {
                delay = 0.7f;
                m_damage = 13.0f;
            }
            if (healthcomparison <= 130) {
                delay = 0.5f;
                m_damage = 18.0f;
            }
            if (healthcomparison <= 60) {
                delay = 0.3f;
                m_damage = 24.0f;
            }
            if (healthcomparison <= 25) {
                delay = 0.1f;
                m_damage = 35.0f;
            }
        }
    }

    /// <summary>
    /// - Thea has the ability to do a charge up time attack with scaling AOE and heal.
    /// - The longer you hold the charge up the bigger the AOE is for players to be healed.
    /// </summary>
    public void GiftOfPoseidon()
    {
        isActive = true;
        skillActive = true;
        int m_coolDown = 20;

        if(isActive == true && m_coolDown == 20)
        {
            m_playerController.controllerOn = false;
            m_aoeTimer += Time.deltaTime;
            m_aoeRadius = Mathf.Lerp(m_aoeRadius, m_aoeMax, m_aoeTimer / m_aoeGrowTime);

            // turn on particle effect
            //ParticleSystem.ShapeModule dome;
            //dome.radius = m_aoeRadius;
            
        }

        if(isActive == false && m_coolDown <= 0)
        {
            return;
        }
    }

    /// <summary>
    /// - checks the distance from both characters to thea and if they are within 
    ///   range when the player releases the trigger, it will heal them according 
    ///   to how long the player charges the heal.
    /// </summary>
    public void GiveHealth()
    {
        float sqrDistance = (m_Kenron.transform.position - m_Nashorn.transform.position).sqrMagnitude;

        if(sqrDistance <= m_aoeRadius * m_aoeRadius)
        {
            m_Kenron.SetHealth(m_Kenron.m_currentHealth + m_aoeTimer * m_GOPEffect);
            m_Nashorn.SetHealth(m_Nashorn.m_currentHealth + m_aoeTimer * m_GOPEffect);
            SetHealth(m_currentHealth + m_aoeTimer * m_GOPEffect);

            m_temp = Instantiate(waterPrefab, transform.position + Vector3.down * (transform.localScale.y / 2), Quaternion.Euler(90, 0, 0));
            if (m_Kenron != null)
            {
                m_temp = Instantiate(waterPrefab, m_Kenron.transform.position + Vector3.down * (m_Kenron.transform.localScale.y / 2), Quaternion.Euler(90, 0, 0), m_Kenron.transform);
            }
            if (m_Nashorn != null)
            {
                m_temp = Instantiate(waterPrefab, m_Nashorn.transform.position + Vector3.down * (m_Nashorn.transform.localScale.y / 2), Quaternion.Euler(90, 0, 0), m_Nashorn.transform);
            }
        }
    }

    /// <summary>
    /// - Resets Theas skill and changes the radius of the AOE to min.
    /// </summary>
    public void ResetGiftOfPoseidon()
    {
        isActive = false;
        m_aoeRadius = m_aoeMin;
        m_playerController.controllerOn = true;
        // turn off particle effect
    }
}
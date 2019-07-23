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
    private Vector3 hitLocation;
    private LayerMask layerMask;
    private GameObject temp;
    private float shotCounter;
    private float counter;
    private bool isActive;
    private bool skillActive = false;

    public float m_aoeGrowth;
    public float m_minAOE;
    public float m_maxAOE;
    [SerializeField] private ParticleSystem m_aoeRadius;

    protected override void Awake()
    {
        base.Awake();
        isActive = false;
        m_Nashorn = FindObjectOfType<Nashorn>();
		m_Kenron = FindObjectOfType<Kenron>();

        m_aoeGrowth = 0f;
        m_minAOE = 2f;
        m_maxAOE = 10f;

        ParticleSystem.MainModule ps_main = m_aoeRadius.main;
        ParticleSystem.ShapeModule ps_shape = m_aoeRadius.shape;
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
            for(int i = 0; i < 20; i++)
            {
                SetHealth(m_currentHealth + 50);
				m_Kenron.SetHealth(m_Kenron.m_currentHealth + 50);
                m_Nashorn.SetHealth(m_Nashorn.m_currentHealth + 50);
				temp = Instantiate(waterPrefab, transform.position + Vector3.down * (transform.localScale.y / 2), Quaternion.Euler(90, 0, 0));
				if (m_Kenron != null)
				{
					temp = Instantiate(waterPrefab, m_Kenron.transform.position + Vector3.down * (m_Kenron.transform.localScale.y / 2), Quaternion.Euler(90, 0, 0), m_Kenron.transform);
				}
                if (m_Nashorn != null)
                {
                    temp = Instantiate(waterPrefab, m_Nashorn.transform.position + Vector3.down * (m_Nashorn.transform.localScale.y / 2), Quaternion.Euler(90, 0, 0), m_Nashorn.transform);
                }
                m_coolDown--;
            }
            isActive = false;
            Destroy(temp);
        }

        if(isActive == false && m_coolDown <= 0)
        {
            return;
        }
    }

}
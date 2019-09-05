﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

/*
  * Author: Afridi Rahim
  *
  * Summary: The Stats and Management of Nashorns Core Mechanics.
  *          Manages his abilites and his skill tree as he improves within the
  *          game
*/

public class Nashorn : BaseCharacter
{
    [Tooltip("The Collider of Nashorns Left Gauntlet")]
    public Collider LeftGauntlet;

    [Tooltip("The Collider of Nashorns Right Gauntlet")]
    public Collider RightGauntlet;

    [Tooltip("Checks if Nashorns Skill is Active")]
    public bool isTaunting;

    [SerializeField]
    [Tooltip("Size of the area of effect for taunt ability")]
    private float m_tauntRadius;
	public float TauntRadius { get => m_tauntRadius; }

    [SerializeField]
    [Tooltip("How vulnerable Nashorn is while taunting (1.0 is default)")]
    private float m_tauntVulnerability;

    [SerializeField]
    [Tooltip("Electric Effect That Appears When Nashorn Taunts")]
    private ParticleSystem m_tauntParticle;

    [SerializeField]
    [Tooltip("Shock Effect That Appears When Nashorn Taunts")]
    private ParticleSystem m_debrisParticle;

    [SerializeField]
    [Tooltip("Increased Radius from Nashorns Roaring Thunder ability")]
    private float m_radiusIncreased;

    [Tooltip("Chance of Stun dealt by Nashorns Macht Des Sturms ability")]
    public float stunChance;

    [SerializeField]
    [Tooltip("Damage done by Chain Lightning dealt by Macht Des Sturms ability")]
    private float m_lightningDamage;

    [SerializeField]
    [Tooltip("Range Chain Lightning spreads dealt by Macht Des Sturms ability")]
    private float m_lightningRadius;


    [SerializeField]
    [Tooltip("Buffer distance to avoid Nashorn getting stuck in walls")]
    private float m_lungeBufferDistance;

    [SerializeField]
    [Tooltip("The max distance Nashorn can lunge")]
    private float m_maxLungeDistance;

    [SerializeField]
    [Tooltip("How fast can Nashorn lunge when attacking?")]
    private float m_lungeSpeed;

    [SerializeField]
    [Tooltip("How much delay between consecutive lunges in seconds")]
    private float m_lungeDelay;

    private float m_lungeDelayTimer;
    // Desired position to lunge.
    private Vector3 m_lungePosition;
    // Nashorn's Collider
    private CapsuleCollider m_collider;
    // Sets the lunge 
    private bool islunging;
    // checks if trigger has been pressed
    private bool triggerIsDown;
    // Kenron Instance
    private Kenron m_Kenron;
    // Thea Instance
    private Thea m_Thea;
    // Empty Object for particle instantiating
    private GameObject particleInstantiate;
    private GameObject temp;

    // Skill is active check
    public bool isActive;
    // Container for Enemy position
    public List<Vector3> listOfPosition;
    // Chain lightning visual
    public LineRenderer lineRenderer;
    // Nearby enemies
    [SerializeField]
    public EnemyData enemies;

	private void Start()
	{
		GameManager.Instance.GiveCharacterReference(this);
        m_collider = GetComponent<CapsuleCollider>();
    }

	protected override void Awake()
    {
        // Initialisation 
        base.Awake();
        m_tauntParticle.GetComponentInChildren<ParticleSystem>();
        LeftGauntlet.enabled = false;
        RightGauntlet.enabled = false;
        isTaunting = false;
        isActive = false;
        triggerIsDown = false;
        islunging = false; 

        if (m_skillPopups.Count > 0)
        {
            foreach (Image display in m_skillPopups)
            {
                display.enabled = false;
            }
        }

        m_Thea = FindObjectOfType<Thea>();
        m_Kenron = FindObjectOfType<Kenron>();
    }

    protected override void FixedUpdate()
    {
        // Empty Check
        if (this.gameObject != null)
        {
            // Updates Player Movement
            base.FixedUpdate();

            // Checks Nashorns Skill Tree
            SkillChecker();
        }
    }

    protected override void Update()
    {
        // Allows Nashorn to perform Melee Punches 
        base.Update();
        Punch();
    }

    public void UnlockSkill()
    {
        if (m_skillPopups.Count > 1)
        {
            if (m_skillUpgrades.Find(skill => skill.Name == "Roaring Thunder"))
            {
                // Icon pops up
                m_skillPopups[1].enabled = true;
            }
            if (m_skillUpgrades.Find(skill => skill.Name == "Kinetic Discharge"))
            {
                // Icon pops up
                m_skillPopups[2].enabled = true;
            }
            if (m_skillUpgrades.Find(skill => skill.Name == "Static Sheild"))
            {
                // Icon pops up
                m_skillPopups[3].enabled = true;
            }
            if (m_skillUpgrades.Find(skill => skill.Name == "Macht Des Sturms"))
            {
                // Icon pops up
                m_skillPopups[4].enabled = true;
            }
        }
    }

    /// <summary>
    /// Checks how many skills Nashorn has obtained from his skill tree
    /// - Roaring Thunder: More Range for his Taunt and Cooldown is Halved
    /// - Shock Wave: Melee now knocksback and has a chance to stun (Passive)
    /// - Kinetic Discharge: Enemies now take damage when attacking Taunted Nashorn
    /// - Macht Des Sturms: Chain Lightning attack that damages and stuns + team mates are granted electric damage (Stun Buff)
    /// </summary>
    public void SkillChecker()
    {
        // Empty Check
        if (this.gameObject != null)
        {
            // Sets the image to true if the skills are found
            UnlockSkill();
            // If the skill is active and the player has the named skill
            if (isTaunting == true && m_skillUpgrades.Find(skill => skill.Name == "Roaring Thunder"))
            {

                // Returns increased Radius
                this.m_tauntRadius = m_tauntRadius + m_radiusIncreased;

                // Cooldown is halved
                skillManager.m_mainSkills[1].m_coolDown = skillManager.m_mainSkills[1].m_coolDown / 2;
            }

            // If the skill is active and the player has the named skill
            if (isTaunting == true && m_skillUpgrades.Find(skill => skill.Name == "Kinetic Discharge"))
            {
                if (enemies.isAttackingNashorn == true) {
                    enemies.TakeDamage(enemies.AttackDamage);
                }
            } 

            // If the skill is active and the player has the named skill
            if (isTaunting == true && m_skillUpgrades.Find(skill => skill.Name == "Macht Des Sturms"))
            {
                // Gives his allies his stun buff
                m_Kenron.nashornBuffGiven = true;
                m_Thea.nashornBuffGiven = true;

                if (isActive == true)
                {
                    Ray ray = new Ray(transform.position, transform.forward);
                    RaycastHit hit;

                    lineRenderer.SetPosition(0, ray.origin);
                    lineRenderer.SetPosition(1, ray.GetPoint(100));

                    if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
                    {
                        lineRenderer.SetPosition(1, hit.point);
                        ChainLightning();
                    }
                    isActive = false;
                }
            }
        }
    }

    /// <summary>
    /// Chain Lightning Attack
    /// </summary>
    public void ChainLightning()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_lightningRadius);

        listOfPosition[1] = hitColliders[1].transform.position;
        listOfPosition[2] = hitColliders[2].transform.position;
        listOfPosition[3] = hitColliders[3].transform.position;
        listOfPosition[4] = hitColliders[4].transform.position;

        lineRenderer.SetPositions(listOfPosition.ToArray());

        foreach (Collider hitCol in hitColliders)
        {
            Debug.Log(hitCol.gameObject.name);
            EnemyData enemy = hitCol.transform.GetComponent<EnemyData>();
            if (enemy != null)
            {
                enemy.TakeDamage(0);
            }
        }
    }

    /// <summary>
    /// Nashorns Attack. By Pressing the Right Trigger, Nashorn Becomes Stationery and Punches his foes switching between fists
    /// </summary>
    public void Punch()
    {
        // if right trigger down and attack animation is not playing
        if (XCI.GetAxis(XboxAxis.RightTrigger, controller) > 0.1f && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack Left") && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack Right") && !islunging)
        {
            islunging = true;
            m_animator.SetBool("LeftGauntlet", !m_animator.GetBool("LeftGauntlet"));
            m_animator.SetBool("Attack", true);

            RightGauntlet.enabled = true;
            LeftGauntlet.enabled = true;
            m_controllerOn = false;

            m_lungeDelayTimer = m_lungeDelay;

            // calculate desired dash position
            int layerMask = Utility.GetIgnoreMask("Enemy", "Player");
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(transform.position, transform.forward, out hit, m_maxLungeDistance + m_lungeBufferDistance, layerMask))
            {
                m_lungePosition = hit.point;
                m_lungePosition -= transform.forward * (m_collider.radius * transform.lossyScale.x + m_lungeBufferDistance);
            }
            else
            {
                m_lungePosition = transform.position + transform.forward * m_maxLungeDistance;
            }
        }
        else
        {
            m_animator.SetBool("Attack", false);
        }

        if (islunging)
        {
            if (!m_controllerOn)
            {
                Vector3 lerpPosition = Vector3.Lerp(transform.position, m_lungePosition, m_lungeSpeed * Time.deltaTime);
                m_rigidbody.MovePosition(lerpPosition);
            }

            // if completed lunge
            if ((m_lungePosition - transform.position).sqrMagnitude <= 0.1f || m_controllerOn)
            {
                // run delay timer
                m_lungeDelayTimer -= Time.deltaTime;
            }

            // if ready to lunge again 
            if (m_lungeDelayTimer <= 0.0f)
            {
                m_controllerOn = true;
                islunging = false;
            }
        }
        else
        {
            // Disable colliders and reset speed
            RightGauntlet.enabled = false;
            LeftGauntlet.enabled = false;
        }
    }

    IEnumerator MachinasDareVisual()
    {
        yield return new WaitForSeconds(0.5f);
        m_debrisParticle.Play();

    }

    /// <summary>
	/// Nashorn's Ability. Boosting His Health up and reducing incoming damage he taunts all enemies to himself
	/// </summary>
    public void Spott(float skillDuration)
    {
        if (skillDuration >= skillManager.m_mainSkills[1].m_duration)
        {
            // Ability is active
            isTaunting = true;

			// taunt enemies
			foreach (EnemyData enemy in GameObject.FindObjectsOfType<EnemyData>())
			{
				float sqrDist = (enemy.transform.position - transform.position).sqrMagnitude;
				if (sqrDist <= m_tauntRadius * m_tauntRadius)
				{
					enemy.Taunted = true;
				}
			}

            // Set active
            isActive = true;

            m_tauntParticle.Play();
            StartCoroutine(MachinasDareVisual());

            // set vulnerability
            m_vulnerability = m_tauntVulnerability;
        }
    }

    /// <summary>
    /// Resets Nashorns Stats back to his base after Spott is Used
    /// </summary>
    public void ResetSkill()
    {
        if (skillManager.m_mainSkills[1].m_currentDuration >= skillManager.m_mainSkills[1].m_duration)
        {
            // Vulnerable once more
            ResetVulernability();

            // Skill no longer active
            isTaunting = false;
        }
    }
}

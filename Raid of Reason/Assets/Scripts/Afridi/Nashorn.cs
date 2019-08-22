using System.Collections;
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
    [Tooltip("The Two Gauntlets That Nashorn Attacks With")]
    public List<GameObject> Gauntlets;

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
    [Tooltip("Particle Effect That Appears When Nashorn Taunts")]
    private GameObject m_tauntParticle;

    [SerializeField]
    [Tooltip("Particle Effect That Appears When Nashorn Taunts")]
    private GameObject m_gauntletParticle;

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

    // Nashorns Rigidbody
    private Rigidbody m_nashornRigidBody;

    // Kenron Instance
    private Kenron m_Kenron;

    // Thea Instance
    private Thea m_Thea;

    // Skill is active check
    public bool isActive;

    // Container for Enemy position
    public List<Vector3> listOfPosition;

    // Chain lightning visual
    public LineRenderer lineRenderer;

    // Empty Object for particle instantiating
    private GameObject particleInstantiate;

    // Nearby enemies
    [SerializeField]
    public EnemyData enemies;

	private void Start()
	{
		GameManager.Instance.GiveCharacterReference(this);
	}

	protected override void Awake()
    {
        // Initialisation 
        base.Awake();
        m_nashornRigidBody = GetComponent<Rigidbody>();
        LeftGauntlet.enabled = false;
        RightGauntlet.enabled = false;
        isTaunting = false;
        isActive = false;
        foreach (Image display in m_skillPopups)
        {
            display.enabled = false;
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
        // Empty Check
        if (this.gameObject != null)
        {
            // Allows Nashorn to perform Melee Punches 
            Punch();
            base.Update();
        }
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
                    //Ray ray = new Ray(transform.position, transform.forward);
                    //RaycastHit hit;

                    //lineRenderer.SetPosition(0, ray.origin);
                    //lineRenderer.SetPosition(1, ray.GetPoint(100));

                    //if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
                    //{
                    //    lineRenderer.SetPosition(1, hit.point);
                    //    ChainLightning();
                    //}
                    //isActive = false;
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
        // Empty Check
        if (Gauntlets.Count == 2 && m_controllerOn)
        {
            // If the Triggers has been pressed
            if (XCI.GetAxis(XboxAxis.RightTrigger, controller) > 0.1)
            {
                // Gauntlet Colliders are Enabled and Nashorn Becomes Stationary 
                LeftGauntlet.enabled = true;
                RightGauntlet.enabled = true;
                SetSpeed(0.0f);

                GameObject LFist = Instantiate(m_gauntletParticle, Gauntlets[0].transform.position + Vector3.up, Quaternion.Euler(19, 0, 0), Gauntlets[0].transform);
                GameObject RFist = Instantiate(m_gauntletParticle, Gauntlets[1].transform.position + Vector3.down, Gauntlets[1].transform.rotation, Gauntlets[1].transform);

				// alternate arm
				m_animator.SetBool("Attack", true);
                m_skillPopups[0].enabled = true;

                Destroy(LFist, skillManager.m_mainSkills[1].m_currentDuration);
                Destroy(RFist, skillManager.m_mainSkills[1].m_currentDuration);
            }
            // or if the trigger isnt pressed
            else if (XCI.GetAxis(XboxAxis.RightTrigger, controller) < 0.1)
            {
                // Disable colliders and reset speed
                LeftGauntlet.enabled = false;
                RightGauntlet.enabled = false;
                SetSpeed(10.0f);

				m_animator.SetBool("Attack", false);
                m_skillPopups[0].enabled = false;
            }
        }
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

            // Set active
            isActive = true;

            //Instantiate Taunt
            GameObject temp = Instantiate(m_tauntParticle, transform.position, transform.rotation, transform);

            // set vulnerability
            m_vulnerability = m_tauntVulnerability;

            // Destroy after time has passed
            Destroy(temp, skillManager.m_mainSkills[1].m_currentDuration);
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

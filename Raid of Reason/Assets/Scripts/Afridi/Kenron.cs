using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

/*
  * Author: Afridi Rahim
  *
  * Summary: The Stats and Management of Kenrons Core Mechanics.
  *          Manages his abilites and his skill tree as he improves within the
  *          game
*/

public class Kenron : BaseCharacter {

    [Tooltip("The Sword Kenron Is Using")]
    public GameObject Amaterasu;

    [Tooltip("Checks if Kenrons Skill is Active")]
    public bool isActive = false;

	[SerializeField]
	[Tooltip("Distance of Dash Attack")]
	private float m_maxDashDistance;

	[SerializeField]
	[Tooltip("How quickly Kenron dashes")]
	private float m_dashSpeed;

	[SerializeField]
	[Tooltip("How much delay between consecutive dashes in seconds")]
	private float m_dashDelay;

	[SerializeField]
	[Tooltip("Buffer distance to avoid Kenron getting stuck in walls")]
	private float m_dashBufferDistance = 3f;

	private Vector3 m_dashVelocity;

    [SerializeField]
    [Tooltip("Hit box for dash attack")]
    private BoxCollider m_dashCollider;

    [SerializeField]
    [Tooltip("Kenrons Damage Boost whilst in Chaos Flame")]
    private float m_chaosFlameDamage;

    [SerializeField]
    [Tooltip("Kenrons Speed Boost whilst in Chaos Flame")]
    private float m_chaosFlameSpeed;

    [SerializeField]
    [Tooltip("Health Gained Back from Kenrons Life Steal Skill")]
    private float m_healthGained;

    [SerializeField]
    [Tooltip("Ability Duration Increase from Kenrons Blood Lust Skill")]
    private float m_durationIncreased;

    [SerializeField]
    [Tooltip("The Radius that effects the enemies with Kenrons Shuras Awakening Skill")]
    private float m_damageRadius;

    [SerializeField]
    [Tooltip("The Damage dealth within the radius hit by Kenrons Shuras Awakening Skill")]
    private float m_damageWithin;

    [SerializeField]
    [Tooltip("Particle Effect That Appears on the Kenrons Body When Chaos Flame is Active")]
    private ParticleSystem m_kenronParticle;

    [SerializeField]
    [Tooltip("Particle Effect That Indicates Chaos Flame is Active")]
    private ParticleSystem m_startParticle;

    [SerializeField]
    [Tooltip("Effect when Cursed Kenron Spawns")]
    private GameObject m_CurseEffect;

    // Enemy reference used for skill checking
    private EnemyData m_Enemy;

    // Desired position to dash
    private Vector3 m_dashPosition;

    // A bool that checks if nashorn has give kenron his buff
    public bool nashornBuffGiven = false;

    // Checks if Kenron is Dashing or Not
    private bool isDashing;

    // Checks if a specific trigger on the controller is pressed
    private bool m_triggerDown;

    // Delay until next dash
    private float m_dashDelayTimer;

	private float m_estimatedDashTime;

    // Sets the burn 
    public bool isBurning;

    // Kenrons Collider
	private CapsuleCollider m_collider;

    // Stat Tracker
    [HideInInspector]
    public StatTrackingManager m_statManager;

	private void Start()
	{
		GameManager.Instance.GiveCharacterReference(this);
        m_collider = GetComponent<CapsuleCollider>();
	}

	protected override void Awake () {
        // Initalisation
        base.Awake();
        CharacterType = Character.KENRON;
        m_Enemy = FindObjectOfType<EnemyData>();
        m_statManager = FindObjectOfType<StatTrackingManager>();

        isDashing = false;
        isBurning = false;

        if (m_skillPopups.Count > 0)
        {
            foreach (Image display in m_skillPopups)
            {
                display.enabled = false;
            }
        }

		// set size of dash hit box
		Vector3 hitBoxSize = new Vector3(m_dashCollider.size.x, m_dashCollider.size.y, m_maxDashDistance);
		m_dashCollider.size = hitBoxSize;
		m_dashCollider.enabled = false;
	}

    protected override void FixedUpdate()
    {
        // Empty Check
        if (this.gameObject != null)
        {
            // Updates Player Movement
			base.FixedUpdate();
        }
	}

    protected override void Update()
    {
        // Updates Player Movement
        base.Update();

        // Checks Kenrons Skill Tree
        SkillChecker();
    
        // Uses his Dash
        DashAttack();
    }

    /// <summary>
    /// Kenrons Skill. By Halving his health, he gains a boost of Speed and Damage 
    /// </summary>
    public void ChaosFlame(float skillDuration)
    {
        // Empty Check
        if (this.gameObject != null && Amaterasu != null)
        {
            if (skillDuration >= skillManager.m_mainSkills[0].m_duration)
            {
                // Sets the Skill to be Active
                isActive = true;
                isBurning = true;
                StartCoroutine(ChaosFlameVisual());
                m_kenronParticle.Play();
                // Halves his Health and sets a higher Damage/Speed
                SetHealth(m_currentHealth / 2);
                SetDamage(m_chaosFlameDamage);
                SetSpeed(m_chaosFlameSpeed);
                m_statManager.chaosFlameUsed++;
            }
        }
    }

    /// <summary>
    /// Kenrons Attack. By Pressing the Right Trigger, Kenron Dashes Forward Dealing Damage to Any Enemies in his direction
    /// </summary>
    public void DashAttack()
    {
        // dash attack
        if (XCI.GetAxis(XboxAxis.RightTrigger, controller) > 0.1f && m_controllerOn && !m_triggerDown)
        {
            // set boolean flags
            m_triggerDown = true;
            m_controllerOn = false;
            isDashing = true;
            m_dashCollider.enabled = true;
			m_dashDelayTimer = m_dashDelay;

			// set animator's trigger
			m_animator.SetBool("Attack", true);

            // Icon pops up
            m_skillPopups[0].enabled = true;

			// calculate desired dash position
			int layerMask = Utility.GetIgnoreMask("Enemy", "Player", "Ignore Raycast");
			RaycastHit hit = new RaycastHit();

			float radius = (m_collider.radius * transform.localScale).magnitude;
			bool collisionAtDashPos = false;
			Vector3 hitBoxEndPos = m_dashPosition;

			if (Physics.SphereCast(transform.position + transform.forward * radius / 2f, radius, transform.forward, out hit, m_maxDashDistance + 0.1f, layerMask) ||
				Physics.Raycast(transform.position + transform.forward * 0.5f, transform.forward, out hit, m_maxDashDistance + 0.1f, layerMask))
			{
				m_dashPosition = hit.point;
				m_dashPosition -= transform.forward * (m_collider.radius * transform.lossyScale.x + m_dashBufferDistance);
				//m_statManager.dashesUsed++;
			}
			else
			{
				m_dashPosition = transform.position + transform.forward * m_maxDashDistance;
			}

			// check for potential collisions at dash position

			var colliders = Physics.OverlapSphere(m_dashPosition, radius);
			Vector3 nearest = new Vector3();
			float nearestSqrDist = Mathf.Infinity;
			foreach (var collider in colliders)
			{
				if (collider.tag != "Enemy" && collider.tag != "Player")
				{
					continue;
				}

				collisionAtDashPos = true;

				float sqrDist = (collider.transform.position - transform.position).sqrMagnitude;
				if (sqrDist < nearestSqrDist)
				{
					nearestSqrDist = sqrDist;
					nearest = collider.transform.position;
				}
			}

			float dashDistance;
			if (collisionAtDashPos)
			{
				hitBoxEndPos = m_dashPosition;
				Vector3 direction = transform.position - nearest;
				m_dashPosition = nearest + direction * (radius / 2f);
				dashDistance = Mathf.Sqrt(nearestSqrDist);
			}
			else
			{
				dashDistance = (m_dashPosition - transform.position).magnitude;
			}

			// calculate estimated time of dash
			m_estimatedDashTime = dashDistance / m_dashSpeed;

			// size hit box
			Vector3 hitBoxSize = new Vector3(m_dashCollider.size.x, m_dashCollider.size.y, dashDistance);
			m_dashCollider.size = hitBoxSize;

			// rotate hit box
			m_dashCollider.transform.rotation = transform.rotation;

			// position hit box
			m_dashCollider.transform.position = transform.position + transform.forward * (dashDistance / 2f);
		}
        else if (XCI.GetAxis(XboxAxis.RightTrigger, controller) < 0.1f && !isDashing)
        {
            m_triggerDown = false;
        }

		if (isDashing)
		{
			m_estimatedDashTime -= Time.deltaTime;

			// if completed dash
			if ((m_dashPosition - transform.position).sqrMagnitude <= 0.1f ||
				m_controllerOn || 
				m_estimatedDashTime <= 0.0f)
            {
				// reset boolean flags
				m_dashCollider.enabled = false;
				m_animator.SetBool("Attack", false);               

                // Icon pops up
                m_skillPopups[0].enabled = false;

                // run delay timer
                m_dashDelayTimer -= Time.deltaTime;

                m_dashCollider.GetComponent<SwordDamage>().CalculateNewMostDamageDealt();
            }
			else
			{
				Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, m_dashPosition, ref m_dashVelocity, 1f / m_dashSpeed);
				m_rigidbody.MovePosition(smoothPosition);
			}

			// if ready to dash again 
			if (m_dashDelayTimer <= 0.0f)
			{
				m_controllerOn = true;
				isDashing = false;

			}
        }
    }

    /// <summary>
    /// The Effects that are given to the enemies/players when Kenrons third skill is active
    /// </summary>
    IEnumerator ShurasReckoningEffect() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, m_damageRadius, LayerMask.GetMask("Enemy"));
        foreach (Collider hit in hitColliders)
        {
            hit.GetComponent<StatusEffectManager>().ApplyBurn(3);
        }
        yield return new WaitForSeconds(3);
    }

    /// <summary>
    /// Checks how many skills Kenron has obtained from his skill tree
    /// - Vile Infusion: Kenron Gains Health Back for each kill (Passive)
    /// - Blood Lust: Kenrons Cooldown is reduced
    /// - Shuras Reckoning: AOE Cleave with a boost in speed
    /// - Curse of Amaterasu: Spawns A Demon Kenron after Main Kenron Dies (Passive)
    /// </summary>
    public void SkillChecker() 
    {
        // Empty Check
        if (this.gameObject != null && m_Enemy != null)
        {
            // Sets the image to true if the skills are found
            UnlockSkill();
            // If Kenron has the skill specified and is killed by Kenron
            if (m_skillUpgrades.Find(skill => skill.Name == "Vile Infusion") && m_Enemy.isDeadbByKenron)
            {
                // Icon pops up
                m_skillPopups[1].enabled = true;
                // Return a bit of health to Kenrons current health
                this.m_currentHealth = m_currentHealth + m_healthGained;
            }
            // If Kenron has the skill specified and is killed by Kenron
            if (isActive == true && m_skillUpgrades.Find(skill => skill.Name == "Bloodlust") && m_Enemy.isDeadbByKenron)
            {
                // Icon pops up
                m_skillPopups[2].enabled = true;
                // Reduces the cooldown from Kenrons Cooldown 
                skillManager.m_mainSkills[0].m_currentDuration = skillManager.m_mainSkills[0].m_currentDuration - m_durationIncreased;
            }
            // if the player does have shuras upgrade applied
            if (m_skillUpgrades.Find(skill => skill.Name == "Shuras Reckoning") && isActive == true)
            {
                if (isBurning)
                {
                    // Icon pops up
                    m_skillPopups[3].enabled = true;
                    StartCoroutine(ShurasReckoningEffect());
                    isBurning = false;
                }
            }
            // If Kenron has the skill specificed and his health is less than or equal to 0
            if (m_skillUpgrades.Find(skill => skill.Name == "Curse of Amaterasu") && m_currentHealth <= 0.0f)
            {
               
            }
        }
    }

    public void UnlockSkill()
    {
        if (m_skillPopups.Count > 1)
        {
            if (m_skillUpgrades.Find(skill => skill.Name == "Vile Infusion"))
            {
                // Icon pops up
                m_skillPopups[1].enabled = true;
            }
            if (m_skillUpgrades.Find(skill => skill.Name == "Bloodlust"))
            {
                // Icon pops up
                m_skillPopups[2].enabled = true;
            }
            if (m_skillUpgrades.Find(skill => skill.Name == "Shuras Reckoning"))
            {
                // Icon pops up
                m_skillPopups[3].enabled = true;
            }
            if (m_skillUpgrades.Find(skill => skill.Name == "Curse of Amaterasu"))
            {
                // Icon pops up
                m_skillPopups[4].enabled = true;
            }
        }
    }

    IEnumerator ChaosFlameVisual()
    {
        m_startParticle.Play();
        yield return new WaitForSeconds(0.1f);
        m_startParticle.Stop();
    }

    /// <summary>
    /// Resets Kenrons Stats back to his base after Chaos Flame is Used
    /// </summary>
    public void ResetSkill()
    {
        // Empty Check
        if (this.gameObject != null)
        {
            if (skillManager.m_mainSkills[0].m_currentDuration >= skillManager.m_mainSkills[0].m_duration)
            {
                // Resets Stats and Skill
                SetDamage(40);
                SetSpeed(15.0f);
                isActive = false;
                isBurning = true;
                m_kenronParticle.Stop();
            }
        }
    }
}

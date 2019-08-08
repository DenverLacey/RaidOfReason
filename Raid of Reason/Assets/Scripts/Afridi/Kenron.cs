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

    [Tooltip("The Collider of Kenrons Sword")]
    public Collider swordCollider;

    [Tooltip("The Aethereal Kenron that Spawns on Death")]
    public ChildKenron childKenron;

    [Tooltip("The Skill Manager that manages the skills of the players")]
    public SkillManager skillManager;

    [Tooltip("The Sword Kenron Is Using")]
    public GameObject Amaterasu;

    [Tooltip("Checks if Kenrons Skill is Active")]
    public bool isActive = false;

    [SerializeField]
    [Tooltip("The Delay until the next Dash")]
    private float m_dashTime;

	[SerializeField]
	[Tooltip("Distance of Dash Attack")]
	private float m_maxDashDistance;

	[SerializeField]
	[Tooltip("How quickly Kenron dashes")]
	private float m_dashSpeed;

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
    [Tooltip("Kenrons Particle Effect whilst in Chaos Flame")]
    private float m_particleTime;

    [SerializeField]
    [Tooltip("Health Gained Back from Kenrons Life Steal Skill")]
    private float m_healthGained;

    [SerializeField]
    [Tooltip("Cooldown Reduction from Kenrons Blood Lust Skill")]
    private float m_cooldownReduced;

    [SerializeField]
    [Tooltip("The Radius that effects the enemies with Kenrons Shuras Awakening Skill")]
    private float m_damageRadius;

    [SerializeField]
    [Tooltip("The Damage dealth within the radius hit by Kenrons Shuras Awakening Skill")]
    private float m_damageWithin;

    [SerializeField]
    [Tooltip("Particle Effect That Appears on the Kenrons Body When Chaos Flame is Active")]
    private GameObject m_kenronParticle;

    [SerializeField]
    [Tooltip("Particle Effect That Appears on the Sword When Chaos Flame is Active")]
    private GameObject m_swordParticle;

    [SerializeField]
    [Tooltip("Effect when Cursed Kenron Spawns")]
    private GameObject m_CurseEffect;

    // Radius Checker for Shuras Recknoning
    private RaycastHit hit;

    // Kenrons Rigid Body
    private Rigidbody m_kenronRigidBody;
    
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

    // Sets the burn 
    public bool isBurning;

    protected override void Awake () {
        // Initalisation
        base.Awake();
        m_Enemy = FindObjectOfType<EnemyData>();
        Amaterasu = GameObject.FindGameObjectWithTag("Amaterasu");
        m_kenronRigidBody = GetComponent<Rigidbody>();

		childKenron = FindObjectOfType<ChildKenron>();

		if (childKenron)
		{
			childKenron.gameObject.SetActive(false);
		}

		if (swordCollider)
		{
			swordCollider.enabled = false;
		}

        isDashing = false;
        isBurning = false;

		// set size of dash hit box
		Vector3 hitBoxSize = new Vector3(m_dashCollider.size.x, m_dashCollider.size.y, m_maxDashDistance);
		m_dashCollider.size = hitBoxSize;
	}

    protected override void FixedUpdate()
    {
        // Empty Check
        if (this.gameObject != null)
        {
            // Updates Player Movement
			base.FixedUpdate();

            // Checks Kenrons Skill Tree
            SkillChecker();
        }
	}

    protected override void Update()
    {
		// dash attack
		if (XCI.GetAxis(XboxAxis.RightTrigger, m_controller) > 0.1f && m_controllerOn && !m_triggerDown)
		{
			// set boolean flags
			m_triggerDown = true;
			m_controllerOn = false;
			isDashing = true;
			m_dashCollider.enabled = true;

			// calculate desired dash position
			int layerMask = LayerMask.GetMask("Enemy", "Player");
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(transform.position, transform.forward, out hit, m_maxDashDistance, layerMask))
			{
				m_dashPosition = hit.point;
				m_dashPosition -= transform.forward * transform.lossyScale.x;
			}
			else
			{
				m_dashPosition = transform.position + transform.forward * m_maxDashDistance;
			}

			// size hit box
			float dashDistance = (m_dashPosition - transform.position).magnitude;
			Vector3 hitBoxSize = new Vector3(m_dashCollider.size.x, m_dashCollider.size.y, dashDistance);
			m_dashCollider.size = hitBoxSize;

			// rotate hit box
			m_dashCollider.transform.rotation = transform.rotation;

			// position hit box
			m_dashCollider.transform.position = transform.position + transform.forward * (dashDistance / 2f);
		}
		else if (XCI.GetAxis(XboxAxis.RightTrigger, m_controller) < 0.1f && !isDashing)
		{
			m_triggerDown = false;
		}

		if (isDashing)
		{
			transform.position = Vector3.Lerp(transform.position, m_dashPosition, m_dashSpeed * Time.deltaTime);

			// if completed dash
			if ((m_dashPosition - transform.position).sqrMagnitude <= 0.1f)
			{
				// reset boolean flags
				m_controllerOn = true;
				isDashing = false;
				m_dashCollider.enabled = false;
			}
		}
    }

    /// <summary>
    /// Kenrons Skill. By Halving his health, he gains a boost of Speed and Damage 
    /// </summary>
    public void ChaosFlame()
    {
        // Empty Check
        if (this.gameObject != null && Amaterasu != null)
        {
            // Sets the Skill to be Active
            isActive = true;
            isBurning = true;

            // Instantiates a Particle at the Sword
            GameObject temp = Instantiate(m_swordParticle, Amaterasu.transform.position + Vector3.zero * 0.5f, Quaternion.Euler(-90, 0, 0), Amaterasu.transform);
            GameObject part = Instantiate(m_kenronParticle, transform.position + Vector3.down * 0.5f, Quaternion.Euler(270, 0, 0), transform);

            // Halves his Health and sets a higher Damage/Speed
            SetHealth(m_currentHealth / 2);
            SetDamage(m_chaosFlameDamage);
            SetSpeed(m_chaosFlameSpeed);

            // Destroys the Particle after 10 Seconds
            Destroy(temp, m_particleTime);
            Destroy(part, m_particleTime);
        }
    }

    /// <summary>
    /// Kenrons Attack. By Pressing the Right Trigger, Kenron Dashes Forward Dealing Damage to Any Enemies in his direction
    /// </summary>
    public void DashAttack()
    {
        // Empty Check
        if (Amaterasu != null)
        {
            // If the trigger has been pressed and the trigger down button is false
            if (XCI.GetAxis(XboxAxis.RightTrigger, m_controller) > 0.1 && !m_triggerDown)
            {
				// Enables the Collider for Attack
				if (swordCollider)
				{
					swordCollider.enabled = true;
				}

                // Trigger is down set to true
                m_triggerDown = true;

                Amaterasu.transform.localRotation = Quaternion.Euler(0, 90, 0); // Will be Removed

                // Starts the Cool down of the Dash to prevent infinite dash
                StartCoroutine(Dash());
            }
            // or if the trigger isnt pressed
            else if (XCI.GetAxis(XboxAxis.RightTrigger, m_controller) < 0.1)
            {
				if (swordCollider)
				{
					// Disable the collider to prevent baseless attack
					swordCollider.enabled = false;
				}

                // Trigger is set to false
                m_triggerDown = false;

                Amaterasu.transform.localRotation = Quaternion.Euler(0, 0, 0); // Will be Removed

                // Sets Kenron Velocity to zero to prevent sliding 
                m_kenronRigidBody.velocity = Vector3.zero;
            }
        }
    }

    /// <summary>
    /// Dashes Kenron forward and sets a cooldown until the next dash
    /// </summary>
    /// <returns> The Wait time until the next dash </returns>
    IEnumerator Dash()
    {
        // If we arent currently dashing 
        if (!isDashing)
        {
            // Add a Sudden Force to kenron and dashes him based on direction he is facing 
            m_kenronRigidBody.AddForce(GetSpeed() * m_kenronRigidBody.transform.forward, ForceMode.Impulse);

            // Kenron Dashing is true
            isDashing = true;
        }

        // Waits until the dash cooldown has ended
        yield return new WaitForSeconds(m_dashTime);

        // Kenrons dash check is reset
        isDashing = false;
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
        if (this.gameObject != null)
        {
            // If Kenron has the skill specified and is killed by Kenron
            if (m_playerSkills.Find(skill => skill.Name == "Vile Infusion") && m_Enemy.isDeadbByKenron)
            {
                // Return a bit of health to Kenrons current health
                this.m_currentHealth = m_currentHealth + m_healthGained;
            }
            // If Kenron has the skill specified and is killed by Kenron
            if (isActive == true && m_playerSkills.Find(skill => skill.Name == "Bloodlust") && m_Enemy.isDeadbByKenron)
            {
                // Reduces the cooldown from Kenrons Cooldown 
                skillManager.m_Skills[0].m_currentCoolDown = skillManager.m_Skills[0].m_currentCoolDown - m_cooldownReduced;
            }
            // if the player does have shuras upgrade applied
            if (m_playerSkills.Find(skill => skill.Name == "Shuras Reckoning") && isActive == true)
            {
                if (isBurning)
                {
                    StartCoroutine(ShurasReckoningEffect());
                    isBurning = false;
                }
            }
            // If Kenron has the skill specificed and his health is less than or equal to 0
            if (m_playerSkills.Find(skill => skill.Name == "Curse of Amaterasu") && m_currentHealth <= 0.0f)
            {
				if (childKenron)
				{
					// Disables Main kenron, Spawns and enables Child kenron at Main Kenrons position 
					childKenron.CheckStatus();
                    childKenron.transform.position = this.gameObject.transform.position;
					childKenron.gameObject.SetActive(true);
                    GameObject part = Instantiate(m_CurseEffect, childKenron.transform.position + Vector3.down * 0.5f, Quaternion.Euler(270, 0, 0), transform);
                    Destroy(part, childKenron.timer);
				}
            }
        }
    }

    /// <summary>
    /// Resets Kenrons Stats back to his base after Chaos Flame is Used
    /// </summary>
    public void ResetSkill()
    {
        // Empty Check
        if (this.gameObject != null)
        {
            // Resets Stats and Skill
            SetDamage(40);
            SetSpeed(15.0f);
            isActive = false;
            isBurning = true;
        }
    }
}

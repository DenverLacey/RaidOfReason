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
	private float m_dashDistance;

	[SerializeField]
	[Tooltip("How quickly Kenron dashes")]
	private float m_dashSpeed;

	[SerializeField]
	[Tooltip("Hit box for dash attack")]
	private BoxCollider m_dashCollider;

	private Vector3 m_dashPosition;

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
    [Tooltip("Particle Effect That Appears on the Kenrons Body When Chaos Flame is Active")]
    private GameObject m_kenronParticle;

    [SerializeField]
    [Tooltip("Particle Effect That Appears on the Sword When Chaos Flame is Active")]
    private GameObject m_swordParticle;

    // Kenrons Rigid Body
    private Rigidbody m_kenronRigidBody;
    
    // Enemy reference used for skill checking
    private BaseEnemy m_Enemy;

    // Checks if Kenron is Dashing or Not
    private bool isDashing;

    // Checks if a specific trigger on the controller is pressed
    private bool m_triggerDown;

    protected override void Awake () {
        // Initalisation
        base.Awake();
        m_Enemy = FindObjectOfType<BaseEnemy>();
        Amaterasu = GameObject.FindGameObjectWithTag("Amaterasu");
        m_kenronRigidBody = GetComponent<Rigidbody>();
	childKenron = FindObjectOfType<ChildKenron>();
        childKenron.gameObject.SetActive(false);
        swordCollider.enabled = false;
        isDashing = false;
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
        if (XCI.GetAxis(XboxAxis.RightTrigger, m_controller) > 0.1f && !m_triggerDown)
		{
			// set boolean flags
			m_triggerDown = true;
			m_controllerOn = false;
			isDashing = true;
			m_dashCollider.enabled = true;

			// orient hit box
			Vector3 hitBoxSize = new Vector3(m_dashCollider.size.x, m_dashCollider.size.y, m_dashDistance);
			m_dashCollider.size = hitBoxSize;

			m_dashCollider.transform.rotation = transform.rotation;
			m_dashCollider.transform.position = transform.position + transform.forward * (m_dashDistance / 2f);

			// set dash position
			m_dashPosition = transform.position + transform.forward * m_dashDistance;
		}

		if (isDashing)
		{
			transform.position = Vector3.Lerp(transform.position, m_dashPosition, m_dashSpeed * Time.deltaTime);

			// if completed dash
			if ((m_dashPosition - transform.position).sqrMagnitude <= 0.1f)
			{
				// reset boolean flags
				m_triggerDown = false;
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

            // Instantiates a Particle at the Sword
            GameObject temp = Instantiate(m_swordParticle, Amaterasu.transform.position + Vector3.zero * 0.5f, Quaternion.Euler(-90, 0, 0), Amaterasu.transform);

            // Halves his Health and sets a higher Damage/Speed
            SetHealth(m_currentHealth / 2);
            SetDamage(m_chaosFlameDamage);
            SetSpeed(m_chaosFlameSpeed);

            // Destroys the Particle after 10 Seconds
            Destroy(temp, m_particleTime);
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
                swordCollider.enabled = true;
                // Trigger is down set to true
                m_triggerDown = true;

                Amaterasu.transform.localRotation = Quaternion.Euler(0, 90, 0); // Will be Removed

                // Starts the Cool down of the Dash to prevent infinite dash
                StartCoroutine(Dash());
            }
            // or if the trigger isnt pressed
            else if (XCI.GetAxis(XboxAxis.RightTrigger, m_controller) < 0.1)
            {
                // Disable the collider to prevent baseless attack
                swordCollider.enabled = false;
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
            if (isActive == true && m_playerSkills.Find(skill => skill.Name == "Blood Lust") && m_Enemy.isDeadbByKenron)
            {
                // Reduces the cooldown from Kenrons Cooldown 
                skillManager.m_Skills[0].m_currentCoolDown = skillManager.m_Skills[0].m_currentCoolDown - m_cooldownReduced;
            }
            // If Kenron has the skill specificed and his health is less than or equal to 0
            if (m_playerSkills.Find(skill => skill.Name == "Curse of Amaterasu") && m_currentHealth <= 0.0f)
            {
                // Disables Main kenron, Spawns and enables Child kenron at Main Kenrons position 
                childKenron.CheckStatus();
                childKenron.transform.position = this.gameObject.transform.position;
                childKenron.gameObject.SetActive(true);
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
        }
    }
}

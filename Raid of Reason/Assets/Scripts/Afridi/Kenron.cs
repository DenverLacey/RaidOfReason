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

public class Kenron : BaseCharacter
{

    [Header("--Dash Attack Stats--")]

    [SerializeField]
    [Tooltip("Distance of Dash Attack")]
    private float m_maxDashDistance;

    [SerializeField]
    [Tooltip("How quickly Kenron dashes")]
    private float m_dashSpeed;

    [SerializeField]
    [Tooltip("How much delay between consecutive dashes in seconds")]
    private float m_dashDelay;

    // charge times
    [SerializeField]
    public int m_currentCharges;

    [SerializeField]
    [Tooltip("Buffer distance to avoid Kenron getting stuck in walls")]
    private float m_dashBufferDistance = 3f;

    [SerializeField]
    [Tooltip("How many charges it takes for dashes to go off")]
    public int m_charges;

    [SerializeField]
    [Tooltip("Time till a charge is recharged again")]
    public float m_rechargeRate;

    private Vector3 m_dashVelocity;

    [SerializeField]
    [Tooltip("Hit box for dash attack")]
    private BoxCollider m_dashCollider;

    [Header("--Skills--")]

    [Tooltip("Checks if Kenrons Skill is Active")]
    public bool isActive = false;

    [SerializeField]
    [Tooltip("Kenrons Minimum Damage Boost whilst in Chaos Flame")]
    private float m_minCFDamage;

    [SerializeField]
    [Tooltip("Kenrons Maximum Damage Boost whilst in Chaos Flame")]
    private float m_maxCFDamage;

    [SerializeField]
    [Tooltip("Kenrons Speed Boost whilst in Chaos Flame")]
    private float m_chaosFlameSpeed;

    [SerializeField]
    [Tooltip("Health Gained Back from Kenrons Vile Infusion Skill")]
    private float m_healthGained;

    [SerializeField]
    [Tooltip("Ability Duration Increase from Kenrons Blood Lust Skill")]
    private float m_RTDurationIncreased;

    [SerializeField]
    [Tooltip("Kenrons Minimum Damage Boost whilst in Chaos Flame with Ronin Mastery")]
    private float m_minSADamage;

    [SerializeField]
    [Tooltip("Kenrons Maximum Damage Boost whilst in Chaos Flame with Ronin Mastery")]
    private float m_maxSADamage;

    [SerializeField]
    [Tooltip("Kenrons Speed Boost whilst in Chaos Flame with Ronin Mastery")]
    private float m_SAFlameSpeed;

    [SerializeField]
    [Tooltip("How long it takes for the trail in Kenrons Ability To Go Away")]
    private float m_BTDegen;


    [Header("--Particles And UI--")]

    [SerializeField]
    [Tooltip("Particle Effect That Appears on the Kenrons Body When Chaos Flame is Active")]
    private ParticleSystem m_kenronParticle;

    [SerializeField]
    [Tooltip("Particle Effect That Indicates Chaos Flame is Active")]
    private ParticleSystem m_startParticle;


    // Enemy reference used for skill checking
    private EnemyData m_Enemy;

    // Desired position to dash
    private Vector3 m_dashPosition;
    private Vector3 m_dashStartPosition;

    // Delay until next dash
    private float m_dashDelayTimer;
    private float m_estimatedDashTime;
    private float m_dashDistance;
    private bool m_dashDone;
    private bool m_InfiniteDash;
    private int m_TempCharge;

    // Checks if Kenron is Dashing or Not
    private bool isDashing;

    // Checks if a specific trigger on the controller is pressed
    private bool m_triggerDown;


    // Kenrons Collider
    private CapsuleCollider m_collider;

    // Stat Tracker
    [HideInInspector]
    public StatTrackingManager m_statManager;
    private PauseMenu m_pauseInfo;

    private void Start()
    {
        GameManager.Instance.GiveCharacterReference(this);
        m_collider = GetComponent<CapsuleCollider>();
        m_pauseInfo = FindObjectOfType<PauseMenu>();
    }

    protected override void Awake()
    {
        // Initalisation
        base.Awake();
        CharacterType = CharacterType.KENRON;
        m_Enemy = FindObjectOfType<EnemyData>();
        m_statManager = FindObjectOfType<StatTrackingManager>();
        m_currentCharges = m_charges;
        m_TempCharge = m_currentCharges - 1;
        isDashing = false;
        m_InfiniteDash = false;

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

        if (gameObject != null)
        {
            // Uses his Dash
            DashAttack();
            SkillChecker();
        }

        Vector3 position = transform.position;
        position.y = 0.1f;
        Debug.DrawLine(position, position + transform.forward * 0.5f);
    }

    /// <summary>
    /// Kenrons Skill. By Halving his health, he gains a boost of Speed and Damage 
    /// </summary>
    public void ChaosFlame(float skillDuration)
    {
        // Empty Check
        if (this.gameObject != null)
        {
            if (skillDuration >= skillManager.m_mainSkills[0].m_duration)
            {
                // Sets the Skill to be Active
                m_InfiniteDash = true;
                isActive = true;
                foreach (GameObject obj in m_statManager.dashDisplays)
                {
                    obj.SetActive(true);
                    obj.GetComponent<Image>().color = Color.gray;
                }
                StartCoroutine(ChaosFlameVisual());
                m_kenronParticle.Play();
                // Halves his Health and sets a higher Damage/Speed
                if (isActive == true && m_skillUpgrades.Find(skill => skill.Name == "Ronin Mastery"))
                {
                    SetHealth(m_currentHealth / 3);
                    SetDamage(m_minSADamage, m_maxSADamage);
                    SetSpeed(m_SAFlameSpeed);
                }
                else
                {
                    SetHealth(m_currentHealth / 2);
                    SetDamage(m_minCFDamage, m_maxCFDamage);
                    SetSpeed(m_chaosFlameSpeed);
                }
            }
        }
    }

    /// <summary>
    /// Kenrons Attack. By Pressing the Right Trigger, Kenron Dashes Forward Dealing Damage to Any Enemies in his direction
    /// </summary>
    public void DashAttack()
    {
        if (isActive && m_InfiniteDash) {
            m_dashDelay = 0.01f;           
        }

        // dash attack
        if (XCI.GetAxis(XboxAxis.RightTrigger, controller) > 0.1f && m_controllerOn && !m_triggerDown)
        {
            if (m_currentCharges > 0 || m_InfiniteDash)
            {
                // set boolean flags
                m_triggerDown = true;
                m_controllerOn = false;
                isDashing = true;
                m_dashDone = false;
                m_dashCollider.enabled = true;
                m_dashDelayTimer = m_dashDelay;
                m_dashStartPosition = transform.position;
				if (!isActive && !m_InfiniteDash)
				{
					m_statManager.dashDisplays[m_TempCharge].SetActive(false);
					m_TempCharge--;
				}
				m_currentCharges--;


				// set animator's trigger
				m_animator.SetBool("Attack", true);


				if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, m_maxDashDistance, LayerMask.GetMask("Environment")))
				{
					m_dashPosition = hit.point - transform.forward * m_dashBufferDistance;
				}
				else
				{
					m_dashPosition = transform.position + transform.forward * m_maxDashDistance;
				}

				m_dashDistance = (m_dashPosition - m_dashStartPosition).magnitude;

				// calculate estimated time of dash
				m_estimatedDashTime = m_maxDashDistance / m_dashSpeed;

				// size hit box
				Vector3 hitBoxSize = new Vector3(m_dashCollider.size.x, m_dashCollider.size.y, m_dashDistance);
                m_dashCollider.size = hitBoxSize;

                // rotate hit box
                m_dashCollider.transform.rotation = transform.rotation;

                // position hit box
                m_dashCollider.transform.position = transform.position + transform.forward * (m_dashDistance / 2f);


                // apply force
                m_rigidbody.AddForce(transform.forward * m_dashSpeed * m_dashDistance / m_maxDashDistance, ForceMode.VelocityChange);

                GameManager.Instance.Kenron.m_statManager.dashesUsed++;
            }
        }
        else if (XCI.GetAxis(XboxAxis.RightTrigger, controller) < 0.1f && !isDashing)
        {
            m_triggerDown = false;
        }

        if (isDashing)
        {
            m_estimatedDashTime -= Time.deltaTime;
            if ((m_dashStartPosition - transform.position).sqrMagnitude >= m_dashDistance * m_dashDistance || m_estimatedDashTime <= 0.0f)
            {

                // reset boolean flags
                m_dashCollider.enabled = false;
                m_animator.SetBool("Attack", false);

                m_dashDone = true;

                // stop kenron
                m_rigidbody.velocity = Vector3.zero;

				transform.position = m_dashPosition;

                // run delay timer
                m_dashDelayTimer -= Time.deltaTime;

                m_dashCollider.GetComponent<SwordDamage>().CalculateNewMostDamageDealt();
            }

            // if ready to dash again 
            if (m_dashDelayTimer <= 0.0f && gameObject.activeSelf)
            {
                if (!isActive && !m_InfiniteDash)
                {
                    StartCoroutine(TimeTillRecharge());
                }
                m_controllerOn = true;
                isDashing = false;
            }
        }
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
        if (gameObject != null && m_Enemy != null)
        {
            // Sets the image to true if the skills are found
            UnlockSkill();

            // If Kenron has the skill specified and is killed by Kenron
            if (m_skillUpgrades.Find(skill => skill.Name == "Vile Infusion") && m_Enemy.isDeadbByKenron)
            {
                // Return a bit of health to Kenrons current health
                this.m_currentHealth = m_currentHealth + m_healthGained;
            }

            // If Kenron has the skill specified and is killed by Kenron
            if (isActive == true && m_skillUpgrades.Find(skill => skill.Name == "Bloodlust") && m_Enemy.isDeadbByKenron)
            {
                // Reduces the cooldown from Kenrons Cooldown 
                skillManager.m_mainSkills[0].m_currentDuration -= m_RTDurationIncreased;
            }
        }
    }

    public void UnlockSkill()
    {
        //if (m_skillPopups != null)
        //{
        //    if (m_skillUpgrades.Find(skill => skill.Name == "Vile Infusion"))
        //    {
        //        // Icon pops up
        //        m_skillPopups[0].gameObject.SetActive(true);
        //    }
        //    if (m_skillUpgrades.Find(skill => skill.Name == "Bloodlust"))
        //    {
        //        // Icon pops up
        //        m_skillPopups[1].gameObject.SetActive(true);
        //    }
        //    if (m_skillUpgrades.Find(skill => skill.Name == "Shuras Awakening"))
        //    {
        //        // Icon pops up
        //        m_skillPopups[2].gameObject.SetActive(true);
        //    }
        //    if (m_skillUpgrades.Find(skill => skill.Name == "Malevolent Inferno"))
        //    {
        //        // Icon pops up
        //        m_skillPopups[3].gameObject.SetActive(true);
        //    }
        //}
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
                isActive = false;
                m_InfiniteDash = false;
                m_currentCharges = m_charges;
                SetDamage(m_minDamage, m_maxDamage);
                SetSpeed(m_movementSpeed);
                m_kenronParticle.Stop();
                m_statManager.chaosFlameUsed++;
                m_TempCharge = m_currentCharges - 1;
                foreach (GameObject obj in m_statManager.dashDisplays)
                {
                    obj.GetComponent<Image>().color = Color.white;
                }
            }
        }
    }


    IEnumerator ChaosFlameVisual()
    {
        m_startParticle.Play();
        yield return new WaitForSeconds(0.1f);
        m_startParticle.Stop();
    }

    IEnumerator TimeTillRecharge()
    {
        yield return new WaitForSeconds(m_rechargeRate);
        if (m_currentCharges < m_charges)
        {
            m_currentCharges++;
            m_TempCharge++;
            m_statManager.dashDisplays[m_TempCharge].SetActive(true);
        }
    }
}
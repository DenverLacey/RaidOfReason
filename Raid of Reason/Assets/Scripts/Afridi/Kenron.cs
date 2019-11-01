using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;
using XInputDotNetPure;

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

    [Tooltip("Health Gained Back from Kenrons Vile Infusion Skill")]
    public float m_healthGained;

	[SerializeField]
	[Tooltip("How long, in seconds, Kenron will be stationary when casting Chaos Flame")]
	private float m_CFMovementDelay = 0.5f;

    [Tooltip("Ability Duration Increase from Kenrons Blood Lust Skill")]
    public float m_RTDurationIncreased;

    [SerializeField]
    [Tooltip("How long it takes for the trail in Kenrons Ability To Go Away")]
    private float m_BTDegen;

    public List<GameObject> dashDisplays = new List<GameObject>();


    [Header("--Particles And UI--")]

    [SerializeField]
    [Tooltip("Particle Effect That Appears on the Kenrons Body When Chaos Flame is Active")]
    private ParticleSystem m_kenronParticle;

    [SerializeField]
    [Tooltip("Particle Effect That Indicates Chaos Flame is Active")]
    private ParticleSystem m_startParticle;

	[SerializeField]
	[Tooltip("Trail Renderer for Dash Trail Effect")]
	private GameObject m_dashTrail;


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
    //private float m_rumbleDuration = 0.1f;
    //private float m_rumbleIntensity = 1000f;
    private int m_TempCharge;
    // Checks if Kenron is Dashing or Not
    private bool isDashing;

    // Checks if a specific trigger on the controller is pressed
    private bool m_triggerDown;


    // Kenrons Collider
    private CapsuleCollider m_collider;

    private void Start()
    {
        GameManager.Instance.GiveCharacterReference(this);
        m_collider = GetComponent<CapsuleCollider>();
    }

    protected override void Awake()
    {
        // Initalisation
        base.Awake();
        CharacterType = CharacterType.KENRON;
        m_Enemy = FindObjectOfType<EnemyData>();
        m_currentCharges = m_charges;
        isDashing = false;
        m_TempCharge = m_currentCharges - 1;
        // set size of dash hit box
        Vector3 hitBoxSize = new Vector3(m_dashCollider.size.x, m_dashCollider.size.y, m_maxDashDistance);
        m_dashCollider.size = hitBoxSize;
        m_dashCollider.enabled = false;
		m_dashTrail.SetActive(false);
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
			RestrictControlsForSeconds(m_CFMovementDelay, MovementAxis.All);

            if (skillDuration >= skillManager.m_mainSkills[0].m_duration)
            {
                isActive = true;
                StartCoroutine(ChaosFlameVisual());
                m_kenronParticle.Play();
                // DoRumble();
                SetDamage(m_minCFDamage, m_maxCFDamage);
                SetHealth(m_currentHealth / 2);
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
            if (m_currentCharges > 0)
            {
                // set boolean flags
                m_triggerDown = true;
                m_controllerOn = false;
                isDashing = true;
                m_dashDone = false;
                m_dashCollider.enabled = true;
                m_dashDelayTimer = m_dashDelay;
                m_dashStartPosition = transform.position;
                // DoRumble();
                dashDisplays[m_TempCharge].SetActive(false);
                m_TempCharge--;
                m_currentCharges--;

				// set animator's trigger
				m_animator.SetBool("Attack", true);

				// activate trail effect
				m_dashTrail.SetActive(true);

				if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, m_maxDashDistance, LayerMask.GetMask("Environment", "Barrier")))
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
            }
        }
        else if (XCI.GetAxis(XboxAxis.RightTrigger, controller) < 0.1f && !isDashing)
        {
            m_triggerDown = false;
        }

        if (isDashing)
        {
            m_estimatedDashTime -= Time.deltaTime;
            if ((m_dashPosition - transform.position).sqrMagnitude <= 0.1f || m_estimatedDashTime <= 0.0f)
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
            }
			else
			{
				Vector3 lerpPosition = Vector3.Lerp(transform.position, m_dashPosition, m_dashSpeed * Time.deltaTime);
				m_rigidbody.MovePosition(lerpPosition);
			}

            // if ready to dash again 
            if (m_dashDelayTimer <= 0.0f && gameObject.activeSelf)
            {
                StartCoroutine(TimeTillRecharge());
                m_controllerOn = true;
                isDashing = false;
				m_dashTrail.SetActive(false);
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
            if (skillManager.m_mainSkills[0].m_currentDuration >= skillManager.m_mainSkills[0].m_duration)
            {
                // Resets Stats and Skill
                isActive = false;
                m_currentCharges = m_charges;
                SetDamage(m_minDamage, m_maxDamage);
                SetSpeed(m_movementSpeed);
                m_TempCharge = m_currentCharges - 1;
                m_kenronParticle.Stop();
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
            dashDisplays[m_TempCharge].SetActive(true);
        }
    }
}
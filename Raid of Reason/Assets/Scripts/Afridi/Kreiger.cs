using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

/*
  * Author: Afridi Rahim
  *
  * Summary: The Stats and Management of Kreigers Core Mechanics.
  *          Manages his abilites and his skill tree as he improves within the
  *          game
*/

public class Kreiger : BaseCharacter
{
    [Header("Punching Attacks")]

    [Tooltip("The Collider of Kreigers Left Gauntlet")]
    public Collider LeftGauntlet;

    [Tooltip("The Collider of Kreigers Right Gauntlet")]
    public Collider RightGauntlet;

    [SerializeField]
    [Tooltip("Buffer distance to avoid Kreiger getting stuck in walls")]
    private float m_lungeBufferDistance;

    [SerializeField]
    [Tooltip("The max distance Kreiger can lunge")]
    private float m_maxLungeDistance;

    [SerializeField]
    [Tooltip("How fast can Kreiger lunge when attacking?")]
    private float m_lungeSpeed;

    [SerializeField]
    [Tooltip("How much delay between consecutive lunges in seconds")]
    private float m_lungeDelay;

    [Tooltip("How much shield will Kreiger gain on every punch?")]
    public float shieldGain;

    [Header("--Skills--")]

    [SerializeField]
    [Tooltip("Size of the area of effect for taunt ability")]
    private float m_tauntRadius;
	public float TauntRadius { get => m_tauntRadius; }

    [SerializeField]
    [Tooltip("How vulnerable Kreiger is while taunting (1.0 is default)")]
    private float m_tauntVulnerability;

    [SerializeField]
    [Tooltip("How much knockback is applied to Kreigers Hydraulic Pummel")]
    public float knockBackForce;

    [SerializeField]
    [Tooltip("Increased Radius from Kreigers Roaring Thunder ability")]
    private float m_RTRadiusIncreased;

    [SerializeField]
    [Tooltip("Increased Duration from Kreigers Roaring Thunder ability")]
    private float m_RTDurationIncreased;

    [SerializeField]
    [Tooltip("How much more knockback Kreiger deals with Kinetic Dischage")]
    public float KDForce;

    [Tooltip("How long the enemy is stunned by Kinetic Dischage")]
    public float KDStun;

    [Tooltip("How Much Damage dealt when an enemy gets hit by another Enemy by Kinetic Dischage")]
    public float KDKnockbackDamage;

    [SerializeField]
    [Tooltip("How much shields Kreigers Static Shield Gives To His Allies")]
    private float m_SSShieldsGiven;

    [SerializeField]
    [Tooltip("How much damage Enemies take with Static Shield")]
    public float SSDamageTaken;

    [Tooltip("Checks if Kreigers Skill is Active")]
    public bool isTaunting;

    // Skill is active check
    public bool isActive;

    [Header("--Particles And UI--")]

    [SerializeField]
    [Tooltip("Electric Effect That Appears When Kreiger Taunts")]
    private ParticleSystem m_tauntParticle;

    [SerializeField]
    [Tooltip("Shock Effect That Appears When Kreiger Taunts")]
    private ParticleSystem m_debrisParticle;

    private float m_lungeDelayTimer;
    // Desired position to lunge.
    private Vector3 m_lungePosition;
    // Kreiger's Collider
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
    private bool runOnce;

    // Stat Tracker
    [HideInInspector]
    public StatTrackingManager m_statManager;

    private float totalAmountTaunted;
    private float amountTaunted;

    public List<EnemyData> m_hitEnemies;

    private void Start()
	{
		GameManager.Instance.GiveCharacterReference(this);
        m_collider = GetComponent<CapsuleCollider>();
        InitialiseUpgrades();
        m_hitEnemies = new List<EnemyData>();
    }


    /// <summary>
    /// Checks how many skills Kreiger has obtained from his skill tree
    /// - Roaring Thunder: More Range for his Taunt and Cooldown is Halved
    /// - Shock Wave: Melee now knocksback and has a chance to stun (Passive)
    /// - Kinetic Discharge: Enemies now take damage when attacking Taunted Kreiger
    /// - Macht Des Sturms: Chain Lightning attack that damages and stuns + team mates are granted electric damage (Stun Buff)
    /// </summary>

    void InitialiseUpgrades()
    {
        UnlockSkill();
        if (m_skillUpgrades.Find(skill => skill.Name == "Roaring Thunder"))
        {
            // Returns increased Radius
            this.m_tauntRadius += m_RTRadiusIncreased;

            // Cooldown is halved
            skillManager.m_mainSkills[1].m_duration += m_RTDurationIncreased;
        }
    }

	protected override void Awake()
    {
        // Initialisation 
        base.Awake();
        CharacterType = CharacterType.KREIGER;
        m_tauntParticle.GetComponentInChildren<ParticleSystem>();
        m_statManager = FindObjectOfType<StatTrackingManager>();
        LeftGauntlet.enabled = false;
        RightGauntlet.enabled = false;
        isTaunting = false;
        isActive = false;
        triggerIsDown = false;
        islunging = false;
        runOnce = true;

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
        }
    }

    protected override void Update()
    {
        // Allows Kreiger to perform Melee Punches 
        base.Update();
        Punch();
    }

    public void UnlockSkill()
    {
        //if (m_skillPopups.Count > 1)
        //{
        //    if (m_skillUpgrades.Find(skill => skill.Name == "Roaring Thunder"))
        //    {
        //        // Icon pops up
        //        m_skillPopups[0].gameObject.SetActive(true);
        //    }
        //    if (m_skillUpgrades.Find(skill => skill.Name == "Kinetic Discharge"))
        //    {
        //        // Icon pops up
        //        m_skillPopups[1].gameObject.SetActive(true);
        //    }
        //    if (m_skillUpgrades.Find(skill => skill.Name == "Static Shield"))
        //    {
        //        // Icon pops up
        //        m_skillPopups[2].gameObject.SetActive(true);
        //    }
        //    if (m_skillUpgrades.Find(skill => skill.Name == "Macht Des Sturms"))
        //    {
        //        // Icon pops up
        //        m_skillPopups[3].gameObject.SetActive(true);
        //    }
        //}
    }

    /// <summary>
    /// Kreigers Attack. By Pressing the Right Trigger, Kreiger Becomes Stationery and Punches his foes switching between fists
    /// </summary>
    public void Punch()
    {
        // if right trigger down and attack animation is not playing
        if (XCI.GetAxis(XboxAxis.RightTrigger, controller) > 0.1f && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack Left") && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack Right") && !islunging)
        {
            m_animator.SetBool("LeftGauntlet", !m_animator.GetBool("LeftGauntlet"));
            m_animator.SetBool("Attack", true);

			CanMove = false;

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
            if (!CanMove)
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
                CanMove = true;
                islunging = false;
            }
        }
    }

    public void SkillChecker()
    {
        if (gameObject != null)
        {
            if (isActive = true && m_skillUpgrades.Find(skill => skill.Name == "Static Shield"))
            {
                if (GameManager.Instance.Kenron.playerState != PlayerState.DEAD && !GameManager.Instance.Kenron)
                {
                    GameManager.Instance.Kenron.currentShield = m_SSShieldsGiven;
                }
                if (GameManager.Instance.Thea.playerState != PlayerState.DEAD && !GameManager.Instance.Thea)
                {
                    GameManager.Instance.Thea.currentShield = m_SSShieldsGiven;
                }
            }
        }
    }

	void StartLunge()
	{
		islunging = true;

		CanRotate = false;

		// enable colliders
		RightGauntlet.enabled = true;
		LeftGauntlet.enabled = true;
	}

	void StopLunge()
	{
		CanRotate = true;

		// Disable colliders
		RightGauntlet.enabled = false;
		LeftGauntlet.enabled = false;
        OnDisable();
	}

    public void OnDisable()
    {
        m_hitEnemies.Clear();
    }

    IEnumerator MachinasDareVisual()
    {
        yield return new WaitForSeconds(0.5f);
        m_debrisParticle.Play();

    }

    /// <summary>
	/// Kreiger's Ability. Boosting His Health up and reducing incoming damage he taunts all enemies to himself
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
                    GameManager.Instance.Kreiger.m_statManager.enemiesTaunted++;
                    totalAmountTaunted++;

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
    /// Resets Kreigers Stats back to his base after Spott is Used
    /// </summary>
    public void ResetSkill()
    {
        if (skillManager.m_mainSkills[1].m_currentDuration >= skillManager.m_mainSkills[1].m_duration)
        {
            // Vulnerable once more
            ResetVulernability();

            // Skill no longer active
            isTaunting = false;

            MostTaunted();
        }
    }

    public void MostTaunted()
    {
        GameManager.Instance.Kreiger.m_statManager.highestTaunted = Mathf.Max(totalAmountTaunted, GameManager.Instance.Kreiger.m_statManager.highestTaunted);
        totalAmountTaunted = 0.0f;
    }
    
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && isActive == true)
        {
            if (m_skillUpgrades.Find(skill => skill.Name == "Static Shield"))
            {
                collision.gameObject.GetComponent<EnemyData>().TakeDamage(SSDamageTaken, GameManager.Instance.Kreiger);             
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

/*
  * Author: Afridi Rahim, Denver Lacey
  * Description: Handle all of Kenrons Core Mechanics 
  * Last Edited: 15/11/2019
*/
public class Kreiger : BaseCharacter
{
    #region Punch Variables
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
    #endregion

    #region Skills
    [Header("--Skills--")]

    [SerializeField]
    [Tooltip("Size of the area of effect for Machinas Dare")]
    private float m_tauntRadius;
	public float TauntRadius { get => m_tauntRadius; }

	[SerializeField]
	[Tooltip("How much damage taunt will do to effected enemies")]
	private float m_tauntDamage = 10f;

	[SerializeField]
	[Tooltip("Force applied to enemies on taunt")]
	private float m_tauntKnockbackForce = 40f;

	[SerializeField]
	[Tooltip("How long enemies will be stunned after taunt")]
	private float m_tauntStunDuration = 1f;

    [SerializeField]
    [Tooltip("How vulnerable Kreiger is while taunting (1.0 is default)")]
    private float m_tauntVulnerability;

	[SerializeField]
	[Tooltip("How long, in seconds, Nashorn will be stationary when casting taunt")]
	private float m_tauntMovementDelay = 1.0f;

	[SerializeField]
	[Tooltip("Intensity of the rumble when Nashorn taunts")]
	private float m_tauntRumbleIntensity = 1f;

	[SerializeField]
	[Tooltip("Duration of the rumble when Nashorn taunts")]
	private float m_tauntRumbleDuration = 0.4f;

    [SerializeField]
    [Tooltip("How much knockback is applied to Kreigers Hydraulic Pummel")]
    public float knockBackForce;

    [Tooltip("How long the enemy is stunned for")]
    public float stunTime;

    [Tooltip("How Much Damage dealt when an enemy gets hit by another Enemy")]
    public float knockBackDamage;

    [SerializeField]
    [Tooltip("How much shields Kreigers Gives To His Allies")]
    private float m_shieldsGiven;

    [SerializeField]
    [Tooltip("How much damage Enemies take when attacking Krieger")]
    public float thornDamage;

    [Tooltip("Checks if Kreigers Skill is Active")]
    public bool isTaunting;
    #endregion

    #region Particles
    [Header("--Particles And UI--")]

    [SerializeField]
    [Tooltip("Electric Effect That Appears When Kreiger Taunts")]
    private ParticleSystem m_tauntParticle;

    [SerializeField]
    [Tooltip("Shock Effect That Appears When Kreiger Taunts")]
    private ParticleSystem m_debrisParticle;

	[SerializeField]
	[Tooltip("Taunt Effect")]
	private TauntEffectIndicator m_tauntEffect;
    #endregion

    private float m_lungeDelayTimer;
    // Desired position to lunge.
    private Vector3 m_lungePosition;
    // Kreiger's Collider
    private CapsuleCollider m_collider;
    // Sets the lunge 
    private bool islunging;
    // checks if trigger has been pressed
    private bool triggerIsDown;

    // Enemies hit by Punches
    public List<EnemyData> m_hitEnemies;

    private void Start()
	{
        m_collider = GetComponent<CapsuleCollider>();
        m_hitEnemies = new List<EnemyData>();
    }

    #region Intialisation
    protected override void Awake()
    {
        // Initialisation 
        base.Awake();
        CharacterType = CharacterType.KREIGER;
        m_tauntParticle.GetComponentInChildren<ParticleSystem>();
        LeftGauntlet.enabled = false;
        RightGauntlet.enabled = false;
        isTaunting = false;
        isSkillActive = false;
        triggerIsDown = false;
        islunging = false;
        GameManager.Instance.GiveCharacterReference(this);
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
    #endregion

    /// <summary>
    /// Kreigers Attack. By Pressing the Right Trigger, Kreiger Becomes Stationery and Punches his foes switching between fists
    /// </summary>
    public void Punch()
    {
        // if right trigger down and attack animation is not playing
        if (XCI.GetAxis(XboxAxis.RightTrigger, controller) > 0.1f && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack Left") && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack Right") && !islunging)
        {
            m_animator.SetBool("Attack", true);

			CanMove = false;

            m_lungeDelayTimer = m_lungeDelay;

			// calculate desired dash position
			int layerMask = LayerMask.GetMask("Environment", "Barrier");
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

    /// <summary>
    /// Allows Krieger to Lunge Forward
    /// </summary>
	void StartLunge()
	{
		islunging = true;

		CanRotate = false;

		m_animator.SetBool("LeftGauntlet", !m_animator.GetBool("LeftGauntlet"));

		// enable colliders
		RightGauntlet.enabled = true;
		LeftGauntlet.enabled = true;

		// play sound effect
		// AkSoundEngine.PostEvent("Machina_Attack_Event", gameObject);
		AudioManager.Instance.PlaySound(SoundType.KRIEGER_ATTACK);
    }

    /// <summary>
    /// Stops Krieger from lunging forward
    /// </summary>
	void StopLunge()
	{
		CanRotate = true;

		// Disable colliders
		RightGauntlet.enabled = false;
		LeftGauntlet.enabled = false;
        OnDisable();
	}

    /// <summary>
    /// Clears any hit enemies if Krieger Dies 
    /// </summary>
    public void OnDisable()
    {
        m_hitEnemies.Clear();
    }


    /// <summary>
    /// Handles the Particle on when Nashorn uses Machinas Dare
    /// </summary>
    /// <returns>The length of how long the particle is active till it ends</returns>
    IEnumerator MachinasDareVisual()
    {
        yield return new WaitForSeconds(0.5f);
        m_debrisParticle.Play();
    }

    /// <summary>
	/// Boosting His Health up and reducing incoming damage he taunts all enemies to himself
	/// </summary>
    public void MachinasDare(float skillDuration)
    {
		RestrictControlsForSeconds(m_tauntMovementDelay, MovementAxis.All);

        // Turns ability UI On/Off
        if (Ability_UI != null)
        {
            Ability_UI.SetActive(false);
        }

        // Ability is active
        isTaunting = true;
        isSkillActive = true;

        GameManager.Instance.Kreiger.currentShield += m_shieldsGiven; 

        // Give Shields to available Player
        if (Utility.IsPlayerAvailable(CharacterType.KENRON)) 
        {
            GameManager.Instance.Kenron.currentShield += m_shieldsGiven;
        }
        if (Utility.IsPlayerAvailable(CharacterType.THEA))
        {
            GameManager.Instance.Thea.currentShield += m_shieldsGiven;
        }
        
        // Plays second taunt particle
        m_tauntParticle.Play();
        StartCoroutine(MachinasDareVisual());

        // set vulnerability
        m_vulnerability = m_tauntVulnerability;

		if (m_animator)
		{
			m_animator.SetTrigger("Taunt");
		}
    }

    /// <summary>
    ///  Plays an effect when an animations is played
    /// </summary>
	public void OnTauntAnimation()
	{
		// m_tauntEffect.Show(m_tauntRadius, transform.position);
		RumbleController(m_tauntRumbleDuration, m_tauntRumbleIntensity, m_tauntRumbleIntensity);

		// taunt enemies
		foreach (EnemyData enemy in GameObject.FindObjectsOfType<EnemyData>())
		{
			float sqrDist = (enemy.transform.position - transform.position).sqrMagnitude;
			if (sqrDist <= m_tauntRadius * m_tauntRadius)
			{
				enemy.Taunted = true;
				enemy.TakeDamage(m_tauntDamage, this);
				Vector3 direction = (enemy.transform.position - transform.position).normalized;
				enemy.KnockBack(direction * m_tauntKnockbackForce, m_tauntStunDuration);
			}
		}
	}

    /// <summary>
    /// Resets Kreigers Stats back to his base after Machinas Dare is Used
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

    /// <summary>
    /// Handles The thorn damage dealt to enemies
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && isSkillActive == true)
        {
             collision.gameObject.GetComponent<EnemyData>().TakeDamage(thornDamage, GameManager.Instance.Kreiger);
        }
    }

    /// <summary>
    /// Handles the character if they die whilst using a skill or punching
    /// </summary>
    public override void ResetCharacter()
	{
		base.ResetCharacter();
		ResetSkill();
		StopLunge();
		islunging = false;
		m_lungeDelayTimer = 0f;
	}

    private void OnSkillReady()
    {
		// AkSoundEngine.PostEvent("Machina_UI_CoolDowns_Event", gameObject);
		AudioManager.Instance.PlaySound(SoundType.KRIEGER_COOLDOWN);
    }
}

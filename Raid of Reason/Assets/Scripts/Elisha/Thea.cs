using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

/* 
 * Author: Elisha_Anagnostakis
 * Description: Thea's main class, which handles her basic attacks and abilities.
 */

public class Thea : BaseCharacter 
{
    [Header("--GOP Growth--")]
    [SerializeField]
    [Tooltip("How big can Thea's AOE get?")]
    private float m_AOEMax;

    [SerializeField]
    [Tooltip("What is Thea's minimum radius of her AOE.")]
    private float m_AOEMin;

    [SerializeField]
    [Tooltip("How fast will Thea's AOE grow?")]
    private float m_AOEGrowTime;

    [SerializeField]
    [Tooltip("How much the Gift of Poseidon heals by?")]
    private float m_GOPEffect;

    [Header("--Skills--")]

    [Tooltip("How much health thea passively heals with her first skill")]
    public float healthRegenerated;

    [Tooltip("The delay between how much health thea passively with her first skill")]
    public float regenEachFrame;

    [SerializeField]
    [Tooltip("How much more healing Thea does with her Fourth ability")]
    public float m_healMultiplier;

    [Tooltip("how much the enemies are knockbacked by theas third skill")]
    public float knockbackForce;

    public bool m_skillActive;

    [SerializeField]
    private List<EnemyData> m_nearbyEnemies = new List<EnemyData>();

    [Header("--Projectile Attacks--")]

    [SerializeField]
    [Tooltip("Insert Theas projectile object.")]
    private GameObject m_projectile;

    public GameObject m_aimCursor;

    [SerializeField]
    [Tooltip("How long of a delay will it take for her next projectile to instiantiate?")]
    private float m_projectileDelay;

    [SerializeField]
    [Tooltip("How fast will Thea's cursor move?")]
    private float aimCursorSpeed;

    public float m_aimCursorRadius;

    [Header("--Particles And UI--")]

    [SerializeField]
    private GameObject m_waterPrefab;

    [SerializeField]
    [Tooltip("The AOE particle used for visual effect.")]
    private ParticleSystem m_HealRadius;

    [SerializeField]
    [Tooltip("The Second AOE particle used for visual effect.")]
    private ParticleSystem m_HealRadius_2;

    [SerializeField]
    [Tooltip("The Final particle used for visual effect.")]
    private ParticleSystem m_HealRadius_3;

    // Stat Tracker
    [HideInInspector]
    public StatTrackingManager m_statManager;


    // Runs once check
    private bool neverDone;


    private Vector3 newLocation;

    private bool m_isHealthRegen;

    private Kenron m_kenron;
    private Nashorn m_nashorn;
    private Vector3 m_hitLocation;
    private LayerMask m_layerMask;
    private float m_shotCounter;
    private float m_counter;
    private bool m_isActive;
    private float m_AOERadius;
    private float m_AOETimer;
    private float m_particleRadius;

    private ParticleSystem.ShapeModule m_AOEShapeModule;
    private ParticleSystem.ShapeModule m_AOEShapeModule_2;

    private CapsuleCollider m_collider;

    private const string PROJECTILE_PREFAB_PATH = "projectile";

	private void Start()
	{
		GameManager.Instance.GiveCharacterReference(this);
        m_collider = GetComponent<CapsuleCollider>();
	}

	/// <summary>
	/// Gets called before Start.
	/// </summary>
	protected override void Awake()
    {
        base.Awake();
        CharacterType = CharacterType.THEA;
        m_isActive = false;
        neverDone = true;
        m_isHealthRegen = false;
        m_statManager = FindObjectOfType<StatTrackingManager>();
        m_waterPrefab.SetActive(false);
        m_nashorn = FindObjectOfType<Nashorn>();
		m_kenron = FindObjectOfType<Kenron>();
        m_AOETimer = 0f;
        m_HealRadius.gameObject.SetActive(false);
        m_HealRadius_2.gameObject.SetActive(false);
        m_AOEShapeModule = m_HealRadius.shape;
        m_AOEShapeModule_2 = m_HealRadius_2.shape;

        if (m_skillPopups.Count != 0)
        {
            foreach (Image display in m_skillPopups)
            {
                display.gameObject.SetActive(false);
            }
        }

        m_aimCursor = GameObject.FindGameObjectWithTag("AimCursor");
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        // If the player is alive.
        if(this.gameObject != null)
        {
			base.FixedUpdate();
            Projectile();
        }
    }

    protected override void Update()
    {
        base.Update();
        SkillChecker();
    }

	protected override void CharacterMovement()
	{
		// get stick input
		float leftX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
		float leftY = XCI.GetAxis(XboxAxis.LeftStickY, controller);

		float rightX = XCI.GetAxis(XboxAxis.RightStickX, controller);
		float rightY = XCI.GetAxis(XboxAxis.RightStickY, controller);

		// make input vectors
		Vector3 movInput = new Vector3(leftX, 0, leftY);
		Vector3 rotInput = new Vector3(rightX, 0, rightY);

		if (CanMove)
		{
			Vector3 movePosition = transform.position + movInput * m_movementSpeed * Time.deltaTime;
			m_rigidbody.MovePosition(movePosition);
		}

		if (CanRotate)
		{
			if (rotInput.sqrMagnitude > 0.01f)
			{
				m_direction = rotInput;
			}

			transform.localRotation = Quaternion.LookRotation(m_direction);
		}

		if (m_animator)
		{
			// Calculate angle between character's direction and forward
			float angle = Vector3.SignedAngle(m_direction, Vector3.forward, Vector3.up);

			// Rotate movement into world space to get animation movement
			Vector3 animationMovement = Quaternion.AngleAxis(angle, Vector3.up) * movInput;

			// Set animator's movement floats
			m_animator.SetFloat("MovX", animationMovement.x);
			m_animator.SetFloat("MovZ", animationMovement.z);
		}
	}

	/// <summary>
	/// Triggering the shootinf
	/// </summary>
	public void Projectile()
    {
        m_counter += Time.deltaTime;
        if (XCI.GetAxis(XboxAxis.RightTrigger, controller) > 0.1f)
        {
            m_shotCounter += Time.deltaTime;

            if (m_counter > m_projectileDelay)
            {
                Vector3 desiredPosition = transform.position + transform.forward;
                desiredPosition.y = 1;
                FireProjectile(desiredPosition, transform.rotation);
                m_counter = 0;
            }
        }
        else if (XCI.GetAxis(XboxAxis.RightTrigger, controller) < 0.1f)
        {
            m_shotCounter = 0;
        }

        #region Old Shooting Mech
        //m_counter += Time.deltaTime;

        //// If the player presses the right trigger button.
        //if (XCI.GetAxis(XboxAxis.RightTrigger, controller) > 0.1)
        //{
        //    //m_skillPopups[0].enabled = true;
        //    // Start the shot counter.
        //    m_shotCounter += Time.deltaTime;

        //    if (m_counter > m_projectileDelay)
        //    {
        //        // Instantiate projectile object.
        //        Vector3 desiredPosition = transform.position + transform.forward;
        //        desiredPosition.y = 1;
        //        GameObject temp = Instantiate(m_projectile, desiredPosition, transform.rotation);
        //        // Set projectile damage and move projectile.
        //        temp.GetComponent<ProjectileMove>().SetDamage(m_damage);
        //        // Reset counter.
        //        m_counter = 0f;
        //    }
        //}
        //// If player releases the right trigger button.
        //else if (XCI.GetAxis(XboxAxis.RightTrigger, controller) < 0.1)
        //{
        //    // Reset the counter.
        //    m_shotCounter = 0f;
        //    //m_skillPopups[0].enabled = false;
        //}
        #endregion
    }

    /// <summary>
    /// Implements the object pooling for the projectiles thea shoots.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="rot"></param>
    private void FireProjectile(Vector3 pos, Quaternion rot)
    {
        var projectile = ObjectPooling.GetPooledObject(PROJECTILE_PREFAB_PATH);

        projectile.transform.position = pos;
        projectile.transform.rotation = rot;

        projectile.SetActive(true);
    }

    void SkillChecker()
    {
        if (this.gameObject != null)
        {
            // Sets the image to true if the skills are found
            UnlockSkill();
            if (m_skillUpgrades.Find(skill => skill.name == "Settling Tide"))
            {
                if (neverDone == true)
                {
                    skillManager.m_mainSkills[2].m_coolDown = skillManager.m_mainSkills[2].m_coolDown / 2;
                    neverDone = false;
                }
                if (m_currentHealth != m_maxHealth && !m_isHealthRegen)
                {
                    StartCoroutine(HealthOverTime());
                }
            }
            if (m_skillUpgrades.Find(skill => skill.name == "Oceans Ally"))
            {
                float healthcomparison = GameManager.Instance.Kenron.m_currentHealth + GameManager.Instance.Nashorn.m_currentHealth;

                if (healthcomparison <= 150)
                {
                    m_projectileDelay = 0.6f;
                }
                if (healthcomparison <= 130)
                {
                    m_projectileDelay = 0.5f;
                }
                if (healthcomparison <= 60)
                {
                    m_projectileDelay = 0.4f;
                }
                if (healthcomparison <= 25)
                {
                    m_projectileDelay = 0.3f;
                }
            }
            if (m_isActive == true && m_skillUpgrades.Find(skill => skill.name == "Hydro Pressure"))
            {
                m_nearbyEnemies = new List<EnemyData>(FindObjectsOfType<EnemyData>());
                foreach (EnemyData enemy in m_nearbyEnemies)
                {
                    float sqrDistance = (this.transform.position - enemy.transform.position).sqrMagnitude;
                    if (sqrDistance <= m_AOERadius * m_AOERadius)
                    {
                        enemy.Stun(0.5f);
                        enemy.Rigidbody.AddExplosionForce(knockbackForce, transform.position, m_AOERadius, 0, ForceMode.Impulse);
                        enemy.TakeDamage(1, this);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Thea has the ability to do a charge up time attack with scaling AOE and heal.
    /// The longer you hold the charge up, the bigger the AOE is which means 
    /// the more health is healed when released. If players are in the AOE when 
    /// the charge up is released the players will be healed the right amount.
    /// </summary>
    public void GiftOfPoseidon()
    { 
        m_isActive = true;

        // Checks if player can use the ability.
        if (m_isActive == true && GameManager.Instance.Thea.gameObject.activeSelf)
        {
            // Start AOE timer.
            m_AOETimer += Time.deltaTime;
            // Disable player movement and rotation.
            m_controllerOn = false;

            m_HealRadius.gameObject.SetActive(true);
            m_HealRadius_2.gameObject.SetActive(true);

            // Grow AOE radius.
            m_AOERadius = Mathf.Lerp(m_AOERadius, m_AOEMax, m_AOETimer / m_AOEGrowTime);
            Debug.Log(m_AOERadius);
            // AOE collider radius grows in conjuction to the lerp.
            Debug.Log("Float radius: " + m_AOERadius);

            m_AOEShapeModule.radius = m_AOERadius;
            m_AOEShapeModule_2.radius = m_AOERadius;
            m_skillActive = true;
        }

        // Checks if ability has been used.
        if (m_isActive == false)
        {
            return;
        }

        if (!GameManager.Instance.Thea.gameObject.activeSelf)
        {
            m_HealRadius.gameObject.SetActive(false);
            m_HealRadius_2.gameObject.SetActive(false);
        }
    }
    public void UnlockSkill()
    {
        if (m_skillPopups.Count > 1)
        {
            if (m_skillUpgrades.Find(skill => skill.Name == "Settling Tide"))
            {
                // Icon pops up
                m_skillPopups[0].gameObject.SetActive(true);
            }
            if (m_skillUpgrades.Find(skill => skill.Name == "Oceans Ally"))
            {
                // Icon pops up
                m_skillPopups[1].gameObject.SetActive(true);
            }
            if (m_skillUpgrades.Find(skill => skill.Name == "Hydro Pressure"))
            {
                // Icon pops up
                m_skillPopups[2].gameObject.SetActive(true);
            }
            if (m_skillUpgrades.Find(skill => skill.Name == "Serenade of Water"))
            {
                // Icon pops up
                m_skillPopups[3].gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// checks the distance from both characters to thea and if they are within 
    /// range when the player releases the trigger, it will heal them according 
    /// to how long the player charges the heal.
    /// </summary>
    public void GiveHealth()
    {

        if (GameManager.Instance.Thea.gameObject.activeSelf)
        {
            // Stat Tracking Stuff
            bool kenHealed = false;
            bool nashHealed = false;

            foreach (BaseCharacter player in GameManager.Instance.Players)
            {
                if (!player || player == this)
                {
                    continue;
                }

                float sqrDistance = (player.transform.position - transform.position).sqrMagnitude;

                // check if inside radius
                if (sqrDistance <= m_AOERadius * m_AOERadius && player.m_controllerOn)
                {
                    if (player.tag == "Nashorn") { nashHealed = true; }
                    if (player.tag == "Kenron") { kenHealed = true; }
                    m_waterPrefab.SetActive(true);
                    if (m_skillUpgrades.Find(skill => skill.Name == "Serenade Of Water"))
                    {
                        player.m_currentHealth += m_AOETimer * m_GOPEffect;
                    }
                    else
                    {
                        player.AddHealth(m_AOETimer * m_GOPEffect);
                    }
                    m_waterPrefab.transform.position = player.transform.position;
                    GameObject m_temp = Instantiate(m_waterPrefab, player.transform.position + Vector3.down * (player.transform.localScale.y / 2), Quaternion.Euler(90, 0, 0), player.transform);
                    Destroy(m_temp, 0.5f);
                    StartCoroutine(GOPVisual());
                }
            }

            // Heal Thea.
            AddHealth(m_AOETimer * m_GOPEffect);
            GameManager.Instance.Thea.m_statManager.damageHealed += m_currentHealth;

            // Stat Tracking
            if (kenHealed && nashHealed) { GameManager.Instance.Thea.m_statManager.gopHitThree++; kenHealed = false; nashHealed = false; }
            if (kenHealed) { GameManager.Instance.Thea.m_statManager.damageHealed += m_kenron.m_currentHealth; }
            if (nashHealed) { GameManager.Instance.Thea.m_statManager.damageHealed += m_nashorn.m_currentHealth; }
            if (m_AOERadius > 9.95f) { GameManager.Instance.Thea.m_statManager.gopFullCharged++; }

            if (m_skillActive = true & m_skillUpgrades.Find(skill => skill.name == "Serenade Of Water"))
            {
                m_kenron.SetHealth(m_kenron.m_currentHealth * m_healMultiplier);
                m_nashorn.SetHealth(m_nashorn.m_currentHealth * m_healMultiplier);
                SetHealth(m_currentHealth * m_healMultiplier);
                m_skillActive = false;
            }
        }


       
    }

    /// <summary>
    /// Resets Theas skill and changes the radius of the AOE to min.
    /// </summary>
    public void ResetGiftOfPoseidon()
    {
        m_AOETimer = 0f;
        m_isActive = false;
        m_AOERadius = m_AOEMin;
        //m_AOEParticleCollider.enabled = false;
        m_controllerOn = true;
        m_AOEShapeModule.radius = 1;

        m_HealRadius.gameObject.SetActive(false);
        m_HealRadius.Stop();

        m_HealRadius_2.gameObject.SetActive(false);
        m_HealRadius_2.Stop();

        GameManager.Instance.Thea.m_statManager.gopUsed++;
        GameManager.Instance.Thea.m_statManager.damageHealed += m_nashorn.m_currentHealth + m_kenron.m_currentHealth + m_currentHealth;
    }

    private IEnumerator HealthOverTime() {
        m_isHealthRegen = true;
        while (m_currentHealth < m_maxHealth) {
            Regenerate();
            yield return new WaitForSeconds(regenEachFrame);
        }
        m_isHealthRegen = false;
    }

    private IEnumerator GOPVisual()
    {
        if (GameManager.Instance.Thea.gameObject.activeSelf)
        {
            m_HealRadius_3.Play();
            yield return new WaitForSeconds(0.5f);
            m_waterPrefab.SetActive(false);
            m_HealRadius_3.Stop();
        }
    }

    public void Regenerate() {
        AddHealth(healthRegenerated);
    }
}
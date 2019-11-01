﻿using System.Collections;
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

    [SerializeField]
    [Tooltip("How long controller will rumble on Gift of Poseidon")]
    private float m_GOPRumbleDuration;

    [SerializeField]
    [Tooltip("How intense rumble will be on Gift of Poseidon")]
    private float m_GOPRumbleIntensity;

    [SerializeField]
    [Tooltip("Movement Speed while casting Gift of Poseidon")]
    private float m_GOPMovement;

    public GameObject GOPMaxCharge;
	private Vector3 m_GOPMaxChargeInitialScale;

    private GameObject GOPChargeMeter;
    private GameObject GOPChargeMeterBar;

    [Header("--Skills--")]

    [Tooltip("How much health thea passively heals with her Settling Tide")]
    public float STHealthRegenerated;

    [Tooltip("The delay between how much health thea passively with her Settling Tide")]
    public float STRegenEachFrame;

    [Tooltip("How much cooldown Reduction Thea gets with Settling Tide")]
    [SerializeField]
    private float STReductionRate;

    [Tooltip("How much charge time reduction Thea gets with Settling Tide")]
    [SerializeField]
    private float STChargeRate;

    [Tooltip("How much slower the Enemy can move inisde GOP with Hydro Pressure")]
    [SerializeField]
    private float HPSpeedReductionMultiplier;

    [Tooltip("How much less damage the Enemy can do inisde GOP with Hydro Pressure (0.5 is Half Damage, 0 is no Damage)")]
    [SerializeField]
    public float HPAttackWeakened;

    [Tooltip("How much bigger the AOE Radius is when using GOP with Hydro Pressure")]
    [SerializeField]
    public float HPGrowthSize;

    [SerializeField]
    [Tooltip("How much more healing Thea does with her Fourth ability")]
    public float m_healMultiplier;

    [Tooltip("Checks How Many Specific Times the Health of her allies have to be for Oceans Ally (MAX SIZE OF 3)")]
    [SerializeField]
    private int[] OAAllyHealthChecks = new int[4];


    [Tooltip("How Many Specific Times the Fire Rate of her Projectiles increases with Oceans Ally (MAX SIZE OF 3)")]
    [SerializeField]
    private float[] OAFireRateIncreased = new float[4];

    [Tooltip("How Long Theas Damage Immunity Lasts for when She Uses Seranade Of Water")]
    private float SOWImmunity;


    [Header("--Projectile Attacks--")]

    [SerializeField]
    [Tooltip("Insert Theas projectile object.")]
    private GameObject m_projectile;

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

	//[SerializeField]
	//[Tooltip("Gift of Poseidon Particle Object")]
	//private GameObject m_GOPParticle;
	//private Vector3 m_GOPCircleInitialScale;
	//private List<ParticleSystem.MainModule> m_GOPWaves;
	//private List<ParticleSystem> m_GOPParticleSystems;
	//private GameObject m_GOPCircle;

	[SerializeField]
	[Tooltip("Initial Effect when activating GIft of Poseidon")]
	private ParticleSystem m_healEffect;

    [SerializeField]
    [Tooltip("The AOE particle used for visual effect.")]
    private ParticleSystem m_HealRadius;

    //[SerializeField]
    //[Tooltip("The Second AOE particle used for visual effect.")]
    //private ParticleSystem m_HealRadius_2;

    //[SerializeField]
    //[Tooltip("The Final particle used for visual effect.")]
    //private ParticleSystem m_HealRadius_3;

    [HideInInspector]
    public bool m_skillActive;

    private Vector3 newLocation;

    private bool m_isHealthRegen;
    private Vector3 m_hitLocation;
    private LayerMask m_layerMask;
    private float m_shotCounter;
    private float m_counter;
    private bool m_isActive;
    private float m_AOERadius;
    private float m_AOETimer;
    private float m_particleRadius;

    public Transform camera;

    private List<EnemyData> m_nearbyEnemies = new List<EnemyData>();

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
        m_isHealthRegen = false;
        m_AOETimer = 0f;
        m_HealRadius.gameObject.SetActive(false);
        //m_HealRadius_2.gameObject.SetActive(false);
        m_AOEShapeModule = m_HealRadius.shape;
        //m_AOEShapeModule_2 = m_HealRadius_2.shape;

        //m_GOPCircle = m_GOPParticle.transform.Find("circle").gameObject;
        //m_GOPCircleInitialScale = m_GOPCircle.transform.localScale;

        //m_GOPParticleSystems = new List<ParticleSystem>(m_GOPParticle.GetComponentsInChildren<ParticleSystem>());

        //m_GOPWaves = new List<ParticleSystem.MainModule>
        //{
        //	m_GOPParticle.transform.GetChild(1).GetComponent<ParticleSystem>().main,
        //	m_GOPParticle.transform.GetChild(2).GetComponent<ParticleSystem>().main,
        //	m_GOPParticle.transform.GetChild(3).GetComponent<ParticleSystem>().main,
        //	m_GOPParticle.transform.GetChild(4).GetComponent<ParticleSystem>().main,
        //	m_GOPParticle.transform.GetChild(5).GetComponent<ParticleSystem>().main,
        //};

        //m_GOPParticle.SetActive(false);

        //      IntialiseUpgrades();
        GOPChargeMeter = GameObject.Find("ChargeMeter");
        GOPChargeMeterBar = GameObject.Find("ChargeMeterBar");
        GOPChargeMeter.transform.localScale = new Vector3(0, 1, 0);
        GOPMaxCharge.SetActive(false);
		m_GOPMaxChargeInitialScale = GOPMaxCharge.transform.localScale;
		GOPMaxCharge.transform.localScale = new Vector3(m_AOEMax * m_GOPMaxChargeInitialScale.x, m_AOEMax * m_GOPMaxChargeInitialScale.y, 0);
		GOPChargeMeter.SetActive(false);
        GOPChargeMeterBar.SetActive(false);
    }

    void IntialiseUpgrades()
    {
        //if (m_skillUpgrades.Find(skill => skill.name == "Settling Tide"))
        //{
        //    skillManager.m_mainSkills[2].m_coolDown /= STReductionRate;
        //    m_AOEGrowTime /= STChargeRate;
        //}
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
        if (m_currentHealth != m_maxHealth && !m_isHealthRegen && gameObject.activeSelf)
        {
            StartCoroutine(HealthOverTime());
        }

        if (GOPChargeMeterBar)
        {
            GOPChargeMeterBar.transform.LookAt(camera);
        }

        


        //if (!m_isActive && abilityUI.gameObject != null)
        //{
        //    abilityUI.Play();
        //}
        //else
        //{
        //    abilityUI.Stop();
        //}

        //float healthcomparison = GameManager.Instance.Kenron.m_currentHealth + GameManager.Instance.Kreiger.m_currentHealth;

        //if (healthcomparison <= OAAllyHealthChecks[0])
        //{
        //    m_projectileDelay = OAFireRateIncreased[0];
        //}
        //if (healthcomparison <= OAAllyHealthChecks[1])
        //{
        //    m_projectileDelay = OAFireRateIncreased[1];
        //}
        //if (healthcomparison <= OAAllyHealthChecks[2])
        //{
        //    m_projectileDelay = OAFireRateIncreased[2];
        //}
        //if (healthcomparison <= OAAllyHealthChecks[3])
        //{
        //    m_projectileDelay = OAFireRateIncreased[3];
        //}

        if (GameManager.Instance.Thea.playerState == PlayerState.DEAD)
        {
			m_healEffect.gameObject.SetActive(false);
            m_HealRadius.gameObject.SetActive(false);
            //m_HealRadius_2.gameObject.SetActive(false);
            //m_HealRadius_3.gameObject.SetActive(false);
            //m_GOPParticle.SetActive(false);
        }
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

        Vector3 camRotEuler = m_camera.transform.eulerAngles;
        camRotEuler.x = 0f; camRotEuler.z = 0f;

        movInput = Quaternion.AngleAxis(camRotEuler.y, Vector3.up) * movInput;
        rotInput = Quaternion.AngleAxis(camRotEuler.y, Vector3.up) * rotInput;

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
	/// Triggering the shooting
	/// </summary>
	public void Projectile()
    {
        m_counter += Time.deltaTime;
        if (XCI.GetAxis(XboxAxis.RightTrigger, controller) > 0.1f && !m_isActive)
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
        else
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

    /// <summary>
    /// Thea has the ability to do a charge up time attack with scaling AOE and heal.
    /// The longer you hold the charge up, the bigger the AOE is which means 
    /// the more health is healed when released. If players are in the AOE when 
    /// the charge up is released the players will be healed the right amount.
    /// </summary>
    public void GiftOfPoseidon(float skillDuration)
    {
        // Resets the charge meter to 0.
        GOPChargeMeter.transform.localScale = new Vector3(0, 1, 0);
        GOPMaxCharge.SetActive(true);

        GOPChargeMeter.SetActive(true);
        GOPChargeMeterBar.SetActive(true);
        m_isActive = true;

        // Checks if player can use the ability.
        if (m_isActive == true && GameManager.Instance.Thea.gameObject.activeSelf && GOPChargeMeter)
        {
            // Start AOE timer.
            m_AOETimer += Time.deltaTime;

            m_movementSpeed = m_GOPMovement;

            m_HealRadius.gameObject.SetActive(true);
            m_HealRadius.Play();
            //m_HealRadius_2.gameObject.SetActive(true);

            // Lerping the AOE
            m_AOERadius = Mathf.Lerp(m_AOERadius, m_AOEMax, m_AOETimer / m_AOEGrowTime);
            // Charge meter charges up based on the radius of the AOE.
            GOPChargeMeter.transform.localScale = new Vector3(m_AOERadius / m_AOEMax, 1);
            m_AOEShapeModule.radius = m_AOERadius;
            //m_AOEShapeModule_2.radius = m_AOERadius;
           
            //m_GOPWaves.ForEach(p => p.startSize = m_AOERadius);
            //m_GOPCircle.transform.localScale = new Vector3(m_AOERadius * m_GOPCircleInitialScale.x, m_GOPCircleInitialScale.y, m_AOERadius * m_GOPCircleInitialScale.z);
            //m_GOPParticle.SetActive(true);

            m_skillActive = true;

            if (m_animator)
            {
                m_animator.SetBool("Casting", true);
            }

            m_nearbyEnemies = new List<EnemyData>(FindObjectsOfType<EnemyData>());
            foreach (EnemyData enemy in m_nearbyEnemies)
            {
                float sqrDistance = (this.transform.position - enemy.transform.position).sqrMagnitude;
                if (sqrDistance <= m_AOERadius * m_AOERadius)
                {
                    enemy.Pathfinder.SetSpeedReduction(HPSpeedReductionMultiplier);
                    enemy.Strength = HPAttackWeakened;
                }
            }

            // Checks if ability has been used.
            if (m_isActive == false)
            {
                return;
            }

            if (!GameManager.Instance.Thea.gameObject.activeSelf)
            {
                m_HealRadius.gameObject.SetActive(false);
                m_HealRadius.Stop();
                //m_HealRadius_2.gameObject.SetActive(false);
                //m_GOPParticle.SetActive(false);
            }
        }
    
    }

	public void EndGIftOfPoseidon()
	{
		foreach (var pathfinder in FindObjectsOfType<EnemyPathfinding>())
		{
			pathfinder.ResetSpeedReduction();
		}

		if (m_animator)
		{
			m_animator.SetBool("Casting", false);
		}
	}

	public void GiftOfPoseidonHealAndReset()
	{
		GiveHealth();
		ResetGiftOfPoseidon();
        RumbleController(m_GOPRumbleDuration, m_GOPRumbleIntensity, m_GOPRumbleIntensity);
        m_healEffect.Play();
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
            foreach (BaseCharacter player in GameManager.Instance.Players)
            {
                if (player == this)
                {
                    continue;
                }

                float sqrDistance = (player.transform.position - transform.position).sqrMagnitude;

                // check if inside radius
                if (sqrDistance <= m_AOERadius * m_AOERadius && player.m_controllerOn)
                {
					GameObject waterObj = Instantiate(m_waterPrefab, player.transform.position + Vector3.down, m_waterPrefab.transform.rotation);
					waterObj.SetActive(true);
					waterObj.transform.parent = player.transform;
					Destroy(waterObj, 2f);
					StartCoroutine(GOPVisual());
                    player.AddHealth(m_GOPEffect);
					m_projectile.GetComponent<ProjectileMove>().InstantiateHealIndicator(player.transform.position, m_GOPEffect);
                }
            }

            // Heal Thea.
            AddHealth(m_GOPEffect * m_AOEGrowTime);      

            //if (m_skillActive = true & m_skillUpgrades.Find(skill => skill.name == "Serenade Of Water"))
            //{
            //    GameManager.Instance.Kenron.SetHealth(GameManager.Instance.Kenron.m_currentHealth * m_healMultiplier);
            //    GameManager.Instance.Kreiger.SetHealth(GameManager.Instance.Kreiger.m_currentHealth * m_healMultiplier);
            //    m_nearbyEnemies = new List<EnemyData>(FindObjectsOfType<EnemyData>());
            //    foreach (EnemyData data in m_nearbyEnemies)
            //    {
            //        data.Strength = 0;
            //    }
            //    SetHealth(m_currentHealth * m_healMultiplier);
            //    m_skillActive = false;
            //}
        }
    }

    /// <summary>
    /// Resets Theas skill and changes the radius of the AOE to min.
    /// </summary>
    public void ResetGiftOfPoseidon()
    {
        m_AOETimer = 0f;
        m_isActive = false;
        m_AOERadius = 0;
        m_movementSpeed = m_currentMovement;
        GOPMaxCharge.SetActive(false);
        GOPChargeMeter.SetActive(false);
        GOPChargeMeterBar.SetActive(false);
        m_AOEShapeModule.radius = 0;

        m_HealRadius.gameObject.SetActive(false);
        m_HealRadius.Stop();

        //m_HealRadius_2.gameObject.SetActive(false);
        //m_HealRadius_2.Stop();

        //      m_GOPWaves.ForEach(ps => ps.startSize = 1f);
        //m_GOPCircle.transform.localScale = m_GOPCircleInitialScale;
        //m_GOPParticleSystems.ForEach(ps => ps.Stop());

        //m_GOPParticle.SetActive(false);
    }

    private IEnumerator HealthOverTime() {
        m_isHealthRegen = true;
        while (m_currentHealth < m_maxHealth) {
            Regenerate();
            yield return new WaitForSeconds(STRegenEachFrame);
        }
        m_isHealthRegen = false;
    }

    private IEnumerator GOPVisual()
    {
        if (GameManager.Instance.Thea.gameObject.activeSelf)
        {
            m_healEffect.Play();
            yield return new WaitForSeconds(0.5f);
            m_healEffect.Stop();
        }
    }

    private IEnumerator DamageImmunity()
    {
        yield return new WaitForSeconds(SOWImmunity);
        foreach (EnemyData data in m_nearbyEnemies)
        {
            data.Strength = 1;
        }
    }

    public void Regenerate() {
        AddHealth(STHealthRegenerated);
    }
}
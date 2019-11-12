using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using XInputDotNetPure;
using UnityEngine.UI;
using DG.Tweening;

/* 
 * Author: Elisha, Denver, Afridi
 * Description: Abstract base class that will be derived by character classes Kenron, Kreiger and Thea. Handles everything to do 
 *              with the characters movements and states.
 */

[RequireComponent(typeof(Rigidbody))]
public abstract class BaseCharacter : MonoBehaviour
{
    public CharacterType CharacterType { get; protected set; }

	public struct MovementAxis
	{
		public static int Move   { get => 0x01 << 0; }
		public static int Rotate { get => 0x01 << 1; }
		public static int All	 { get => Move | Rotate; }

		private static int[] m_enumerate = { Move, Rotate };
		public static int[] Enumerate { get => m_enumerate; }
	}

	public enum PlayerState
    {
        ALIVE,
        DEAD,
		NP
    }

    public delegate void OnTakeDamage(BaseCharacter player);
    public delegate void OnCriticalDamage(BaseCharacter player);

    [Header("--Player Stats--")]

    public PlayerState playerState;
    public float m_maxHealth;
    public float m_currentHealth;
    [Tooltip("This variable indicates how much more health the player can recieve")]
    public float m_maxOverheal;
    protected Rigidbody m_rigidbody;
    private float m_damage;

    [SerializeField]
    [Tooltip("How fast will the player move?")]
    protected float m_movementSpeed;
    public GameObject Ability_UI;

    protected float m_currentMovement;

    [SerializeField]
    [Tooltip("How much damage will the player deal?")]
    protected float m_minDamage;

    private float m_mincurrentDamage;
    private float m_maxcurrentDamage;

    [SerializeField]
    [Tooltip("How much damage will the player deal?")]
    protected float m_maxDamage;

    [Tooltip("How much max shield can the player get?")]
    public float m_maxShield;

    public float currentShield;

    [Tooltip("The delay between the materials switching")]
    public float feedbackAmount;

    [SerializeField]
    [Tooltip("How long will it take for the shield to start degenerating?")]
    private float m_shieldBuffer;

    [SerializeField]
    [Tooltip("How much shield degenerates?")]
    private float m_shieldDegenerateAmount;

    [Tooltip("Pick what controller this player is.")]
    public XboxController controller;

	[Tooltip("Pick the player index of this character")]
	public PlayerIndex playerIndex;

    // Player Colliders
    private CapsuleCollider m_playerCollider;
    public ObjectiveManager man;

    [Tooltip("The Skill Manager that manages the skills of the players")]
    public SkillManager skillManager;
    private HealthBarUI m_healthBarRef;
    private GameObject m_playerStats;

    [HideInInspector]
    public bool m_controllerOn;
	private int m_movementAxes = MovementAxis.Move | MovementAxis.Rotate;

    public OnTakeDamage onTakeDamage;

	public bool CanMove
	{
		get => (m_movementAxes & MovementAxis.Move) == MovementAxis.Move;
		set => m_movementAxes = value ? m_movementAxes | MovementAxis.Move : m_movementAxes & ~MovementAxis.Move;
	}

	public bool CanRotate
	{
		get => (m_movementAxes & MovementAxis.Rotate) == MovementAxis.Rotate;
		set => m_movementAxes = value ? m_movementAxes | MovementAxis.Rotate : m_movementAxes & ~MovementAxis.Rotate;
	}

    //public Material damageFeedback;
    //private Material originalMaterial;
    //public Material[] playerMaterial;

    [SerializeField]
    [Tooltip("Material to show daamge")]
    private Material m_damageMaterial;
    private List<SkinnedMeshRenderer> m_playerRenderers;
    private List<Material> m_originalMaterials = new List<Material>();

    [HideInInspector]
    public MultiTargetCamera m_camera;
    protected Animator m_animator;
    protected float m_vulnerability;
	public bool isSkillActive;
    protected float m_rotationSpeed = 250.0f;
    protected Vector3 m_direction;
    private ParticleSystem.ColorOverLifetimeModule m_original;
    public ParticleSystem m_spriteRend;
    private DeathMenu m_deathMenu;
    private PauseMenu m_pauseInfo;

    /// <summary>
    /// This will be called first.
    /// </summary>
    protected virtual void Awake()
    {
        m_camera = FindObjectOfType<MultiTargetCamera>();
        m_playerCollider = GetComponent<CapsuleCollider>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponentInChildren<Animator>();
        m_deathMenu = FindObjectOfType<DeathMenu>();
        m_pauseInfo = FindObjectOfType<PauseMenu>();
        m_currentHealth = m_maxHealth;
        m_mincurrentDamage = m_minDamage;
        m_maxcurrentDamage = m_maxDamage;
        m_vulnerability = 1.0f;
        m_controllerOn = true;
        isSkillActive = false;
        man = FindObjectOfType<ObjectiveManager>();
        currentShield = 0;
        skillManager = FindObjectOfType<SkillManager>();
        m_original = m_spriteRend.colorOverLifetime;
        m_healthBarRef = FindObjectOfType<HealthBarUI>();
        m_playerStats = GameObject.Find("---Stats---");
        m_playerStats.SetActive(true);

        m_playerRenderers = new List<SkinnedMeshRenderer>(GetComponentsInChildren<SkinnedMeshRenderer>());
        foreach (var rend in m_playerRenderers)
        {
            m_originalMaterials.Add(rend.material);
        }
        m_currentMovement = m_movementSpeed;
    }

    /// <summary>
    /// Physics update.
    /// </summary>
    protected virtual void FixedUpdate()
    {
        
	}

    /// <summary>
    /// Updates every frame.
    /// </summary>
    protected virtual void Update()
    {
        if (m_pauseInfo.m_isPaused)
            return;

        // If player has shield
        if (currentShield > 0)
        {
            // Start buffer
            StartCoroutine(StartShieldBuffer(m_shieldBuffer));
        }

        // if the player has no shield.
        if (currentShield <= 0)
        {
            // Set shield to 0.
            currentShield = 0;
        }

        if (currentShield > m_maxShield)
        {
            currentShield = m_maxShield;
        }

        if (m_currentHealth < 0)
        {
            m_currentHealth = 0;    
        }

        // Checks the states of all derived players of this class.
        switch (playerState)
        {
            case PlayerState.ALIVE:
                // Call this.
                m_playerCollider.enabled = true;
                CharacterMovement();
                break;

            case PlayerState.DEAD:
				StopRumbleController();
				SoftDeactivate();
                break;
        }
    }

    /// <summary>
    /// Handles the main movement of each derived character using Xinput.
    /// </summary>
    virtual protected void CharacterMovement() 
    {
        // Checks if player is active.
        if (m_controllerOn)
        {
			// get stick input
			float leftX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
			float leftY = XCI.GetAxis(XboxAxis.LeftStickY, controller);

			float rightX = XCI.GetAxis(XboxAxis.RightStickX, controller);
			float rightY = XCI.GetAxis(XboxAxis.RightStickY, controller);

			// make sure player movement is relative to the direction of the cameras forward
			Vector3 input = new Vector3(leftX, 0, leftY);

			// make sure player direction override is relative to the direction of the cameras forward
			Vector3 directionOverride = new Vector3(rightX, 0, rightY);

			// make input vectors relative to camera's rotation
			Vector3 camRotEuler = m_camera.transform.eulerAngles;
			camRotEuler.x = 0.0f; camRotEuler.z = 0.0f;

            input = Quaternion.AngleAxis(camRotEuler.y, Vector3.up) * input;
            directionOverride = Quaternion.AngleAxis(camRotEuler.y, Vector3.up) * directionOverride;

			if (CanRotate)
			{
				// determine which direction vector to rotate to
				if (directionOverride.sqrMagnitude > 0.01f)
				{
					m_direction = directionOverride;
				}
				else if (input.sqrMagnitude > 0.01f)
				{
					m_direction = input;
				}

				// rotate player
				transform.localRotation = Quaternion.LookRotation(m_direction);
			}

			if (CanMove)
			{
				Vector3 movePosition = transform.position + input * m_movementSpeed * Time.deltaTime;
				m_rigidbody.MovePosition(movePosition);
			}
			else
			{
				input = Vector3.zero;
			}

			if (m_animator)
			{
				// Calculate angle between character's direction and forward
				float angle = Vector3.SignedAngle(m_direction, Vector3.forward, Vector3.up);

				// Rotate movement into world space to get animation movement
				Vector3 animationMovement = Quaternion.AngleAxis(angle, Vector3.up) * input;

				// Set animator's movement floats
				m_animator.SetFloat("MovX", animationMovement.x);
				m_animator.SetFloat("MovZ", animationMovement.z);
			}
		}
    }

    /// <summary>
    /// This allows the player to take damage.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage)
    {
        if (currentShield > 0)
        {
            currentShield -= damage * m_vulnerability;
        }

        if (currentShield <= 0)
        {
            // Take an amount of damage from the players current health.
            m_currentHealth -= damage * m_vulnerability;
            if (this.gameObject.activeSelf == true)
            {
                for (int i = 0; i < m_playerRenderers.Count; i++)
                {
                    m_playerRenderers[i].material = m_damageMaterial;
                }
                StartCoroutine(ResetMaterialsCoroutine(feedbackAmount));
            }
        }

        // If player has no health.
        if (m_currentHealth <= 0.0f)
        {
			if (playerState != PlayerState.DEAD)
				RespawnManager.RespawnPlayer(this);

            playerState = PlayerState.DEAD;
        }
		else
		{
			RumbleController(.1f);
		}

        //// checks if all players are dead
        //if (GameManager.Instance.DeadPlayers.Count == 3)
        //{
        //    StartCoroutine(DeathScreenDelay(1f));
        //}

        onTakeDamage?.Invoke(this);
	} 

    //public IEnumerator DeathScreenDelay(float duration)
    //{
    //    yield return new WaitForSeconds(duration);
    //    m_playerStats.SetActive(false);
    //    m_deathMenu.DeathScreen();
    //}

    /// <summary>
    /// Sets damage to a float value.
    /// </summary>
    /// <param name="damage"></param>
    virtual public void SetDamage(float minimum, float maximum)
    {
        m_mincurrentDamage = minimum;
        m_maxcurrentDamage = maximum;      
    }

    /// <summary>
    /// Sets health to a float value.
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(float health)
    {
        m_currentHealth = health;

        if (m_currentHealth > m_maxHealth)
        {
            m_currentHealth = m_maxHealth;
        }
    }

    public void AddHealth(float amount)
    {
        SetHealth(m_currentHealth + amount);
    }

    /// <summary>
    /// Returns the players movement speed.
    /// </summary>
    /// <returns> float value. </returns>
    virtual public float GetSpeed()
    {
        return m_movementSpeed;
    }

    /// <summary>
    /// Sets the players speed to a float value.
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        m_movementSpeed = speed;
    }

    /// <summary>
    /// Returns the damage taken.
    /// </summary>
    /// <returns> float value. </returns>
    virtual public float GetDamage()
    {
        return m_damage = Random.Range(m_mincurrentDamage, m_maxcurrentDamage);
    }

    virtual public void ResetDamage()
    {
        m_mincurrentDamage = m_minDamage;
        m_maxcurrentDamage = m_maxDamage;
    }

    /// <summary>
    /// Returns the current health of the player.
    /// </summary>
    /// <returns> float value. </returns>
    public float GetHealth()
    {
        return m_currentHealth;
    }

    /// <summary>
    /// Sets the max health of the player.
    /// </summary>
    /// <param name="maxhealth"></param>
    public void SetMaxHealth(float maxhealth) {
        maxhealth = m_maxHealth;
    }

    /// <summary>
    /// Gets the player max's health.
    /// </summary>
    /// <returns> float value. </returns>
    public float GetMaxHealth()
    {
        return m_maxHealth;
    }

    /// <summary>
    /// Gets Kreigers vulnerability status.
    /// </summary>
    /// <returns> float value. </returns>
    public float GetVulnerability() {
        return m_vulnerability;
    }

    /// <summary>
    /// Sets Kreigers vulnerability status.
    /// </summary>
    /// <param name="vulnerability"></param>
    public void SetVulnerability(float vulnerability) {
        m_vulnerability = vulnerability;
    }

    /// <summary>
    /// Resets Kreigers vulnerability to 1.
    /// </summary>
    public void ResetVulernability() {
        m_vulnerability = 1.0f;
    }

    /// <summary>
    /// Buffer for shield before degeneration.
    /// </summary>
    /// <param name="buffer"></param>
    /// <returns></returns>
    IEnumerator StartShieldBuffer(float buffer)
    {
        yield return new WaitForSeconds(buffer);
        // Degenerate shield per second by the amount.
        currentShield -= m_shieldDegenerateAmount * Time.deltaTime;
    }

    IEnumerator ResetMaterialsCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        ResetMaterials();
    }
    void ResetMaterials()
    {
        for (int i = 0; i < m_playerRenderers.Count; i++)
        {
            m_playerRenderers[i].material = m_originalMaterials[i];
        }
    }

	public void SetPlayerToNotPlaying()
	{
		playerState = PlayerState.NP;
		SoftDeactivate();
	}

	public void SoftActivate()
	{
        gameObject.SetActive(true);
		transform.Find("Model").gameObject.SetActive(true);
		ResetCharacter();
	}

	public void SoftDeactivate()
	{
		transform.Find("Model").gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

	public void RumbleController(float duration, float leftIntensity = 1f, float rightIntensity = 1f)
	{
		StartCoroutine(RumbleControllerCoroutine(duration, leftIntensity, rightIntensity));
	}

	private void StopRumbleController()
	{
		GamePad.SetVibration(playerIndex, 0f, 0f);
	}

	private IEnumerator RumbleControllerCoroutine(float duration, float leftIntensity = 1f, float rightIntensity = 1f)
	{
		GamePad.SetVibration(playerIndex, leftIntensity, rightIntensity);
		yield return new WaitForSecondsRealtime(duration);
		StopRumbleController();
	}

	/// <summary>
	/// Turns off given movement axes
	/// </summary>
	/// <param name="duration"> How long controls will be restricted </param>
	/// <param name="movementAxis"> MovementAxis to restrict </param>
	public void RestrictControlsForSeconds(float duration, int movementAxis)
	{
		StartCoroutine(ResetMovementAxis(duration, movementAxis, m_movementAxes));
	}

	private IEnumerator ResetMovementAxis(float duration, int movementAxis, int original)
	{
        if (gameObject.activeSelf)
		    m_movementAxes = ~movementAxis;
		yield return new WaitForSeconds(duration);
        do
        {
            m_movementAxes = original;
        } while (!gameObject.activeSelf);
	}

	public virtual void ResetCharacter()
	{
		CanMove = true;
		CanRotate = true;

		ResetDamage();
		ResetMaterials();
		ResetVulernability();

		playerState = PlayerState.ALIVE;
		SetHealth(m_maxHealth);

		m_animator.SetFloat("MovX", 0f);
		m_animator.SetFloat("MovZ", 0f);
		m_animator.SetBool("Attack", false);
	}
}
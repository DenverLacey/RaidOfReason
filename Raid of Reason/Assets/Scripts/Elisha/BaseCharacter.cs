using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

/* 
 * Author: Elisha, Denver, Afridi
 * Description: Abstract base class that will be derived by character classes Kenron, Nashorn and Thea. Handles everything to do 
 *              with the characters movements and states.
 */

[RequireComponent(typeof(Rigidbody))]
public abstract class BaseCharacter : MonoBehaviour
{
    public Character CharacterType { get; protected set; }

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
        DEAD
    }

    public PlayerState playerState;
    public float m_maxHealth;
    public float m_currentHealth;
    protected Rigidbody m_rigidbody;

    // Damage Dealt
    private float m_damage;

    [SerializeField]
    [Tooltip("How much damage will the player deal?")]
    protected float m_minDamage;

    [SerializeField]
    protected float m_mincurrentDamage;

    [SerializeField]
    [Tooltip("How much damage will the player deal?")]
    protected float m_maxDamage;

    [SerializeField]
    protected float m_maxcurrentDamage;

    [SerializeField]
    [Tooltip("Hpw fast will the player move?")]
    protected float m_movementSpeed;

    [Tooltip("Pick what controller this player is.")]
    public XboxController controller;

    // Player Colliders
    private CapsuleCollider m_playerCollider;

    public List<Image> m_skillPopups = new List<Image>();

    [Tooltip("The Skill Manager that manages the skills of the players")]
    public SkillManager skillManager;

    [Tooltip("A List of How Many Skill Upgrades the Players Have")]
    public List<SkillsAbilities> m_skillUpgrades = new List<SkillsAbilities>();

    [Tooltip("How much max shield can the player get?")]
    public float m_maxShield;

    public float currentShield;

    [SerializeField]
    [Tooltip("How long will it take for the shield to start degenerating?")]
    private float m_shieldBuffer;

    [SerializeField]
    [Tooltip("How long will it take to degenerate shield?")]
    private float m_shieldDegenerationTimer;

    [SerializeField]
    [Tooltip("How much shield degenerates?")]
    private float m_shieldDegenerateAmount;

    [SerializeField]
    [Tooltip("The particle that spawns on death")]
    private ParticleSystem m_deathParticle;

    [HideInInspector]
    public bool m_controllerOn;
	private int movementAxes = MovementAxis.Move | MovementAxis.Rotate;

	public bool CanMove
	{
		get => (movementAxes & MovementAxis.Move) == MovementAxis.Move;
		set => movementAxes = value ? movementAxes | MovementAxis.Move : movementAxes & ~MovementAxis.Move;
	}

	public bool CanRotate
	{
		get => (movementAxes & MovementAxis.Rotate) == MovementAxis.Rotate;
		set => movementAxes = value ? movementAxes | MovementAxis.Rotate : movementAxes & ~MovementAxis.Rotate;
	}

	[HideInInspector]
    public MultiTargetCamera m_camera;

    protected Animator m_animator;
   
    protected float m_vulnerability;
    protected bool m_bActive;
    protected float m_rotationSpeed = 250.0f;
    private Vector3 m_direction;
    protected Vector3 m_prevRotDirection = Vector3.forward;
    private MeshRenderer m_renderer;
    private Color m_originalColour;


    private Color m_original;
    private SpriteRenderer m_spriteRend;


    /// <summary>
    /// This will be called first.
    /// </summary>
    protected virtual void Awake()
    {
       // m_renderer = GetComponentInChildren<MeshRenderer>();
        // Gets the original colour of the player.
        //m_originalColour = m_renderer.sharedMaterial.color;
        m_camera = FindObjectOfType<MultiTargetCamera>();
        m_playerCollider = GetComponent<CapsuleCollider>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponentInChildren<Animator>();
        m_currentHealth = m_maxHealth;
        m_mincurrentDamage = m_minDamage;
        m_maxcurrentDamage = m_maxDamage;
        m_vulnerability = 1.0f;
        m_controllerOn = true;
        m_bActive = false;
        currentShield = 0;

        m_spriteRend = GetComponentInChildren<SpriteRenderer>();
        m_original = m_spriteRend.sharedMaterial.color;
    }

    /// <summary>
    /// Physics update.
    /// </summary>
    protected virtual void FixedUpdate()
    {
        // Checks the states of all derived players of this class.
		switch (playerState)
		{
			case PlayerState.ALIVE:
                // Call this.
                m_playerCollider.enabled = true;
				CharacterMovement();
                break;

			case PlayerState.DEAD:
                // Player gets removed from camera array.
                if (m_camera.targets.Count > 0)
                {
                    m_camera.targets.Remove(transform);
                }
                gameObject.SetActive(false);
                Destroy(gameObject);
                Destroy(gameObject);
                break;

			default:
                Debug.Log("default");
				break;
		}
	}

    /// <summary>
    /// Updates every frame.
    /// </summary>
    protected virtual void Update()
    {
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
    }

    /// <summary>
    /// Handles the main movement of each derived character using Xinput.
    /// </summary>
    virtual protected void CharacterMovement() 
    {
        // Checks if player is active.
        if (m_controllerOn)
        {
			// Calculates the x axis of the left stick (left and right movement).
			float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller) * m_movementSpeed;
			// Calculates the z axis of the left stick (forward and backward movement).
			float axisZ = XCI.GetAxis(XboxAxis.LeftStickY, controller) * m_movementSpeed;
			// Makes sure Player movement is relative to the direction of the cameras forward.
			Vector3 movement = m_camera.transform.TransformDirection(axisX, 0, axisZ);

			if (CanMove)
			{
				m_rigidbody.MovePosition(transform.position + new Vector3(movement.x, 0, movement.z) * Time.deltaTime);
			}

			// Calculates the rotation on the x axis of the right stick.
			float rotAxisX = XCI.GetAxis(XboxAxis.RightStickX, controller) * m_rotationSpeed;
			// Calculates the rotation on the z axis of the right stick.
			float rotAxisZ = XCI.GetAxis(XboxAxis.RightStickY, controller) * m_rotationSpeed;
			// Makes sure Player rotation is relative to the direction of the cameras forward.
			Vector3 rawDir = m_camera.transform.TransformDirection(rotAxisX, 0, rotAxisZ);
			Vector3 direction = new Vector3(rawDir.x, 0, rawDir.z);
			// Checks if the magnitude of the vector is less than 0.1
			if (direction.magnitude < 0.1f)
			{
				// Change direction of the vector.
				direction = m_prevRotDirection;
			}
			// Normalise the vector to 1.
			direction.Normalize();
			// New direction
			m_prevRotDirection = direction;

			if (CanRotate)
			{
				// Rotate the player to that direction.
				transform.localRotation = Quaternion.LookRotation(direction);
			}

			if (m_animator)
			{
				// Calculate angle between character's direction and forward
				float angle = Vector3.SignedAngle(direction, Vector3.forward, Vector3.up);

				// Rotate movement into world space to get animation movement
				Vector3 animationMovement = Quaternion.AngleAxis(angle, Vector3.up) * movement.normalized;

				// flip x movement if walking backwards
				animationMovement.x *= animationMovement.z >= 0 ? 1 : -1;

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
        if (this.gameObject.tag == "Nashorn")
        {
            GameManager.Instance.Nashorn.m_statManager.damageTaken += damage;
            GameManager.Instance.Nashorn.m_statManager.totalSheildsCharged += GameManager.Instance.Nashorn.currentShield;
        }

        if (currentShield > 0)
        {
            currentShield -= damage * m_vulnerability;
            IndicateHit();
        }

        if (currentShield <= 0)
        {
            // Take an amount of damage from the players current health.
            m_currentHealth -= damage * m_vulnerability;
            // Player damage indicator.
            IndicateHit();
        }

        // If player has no health.
        if (m_currentHealth <= 0.0f)
        {
            playerState = PlayerState.DEAD;
        }
    } 

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
    /// Gets Nashorns vulnerability status.
    /// </summary>
    /// <returns> float value. </returns>
    public float GetVulnerability() {
        return m_vulnerability;
    }

    /// <summary>
    /// Sets Nashorns vulnerability status.
    /// </summary>
    /// <param name="vulnerability"></param>
    public void SetVulnerability(float vulnerability) {
        m_vulnerability = vulnerability;
    }

    /// <summary>
    /// Resets Nashorns vulnerability to 1.
    /// </summary>
    public void ResetVulernability() {
        m_vulnerability = 1.0f;
    }

    /// <summary>
    /// A corotine that resets the players mesh colour back to normal when called.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="delay"></param>
    /// <returns> Gameobject and a float value. </returns>
    IEnumerator ResetMaterialColour(GameObject player, float delay)
    {
        // Suspends the coroutine execution for the given amount of seconds.
        yield return new WaitForSeconds(delay);

        // If player gets returned.
        if (player)
        {
            // Change players mesh colour back to the original colour.
            player.GetComponent<MeshRenderer>().material.color = Color.clear;
        }
    }

    /// <summary>
    /// Changes colour to red when player is hit.
    /// </summary>
    void IndicateHit()
    {
        m_spriteRend.material.color = Color.red;
        StartCoroutine(ResetSpriteColour(.2f));
    }

    /// <summary>
    /// Resets colour.
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    //IEnumerator ResetColour(float duration)
    //{
    //    yield return new WaitForSeconds(duration);
    //    m_renderer.material.color = m_originalColour;
    //}

    IEnumerator ResetSpriteColour(float duration)
    {
        yield return new WaitForSeconds(duration);
        m_spriteRend.material.color = m_original;
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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using TMPro;

/* 
 * Author: Elisha_Anagnostakis, Denver_Lacey, Afridi_Rahim
 * Description: Abstract base class that will be derived by character classes Kenron, Nashorn and Theá. Handles everything to do 
 *              with the characters movements and states.
 */

[RequireComponent(typeof(Animator))]
public abstract class BaseCharacter : MonoBehaviour {

    public enum PlayerState
    {
        ALIVE,
        REVIVE,
        DEAD
    }

    [SerializeField]
    private PlayerState m_playerStates;
    [SerializeField]
    protected float m_damage;
    [SerializeField]
    private float m_movementSpeed;
    [SerializeField]
    public int m_playerSkillPoints;
    public List<SkillsAbilities> m_playerSkills = new List<SkillsAbilities>();
    [SerializeField]
    public Dictionary<string, SkillsAbilities> m_dictSkills = new Dictionary<string, SkillsAbilities>();

    [HideInInspector]
    public bool m_controllerOn;
    public float m_maxHealth;
    public float m_currentHealth;
    public XboxController m_controller;
    public MultiTargetCamera m_camera;
    public TextMeshProUGUI m_skillDisplay;
    public TextMeshProUGUI m_skillMaxed;

    // a multiplier for damage taken
    protected float m_vulnerability;
    private float m_rotationSpeed = 250.0f;
    private Vector3 m_direction;
    private Vector3 m_prevRotDirection = Vector3.forward;
    protected bool m_bActive;
    protected Animator m_animator;

    public ReviveAlly reviveAlly;
    public float m_timeTillDeath;
    public float m_timeToRevive;

    public int SkillPoints {
        get { return m_playerSkillPoints;  }
        set {
            m_playerSkillPoints = value;
        }
    }

    /// <summary>
    /// This will be called first.
    /// </summary>
    protected virtual void Awake () {
        m_currentHealth = m_maxHealth;
        m_vulnerability = 1.0f;
        m_bActive = false;
        m_camera = FindObjectOfType<MultiTargetCamera>();
        m_animator = GetComponent<Animator>();
        m_controllerOn = true;
    }

    /// <summary>
    /// Physics update.
    /// </summary>
    protected virtual void FixedUpdate()
    {
        // Checks the states of all derived players of this class.
		switch (m_playerStates)
		{
			case PlayerState.ALIVE:
                // If a controller is active.
                if (XCI.IsPluggedIn(XboxController.Any))
                {
                    // Call this.
                    CharacterMovement();
                }
                else {
                    PCCharacterMovement();
                }
				break;
			case PlayerState.REVIVE:
				Debug.Log("revive state activated");
                // TODO: What will happen within the revive state.
				break;
			case PlayerState.DEAD:
				Debug.Log("dead state activated");
                // TODO: What will happen in the dead state.
				break;

			default:
                Debug.Log("default");
				break;
		}
        m_skillDisplay.text = m_playerSkillPoints.ToString();
	}

    /// <summary>
    /// Updates every frame.
    /// </summary>
    protected virtual void Update() {
        if (m_playerSkills.Count == 4)
        {
            m_skillDisplay.gameObject.SetActive(false);
            m_skillMaxed.gameObject.SetActive(true);
        }

        // Makes sure health doesnt exceed limit.
        if (m_currentHealth >= m_maxHealth)
            m_currentHealth = m_maxHealth;
    }

    /// <summary>
    /// Handles the main movement of each derived character using Xinput.
    /// </summary>
    virtual protected void CharacterMovement() {

        // Checks if player is active.
        if (m_controllerOn)
        {
            // Calculates the x axis of the left stick (left and right movement).
            float axisX = XCI.GetAxis(XboxAxis.LeftStickX, m_controller) * m_movementSpeed;
            // Calculates the z axis of the left stick (forward and backward movement).
            float axisZ = XCI.GetAxis(XboxAxis.LeftStickY, m_controller) * m_movementSpeed;
            // Makes sure Player movement is relative to the direction of the cameras forward.
            Vector3 movement = m_camera.transform.TransformDirection(axisX, 0, axisZ);
            transform.position += new Vector3(movement.x, 0, movement.z) * Time.deltaTime;

            // Calculates the rotation on the x axis of the right stick.
            float rotAxisX = XCI.GetAxis(XboxAxis.RightStickX, m_controller) * m_rotationSpeed;
            // Calculates the rotation on the z axis of the right stick.
            float rotAxisZ = XCI.GetAxis(XboxAxis.RightStickY, m_controller) * m_rotationSpeed;
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
            // Rotate the player to that direction.
            transform.localRotation = Quaternion.LookRotation(direction);

            if (m_animator)
            {
                // Calculate angle between character's direction and forward
                float angle = Vector3.SignedAngle(direction, Vector3.forward, Vector3.up);

                // Rotate movement into world space to get animation movement
                Vector3 animationMovement = Quaternion.AngleAxis(angle, Vector3.up) * movement.normalized;

                // Set animator's movement floats
                m_animator.SetFloat("MovX", animationMovement.x);
                m_animator.SetFloat("MovZ", animationMovement.z);
                m_animator.SetFloat("Speed", movement.magnitude);
            }
        }
    }


    virtual protected void PCCharacterMovement() {
        // TODO: Needs work according to Afridis comment.
    }

    /// <summary>
    /// This allows the player to take damage.
    /// </summary>
    /// <param name="damage"></param>
    public virtual void TakeDamage(float damage)
    {
        // Take an amount of damage from the players current health.
        m_currentHealth -= damage * m_vulnerability;
        // Change the players mesh colour to red.
        GetComponent<MeshRenderer>().material.color = Color.red;
        // Change the player back to their original mesh colour after .2 seconds of being hit.
        StartCoroutine(ResetMaterialColour(this.gameObject, .2f));

        // If player has no health.
        if (m_currentHealth <= 0.0f)
        {
            // Player revive state.
            OnDeath();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// This gets called when player has no health. Allows the player to be revived by an ally.
    /// </summary>
    protected virtual void OnDeath()
    {
        // TODO: Player enters revive state.
    }

    /// <summary>
    /// Sets damage to a float value.
    /// </summary>
    /// <param name="damage"></param>
    virtual public void SetDamage(float damage)
    {
        m_damage = damage;
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
        return m_damage;
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

    public delegate void OnPointChange();
    public event OnPointChange onPointChange;

    /// <summary>
    /// Updates players skill points amounts.
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateSkillPont(int amount) {
        m_playerSkillPoints += amount;
    }

    /// <summary>
    /// Allows the user to access player states in other scripts.
    /// </summary>
    /// <param name="alive"></param>
    /// <param name="revive"></param>
    /// <param name="dead"></param>
    public void SetPlayerState(PlayerState alive, PlayerState revive, PlayerState dead)
    {
        // TODO: give access to the states from other scripts.
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
}
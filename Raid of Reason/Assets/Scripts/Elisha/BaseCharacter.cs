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

    public PlayerState playerState;

    public float m_maxHealth;
    public float m_currentHealth;

    [SerializeField]
    [Tooltip("How long can a player be in revive state before they die?")]
    private float m_deathTimer;

    [SerializeField]
    [Tooltip("How much damage will the player deal?")]
    protected float m_damage;

    [SerializeField]
    [Tooltip("Hpw fast will the player move?")]
    protected float m_movementSpeed;

    [Tooltip("Pick what controller this player is.")]
    public XboxController m_controller;

    [SerializeField]
    [Tooltip("How long will it take the player to revive his teammate?")]
    private float m_reviveTimer;

    [SerializeField]
    [Tooltip("How big is the radius of the revive?")]
    private float m_reviveRadius;

    [SerializeField]
    [Tooltip("How much health will the player get back when revived?")]
    private float m_healthUponRevive;

    [Space]

    [SerializeField]
    public int m_playerSkillPoints;

    public List<SkillsAbilities> m_playerSkills = new List<SkillsAbilities>();
    [Space]
    public Dictionary<string, SkillsAbilities> m_dictSkills = new Dictionary<string, SkillsAbilities>();

    [HideInInspector]
    public bool m_controllerOn;
    public MultiTargetCamera m_camera;
    public TextMeshProUGUI m_skillDisplay;
    public TextMeshProUGUI m_skillMaxed;
    protected Animator m_animator;
    public SphereCollider m_reviveColliderRadius;
   
    protected float m_vulnerability;
    protected bool m_bActive;

    protected float m_rotationSpeed = 250.0f;
    private Vector3 m_direction;
    protected Vector3 m_prevRotDirection = Vector3.forward;
    private bool m_isRevived = false;
    private bool m_isReviving = false;

    public GameObject reviveParticle;


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

        m_reviveColliderRadius.enabled = false;
        m_reviveTimer = 5f;
        reviveParticle.transform.position = this.transform.position;
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
                CharacterMovement();
				break;
			case PlayerState.REVIVE:
                ReviveTeamMate();
				break;
			case PlayerState.DEAD:
                // Player gets removed from camera array.
                if (m_camera.m_targets.Count > 0)
                    m_camera.m_targets.Remove(this.gameObject.transform);
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
        }
    }

    /// <summary>
    /// Checks if the downed player is in REVIVE state which then gives allies a chance to revive them.
    /// </summary>
    public void ReviveTeamMate()
    {
        // Cycles through every instance of the players in the game.
        foreach (var player in GameManager.Instance.Players)
        {
            // Checks for the 2 other players that are not down.
            if (player == this) { continue; }
            // Calculates the distance between the downed player and the other players.
            float sqrDistance = (player.transform.position - this.transform.position).sqrMagnitude;

            // If the player is in a downed state.
            if (this.playerState == PlayerState.REVIVE)
            {
                // Enable thier revive collider.
                m_reviveColliderRadius.enabled = true;
                // Have the colliders radius equal the AOE float radius.
                m_reviveColliderRadius.radius = m_reviveRadius;

                // Checks to see if player isnt getting revived.
                if (!m_isReviving)
                {
                    // Starts the timer that the player can be in revive state for.
                    m_deathTimer -= Time.deltaTime;
                }

                // If the timer is 0 and the player hasnt been revived.
                if(m_deathTimer <= 0 && !m_isRevived)
                {
                    // Kill the player 
                    this.playerState = PlayerState.DEAD;
                    this.gameObject.SetActive(false);
                }

                // Checks if the 2 players that are alive are within the revive distance.
                if (sqrDistance <= m_reviveRadius * m_reviveRadius)
                {
                    // TODO: Show in UI the player can hold B to revive
                    Debug.Log("Hold B to revive");

                    // Checks if the player holds the B button
                    if (XCI.GetButton(XboxButton.B, player.m_controller))
                    {
                        // Means they are reviving the player.
                        m_isReviving = true;

                        // If thats true.
                        if (m_isReviving)
                        {
                            // Start the revive timer.
                            m_reviveTimer -= Time.deltaTime;

                            // TODO: Start revive particle effect.
                            // If the revive timer hits 0 and the player is reviving.
                            if (m_reviveTimer <= 0f && m_isReviving)
                            {
                                // Player revived.
                                m_isRevived = true;
                                m_isReviving = false;
                                reviveParticle.SetActive(false);

                                // Change them to the ALIVE state.
                                playerState = PlayerState.ALIVE;
                                // Give the player some health when they get back up.
                                this.m_currentHealth = m_healthUponRevive;
                                // TODO: Stop revive particle effect.
                                // Reset timer.
                                m_reviveTimer = 5f;
                                // Disable the revive collider.
                                m_reviveColliderRadius.enabled = false;
                            }
                        }
                    }
                    // If the user lets go of the revive button, reset the timer for reviving.
                    else if(XCI.GetButtonUp(XboxButton.B, player.m_controller))
                    {
                        m_reviveTimer = 5f;
                    }
                }
                else
                {
                    m_isReviving = false;
                }
            }
        }
    }

    /// <summary>
    /// This gets called when player has no health. Allows the player to be revived by an ally.
    /// </summary>
    protected virtual void OnDeath()
    {
        playerState = PlayerState.REVIVE;
        transform.Rotate(90, 0, 0, Space.Self);
        reviveParticle.SetActive(true);
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
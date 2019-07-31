using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

/*
  * Author: Afridi Rahim
  *
  * Summary: The Stats and Management of Nashorns Core Mechanics.
  *          Manages his abilites and his skill tree as he improves within the
  *          game
*/

public class Nashorn : BaseCharacter
{
    [Tooltip("The Two Gauntlets That Nashorn Attacks With")]
    public List<GameObject> Gauntlets;

    [Tooltip("The Collider of Nashorns Left Gauntlet")]
    public Collider LeftGauntlet;

    [Tooltip("The Collider of Nashorns Right Gauntlet")]
    public Collider RightGauntlet;

    [Tooltip("The Skill Manager that manages the skills of the players")]
    public SkillManager skillManager;

    [Tooltip("Checks if Nashorns Skill is Active")]
    public bool isTaunting;

    [SerializeField]
    [Tooltip("Size of the area of effect for taunt ability")]
    private float m_tauntRadius;

    [SerializeField]
    [Tooltip("How vulnerable Nashorn is while taunting (1.0 is default)")]
    private float m_tauntVulnerability;

    [SerializeField]
    [Tooltip("Particle Effect That Appears When Nashorn Taunts")]
    private GameObject m_tauntParticle;

    [SerializeField]
    [Tooltip("Particle Effect That Appears When Nashorn Taunts")]
    private GameObject m_gauntletParticle;

    [SerializeField]
    [Tooltip("Increased Radius from Nashorns Roaring Thunder ability")]
    private float m_radiusIncreased;

    // Amount of Nearby Enemies near Nashorn
    private List<BaseEnemy> m_nearbyEnemies = new List<BaseEnemy>();

    // Swaps Between Left and Right Gauntlets: 0 = Left / 1 = Right
    private uint u_gauntletIndex = 0;

    // Nashorns Rigidbody
    private Rigidbody m_nashornRigidBody;

    // Empty Object for particle instantiating
    private GameObject particleInstantiate;

    protected override void Awake()
    {
        // Initialisation 
        base.Awake();
        m_nashornRigidBody = GetComponent<Rigidbody>();
        LeftGauntlet.enabled = false;
        RightGauntlet.enabled = false;
        isTaunting = false;

        for (int i = 0; i < 2; i++)
        {
            // Ignores the collider on the player
            Physics.IgnoreCollision(GetComponent<Collider>(), 
                                    Gauntlets[i].GetComponent<Collider>(), 
                                    true);
        }
    }

    protected override void FixedUpdate()
    {
        // Empty Check
        if (this.gameObject != null)
        {
            // Updates Player Movement
            base.FixedUpdate();

            // Checks Nashorns Skill Tree
            SkillChecker();
        }
    }

    protected override void Update()
    {
        // Empty Check
        if (this.gameObject != null)
        {
            // Allows Nashorn to perform Melee Punches 
            Punch();
        }
    }

    /// <summary>
    /// Checks how many skills Nashorn has obtained from his skill tree
    /// - Roaring Thunder: More Range for his Taunt and Cooldown is Halved
    /// - Shock Wave: Melee now knocksback and has a chance to stun (Passive)
    /// - Kinetic Discharge: Enemies now take damage when attacking Taunted Nashorn
    /// - Macht Des Sturms: Chain Lightning attack that damages and stuns + team mates are granted electric damage (Stun Buff)
    /// </summary>
    public void SkillChecker()
    {
        // Empty Check
        if (this.gameObject != null)
        {
            // If the skill is active and the player has the named skill
            if (isTaunting == true && m_playerSkills.Find(skill => skill.Name == "Roaring Thunder"))
            {

                // Returns increased Radius
                this.m_tauntRadius = m_tauntRadius + m_radiusIncreased;

                // Cooldown is halved
                skillManager.m_Skills[1].m_coolDown = skillManager.m_Skills[1].m_coolDown / 2;
            }
        }
    }

    /// <summary>
    /// Nashorns Attack. By Pressing the Right Trigger, Nashorn Becomes Stationery and Punches his foes switching between fists
    /// </summary>
    public void Punch()
    {
        // Empty Check
        if (Gauntlets.Count == 2)
        {
            // If the Triggers has been pressed
            if (XCI.GetAxis(XboxAxis.RightTrigger, m_controller) > 0.1)
            {
                // Gauntlet Colliders are Enabled and Nashorn Becomes Stationary 
                LeftGauntlet.enabled = true;
                RightGauntlet.enabled = true;
                SetSpeed(0.0f);

                // If its the left Gauntlet
                if (u_gauntletIndex == 0)
                {
                    Gauntlets[0].gameObject.transform.localPosition = new Vector3(-0.75f, 0, 0.8f); // Will be Removed

                    // Spawns a particle at the gauntlet that is attacking
                    particleInstantiate = Instantiate(m_gauntletParticle, Gauntlets[0].transform.position + Vector3.forward * (Gauntlets[0].transform.localScale.y / 2), Quaternion.Euler(-180, 0, 0), Gauntlets[0].transform);
                }
                // If its the right Gauntlet
                if (u_gauntletIndex == 1)
                {
                    Gauntlets[1].gameObject.transform.localPosition = new Vector3(0.75f, 0, 0.8f); // Will be Removed
                    
                    // Spawns a particle at the gauntlet that is attacking
                    particleInstantiate = Instantiate(m_gauntletParticle, Gauntlets[1].transform.position + Vector3.forward * (Gauntlets[1].transform.localScale.y / 2), Quaternion.Euler(-180, 0, 0), Gauntlets[1].transform);
                }
            }
            // or if the trigger isnt pressed
            else if (XCI.GetAxis(XboxAxis.RightTrigger, m_controller) < 0.1)
            {
                // Disable colliders and reset speed
                LeftGauntlet.enabled = false;
                RightGauntlet.enabled = false;
                SetSpeed(10.0f);

                // If its the left Gauntlet
                if (u_gauntletIndex == 0)
                {
                    Gauntlets[0].gameObject.transform.localPosition = new Vector3(-0.75f, 0, 0.0f); // Will be Removed
                    
                    // Sets the index and destroys the particle
                    u_gauntletIndex = 1;
                    Destroy(particleInstantiate);
                }
                // If its the right Gauntlet
                if (u_gauntletIndex == 1)
                {
                    Gauntlets[1].gameObject.transform.localPosition = new Vector3(0.75f, 0, 0.0f); // Will be Removed

                    // Sets the index and destroys the particle
                    u_gauntletIndex = 0;
                    Destroy(particleInstantiate);
                }
            }
        }
    }

    /// <summary>
	/// Nashorn's Ability. Boosting His Health up and reducing incoming damage he taunts all enemies to himself
	/// </summary>
    public void Spott()
    {
		// Ability is active
		isTaunting = true;

		// set vulnerability
		m_vulnerability = m_tauntVulnerability;
    }

    /// <summary>
    /// Resets Nashorns Stats back to his base after Spott is Used
    /// </summary>
    public void ResetSkill()
    {
        // Vulnerable once more
        ResetVulernability();

        // Skill no longer active
        isTaunting = false;

        // All Enemies are un-taunted
        foreach (BaseEnemy enemy in m_nearbyEnemies)
        {
            if (enemy.State == BaseEnemy.AI_STATE.TAUNTED)
            {
                enemy.StopTaunt();
            }
        }
    }

    /// <summary>
    /// When an Enemy dies, they must be stopped in thier taunt state
    /// </summary>
	protected override void OnDeath() {
        // For any enemies dead, stop taunting
		foreach (BaseEnemy enemy in m_nearbyEnemies)
        {
			if (enemy.State == BaseEnemy.AI_STATE.TAUNTED)
            {
				enemy.StopTaunt();
			}
		}
	}
}

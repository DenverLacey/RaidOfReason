/*
 * Author: Denver
 * Description:	EnemyData class that holds all data for all enemy types and some functions to
 *				alter that state from within the behaviour tree
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public enum EnemyType 
{
    SUICIDE,
    MELEE,
    RANGE,
    SPAWNER
}

[System.Serializable]
public struct EnemyAttackRange 
{
    public float min;
    public float max;
}

/// <summary>
/// Encapsulates all data for all enemy types
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(EnemyPathfinding))]
public class EnemyData : MonoBehaviour 
{
    [SerializeField] 
	private EnemyType m_type;

    [SerializeField]
    private GameObject m_tauntIcon;

    public EnemyType Type { get => m_type; }
    public float ViewRange { get; private set; }
    public float MaxHealth { get; private set; }
    public float Health { get; private set; }
    public EnemyAttackRange AttackRange { get; private set; }
    public float AttackCooldown { get; private set; }
    public float AttackTimer { get; set; }
	public float AttackDamage { get; private set; }
    public bool Attacking { get; set; }
	public bool Taunted { get; set; }
	public bool Stunned { get; set; }
	public GameObject[] AttackPrefabs { get; private set; }
    public GameObject TauntIcon { get => m_tauntIcon; }

    public Vector3 Target { get; set; }
	public BaseCharacter TargetPlayer { get; set; }

	public Behaviour PendingBehaviour { get; set; }

	public Rigidbody Rigidbody { get; private set; }

	[SerializeField]
	[Tooltip("Renderer of Enemy's mesh")]
	private Renderer m_renderer;

	public Renderer Renderer { get => m_renderer; }

	[SerializeField]
	[Tooltip("Animator Controller component of enemy")]
	private Animator m_animator;

	[HideInInspector]
    // Afridi added this for the skill tree
    public bool isDeadbByKenron = false;

    [HideInInspector]
    // Afridi added this for the skill tree
    public bool isAttackingNashorn = true;

	private Collider m_collider;

	// Pathfinding Stuff
	public EnemyPathfinding Pathfinder { get; private set; }

	private void Awake()
	{
		Rigidbody = GetComponent<Rigidbody>();
		m_collider = GetComponent<Collider>();

		Pathfinder = GetComponent<EnemyPathfinding>();
    }

	/// <summary>
	/// Initialises the enemy's values
	/// </summary>
	/// <param name="viewRange">
	/// How far away enemy can see player
	/// </param>
	/// <param name="maxHealth">
	/// Maximum health of enemy
	/// </param>
	/// <param name="attackRange">
	/// Distance range in which enemy will attack
	/// </param>
	/// <param name="attackCooldown">
	/// How often enemy will attack
	/// </param>
	/// <param name="attackDamage">
	/// How much damage enemy's attack deals
	/// </param>
	/// <param name="attackPrefabs">
	/// effect object of enemy's attack
	/// </param>
	/// <param name="players">
	/// References to player objects
	/// </param>
	public void Init(float viewRange, float maxHealth, EnemyAttackRange attackRange, float attackCooldown, float attackDamage, GameObject[] attackPrefabs)
    {
        ViewRange = viewRange;
        MaxHealth = maxHealth;
        Health = MaxHealth;
        AttackRange = attackRange;
        AttackCooldown = attackCooldown;
        AttackTimer = 0.0f;
		AttackDamage = attackDamage;
		AttackPrefabs = attackPrefabs;
		Stunned = false;
    }

	/// <summary>
	/// Stuns enemy for a given amount of seconds
	/// </summary>
	/// <param name="duration">
	/// How long to stun enemy for in seconds
	/// </param>
	public void Stun(float duration) 
	{
		Stunned = true;
		StartCoroutine(ResetStun(duration));
	}

	/// <summary>
	/// Unstuns the enemy once a given amount of seconds has elapsed
	/// </summary>
	/// <param name="duration">
	/// How long to wait before unstunning the enemy in seconds
	/// </param>
	/// <returns>
	/// A WaitForSeconds
	/// </returns>
	IEnumerator ResetStun(float duration) 
	{
		yield return new WaitForSeconds(duration);
		Stunned = false;
	}

	/// <summary>
	/// Decreases enemy's health by an amount
	/// </summary>
	/// <param name="damage">
	/// How much to decrease enemy's health
	/// </param>
	public void TakeDamage(float damage)
	{
		Health -= damage;

		IndicateHit();

		if (Health <= 0)
		{
			Die();
		}
	}

	/// <summary>
	/// Changes enemy's renderer's material colour to red for .2 seconds
	/// </summary>
	public void IndicateHit()
	{
		Renderer.material.color = Color.red;
		StartCoroutine(ResetColour(.2f));
	}

	IEnumerator ResetColour(float duration)
	{
		yield return new WaitForSeconds(duration);
		Renderer.material.color = Color.clear;
	}

    IEnumerator DissolveLerp(float time)
    {
        yield return new WaitForEndOfFrame();
        float lerp = Mathf.Lerp(Renderer.material.GetFloat("_DissolveAmount"), 1f, time * Time.deltaTime);
        Renderer.material.SetFloat("_DissolveAmount", lerp);
        if(lerp > 0.98f)
        {
            Destroy(gameObject);
        }
    }

	/// <summary>
	/// Performs all functionality involved with an enemy death
	/// </summary>
	public void Die()
	{
        StartCoroutine(DissolveLerp(1));
	}

	/// <summary>
	/// Executes enemy's pending behaviour
	/// </summary>
	/// <returns>
	/// The result of the behaviour
	/// </returns>
	public Behaviour.Result ExecutePendingBehaviour()
	{
		return PendingBehaviour.Execute(this);
	}

	public void SetAnimatorFloat(string id, float value)
	{
		if (m_animator)
		{
			m_animator.SetFloat(id, value);
		}
	}

	public void SetAnimatorTrigger(string id)
	{
		if (m_animator)
		{
			m_animator.SetTrigger(id);
		}
	}
}

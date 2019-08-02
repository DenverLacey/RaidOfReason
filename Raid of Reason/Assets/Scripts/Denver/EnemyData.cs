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
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyData : MonoBehaviour 
{
    [SerializeField] 
	private EnemyType m_type;

	public EnemyType Type { get => m_type; }
    public float ViewRange { get; private set; }
    public int MaxHealth { get; private set; }
    public int Health { get; private set; }
    public EnemyAttackRange AttackRange { get; private set; }
    public float AttackCooldown { get; private set; }
    public float AttackTimer { get; set; }
	public int AttackDamage { get; private set; }
    public bool Attacking { get; set; }
	public bool Taunted { get; set; }
	public bool Stunned { get; set; }
	public GameObject[] AttackPrefabs { get; private set; }

    public Vector3 Target { get; set; }

    public NavMeshAgent NavMeshAgent { get; private set; }
	public Vector3 PendingDestination { get; set; }

	public Rigidbody Rigidbody { get; private set; }

	[HideInInspector]
    // Afridi added this for the skill tree
    public bool isDeadbByKenron = false;

	private MeshRenderer m_renderer;

	private void Start()
	{
		m_renderer = GetComponent<MeshRenderer>();
		Rigidbody = GetComponent<Rigidbody>();
		NavMeshAgent = GetComponent<NavMeshAgent>();
		NavMeshAgent.destination = transform.position;
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
	public void Init(float viewRange, int maxHealth, EnemyAttackRange attackRange, float attackCooldown, int attackDamage, GameObject[] attackPrefabs)
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
	public void TakeDamage(int damage)
	{
		Health -= damage;

		IndicateHit();

		if (Health <= 0)
		{
			OnDeath();
		}
	}

	void IndicateHit()
	{
		m_renderer.material.color = Color.red;
		StartCoroutine(ResetColour(.2f));
	}

	IEnumerator ResetColour(float duration)
	{
		yield return new WaitForSeconds(duration);
		m_renderer.material.color = Color.clear;
	}

	/// <summary>
	/// Destroys the enemy's GameObject
	/// </summary>
	public void OnDeath()
	{
		Destroy(gameObject);
	}

	private void Update()
	{
		Debug.DrawLine(transform.position, NavMeshAgent.destination, Color.green, Time.deltaTime);
	}
}

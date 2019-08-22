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

	public bool ManualSteering { get; set; }

	public Rigidbody Rigidbody { get; private set; }

	public MeshRenderer Renderer { get; private set; }

	[HideInInspector]
    // Afridi added this for the skill tree
    public bool isDeadbByKenron = false;

    [HideInInspector]
    // Afridi added this for the skill tree
    public bool isAttackingNashorn = true;

	private NavMeshPath m_path;
	private bool m_pathing;
	private int m_currentCorner;
	private float m_speed = 3f;
	private float m_steeringSpeed = 10f;
	private Collider m_collider;
	private Vector3 m_currentDestination;

	private void Start()
	{
		Rigidbody = GetComponent<Rigidbody>();
        Renderer = GetComponent<MeshRenderer>();
		m_collider = GetComponent<Collider>();

		m_path = new NavMeshPath();
		NavMesh.CalculatePath(transform.position, transform.position, NavMesh.AllAreas, m_path);

        if (!Renderer)
        {
            Renderer = GetComponentInChildren<MeshRenderer>();
        }
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
			OnDeath();
		}
	}

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

	/// <summary>
	/// Destroys the enemy's GameObject
	/// </summary>
	public void OnDeath()
	{
		Destroy(gameObject);
	}

	private void Update()
	{
		if (m_path.corners.Length == 0)
		{
			return;
		}

		// move to destination
		if (!Stunned && m_pathing && m_currentCorner != m_path.corners.Length)
		{
			Vector3 currentTarget = m_path.corners[m_currentCorner];
			currentTarget.y = transform.position.y;

			// draw debug line that follows path
			Vector3 currentPosition = transform.position;
			currentPosition.y = 0.1f;
			Debug.DrawLine(currentPosition, m_path.corners[m_currentCorner], Color.green);

			for (int i = m_currentCorner + 1; i < m_path.corners.Length; i++)
			{
				Debug.DrawLine(m_path.corners[i - 1], m_path.corners[i], Color.green);
			}

			// calculate movement vector
			Vector3 movementVector = (currentTarget - transform.position).normalized;
			Rigidbody.MovePosition(transform.position + movementVector * m_speed * Time.deltaTime);

			// if reached current corner, move to next
			if (AtCorner(m_path.corners[m_currentCorner])) 
			{
				m_currentCorner++;
			}

			// do steering
			if (!ManualSteering)
			{
				Quaternion desiredRotation = Quaternion.LookRotation(movementVector);
				transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, m_steeringSpeed * Time.deltaTime);
			}
		}
	}

	public void SetDestination(Vector3 destination)
	{
		destination.y = 0f;

		if (!AtCorner(destination))
		{
			m_pathing = true;
			NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, m_path);
			m_currentCorner = 1;
		}
		else
		{
			StopPathing();
		}
	}

	public void StopPathing()
	{
		NavMesh.CalculatePath(transform.position, transform.position, NavMesh.AllAreas, m_path);
		m_currentCorner = 1;
		m_pathing = false;
	}

	public Vector3 GetDestination()
	{
		if (m_path.corners.Length == 0)
		{
			return transform.position;
		}
		else
		{
			Vector3 destination = m_path.corners[m_path.corners.Length - 1];
			destination.y = transform.position.y;
			return destination;
		}
	}

	private bool AtCorner(Vector3 corner)
	{
		Vector3 difference = corner - transform.position;
		float sqrDistancX = difference.sqrMagnitude - difference.y * difference.y;
		return sqrDistancX <= 0.2f;
	}

	public Behaviour.Result ExecutePendingBehaviour()
	{
		return PendingBehaviour.Execute(this);
	}
}

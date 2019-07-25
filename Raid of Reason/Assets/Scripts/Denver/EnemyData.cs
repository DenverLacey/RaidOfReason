using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public enum EnemyType {
    SUICIDE,
    MELEE,
    RANGE,
    SPAWNER
}

[System.Serializable]
public struct EnemyAttackRange {
    public float min;
    public float max;
}

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyData : MonoBehaviour {

    [SerializeField] private EnemyType m_type;
    public EnemyType Type { get => m_type; }

    public float ViewRange { get; private set; }
    public int MaxHealth { get; private set; }
    public int Health { get; private set; }
    public EnemyAttackRange AttackRange { get; private set; }
    public float AttackCooldown { get; private set; }
    public float AttackTimer { get; set; }
	public int Damage { get; private set; }
    public bool Attacking { get; set; }
	public bool Taunted { get; set; }
	public bool Stunned { get; set; }
	public GameObject AttackPrefab { get; private set; }
	
    public BaseCharacter[] Players { get; private set; }
    public Vector3 Target { get; set; }

    public NavMeshAgent NavMeshAgent { get; private set; }

    // Afridi added this for the skill tree
    public bool isDeadbByKenron = false;

    /// <sumarry>
    /// Initialises the enemy's values
    /// </sumarry>
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
    public void Init(float viewRange, int maxHealth, EnemyAttackRange attackRange, float attackCooldown, int damage, GameObject attackPrefab, BaseCharacter[] players)
    {
        ViewRange = viewRange;
        MaxHealth = maxHealth;
        Health = MaxHealth;
        AttackRange = attackRange;
        AttackCooldown = attackCooldown;
        AttackTimer = 0.0f;
		Damage = damage;
		AttackPrefab = attackPrefab;
		Stunned = false;
        Players = players;
        NavMeshAgent = GetComponent<NavMeshAgent>();
		NavMeshAgent.destination = Vector3.positiveInfinity;
    }

	public void Stun(float duration) {
		Stunned = true;
		StartCoroutine(ResetStun(duration));
	}

	IEnumerator ResetStun(float duration) {
		yield return new WaitForSeconds(duration);
		Stunned = false;
	}

	/// <summary>
	/// Finds closest point to source on NavMesh
	/// </summary>
	/// <param name="source">
	/// The position the enemy would like to go to
	/// </param>
	/// <returns>
	/// Closest Point to source on NavMesh. Returns source if already on NavMesh. Returns positiveInfinity if none found
	/// </returns>
	public Vector3 FindClosestPoint(Vector3 source) {
		NavMeshHit hit = new NavMeshHit();
		int navMeshArea = NavMesh.GetAreaFromName("Walkable");

		// if source is already on NavMesh
		if (NavMesh.SamplePosition(source, out hit, 1, navMeshArea)) {
			return source;
		}

		if (NavMesh.FindClosestEdge(source, out hit, navMeshArea)) {
			return hit.position;
		}

		return Vector3.positiveInfinity;
	}
}

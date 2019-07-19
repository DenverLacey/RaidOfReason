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
    public float AttackTimer { get; private set; }
    public bool Attacking { get; private set; }

    public NavMeshAgent m_navMeshAgent { get; private set; }

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
    public void Init(float viewRange, int maxHealth, EnemyAttackRange attackRange, float attackCooldown)
    {
        ViewRange = viewRange;

        MaxHealth = maxHealth;
        Health = MaxHealth;

        AttackRange = attackRange;

        AttackCooldown = attackCooldown;
        AttackTimer = 0.0f;

        m_navMeshAgent = GetComponent<NavMeshAgent>();
    }
}

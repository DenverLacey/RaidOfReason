using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Enemy Type View Ranges")]
    [SerializeField] private float m_suicideEnemyViewRange;
    [SerializeField] private float m_meleeEnemyViewRange;
    [SerializeField] private float m_rangeEnemyViewRange;
    [SerializeField] private float m_spawnerEnemyViewRange;

    [Header("Enemy Type Attack Ranges")]
    [SerializeField] private float m_suicideEnemyAttackRange;
    [SerializeField] private float m_meleeEnemyAttackRange;
    [SerializeField] private EnemyAttackRange m_rangeEnemyAttackRange;
    [SerializeField] private EnemyAttackRange m_spawnerEnemyAttackRange;

    [Header("Enemy Type Attack Cooldowns")]
    [SerializeField] private float m_suicideEnemyAttackCooldown;
    [SerializeField] private float m_meleeEnemyAttackCooldown;
    [SerializeField] private float m_rangeEnemyAttackCooldown;
    [SerializeField] private float m_spawnerEnemyAttackCooldown;

    [Header("Enemy Type Max Healths")]
    [SerializeField] private int m_suicideEnemyMaxHealth;
    [SerializeField] private int m_meleeEnemyMaxHealth;
    [SerializeField] private int m_rangeEnemyMaxHealth;
    [SerializeField] private int m_spawnerEnemyMaxHealth;

    [Header("Enemy Type Damages")]
    [SerializeField] private int m_suicideEnemyDamage;
    [SerializeField] private int m_meleeEnemyDamage;
    [SerializeField] private int m_rangeEnemyDamage;

    [Header("Enemy Objects")]
    [SerializeField] private EnemyData[] m_enemies;

    // Start is called before the first frame update
    void Start() {
        InitEnemies();
        InitBehaviourTrees();
    }

    private void InitEnemies() {
        EnemyAttackRange suicideAttackRange;
        suicideAttackRange.min = 0.0f;
        suicideAttackRange.max = m_suicideEnemyAttackRange;

        EnemyAttackRange meleeAttackRange;
        meleeAttackRange.min = 0.0f;
        meleeAttackRange.max = m_meleeEnemyAttackRange;

        foreach (EnemyData enemy in m_enemies) {
            switch (enemy.Type) {
                case EnemyType.SUICIDE:
                    enemy.Init(m_suicideEnemyViewRange, m_suicideEnemyMaxHealth, suicideAttackRange, m_suicideEnemyAttackCooldown);
                    break;
                
                case EnemyType.MELEE:
                    enemy.Init(m_meleeEnemyViewRange, m_meleeEnemyMaxHealth, meleeAttackRange, m_meleeEnemyAttackCooldown);
                    break;

                case EnemyType.RANGE:
                    enemy.Init(m_rangeEnemyViewRange, m_rangeEnemyMaxHealth, m_rangeEnemyAttackRange, m_rangeEnemyAttackCooldown);
                    break;

                case EnemyType.SPAWNER:
                    enemy.Init(m_spawnerEnemyViewRange, m_spawnerEnemyMaxHealth, m_spawnerEnemyAttackRange, m_spawnerEnemyAttackCooldown);
                    break;

                default:
                    Debug.LogError("Failed to initiailise enemy!", this);
                    break;
            }
        }
    }

    private void InitBehaviourTrees() {

    }
}

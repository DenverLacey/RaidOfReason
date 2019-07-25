using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyTypeFloatPair {
    public EnemyType key;
    public float value;
}

[System.Serializable]
public struct EnemyTypeEnemyAttackRangePair {
    public EnemyType key;
    public EnemyAttackRange value;
}

[System.Serializable]
public struct EnemyTypeIntPair {
    public EnemyType key;
    public int value;
}

[System.Serializable]
public struct EnemyTypeBehaviourTreePair {
    public EnemyType key;
    public BehaviourTree value;
}

[System.Serializable]
public struct EnemyTypeGameObjectPair {
	public EnemyType key;
	public GameObject value;
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<EnemyTypeFloatPair> m_viewRanges;
    Dictionary<EnemyType, float> m_viewRangeDict = new Dictionary<EnemyType, float>();

    [SerializeField] private List<EnemyTypeEnemyAttackRangePair> m_attackRanges;
    Dictionary<EnemyType, EnemyAttackRange> m_attackRangeDict = new Dictionary<EnemyType, EnemyAttackRange>();

    [SerializeField] private List<EnemyTypeFloatPair> m_attackCooldowns;
    Dictionary<EnemyType, float> m_attackCooldownDict = new Dictionary<EnemyType, float>();

    [SerializeField] private List<EnemyTypeIntPair> m_maxHealths;
    Dictionary<EnemyType, int> m_maxHealthDict = new Dictionary<EnemyType, int>();

    [SerializeField] private List<EnemyTypeIntPair> m_damages;
    Dictionary<EnemyType, int> m_damageDict = new Dictionary<EnemyType, int>();

    [SerializeField] private List<EnemyTypeBehaviourTreePair> m_behaviourTrees;
    Dictionary<EnemyType, BehaviourTree> m_behaviourTreeDict = new Dictionary<EnemyType, BehaviourTree>();

	[SerializeField] private List<EnemyTypeGameObjectPair> m_attackPrefabs;
	Dictionary<EnemyType, GameObject> m_attackPrefabDict = new Dictionary<EnemyType, GameObject>();

    [SerializeField] private EnemyData[] m_enemies;

    private BaseCharacter[] m_players;

    // Start is called before the first frame update
    void Start() {
        foreach (var range in m_viewRanges) {
            m_viewRangeDict.Add(range.key, range.value);
        }
        foreach (var range in m_attackRanges) {
            m_attackRangeDict.Add(range.key, range.value);
        }
        foreach (var cooldown in m_attackCooldowns) {
            m_attackCooldownDict.Add(cooldown.key, cooldown.value);
        }
        foreach (var health in m_maxHealths) {
            m_maxHealthDict.Add(health.key, health.value);
        }
        foreach (var damage in m_damages) {
            m_damageDict.Add(damage.key, damage.value);
        }
        foreach (var tree in m_behaviourTrees) {
            m_behaviourTreeDict.Add(tree.key, tree.value);
        }
		foreach (var prefab in m_attackPrefabs) {
			m_attackPrefabDict.Add(prefab.key, prefab.value);
		}

        m_players = FindObjectsOfType<BaseCharacter>();
        InitEnemies();
    }

    void Update() {
        foreach (EnemyData enemy in m_enemies) {
            m_behaviourTreeDict[enemy.Type].Execute(enemy);
        }
    }

    private void InitEnemies() {
        foreach (EnemyData enemy in m_enemies) {
			enemy.Init(
				m_viewRangeDict[enemy.Type],
				m_maxHealthDict[enemy.Type],
				m_attackRangeDict[enemy.Type],
				m_attackCooldownDict[enemy.Type],
				m_damageDict[enemy.Type],
				m_attackPrefabDict[enemy.Type],
                m_players
            );
        }
    }
}

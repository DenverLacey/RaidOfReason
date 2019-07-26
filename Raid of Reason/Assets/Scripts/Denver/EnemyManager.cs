/*
 * Author: Denver
 * Description:	EnemyManager that controls every enemy in a room.  All helper Pair structs
 *				are here too
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyTypeFloatPair 
{
    public EnemyType key;
    public float value;
}

[System.Serializable]
public struct EnemyTypeEnemyAttackRangePair 
{
    public EnemyType key;
    public EnemyAttackRange value;
}

[System.Serializable]
public struct EnemyTypeIntPair 
{
    public EnemyType key;
    public int value;
}

[System.Serializable]
public struct EnemyTypeBehaviourTreePair 
{
    public EnemyType key;
    public BehaviourTree value;
}

[System.Serializable]
public struct EnemyTypeGameObjectPair 
{
	public EnemyType key;
	public GameObject value;
}

/// <summary>
/// EnemyManager will handle giving each enemy their initial data based on their type and 
/// will execute assosiated behaviour tree on each enemy that it manages
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [SerializeField]
	private List<EnemyTypeFloatPair> m_viewRanges;

	Dictionary<EnemyType, float> m_viewRangeDict = new Dictionary<EnemyType, float>();

    [SerializeField]
	private List<EnemyTypeEnemyAttackRangePair> m_attackRanges;

	private Dictionary<EnemyType, EnemyAttackRange> m_attackRangeDict = new Dictionary<EnemyType, EnemyAttackRange>();

    [SerializeField]
	private List<EnemyTypeFloatPair> m_attackCooldowns;

	private Dictionary<EnemyType, float> m_attackCooldownDict = new Dictionary<EnemyType, float>();

	[SerializeField]
	private List<EnemyTypeIntPair> m_maxHealths;

	private Dictionary<EnemyType, int> m_maxHealthDict = new Dictionary<EnemyType, int>();

    [SerializeField]
	private List<EnemyTypeIntPair> m_attackDamages;

	private Dictionary<EnemyType, int> m_attackDamageDict = new Dictionary<EnemyType, int>();

    [SerializeField]
	private List<EnemyTypeBehaviourTreePair> m_behaviourTrees;

	private Dictionary<EnemyType, BehaviourTree> m_behaviourTreeDict = new Dictionary<EnemyType, BehaviourTree>();

	[SerializeField]
	private List<EnemyTypeGameObjectPair> m_attackPrefabs;

	private Dictionary<EnemyType, GameObject> m_attackPrefabDict = new Dictionary<EnemyType, GameObject>();

    [SerializeField]
	private List<EnemyData> m_enemies = new List<EnemyData>();

    private BaseCharacter[] m_players;

    // Start is called before the first frame update
    void Start() 
	{
		// move data from lists into dictionaries
        foreach (var range in m_viewRanges)			{ m_viewRangeDict.Add(range.key, range.value); }
        foreach (var range in m_attackRanges)		{ m_attackRangeDict.Add(range.key, range.value); }
        foreach (var cooldown in m_attackCooldowns) { m_attackCooldownDict.Add(cooldown.key, cooldown.value); }
        foreach (var health in m_maxHealths)		{ m_maxHealthDict.Add(health.key, health.value); }
        foreach (var damage in m_attackDamages)		{ m_attackDamageDict.Add(damage.key, damage.value); }
        foreach (var tree in m_behaviourTrees)		{ m_behaviourTreeDict.Add(tree.key, tree.value); }
		foreach (var prefab in m_attackPrefabs)		{ m_attackPrefabDict.Add(prefab.key, prefab.value); }

		// get reference to players
        m_players = FindObjectsOfType<BaseCharacter>();

        InitEnemies();
    }

    void Update() 
	{
		// remove all destroyed enemies from list
		m_enemies.RemoveAll(enemy => !enemy);

		// execute associated behaviour tree on each enemy
		foreach (EnemyData enemy in m_enemies) 
		{
			m_behaviourTreeDict[enemy.Type].Execute(enemy);
        }
    }

	/// <summary>
	/// Calls Init function for every enemy the manager handles
	/// </summary>
    private void InitEnemies() 
	{
        foreach (EnemyData enemy in m_enemies) 
		{
			enemy.Init(
				m_viewRangeDict[enemy.Type],
				m_maxHealthDict[enemy.Type],
				m_attackRangeDict[enemy.Type],
				m_attackCooldownDict[enemy.Type],
				m_attackDamageDict[enemy.Type],
				m_attackPrefabDict[enemy.Type],
                m_players
            );
        }
    }
}

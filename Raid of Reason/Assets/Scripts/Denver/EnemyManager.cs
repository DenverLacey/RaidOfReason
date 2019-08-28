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
public struct EnemyTypeBehaviourTreePair 
{
    public EnemyType key;
    public BehaviourTree value;
}

[System.Serializable]
public struct EnemyTypeGameObjectsPair 
{
	public EnemyType key;
	public GameObject[] value;
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
	private List<EnemyTypeFloatPair> m_maxHealths;

	private Dictionary<EnemyType, float> m_maxHealthDict = new Dictionary<EnemyType, float>();

    [SerializeField]
	private List<EnemyTypeFloatPair> m_attackDamages;

	private Dictionary<EnemyType, float> m_attackDamageDict = new Dictionary<EnemyType, float>();

    [SerializeField]
	private List<EnemyTypeBehaviourTreePair> m_behaviourTrees;

	private Dictionary<EnemyType, BehaviourTree> m_behaviourTreeDict = new Dictionary<EnemyType, BehaviourTree>();

	[SerializeField]
	private List<EnemyTypeGameObjectsPair> m_attackPrefabs;

	private Dictionary<EnemyType, GameObject[]> m_attackPrefabDict = new Dictionary<EnemyType, GameObject[]>();

	private List<EnemyData> m_enemies = new List<EnemyData>();

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
    }

	/// <summary>
	/// Executes Behaviour Trees on each enemy the manager handles and
	/// removes destroyed enemies from m_enemies
	/// </summary>
    void FixedUpdate() 
	{
		// remove all destroyed enemies from list
		m_enemies.RemoveAll(enemy => !enemy);

		// execute associated behaviour tree on each enemy
		foreach (EnemyData enemy in m_enemies) 
		{
			if (enemy.PendingBehaviour)
			{
				Behaviour.Result result = enemy.ExecutePendingBehaviour();

				if (result != Behaviour.Result.PENDING_ABORT &&
					result != Behaviour.Result.PENDING_COMPOSITE &&
					result != Behaviour.Result.PENDING_MONO)
				{
					enemy.PendingBehaviour = null;
				}
			}
			else
			{
				m_behaviourTreeDict[enemy.Type].Execute(enemy);
			}
        }
    }

	/// <summary>
	/// Calls enemy's init function and adds them to list of enemies for the manager to handle
	/// </summary>
	/// <param name="enemy">
	/// Enemy to initialise
	/// </param>
    private void InitEnemy(EnemyData enemy) 
	{
		enemy.Init(
			m_viewRangeDict[enemy.Type],
			m_maxHealthDict[enemy.Type],
			m_attackRangeDict[enemy.Type],
			m_attackCooldownDict[enemy.Type],
			m_attackDamageDict[enemy.Type],
			m_attackPrefabDict[enemy.Type]
		);

		m_enemies.Add(enemy);
    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Enemy")
		{
			InitEnemy(other.GetComponent<EnemyData>());
		}
	}
}

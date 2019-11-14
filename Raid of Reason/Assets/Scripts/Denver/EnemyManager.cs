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
    public string key;
    public float value;
}

[System.Serializable]
public struct EnemyTypeEnemyAttackRangePair 
{
    public string key;
    public EnemyAttackRange value;
}

[System.Serializable]
public struct EnemyTypeBehaviourTreePair 
{
    public string key;
    public BehaviourTree value;
}

[System.Serializable]
public struct EnemyTypeGameObjectsPair 
{
	public string key;
	public GameObject[] value;
}

[System.Serializable]
public struct EnemyTypeCharacterTypePair
{
	public string key;
	public CharacterType value;
}

/// <summary>
/// EnemyManager will handle giving each enemy their initial data based on their type and 
/// will execute assosiated behaviour tree on each enemy that it manages
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [SerializeField]
	private List<EnemyTypeFloatPair> m_viewRanges;

	Dictionary<string, float> m_viewRangeDict = new Dictionary<string, float>();

	[SerializeField]
	private List<EnemyTypeCharacterTypePair> m_characterPriorities = new List<EnemyTypeCharacterTypePair>();

	Dictionary<string, CharacterType> m_characterPriorityDict = new Dictionary<string, CharacterType>();

	[SerializeField]
	private List<EnemyTypeFloatPair> m_priorityThresholds = new List<EnemyTypeFloatPair>();

	Dictionary<string, float> m_priorityThresholdDict = new Dictionary<string, float>();

    [SerializeField]
	private List<EnemyTypeEnemyAttackRangePair> m_attackRanges;

	private Dictionary<string, EnemyAttackRange> m_attackRangeDict = new Dictionary<string, EnemyAttackRange>();

    [SerializeField]
	private List<EnemyTypeFloatPair> m_attackCooldowns;

	private Dictionary<string, float> m_attackCooldownDict = new Dictionary<string, float>();

	[SerializeField]
	private List<EnemyTypeFloatPair> m_maxHealths;

	private Dictionary<string, float> m_maxHealthDict = new Dictionary<string, float>();

    [SerializeField]
	private List<EnemyTypeFloatPair> m_attackDamages;

	private Dictionary<string, float> m_attackDamageDict = new Dictionary<string, float>();

    [SerializeField]
	private List<EnemyTypeBehaviourTreePair> m_behaviourTrees;

	public Dictionary<string, BehaviourTree> BehaviourTrees { get; private set; }

	[SerializeField]
	private List<EnemyTypeGameObjectsPair> m_attackPrefabs;

	private Dictionary<string, GameObject[]> m_attackPrefabDict = new Dictionary<string, GameObject[]>();

	[SerializeField]
	private GameObject m_damageIndicatorPrefab;
	public GameObject DamageIndicatorPrefab { get => m_damageIndicatorPrefab;  }

    // Start is called before the first frame update
    void Start() 
	{
		BehaviourTrees = new Dictionary<string, BehaviourTree>();

		// move data from lists into dictionaries
        foreach (var range in m_viewRanges)				{ m_viewRangeDict.Add(range.key, range.value); }
		foreach (var priority in m_characterPriorities) { m_characterPriorityDict.Add(priority.key, priority.value); }
		foreach (var threshold in m_priorityThresholds) { m_priorityThresholdDict.Add(threshold.key, threshold.value); }
        foreach (var range in m_attackRanges)			{ m_attackRangeDict.Add(range.key, range.value); }
        foreach (var cooldown in m_attackCooldowns)		{ m_attackCooldownDict.Add(cooldown.key, cooldown.value); }
        foreach (var health in m_maxHealths)			{ m_maxHealthDict.Add(health.key, health.value); }
        foreach (var damage in m_attackDamages)			{ m_attackDamageDict.Add(damage.key, damage.value); }
        foreach (var tree in m_behaviourTrees)			{ BehaviourTrees.Add(tree.key, tree.value); }
		foreach (var prefab in m_attackPrefabs)			{ m_attackPrefabDict.Add(prefab.key, prefab.value); }
    }

	/// <summary>
	/// Executes Behaviour Trees on each enemy the Manager handles and
	/// removes destroyed enemies from m_enemies
	/// </summary>
    void FixedUpdate() 
	{
    }

	/// <summary>
	/// Calls enemy's init function and adds them to list of enemies for the Manager to handle
	/// </summary>
	/// <param name="enemy">
	/// Enemy to initialise
	/// </param>
    public void InitEnemy(EnemyData enemy) 
	{
		try
		{
			enemy.Init(
				m_viewRangeDict[enemy.Type],
				m_maxHealthDict[enemy.Type],
				m_attackRangeDict[enemy.Type],
				m_attackCooldownDict[enemy.Type],
				m_attackDamageDict[enemy.Type],
				m_characterPriorityDict[enemy.Type],
				m_priorityThresholdDict[enemy.Type],
				m_damageIndicatorPrefab,
				m_attackPrefabDict[enemy.Type]
			);
		}
		catch (System.Exception e)
		{
			Debug.LogErrorFormat("Unknown Enemy Type: {0}\n{1}", enemy.Type, e.Message);
		}
    }
}

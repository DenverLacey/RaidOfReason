/*
 * Author: Denver
 * Description:	Handles all Enemy Zone functionality
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Area where enemies operate
/// </summary>
public class EnemyZone : MonoBehaviour
{
	[Tooltip("Distance from zone edge all players must be for zone to be culled")]
	[SerializeField]
	private Vector2 m_cullDistance = new Vector2(30, 30);
	private Vector3 m_cullDistanceV3;

	[SerializeField]
	private List<EnemyTypeFloatPair> m_viewRangeOverrides;
	Dictionary<string, float> m_viewRangeDict = new Dictionary<string, float>();

	[SerializeField]
	private List<EnemyTypeCharacterTypePair> m_characterPriorityOverrides = new List<EnemyTypeCharacterTypePair>();
	Dictionary<string, CharacterType> m_characterPriorityDict = new Dictionary<string, CharacterType>();

	[SerializeField]
	private List<EnemyTypeFloatPair> m_priorityThresholdOverrides = new List<EnemyTypeFloatPair>();
	Dictionary<string, float> m_priorityThresholdDict = new Dictionary<string, float>();

	[SerializeField]
	private List<EnemyTypeEnemyAttackRangePair> m_attackRangeOverrides;
	private Dictionary<string, EnemyAttackRange> m_attackRangeDict = new Dictionary<string, EnemyAttackRange>();

	[SerializeField]
	private List<EnemyTypeFloatPair> m_attackCooldownOverrides;
	private Dictionary<string, float> m_attackCooldownDict = new Dictionary<string, float>();

	[SerializeField]
	private List<EnemyTypeFloatPair> m_maxHealthOverrides;
	private Dictionary<string, float> m_maxHealthDict = new Dictionary<string, float>();

	[SerializeField]
	private List<EnemyTypeFloatPair> m_attackDamageOverrides;
	private Dictionary<string, float> m_attackDamageDict = new Dictionary<string, float>();

	[SerializeField]
	private List<EnemyTypeBehaviourTreePair> m_behaviourTreeOverrides;
	public Dictionary<string, BehaviourTree> BehaviourTreeOverrides { get; private set; }

	[SerializeField]
	private List<EnemyTypeGameObjectsPair> m_attackPrefabOverrides;
	private Dictionary<string, GameObject[]> m_attackPrefabDict = new Dictionary<string, GameObject[]>();

	private List<EnemyData> m_enemies;

	public int EnemyCount { get => m_enemies.Count; }

    public List<EnemyData> Enemies { get => m_enemies; }

	private bool m_active;

	private List<GameObject> m_deathParticlePool;

	private EnemyManager m_enemyManager;

	private BoxCollider[] m_colliders;

	private List<Bounds> m_cullBoundaries;

    // Start is called before the first frame update
    void Start()
    {
		m_enemies = new List<EnemyData>();
		m_enemyManager = FindObjectOfType<EnemyManager>();
		m_colliders = GetComponentsInChildren<BoxCollider>();

		m_cullBoundaries = new List<Bounds>();
		m_cullDistanceV3 = new Vector3(m_cullDistance.x, 0f, m_cullDistance.y);
		foreach (var collider in m_colliders)
		{
			Bounds boundary = collider.bounds;
			boundary.min -= m_cullDistanceV3;
			boundary.max += m_cullDistanceV3;
			m_cullBoundaries.Add(boundary);
		}

		if (!ObjectPooling.PoolExistsForPrefab("EnemyDeathParticle"))
		{
			ObjectPooling.CreateObjectPool("EnemyDeathParticle", 20);
		}

        BehaviourTreeOverrides = new Dictionary<string, BehaviourTree>();

		// move data from lists into dictionaries
		foreach (var range in m_viewRangeOverrides) { m_viewRangeDict.Add(range.key, range.value); }
		foreach (var priority in m_characterPriorityOverrides) { m_characterPriorityDict.Add(priority.key, priority.value); }
		foreach (var threshold in m_priorityThresholdOverrides) { m_priorityThresholdDict.Add(threshold.key, threshold.value); }
		foreach (var range in m_attackRangeOverrides) { m_attackRangeDict.Add(range.key, range.value); }
		foreach (var cooldown in m_attackCooldownOverrides) { m_attackCooldownDict.Add(cooldown.key, cooldown.value); }
		foreach (var health in m_maxHealthOverrides) { m_maxHealthDict.Add(health.key, health.value); }
		foreach (var damage in m_attackDamageOverrides) { m_attackDamageDict.Add(damage.key, damage.value); }
		foreach (var tree in m_behaviourTreeOverrides) { BehaviourTreeOverrides.Add(tree.key, tree.value); }
		foreach (var prefab in m_attackPrefabOverrides) { m_attackPrefabDict.Add(prefab.key, prefab.value); }
	}
	
    void FixedUpdate()
    {
		m_active = false;

		// check if any player is close by
		foreach (var player in GameManager.Instance.AlivePlayers)
		{
			foreach (var cullBoundary in m_cullBoundaries)
			{
				if (cullBoundary.min.x <= player.transform.position.x && player.transform.position.x <= cullBoundary.max.x &&
					cullBoundary.min.z <= player.transform.position.z && player.transform.position.z <= cullBoundary.max.z)
				{
					m_active = true;
					goto PlayerWithinCullDistance;
				}
			}
		}

		PlayerWithinCullDistance:

		if (m_active)
		{
			// remove all destroyed enemies from list
			m_enemies.RemoveAll(enemy => !enemy);

			// reactivate enemies
			m_enemies.ForEach(e => e.gameObject.SetActive(true));

			// execute associated behaviour tree for each enemy
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
                else if (BehaviourTreeOverrides.ContainsKey(enemy.Type))
                {
                    BehaviourTreeOverrides[enemy.Type].Execute(enemy);
                }
				else
				{

					m_enemyManager.BehaviourTrees[enemy.Type].Execute(enemy);
				}
			}
		}
		else
		{
			DeactivateReturnedEnemies();
		}

    }

	/// <summary>
	/// Initialises all enemies that are apart of zone
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
	{
		EnemyData enemy = null;

		if (other.tag == "Enemy")
		{
			enemy = other.GetComponent<EnemyData>();
		}
		else if (other.tag == "EnemyEnemyCollision")
		{
			enemy = other.GetComponentInParent<EnemyData>();
		}

		if (enemy && !m_enemies.Contains(enemy))
		{
			if (m_viewRangeDict.ContainsKey(enemy.Type))
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
						m_enemyManager.DamageIndicatorPrefab,
						m_attackPrefabDict[enemy.Type]
					);
				}
				catch (System.Exception e)
				{
					Debug.LogErrorFormat("Unknown Enemy Type: {0}\n{1}", enemy.Type, e.Message);
				}
			}
			else
			{
				m_enemyManager.InitEnemy(enemy);
			}

			m_enemies.Add(enemy);
			enemy.Zone = this;
		}
	}

	/// <summary>
	/// Generates a random Vector3 position within the zone
	/// </summary>
	/// <param name="y">
	/// The desired Y level of the position
	/// </param>
	/// <returns>
	/// A random position within the zone
	/// </returns>
	public Vector3 GetRandomPoint(float y)
	{
		// pick random collider in zone
		int index = Random.Range(0, m_colliders.Length);
		BoxCollider collider = m_colliders[index];

		// define boundary corners
		float minx = collider.bounds.min.x;
		float maxx = collider.bounds.max.x;
		float minz = collider.bounds.min.z;
		float maxz = collider.bounds.max.z;

		// lerp to find random point within collider
		float x = Mathf.Lerp(minx, maxx, Random.Range(0f, 1f));
		float z = Mathf.Lerp(minz, maxz, Random.Range(0f, 1f));

		return new Vector3(x, y, z);
	}

	/// <summary>
	/// Determines if a given point is within the zone
	/// </summary>
	/// <param name="point">
	/// The point to query
	/// </param>
	/// <returns>
	/// True if point is within zone. False if otherwise
	/// </returns>
	public bool ContainsPoint(Vector3 point)
	{
		foreach (var collider in m_colliders)
		{
			if (IsPointInCollider(point, collider))
				return true;
		}
		return false;
	}

	/// <summary>
	/// Calculates closest position within the zone from a given point
	/// </summary>
	/// <param name="point">
	/// The point to query
	/// </param>
	/// <returns>
	/// Closest position to given point
	/// </returns>
	public Vector3 ClampPoint(Vector3 point)
	{
		point.y = 0f;
		Vector3 closestClamped = point;
		float closestSqrDistance = float.MaxValue;

		foreach (var collider in m_colliders)
		{
			if (IsPointInCollider(point, collider))
			{
				return point;
			}

			Vector3 clamped = point;
			clamped.x = Mathf.Clamp(clamped.x, collider.bounds.min.x, collider.bounds.max.x);
			clamped.z = Mathf.Clamp(clamped.z, collider.bounds.min.z, collider.bounds.max.z);

			float sqrDistance = (clamped - point).sqrMagnitude;
			if (sqrDistance < closestSqrDistance)
			{
				closestSqrDistance = sqrDistance;
				closestClamped = clamped;
			}
		}

		return closestClamped;
	}

	/// <summary>
	/// Determines if a given point is inside a given collider
	/// </summary>
	/// <param name="point">
	/// The point to query
	/// </param>
	/// <param name="collider">
	/// The collider to query
	/// </param>
	/// <returns>
	/// True if point is inside collider. False if otherwise
	/// </returns>
	private bool IsPointInCollider(Vector3 point, BoxCollider collider)
	{
		return	collider.bounds.min.x <= point.x && point.x <= collider.bounds.max.x &&
				collider.bounds.min.z <= point.z && point.z <= collider.bounds.max.z;
	}

	/// <summary>
	/// Deactivates all enemies inside zone. All other enemies are told
	/// to path back to the zone
	/// </summary>
	private void DeactivateReturnedEnemies()
	{
		foreach (var enemy in m_enemies)
		{
			if (!ContainsPoint(enemy.transform.position))
			{
				enemy.Pathfinder.SetDestination(transform.position);
				continue;
			}

			enemy.gameObject.SetActive(false);
		}
	}
}

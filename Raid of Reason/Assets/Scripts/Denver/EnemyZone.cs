using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyZone : MonoBehaviour
{
	[Tooltip("Distance from zone edge all players must be for zone to be culled")]
	[SerializeField]
	private Vector2 m_cullDistance = new Vector2(30, 30);
	private Vector3 m_cullDistanceV3;

	private List<EnemyData> m_enemies;

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
    }
	
    void FixedUpdate()
    {
		m_active = false;
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
			m_enemyManager.InitEnemy(enemy);
			m_enemies.Add(enemy);
			enemy.Zone = this;
		}
	}

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

	public bool ContainsPoint(Vector3 point)
	{
		foreach (var collider in m_colliders)
		{
			if (IsPointInCollider(point, collider))
				return true;
		}
		return false;
	}

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

	private bool IsPointInCollider(Vector3 point, BoxCollider collider)
	{
		if (collider.bounds.min.x <= point.x && point.x <= collider.bounds.max.x &&
			collider.bounds.min.z <= point.z && point.z <= collider.bounds.max.z)
		{
			return true;
		}
		return false;
	}

	private void DeactivateReturnedEnemies()
	{
		foreach (var enemy in m_enemies)
		{
			if (!ContainsPoint(enemy.transform.position))
			{
				enemy.Pathfinder.SetDestination(enemy.Zone.transform.position);
				continue;
			}

			enemy.gameObject.SetActive(false);
		}
	}
}

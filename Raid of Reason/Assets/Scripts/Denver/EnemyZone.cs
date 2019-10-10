using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class EnemyZone : MonoBehaviour
{
	private List<EnemyData> m_enemies;
	private List<BaseCharacter> m_baseCharacters;

	private bool m_active;
	private bool m_enemiesDeactivated;

	private List<GameObject> m_deathParticlePool;

	private EnemyManager m_enemyManager;

	private BoxCollider[] m_colliders;

    // Start is called before the first frame update
    void Start()
    {
		m_enemies = new List<EnemyData>();
		m_baseCharacters = new List<BaseCharacter>();
		m_deathParticlePool = ObjectPooling.CreateObjectPool("EnemyDeathParticle", 20);
		m_enemyManager = FindObjectOfType<EnemyManager>();
		m_colliders = GetComponentsInChildren<BoxCollider>();
    }
	
    void FixedUpdate()
    {
		if (m_active)
		{
			// reactivate enemies
			m_enemies.ForEach(e => e.gameObject.SetActive(true));
			m_enemiesDeactivated = false;

			// remove all destroyed enemies from list
			m_enemies.RemoveAll(enemy => !enemy);

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
			if (m_enemies.Count == 0)
			{
				m_enemiesDeactivated = false;
			}
			else
			{
				m_enemies.ForEach(e => e.gameObject.SetActive(false));
				m_enemiesDeactivated = true;
			}
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

		if (enemy)
		{
			m_enemyManager.InitEnemy(enemy);
			m_enemies.Add(enemy);
			enemy.Zone = this;
		}
	}

	private void OnBecameInvisible()
	{
		m_active = false;
	}

	private void OnBecameVisible()
	{
		m_active = true;
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

	public bool IsPointInZone(Vector3 point)
	{
		foreach (var collider in m_colliders)
		{
			if (IsPointInCollider(point, collider))
				return true;
		}
		return false;
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
}

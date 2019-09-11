using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : MonoBehaviour
{
	[SerializeField]
	[Tooltip("How fast the enemy will move")]
	private float m_speed = 3f;

	[SerializeField]
	[Tooltip("How fast the enemy will turn")]
	private float m_steeringSpeed = 10f;

	[SerializeField]
	[Tooltip("How close enemy needs to be to target destination before it finds new target")]
	private float m_destBufferDist = 0.1f;

	private NavMeshPath m_path;
	private bool m_pathing;
	private int m_currentCorner;
	private float m_yLevel;

	[HideInInspector]
	public bool manualSteering;

	EnemyData m_enemy;

	private void Awake()
	{
		m_enemy = GetComponent<EnemyData>();
	}

	// Start is called before the first frame update
	void Start()
    {
		m_path = new NavMeshPath();
		m_yLevel = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
		if (m_path.corners.Length == 0)
		{
			return;
		}

		// move to destination
		if (!m_enemy.Stunned && m_pathing && m_currentCorner != m_path.corners.Length)
		{
			Vector3 currentTarget = m_path.corners[m_currentCorner];
			currentTarget.y = transform.position.y;

			// draw debug line that follows path
			Vector3 currentPosition = transform.position;
			currentPosition.y = 0.1f;
			Debug.DrawLine(currentPosition, m_path.corners[m_currentCorner], Color.green);

			for (int i = m_currentCorner + 1; i < m_path.corners.Length; i++)
			{
				Debug.DrawLine(m_path.corners[i - 1], m_path.corners[i], Color.green);
			}

			// calculate movement vector
			Vector3 movementVector = (currentTarget - transform.position).normalized;

			// calculate desiredPosition
			Vector3 desiredPosition = transform.position + movementVector * m_speed * Time.deltaTime;
			desiredPosition.y = m_yLevel;
			m_enemy.Rigidbody.MovePosition(desiredPosition);

			// animation
			m_enemy.SetAnimatorFloat("Speed", movementVector.magnitude);

			// if reached current corner, move to next
			if (AtPosition(m_path.corners[m_currentCorner]))
			{
				m_currentCorner++;
			}

			// do steering
			if (!manualSteering)
			{
				Quaternion desiredRotation = Quaternion.LookRotation(movementVector);
				transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, m_steeringSpeed * Time.deltaTime);
			}
		}
		else
		{
			m_enemy.SetAnimatorFloat("Speed", 0f);
		}
	}

	/// <summary>
	/// Sets a new destination for enemy to path to.  If new destination
	/// is close to where enemy already is no pathfinding will occur
	/// </summary>
	/// <param name="destination">
	/// New destination
	/// </param>
	public void SetDestination(Vector3 destination)
	{
		destination.y = 0f;

		if (!AtPosition(destination))
		{
			m_pathing = true;
			NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, m_path);
			m_currentCorner = 1;
		}
		else
		{
			StopPathing();
		}
	}

	/// <summary>
	/// Stops Enemy from following path. Also deletes current path
	/// </summary>
	public void StopPathing()
	{
		NavMesh.CalculatePath(transform.position, transform.position, NavMesh.AllAreas, m_path);
		m_currentCorner = 1;
		m_pathing = false;
	}

	/// <summary>
	/// Gets Destination of the enemy
	/// </summary>
	/// <returns>
	/// The last position of the enemy's current path
	/// </returns>
	public Vector3 GetDestination()
	{
		if (m_path.corners.Length == 0)
		{
			return transform.position;
		}
		else
		{
			Vector3 destination = m_path.corners[m_path.corners.Length - 1];
			destination.y = transform.position.y;
			return destination;
		}
	}

	/// <summary>
	/// Determines if enemy is close to a position
	/// </summary>
	/// <param name="position">
	/// Position the enemy might be close to
	/// </param>
	/// <returns>
	/// If enemy is close to given position
	/// </returns>
	private bool AtPosition(Vector3 position)
	{
		Vector2 difference = new Vector2(
			position.x - transform.position.x,
			position.z - transform.position.z
		);

		difference.x = Mathf.Abs(difference.x);
		difference.y = Mathf.Abs(difference.y);

		return difference.magnitude < m_destBufferDist;
	}
}

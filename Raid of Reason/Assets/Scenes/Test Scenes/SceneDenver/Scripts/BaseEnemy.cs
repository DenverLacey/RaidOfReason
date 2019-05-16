using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class BaseEnemy : MonoBehaviour
{
	[Tooltip("How far away the enemy can see the player from.")]
    [SerializeField] protected float m_viewRange;

	[Tooltip("Enemy's maximum amount of health.")]
	[SerializeField] protected int m_maxHealth;

	protected int m_health;
	public int Health { get { return m_health; } }

    protected NavMeshAgent m_navMeshAgent;
    protected GameObject[] m_players;
	protected Vector3 m_target;

    protected enum AI_STATE {
	    WANDER,
	    ATTACK
    };

	protected AI_STATE m_currentState;
	protected AI_STATE m_oldState;

    protected abstract void Attack();

    protected virtual void Start() {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_players = GameObject.FindGameObjectsWithTag("Player");
        m_currentState = AI_STATE.WANDER;
        m_oldState = m_currentState;
		m_health = m_maxHealth;
    }

    protected virtual void Update() {
		// determine state
		m_currentState = DetermineState();

		switch (m_currentState) {
			case AI_STATE.WANDER:
				Wander();
				break;

			case AI_STATE.ATTACK:
				Attack();
				break;

			default:
				Debug.LogError("State couldn't be determined!", this);
				break;
		}
		m_navMeshAgent.destination = m_target;
	}

    protected virtual AI_STATE DetermineState() {
		AI_STATE s = AI_STATE.WANDER;

		Transform closest = null;
		float D = m_viewRange;

		foreach (GameObject p in m_players) {
			Ray ray = new Ray(transform.position, p.transform.position - transform.position);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, D)) {
				if (hit.collider.tag == "Player") {
					closest = p.transform;
					D = Vector3.Distance(transform.position, p.transform.position);
					s = AI_STATE.ATTACK;
				}
			}
		}

		if (closest) {
			m_target = closest.position;
		}

		return s;
	}

    protected Vector3 FindClosestPoint(Vector3 source) {

		NavMeshHit hit;
		if (NavMesh.SamplePosition(source, out hit, transform.localScale.y, 0)) {
			return source;
		}

		if (NavMesh.FindClosestEdge(source, out hit, 0)) {
			return hit.position;
		}
		return source;
	}

    protected virtual void Wander() {

		if (Vector3.Distance(transform.position, m_target) > 1f) {
			return;
		} 

		// get nav mesh triangulation
		NavMeshTriangulation data = NavMesh.CalculateTriangulation();

		// pick random number (t)
		int t = Random.Range(0, data.vertices.Length);

		// get vertex at t
		Vector3 point = data.vertices[t];

		// set target destination
		m_target = point;

		m_oldState = AI_STATE.WANDER;
	}

	public virtual void TakeDamage(int damamge) {
		m_health -= damamge;

		if (m_health <= 0.0f) {
			Destroy(gameObject);
		}
	}

}

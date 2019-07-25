using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Abstract BaseEnemy class from which all enemies derive from.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public abstract class BaseEnemy : MonoBehaviour
{
	[Tooltip("How far away the enemy can see the player from.")]
    [SerializeField] protected float m_viewRange;
	public float ViewRange { get => m_viewRange; }

	[Tooltip("Enemy's maximum amount of health.")]
	[SerializeField] public int m_maxHealth;

	[Tooltip("How much damage the enemy will deal.")]
	[SerializeField] protected int m_damage;
	public int Damage { get => m_damage; }

	protected float m_health;
	public float Health { get => m_health; }
	
	protected bool m_stunned;

    protected NavMeshAgent m_navMeshAgent;
    protected BaseCharacter[] m_players;
	public BaseCharacter[] Players { get => m_players; }

	protected Vector3 m_target;
	public Vector3 Target { get => m_target; set => m_target = value; }

	protected Nashorn m_nashorn;

    //Afridi: I added this for my skill tree
    public bool isDeadbByKenron = false;

    public enum AI_STATE {
	    WANDER,
	    ATTACK,
		TAUNTED
    };

	protected AI_STATE m_currentState;
	protected AI_STATE m_oldState;

    public AI_STATE State { get => m_currentState; }

    protected abstract void Attack();
	protected abstract void Taunted();

    protected virtual void Start() {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_players = GameObject.FindObjectsOfType<BaseCharacter>();
		m_nashorn = GameObject.FindObjectOfType<Nashorn>();
        m_currentState = AI_STATE.WANDER;
        m_oldState = m_currentState;
		m_health = m_maxHealth;
    }

    protected virtual void Update() {
    	if (m_stunned) return;
		
		// determine state
		if (m_currentState != AI_STATE.TAUNTED || m_target == null) {
			m_currentState = DetermineState();
		}

		switch (m_currentState) {
			case AI_STATE.WANDER:
				Wander();
				break;

			case AI_STATE.ATTACK:
				Attack();
				break;

			case AI_STATE.TAUNTED:
				Taunted();
				break;

			default:
				Debug.LogError("State couldn't be determined!", this);
				break;
		}
		m_navMeshAgent.destination = m_target;
		m_oldState = m_currentState;
	}

    protected virtual AI_STATE DetermineState() {
		AI_STATE s = AI_STATE.WANDER;

		Transform closest = null;
		float D = m_viewRange;

		foreach (var p in m_players) {
            if (!p) continue;

			Ray ray = new Ray(transform.position, p.transform.position - transform.position);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, D)) {
				if (hit.collider.tag == "Kenron" || hit.collider.tag == "Thea" || hit.collider.tag == "Nashorn") {
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

	/// <summary>
	/// Finds the closest point on the nav mesh to the source.
	/// </summary>
	/// <param name="source">
	/// A Vector3. The source position that you want to know the closest point on the nav mesh.
	/// </param>
	/// <returns>
	/// A Vector3. The closest point on the nav mesh to the source position.
	/// </returns>
    protected Vector3 FindClosestPoint(Vector3 source) {

		NavMeshHit hit;
		if (NavMesh.SamplePosition(source, out hit, transform.localScale.y, NavMesh.GetAreaFromName("walkable"))) {
			if (hit.hit) {
				return source;
			}
		}

		if (NavMesh.FindClosestEdge(source, out hit, NavMesh.GetAreaFromName("walkable"))) {
			if (hit.hit) {
				return hit.position;
			}
		}
		return Vector3.positiveInfinity;
	}

    protected virtual void Wander() {
		if (Vector3.Distance(transform.position, m_target) > 2f && m_oldState == AI_STATE.WANDER) {
			return;
		}

		// get nav mesh triangulation
		NavMeshTriangulation data = NavMesh.CalculateTriangulation();

		// pick random triangle (t)
		int t = 3 * Random.Range(0, data.vertices.Length / 3);

		// get vertex at t
		Vector3 point = data.vertices[t];

		// lerp point so point can be anywhere within a tri
		point = Vector3.Lerp(point, data.vertices[t + 1], Random.Range(0f, 1f));
		point = Vector3.Lerp(point, data.vertices[t + 2], Random.Range(0f, 1f));

		// set target destination
		m_target = point;
	}

	/// <summary>
	/// Subtracts some damage from the enemy's health and will destroy enemy if it's dead.
	/// </summary>
	/// <param name="damage">
	/// How much damage the enemy will sustain.
	/// </param>
	public virtual void TakeDamage(float damage) {
		m_health -= damage;

		if (m_health <= 0.0f) {
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Deactivates enemy's decision making for a duration.
	/// </summary>
	/// <param name="duration">
	/// How long the enemy will be stunned for.
	/// </param>
	public virtual void Stun(float duration) {
		m_stunned = true;
        StartCoroutine(StunReset(duration));
	}

	/// <summary>
	/// Unstuns enemy after a duration.
	/// </summary>
	/// <param name="duration">
	/// How long to wait before unstunning enemy.
	/// </param>
	/// <returns>
	/// An IEnumerator. WaitForEndOfFrame
	/// </returns>
    IEnumerator StunReset(float duration) {
        duration -= Time.deltaTime;
        if (duration > 0f) yield return new WaitForEndOfFrame();
        m_stunned = false;
    }
	
	/// <summary>
	/// Sets enemy's current state to TAUNTED and sets enemy's target to nashorn.
	/// </summary>
	public virtual void Taunt() {
		m_currentState = AI_STATE.TAUNTED;
		m_target = m_nashorn.transform.position;
	}

	public virtual void StopTaunt() {
		m_currentState = AI_STATE.WANDER;
	}

    //Afridi : Added this for skill tree
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Amaterasu")
        {
            if (m_health <= 0.0f)
            {
                Kenron kenron = other.GetComponentInParent<Kenron>();
                isDeadbByKenron = true;
                kenron.SkillChecker();
                isDeadbByKenron = false;
            }
        }
    }
}

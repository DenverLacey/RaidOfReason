/*
 * Author: Denver
 * Description:	EnemyData class that holds all data for all enemy types and some functions to
 *				alter that state from within the behaviour tree
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public enum EnemyType 
{
    SUICIDE,
    MELEE,
    RANGE,
    SPAWNER
}

[System.Serializable]
public struct EnemyAttackRange 
{
    public float min;
    public float max;
}

/// <summary>
/// Encapsulates all data for all enemy types
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(EnemyPathfinding))]
public class EnemyData : MonoBehaviour 
{
    [SerializeField] 
	private EnemyType m_type;

    [SerializeField]
    private ParticleSystem m_tauntedEffect;

    [SerializeField]
    private ParticleSystem m_electricEffect;

    [SerializeField]
    private ParticleSystem m_bloodEffect;

    [SerializeField]
    private ParticleSystem m_waterEffect;

    [SerializeField]
    private ParticleSystem m_burningEffect;

    public EnemyType Type { get => m_type; }
    public float ViewRange { get; private set; }
	public float SqrViewRange { get => ViewRange * ViewRange; }
    public float MaxHealth { get; private set; }
    public float Health { get; private set; }
    public EnemyAttackRange AttackRange { get; private set; }
    public float AttackCooldown { get; private set; }
    public float AttackTimer { get; set; }
	public float AttackDamage { get; private set; }
    public bool Attacking { get; set; }

	private bool m_taunted;
	public bool Taunted
	{
		get => m_taunted;
		set
		{
			m_taunted = value;

			if (m_taunted)
			{
				m_tauntedEffect.Play();
			}
			else
			{
				m_tauntedEffect.Stop();
			}
		}
	}
	public bool Stunned { get; set; }
    private bool m_knockedBack;
	public GameObject[] AttackPrefabs { get; private set; }
	public CharacterType PriorityCharacter { get; private set; }
	public float PriorityThreshold { get; private set; }
	public float SqrPriorityThreashold { get => PriorityThreshold * PriorityThreshold; }

    public Vector3 Target { get; set; }
	public BaseCharacter TargetPlayer { get; set; }

	public Behaviour PendingBehaviour { get; set; }

	public Rigidbody Rigidbody { get; private set; }

	[SerializeField]
	[Tooltip("Renderer of Enemy's mesh")]
	private Renderer m_renderer;

	public Renderer Renderer { get => m_renderer; }

	[SerializeField]
	[Tooltip("Animator Controller component of enemy")]
	private Animator m_animator;

	[HideInInspector]
    // Afridi added this for the skill tree
    public bool isDeadbByKenron = false;

    [HideInInspector]
    // Afridi added this for the skill tree
    public bool isAttackingNashorn;

	private Collider m_collider;

	// Pathfinding Stuff
	public EnemyPathfinding Pathfinder { get; private set; }

	private GameObject m_damageIndicator;

	public Behaviour.OnAnimationEvent OnAttackDelegate { get; set; }

	private void Awake()
	{
		Rigidbody = GetComponent<Rigidbody>();
		m_collider = GetComponent<Collider>();
        isAttackingNashorn = false;
		Pathfinder = GetComponent<EnemyPathfinding>();
    }

	/// <summary>
	/// Initialises the enemy's values
	/// </summary>
	/// <param name="viewRange">
	/// How far away enemy can see player
	/// </param>
	/// <param name="maxHealth">
	/// Maximum health of enemy
	/// </param>
	/// <param name="attackRange">
	/// Distance range in which enemy will attack
	/// </param>
	/// <param name="attackCooldown">
	/// How often enemy will attack
	/// </param>
	/// <param name="attackDamage">
	/// How much damage enemy's attack deals
	/// </param>
	/// <param name="priorityCharacter">
	/// Which character enemy should focus
	/// </param>
	/// <param name="priorityThreshold">
	/// How much closer another character has to be to override character priority
	/// </param>
	/// <param name="damageIndicatorPrefab">
	/// prefa for damage indicator object
	/// </param>
	/// <param name="attackPrefabs">
	/// effect object of enemy's attack
	/// </param>
	public void Init(
		float viewRange, 
		float maxHealth, 
		EnemyAttackRange attackRange, float attackCooldown, float attackDamage, 
		CharacterType priorityCharacter, float priorityThreshold,
		GameObject damageIndicatorPrefab, GameObject[] attackPrefabs)
    {
        ViewRange = viewRange;

        MaxHealth = maxHealth;
        Health = MaxHealth;

        AttackRange = attackRange;
        AttackCooldown = attackCooldown;
        AttackTimer = 0.0f;
		AttackDamage = attackDamage;
		AttackPrefabs = attackPrefabs;
		m_damageIndicator = damageIndicatorPrefab;

		Stunned = false;

		PriorityCharacter = priorityCharacter;
		PriorityThreshold = priorityThreshold;
    }

	/// <summary>
	/// Stuns enemy for a given amount of seconds
	/// </summary>
	/// <param name="duration">
	/// How long to stun enemy for in seconds
	/// </param>
	public void Stun(float duration) 
	{
		Stunned = true;

		DebugTools.LogVariable("Stunned", Stunned);

		StartCoroutine(ResetStun(duration));
	}

	/// <summary>
	/// Unstuns the enemy once a given amount of seconds has elapsed
	/// </summary>
	/// <param name="duration">
	/// How long to wait before unstunning the enemy in seconds
	/// </param>
	/// <returns>
	/// A WaitForSeconds
	/// </returns>
	IEnumerator ResetStun(float duration) 
	{
		yield return new WaitForSeconds(duration);
		Stunned = false;
		DebugTools.LogVariable("Stunned", Stunned);
	}

    public void KnockBack(float duration)
    {
        m_knockedBack = true;
        Stunned = true;
        StartCoroutine(ResetKnockBack(duration));
    }

    IEnumerator ResetKnockBack(float duration)
    {
        yield return new WaitForSeconds(duration);
        m_knockedBack = false;
        Stunned = false;
    }

	/// <summary>
	/// Decreases enemy's health by an amount
	/// </summary>
	/// <param name="damage">
	/// How much to decrease enemy's health
	/// </param>
	public void TakeDamage(float damage, BaseCharacter character)
	{
		Health -= damage;

		if (Health <= 0)
		{
			Die();
		}

		DisplayDamage(damage);
		
		IndicateHit(character);
	}

	/// <summary>
	/// Changes enemy's renderer's material colour to red for .2 seconds
	/// </summary>
	public void IndicateHit(BaseCharacter character)
	{
        Renderer.material.color = Color.red;
        StartCoroutine(ResetColour(.2f));

        switch (character.CharacterType)
        {
            case CharacterType.KENRON:
                if (GameManager.Instance.Kenron.isActive && GameManager.Instance.Kenron.m_skillUpgrades.Find(skill => skill.Name == "Shuras Reckoning"))
                {
                    if(m_burningEffect != null)
                    {
                        m_burningEffect.Play();
                    }
                    else
                    {
                        Debug.LogFormat("{0} is null", m_burningEffect.name);
                    }
                }
                else
                {
                    if(m_bloodEffect != null)
                    {
                        m_bloodEffect.Play();
                    }
                    else
                    {
                        Debug.LogFormat("{0} is null", m_bloodEffect.name);
                    }
                    m_burningEffect.Stop();
                }
                break;

            case CharacterType.NASHORN:
                if (m_electricEffect != null)
                {
                    m_electricEffect.Play();
                }
                else
                {
                    Debug.LogFormat("{0} is null", m_electricEffect.name);
                }
                break;

            case CharacterType.THEA:
                if (m_waterEffect != null)
                {
                    m_waterEffect.Play();
                }
                else
                {
                    Debug.LogFormat("{0} is null", m_waterEffect.name);
                }
                break;

            default:
                m_waterEffect.Stop();
                m_burningEffect.Stop();
                m_electricEffect.Stop();
                m_bloodEffect.Stop();
                break;
        }
	}

	IEnumerator ResetColour(float duration)
	{
		yield return new WaitForSeconds(duration);
		Renderer.material.color = Color.clear;
	}

	void DisplayDamage(float damage)
	{
		// spawn new damage indicator
		DamageIndicator damageIndicator = Instantiate(m_damageIndicator, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
		damageIndicator.Init(damage);
	}

    IEnumerator DissolveLerp(float time)
    {
        float lerp = 0f;
        Stunned = true;
        while (lerp < 0.5f)
        {
            yield return new WaitForEndOfFrame();
            lerp = Mathf.Lerp(Renderer.material.GetFloat("_DissolveAmount"), 1f, time * Time.deltaTime);
            Renderer.material.SetFloat("_DissolveAmount", lerp);
        }
        Destroy(gameObject);
    }

	/// <summary>
	/// Performs all functionality involved with an enemy death
	/// </summary>
	public void Die()
	{
        StartCoroutine(DissolveLerp(1));
	}

	/// <summary>
	/// Executes enemy's pending behaviour
	/// </summary>
	/// <returns>
	/// The result of the behaviour
	/// </returns>
	public Behaviour.Result ExecutePendingBehaviour()
	{
		return PendingBehaviour.Execute(this);
	}

	public void SetAnimatorFloat(string id, float value)
	{
		if (m_animator)
		{
			m_animator.SetFloat(id, value);
		}
	}

	public void SetAnimatorBool(string id, bool value)
	{
		if (m_animator)
		{
			m_animator.SetBool(id, value);
		}
	}

	public void SetAnimatorTrigger(string id)
	{
		if (m_animator)
		{
			m_animator.SetTrigger(id);
		}
	}

	void OnAttackAnimation()
	{
		OnAttackDelegate?.Invoke(this);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyEnemyCollision")
        {
            if (m_knockedBack == true)
            {
                other.GetComponentInParent<EnemyData>().TakeDamage(GameManager.Instance.Nashorn.KDKnockbackDamage, GameManager.Instance.Nashorn);
            }
        }
    }
}

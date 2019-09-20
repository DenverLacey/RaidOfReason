/*
 * Author: Denver
 * Description: EnemyProjectile class that handles functionality for RangeEnemy's
 *              projectiles
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Projectile object fired by the RangeEnemy
/// </summary>
public class EnemyProjectile : MonoBehaviour
{
    [Tooltip("Time until projectile despawns.")]
    [SerializeField] 
    private float m_lifetime;

    [Tooltip("How fast the projectile will travel.")]
    [SerializeField] 
    private float m_speed;

	[Tooltip("Maximum angle of inaccuracy")]
	[SerializeField]
	private float m_maxInaccuracyAngle;

    private float m_timer;
    private float m_damage;

	private EnemyData m_parent;

    // Start is called before the first frame update
    void Start() 
    {
        m_timer = m_lifetime;

		// introduce inaccuracy to shot
		float angle = Random.Range(-m_maxInaccuracyAngle, m_maxInaccuracyAngle);
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.up) * transform.rotation;
    }

    // Update is called once per frame
    void Update() 
    {
        transform.Translate(Vector3.forward * m_speed * Time.deltaTime);
        m_timer -= Time.deltaTime;

        if (m_timer <= 0.0f) 
        {
			KillProjectile();
        }
    }

	/// <summary>
	/// Initialises Enemy projectile
	/// </summary>
	/// <param name="damage">
	/// How much damage projectile will inflict
	/// </param>
	/// <param name="parent">
	/// Who fired projectile
	/// </param>
    public void Init(float damage, EnemyData parent)
	{
		m_damage = damage;
		m_parent = parent;
	}

    /// <summary>
    /// Deals damage to collided object if hit player
    /// </summary>
    /// <param name="other">
    /// Collider of the object that projectile collided with
    /// </param>
    void OnTriggerEnter(Collider other) 
    {
		// if hit player
        if (Utility.TagIsPlayerTag(other.tag)) 
        {
            BaseCharacter player = other.GetComponent<BaseCharacter>();
            player.TakeDamage(m_damage);
        }

		// if hit some object
		if (other.tag != "EnemyManager" && other.tag != "Enemy" && other.name != "projectile(Clone)" && other.name != "Wall Detector")
		{
			KillProjectile();
		}
    }

	void KillProjectile()
	{
		Destroy(gameObject);
	}
}

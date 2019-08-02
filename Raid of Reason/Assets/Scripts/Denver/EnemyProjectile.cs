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

    private float m_timer;
    private int m_damage;

	private EnemyData m_parent;

    // Start is called before the first frame update
    void Start() 
    {
        m_timer = m_lifetime;
    }

    // Update is called once per frame
    void Update() 
    {
        transform.Translate(Vector3.forward * m_speed * Time.deltaTime);
        m_timer -= Time.deltaTime;

        if (m_timer <= 0.0f) 
        {
            Destroy(gameObject);
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
    public void Init(int damage, EnemyData parent)
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
        if (other.tag == "Kenron" || other.tag == "Thea" || other.tag == "Nashorn") 
        {
            BaseCharacter player = other.GetComponent<BaseCharacter>();
            player.TakeDamage(m_damage);
            if (other.tag == "Nashorn")
                m_parent.isAttackingNashorn = true;          
        }
        
		if (other.tag != "EnemyManager")
		{
			Destroy(gameObject);
		}

    }
}

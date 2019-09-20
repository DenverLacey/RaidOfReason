using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Author: Elisha_Anagnostakis
 * Description: This class handles the speed at which thea's projectiles 
 *              travel within the world when shot.
 */

public class ProjectileMove : MonoBehaviour {

    [SerializeField]
    private float m_projectileLife;
    [SerializeField]
    private float m_projectileSpeed;
    [SerializeField]
    private float m_healAmount;

	private bool m_hasHitKenron;
	private bool m_hasHitNashorn;

    private void Start()
    {
        Invoke("Destroy", m_projectileLife);
    }

    private void OnDisable()
    {
        m_hasHitKenron = false;
        m_hasHitNashorn = false;
    }


    // Update is called once per frame
    void Update () {

        // If projectile is active
		if(m_projectileSpeed != 0)
        {
            // Project the objects transform forward 
            var forward = transform.InverseTransformDirection(transform.forward);
            transform.Translate(forward * m_projectileSpeed * Time.deltaTime);
        }
	}

    public void Destroy()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// This function resloves what happens when Theas projectile collides 
    /// with enemies, allies or the enviornment.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
	{
        Physics.IgnoreCollision(GameManager.Instance.Thea.GetComponent<Collider>(), other);

        if (other.gameObject.tag == "Enemy")
        {
            EnemyData enemy = other.gameObject.GetComponent<EnemyData>();
           
            if (enemy)
            {
                enemy.TakeDamage(GameManager.Instance.Thea.GetDamage(), GameManager.Instance.Thea);
                             
            }
		}
        else if ((other.gameObject.tag == "Kenron" && !m_hasHitKenron) || (other.gameObject.tag == "Nashorn" && !m_hasHitNashorn))
        {
			if (other.tag == "Kenron")
			{
				m_hasHitKenron = true;
			}
			else if (other.tag == "Nashorn")
			{
				m_hasHitNashorn = true;
			}

			BaseCharacter hitPlayer = other.gameObject.GetComponent<BaseCharacter>();

			if (hitPlayer.playerState == BaseCharacter.PlayerState.ALIVE)
			{
				hitPlayer.m_currentHealth += m_healAmount;

				if (GameManager.Instance.Thea.m_statManager)
				{
					GameManager.Instance.Thea.m_statManager.damageHealed += m_healAmount;
				}
            }
		}
    }
}
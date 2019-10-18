using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SwordDamage : MonoBehaviour
{
	private Collider m_collider;
    public float damageDealtInDash;

  
	private void Start()
	{
        m_collider = GetComponent<Collider>();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
            damageDealtInDash += GameManager.Instance.Kenron.GetDamage();

			EnemyData enemy = other.gameObject.GetComponent<EnemyData>();
            enemy.TakeDamage(GameManager.Instance.Kenron.GetDamage(), GameManager.Instance.Kenron);

            enemy.IndicateHit(GameManager.Instance.Kenron);

            if (enemy.Health <= 0) {
                enemy.isDeadbByKenron = true;
                GameManager.Instance.Kenron.SkillChecker();
                enemy.isDeadbByKenron = false;
            }
		}
	}
}
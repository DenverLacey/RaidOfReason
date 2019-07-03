using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
   [SerializeField]
    private int m_damage;
    public Kenron Kenron;

	public void SetDamage(int damage)
	{
		this.m_damage = damage;
	}
	public void OnTriggerEnter(Collider other)
	{
		string tag = other.gameObject.tag;
		if (tag == "Enemy")
		{
			BaseEnemy enemy = other.gameObject.GetComponent<BaseEnemy>();

			if (enemy)
			{
				SetDamage(m_damage);
				enemy.TakeDamage(m_damage);
                if (Kenron.playerSkills.Find(skill => skill.Name == "Shuras Reckoning") && Kenron.playerSkills.Count < 0) {
                    if (Kenron.isActive == true)
                    {
                        other.GetComponent<StatusEffectManager>().ApplyBurn(4);
                    }
                }
			}

			Debug.Log("attack successful " + tag);
		}
		else
		{
			Debug.Log("attack unsuccessful " + tag);
		}
	}
}

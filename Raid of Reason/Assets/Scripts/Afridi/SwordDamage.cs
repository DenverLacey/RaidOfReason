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

            // if the player doesnt have shuras upgrade applied
			if (enemy && !Kenron.m_playerSkills.Find(skill => skill.Name == "Shuras Reckoning"))
			{
				SetDamage(m_damage);
				enemy.TakeDamage(m_damage);
                enemy.GetComponent<MeshRenderer>().material.color = Color.red;
                StartCoroutine(ResetMaterialColour(enemy, .2f));
			}

            // if the player does have shuras upgrade applied
            if(enemy && Kenron.m_playerSkills.Find(skill => skill.Name == "Shuras Reckoning") && Kenron.isActive == true)
            {
                Debug.Log("BURNING");
                other.GetComponent<StatusEffectManager>().ApplyBurn(4);
            }

			Debug.Log("attack successful " + tag);
		}
		else
		{
			Debug.Log("attack unsuccessful " + tag);
		}
	}

    IEnumerator ResetMaterialColour(BaseEnemy enemy, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (enemy)
        {
            enemy.GetComponent<MeshRenderer>().material.color = Color.clear;
        } 
    }
}
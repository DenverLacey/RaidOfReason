using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			EnemyData enemy = other.gameObject.GetComponent<EnemyData>();
            enemy.TakeDamage(GameManager.Instance.Kenron.GetDamage());

            // if the player doesnt have shuras upgrade applied
            if (enemy && !GameManager.Instance.Kenron.m_playerSkills.Find(skill => skill.Name == "Shuras Reckoning"))
			{
				enemy.TakeDamage(GameManager.Instance.Kenron.GetDamage());
                enemy.GetComponent<MeshRenderer>().material.color = Color.red;
                StartCoroutine(ResetMaterialColour(enemy, 0.2f));
			}

            // if the player does have shuras upgrade applied
            if(enemy && GameManager.Instance.Kenron.m_playerSkills.Find(skill => skill.Name == "Shuras Reckoning") && GameManager.Instance.Kenron.isActive == true)
            {
                other.GetComponent<StatusEffectManager>().ApplyBurn(4);
            }

            if (enemy.Health <= 0) {
                enemy.isDeadbByKenron = true;
                GameManager.Instance.Kenron.SkillChecker();
                enemy.isDeadbByKenron = false;
            }

            //if (kenron.nashornBuffGiven == true && nashorn.isTaunting == true)
            //{
            //    float randomValue = Random.value;
            //    if (nashorn.stunChance < randomValue)
            //    {
            //        other.GetComponent<StatusEffectManager>().ApplyStun(1.5f);
            //    }
            //}
            //else if (nashorn.isTaunting == false) {
            //    kenron.nashornBuffGiven = false;
            //}

		}
	}

    IEnumerator ResetMaterialColour(EnemyData enemy, float delay)
    {
        yield return new WaitForSeconds(delay);

		if (enemy)
		{
			MeshRenderer renderer = enemy.GetComponent<MeshRenderer>();
			if (renderer)
			{
				renderer.material.color = Color.clear;
			}
		}
    }
}
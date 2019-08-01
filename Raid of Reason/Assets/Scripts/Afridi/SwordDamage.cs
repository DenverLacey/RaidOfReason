using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
	[SerializeField]
    private int m_damage;
    public Kenron kenron;
    public Nashorn nashorn;

	public void OnTriggerEnter(Collider other)
	{
		string tag = other.gameObject.tag;
		if (tag == "Enemy")
		{
			EnemyData enemy = other.gameObject.GetComponent<EnemyData>();

            // if the player doesnt have shuras upgrade applied
			if (enemy && !kenron.m_playerSkills.Find(skill => skill.Name == "Shuras Reckoning"))
			{
				enemy.TakeDamage(m_damage);
                enemy.GetComponent<MeshRenderer>().material.color = Color.red;
                StartCoroutine(ResetMaterialColour(enemy, .2f));
			}

            // if the player does have shuras upgrade applied
            if(enemy && kenron.m_playerSkills.Find(skill => skill.Name == "Shuras Reckoning") && kenron.isActive == true)
            {
                Debug.Log("BURNING");
                other.GetComponent<StatusEffectManager>().ApplyBurn(4);
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
			Debug.Log("attack successful " + tag);
		}
		else
		{
			Debug.Log("attack unsuccessful " + tag);
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
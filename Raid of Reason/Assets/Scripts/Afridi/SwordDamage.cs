﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SwordDamage : MonoBehaviour
{
	private Collider m_collider;

	private void Start()
	{
		m_collider = GetComponent<Collider>();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			EnemyData enemy = other.gameObject.GetComponent<EnemyData>();
            enemy.TakeDamage(GameManager.Instance.Kenron.GetDamage());

            // if the player doesnt have shuras upgrade applied
            if (enemy && !GameManager.Instance.Kenron.m_skillUpgrades.Find(skill => skill.Name == "Shuras Reckoning"))
			{
				enemy.IndicateHit();
			}

            // if the player does have shuras upgrade applied
            if(enemy && GameManager.Instance.Kenron.m_skillUpgrades.Find(skill => skill.Name == "Shuras Reckoning") && GameManager.Instance.Kenron.isActive == true)
            {
                other.GetComponent<StatusEffectManager>().ApplyBurn(4);
            }

            if (enemy.Health <= 0) {
                enemy.isDeadbByKenron = true;
                GameManager.Instance.Kenron.SkillChecker();
                enemy.isDeadbByKenron = false;
            }
		}
	}
}
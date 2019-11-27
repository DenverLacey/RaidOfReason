﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Afridi Rahim
 * Description: Handles Kenrons Damage 
 * Last Edited: 15/11/2019
*/
[RequireComponent(typeof(Collider))]
public class SwordDamage : MonoBehaviour
{
    /// <summary>
    /// This Trigger enables Kenron To Do Damage
    /// </summary>
    /// <param name="other">The other object the player passes through</param>
	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
		{
			EnemyData enemy = other.gameObject.GetComponent<EnemyData>();
        
            // Deals Damage with respective Effect
            enemy.TakeDamage(GameManager.Instance.Kenron.GetDamage(), GameManager.Instance.Kenron);

			// Kenros Skill Merge
			if (GameManager.Instance.Kenron.isSkillActive)
			{
				GameManager.Instance.Kenron.AddHealth(GameManager.Instance.Kenron.healthGained);
				GameManager.Instance.Kenron.skillManager.m_mainSkills[0].m_currentDuration -= GameManager.Instance.Kenron.durationIncreased;
			}
		}
	}
}
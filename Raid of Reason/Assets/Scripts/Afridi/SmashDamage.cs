using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using XInputDotNetPure;
/*
 * Author: Afridi Rahim, Denver Lacey, Elisha Anagnostakis
 * Description: Handles the Damage, KnockBack and Stun time for Kriegers Damage
 * Last Edited: 15/11/2019
     */
public class SmashDamage : MonoBehaviour
{ 
    private float m_rumbleDuration = 0.1f;
    private float m_rumbleIntensity = 1000f;

    /// <summary>
    /// Handles how kuch damage/knockback/stun is dealt
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
			EnemyData enemy = other.gameObject.GetComponent<EnemyData>();
            // Knock back enemies with every punch Kreiger lands.
            Rigidbody rb = other.GetComponent<Rigidbody>();

            // Checks if the enemy is already in the list 
            if (GameManager.Instance.Kreiger.m_hitEnemies.Contains(enemy))
                return;
            else
                // Adds the enemy to the list when hit
            GameManager.Instance.Kreiger.m_hitEnemies.Add(enemy);
            // Gives shield
            GameManager.Instance.Kreiger.currentShield += GameManager.Instance.Kreiger.shieldGain;
            // Controller vibration
            DoRumble();

           Vector3 direction = other.transform.position - GameManager.Instance.Kreiger.transform.position;        
            
            // Knocks Back and Stuns Enemies
            enemy.KnockBack(direction.normalized * GameManager.Instance.Kreiger.knockBackForce, GameManager.Instance.Kreiger.stunTime);
            enemy.TakeDamage(GameManager.Instance.Kreiger.GetDamage(), GameManager.Instance.Kreiger);
        }
    }

    /// <summary>
    /// Applys Rumble
    /// </summary>
    public void DoRumble()
    {
        GamePad.SetVibration(GameManager.Instance.Kreiger.playerIndex, m_rumbleIntensity, m_rumbleIntensity);
        StartCoroutine(StopRumble());
    }

    /// <summary>
    /// Disable Rumble
    /// </summary>
    /// <returns>Duration till rumble has to stop</returns>
    public IEnumerator StopRumble()
    {
        yield return new WaitForSeconds(m_rumbleDuration);
        GamePad.SetVibration(GameManager.Instance.Kreiger.playerIndex, 0f, 0f);
    }
}
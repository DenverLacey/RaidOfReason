using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Afridi Rahim
 *
 *  Summary:
 *  This Script is used for Kenrons Final ability that spawns an ethereal version of himself
*/

public class ChildKenron : MonoBehaviour
{
    //Main Kenron that is being played
    [Tooltip("Kenron that is being Played")]
    public Kenron m_Kenron;
    [Tooltip("The Aethereal Kenron Spawned after Death")]
    //This Current Instance
    public ChildKenron m_Child;

    //Temporary Addition
    private float timer = 4.0f;


    public void Update()
    {
        // Right now this is a timer, but how it works is that once Main Kenron is revived, the Child Kenron will still be active
        if (m_Kenron.m_currentHealth <= 0.0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                //Enables Main Kenron with Weak stats and disables Child Kenron
                m_Child.gameObject.SetActive(false);
                m_Kenron.enabled = true;
                m_Kenron.gameObject.SetActive(true);
                m_Kenron.SetHealth(40);
                m_Kenron.SetDamage(35);
                
            }
        }
    }

    /// <summary>
    /// - This function checks the status (health) of the Kenron in the game
    /// </summary>
    public void CheckStatus() {

        if (m_Kenron.m_currentHealth <= 0.0f) {
            m_Kenron.enabled = false;
        }       
    }    
}

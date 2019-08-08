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
    public Kenron Kenron;
    [Tooltip("The Aethereal Kenron Spawned after Death")]
    //This Current Instance
    public ChildKenron Child;

    //Temporary Addition
    [Tooltip("Time until Main Kenron Comes back")]
    public float timer = 4.0f;


    public void Update()
    {
        // Right now this is a timer, but how it works is that once Main Kenron is revived, the Child Kenron will still be active
        if (Kenron.m_currentHealth <= 0.0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                //Enables Main Kenron with Weakened stats and disables Child Kenron
                Child.gameObject.SetActive(false);
                Kenron.enabled = true;
                Kenron.gameObject.SetActive(true);
                Kenron.SetHealth(40);
                Kenron.SetDamage(35);
            }
        }
    }

    /// <summary>
    /// This function checks the status (health) of the Main Kenron in the game
    /// </summary>
    public void CheckStatus()
    {
        // If main kenrons health is 0
        if (Kenron.m_currentHealth <= 0.0f)
        {
            // disable him (Should be replaced with down animation)
            Kenron.enabled = false;
        }       
    }    
}

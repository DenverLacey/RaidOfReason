using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Afridi Rahim, Denver Lacey
 * Description: Handles How the Objectives Start Up
 */
[RequireComponent(typeof(BoxCollider))]
public class TriggerObjective : MonoBehaviour
{
    private ObjectiveManager m_objectiveManager;
    private bool m_playerHere = false;

    private void Awake()
    {
        // Intialise Collider and Manager
        m_objectiveManager = FindObjectOfType<ObjectiveManager>();
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }

    /// <summary>
    /// This handles the trigger of the objective. If at least one player passes through the trigger the objective starts up
    /// </summary>
    /// <param name="other">The other object in collision</param>
    void OnTriggerEnter(Collider other)
    {
        // Checks if it is a player
        if (Utility.TagIsPlayerTag(other.tag))
        {
            m_playerHere = true;
        }

        // if its a player
        if (m_playerHere)
        {
            // trigger
            m_objectiveManager.ObjectiveTriggered = true;
            m_playerHere = false;
            this.gameObject.SetActive(false);
        }
    }
}

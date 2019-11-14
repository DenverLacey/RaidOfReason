/*
 * Author: Denver
 * Description:	Handles functionality of the respawn triggers
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Updates the RespawnManager's respawn point when a player enters the trigger
/// </summary>
[RequireComponent(typeof(Collider))]
public class RespawnTrigger : MonoBehaviour
{
    [Tooltip("Transform of the spawn point")]
    [SerializeField]
    private Transform m_spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (Utility.TagIsPlayerTag(other.tag))
        {
            RespawnManager.UpdateSpawnPoint(m_spawnPoint.position);
        }
    }
}

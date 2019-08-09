using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
  * Author: Afridi Rahim
  * 
  * Summary:
  * This Script is here to count how many enemies are spawned per room.
*/
public class EnemyCounter : MonoBehaviour
{
    //The Current Game Manager
    [Tooltip("Game Manager that is being Used in the Game")]
    public GameManager m_Manager;

    /// <summary>
    /// Initialises the Game Manager
    /// </summary>
    /// <param name="manager"> The Game Manager in the Current Game </param>
    public void Init(GameManager manager)
    {
        m_Manager = manager;
    }

    /// <summary>
    /// Destroys the Current Enemy from the Manager
    /// </summary>
    public void OnDestroy()
    {
        // Destroys the current enemy
        // m_Manager.OnEnemyDestroyed(this);
    }

}
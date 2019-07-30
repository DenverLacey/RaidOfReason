using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
  * Author: Afridi Rahim
  *  
  * Summary:
  * This Script manages the game loop. Controlling the Enemy Spawn rates at thier 
  * specified spawn points. By clearing rooms the players gain points
*/
public class GameManager : MonoBehaviour
{
	private GameManager()
	{
		if (m_instance == null)
		{
			m_instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private static GameManager m_instance = null;
	public static GameManager Instance { get => m_instance; }

    [Tooltip("Number Of Enemies allowed to spawn in the room")]
    public int numOfEnemies;
    // The current amount of enemies spawned in the room
    private int m_currentEnemiesInRoom = 0;

    // All Three Players Within the Game
    private Kenron m_Kenron;
    private Nashorn m_Nashorn;
    private Theá m_Thea;

	public BaseCharacter[] Players { get; private set; }

    [Tooltip("The Explosive Enemy")]
    public GameObject explosiveEnemy;
    [Tooltip("How Long it takes to spawn an Explosive Type")]
    public int explosiveSpawnRate;

    [Tooltip("The Melee Enemy")]
    public GameObject meleeEnemy;
    [Tooltip("How Long it takes to spawn an Melee Type")]
    public int meleeSpawnRate;

    [Tooltip("The Ranged Enemy")]
    public GameObject rangedEnemy;
    [Tooltip("How Long it takes to spawn an Ranged Type")]
    public int rangedSpawnRate;

    [Tooltip("The Horde Enemy")]
    public GameObject hordeEnemy;
    [Tooltip("How Long it takes to spawn an Horde Type")]
    public int hordeSpawnRate;

    [Tooltip("The Number Of Spawn Points in a Room")]
    public Transform[] spawnPoints;
     
    // Checks if the maximum number of enemies have been spawned
    private bool m_capReached = false;

    void Start()
    {
        //If the maximum amount of enemies haven't been spawned
        if (m_capReached == false)
        {
            // Repedeately Spawns The Types of enemies depnding on type and Rate
            InvokeRepeating("SpawnMelee", meleeSpawnRate, meleeSpawnRate);
            InvokeRepeating("SpawnRanged", rangedSpawnRate, rangedSpawnRate);
            InvokeRepeating("SpawnExplosive", explosiveSpawnRate, explosiveSpawnRate);
            InvokeRepeating("SpawnHorde", hordeSpawnRate, hordeSpawnRate);
        }
        // Sets the current amount to 0
        m_currentEnemiesInRoom = 0;

        // Finds All Three Players within the game
        m_Kenron = FindObjectOfType<Kenron>();
        m_Nashorn = FindObjectOfType<Nashorn>();
        m_Thea = FindObjectOfType<Theá>();

		Players = new BaseCharacter[] { m_Kenron, m_Nashorn, m_Thea };
    }

    void Update()
    {
        //If the current amount is equal to the maximum number of enemies
        if (m_currentEnemiesInRoom == numOfEnemies)
        {
            // Cancels the Spawn Functions
            CancelInvoke("SpawnMelee");
            CancelInvoke("SpawnExplosive");
            CancelInvoke("SpawnRanged");
            CancelInvoke("SpawnHorde");

            //Cap has been reached is set to true
            m_capReached = true;
        }

        //If the maximum amount has been reached and current enemies are 0
        if (m_capReached == true && m_currentEnemiesInRoom == 0)
        {
            // Calls Clear Room
            ClearedRoom();
        }
    }

    /// <summary>
    /// Spawns the Melee Enemy Type with its own Spawn Time
    /// </summary>
    public void SpawnMelee()
    {
        // If the current amount is greater than the maximum
        if (m_currentEnemiesInRoom >= numOfEnemies)
        {
            // Return false cancelling the function
            return;
        }

        // Randomly spawns the Enemy at a range of 0 and the amount of spawn points there are
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        // Create a new instance of the enemy at the spawn point 
        GameObject temp = Instantiate(meleeEnemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

        // Initialises the current instance of enemy as an EnemyCounter 
        temp.AddComponent<EnemyCounter>().Init(this);

        // Current Number of enemies in the room is increased
        m_currentEnemiesInRoom++;
    }


    /// <summary>
    /// Spawns the Explosive Enemy Type with its own Spawn Time
    /// </summary>
    public void SpawnExplosive()
    {
        // If the current amount is greater than the maximum
        if (m_currentEnemiesInRoom >= numOfEnemies)
        {
            // Return false cancelling the function
            return;
        }

        // Randomly spawns the Enemy at a range of 0 and the amount of spawn points there are
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        // Create a new instance of the enemy at the spawn point 
        GameObject temp = Instantiate(explosiveEnemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

        // Initialises the current instance of enemy as an EnemyCounter 
        temp.AddComponent<EnemyCounter>().Init(this);

        // Current Number of enemies in the room is increased
        m_currentEnemiesInRoom++;
    }
    

    /// <summary>
    /// Spawns the Ranged Enemy Type with its own Spawn Time
    /// </summary>
    public void SpawnRanged()
    {
        // If the current amount is greater than the maximum
        if (m_currentEnemiesInRoom >= numOfEnemies)
        {
            // Return false cancelling the function
            return;
        }

        // Randomly spawns the Enemy at a range of 0 and the amount of spawn points there are
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        // Create a new instance of the enemy at the spawn point 
        GameObject temp = Instantiate(rangedEnemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

        // Initialises the current instance of enemy as an EnemyCounter 
        temp.AddComponent<EnemyCounter>().Init(this);

        // Current Number of enemies in the room is increased
        m_currentEnemiesInRoom++;
    }


    /// <summary>
    /// Spawns the Horde Enemy Type with its own Spawn Time
    /// </summary>
    public void SpawnHorde()
    {
        // If the current amount is greater than the maximum
        if (m_currentEnemiesInRoom >= numOfEnemies)
        {
            // Return false cancelling the function
            return;
        }

        // Randomly spawns the Enemy at a range of 0 and the amount of spawn points there are
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        // Create a new instance of the enemy at the spawn point 
        GameObject temp = Instantiate(hordeEnemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

        // Initialises the current instance of enemy as an EnemyCounter 
        temp.AddComponent<EnemyCounter>().Init(this);

        // Current Number of enemies in the room is increased
        m_currentEnemiesInRoom++;
    }


    /// <summary>
    /// Decrements the Current Number of Enemies in the room
    /// </summary>
    /// <param name="enemy"> The Enemy Killed </param>
    public void OnEnemyDestroyed(EnemyCounter enemy) {
        m_currentEnemiesInRoom--;
    }

    /// <summary>
    /// The Victory Condition. When A room Is Cleared Each player gets a skill point and the room is reset
    /// </summary>
    public void ClearedRoom()
    {
        // Resets Room and gives all player a point
        m_capReached = false;
        m_Kenron.m_playerSkillPoints++;
        m_Nashorn.m_playerSkillPoints++;
        m_Thea.m_playerSkillPoints++;
    }
}

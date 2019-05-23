using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject melee;
    public GameObject ranged;
    public GameObject suicide;

    public float meleeSpawnTime;
    public float rangedSpawnTime;
    public float suicideSpawnTime;

    public Transform[] spawnPoints;         // An array of the spawn points this enemy can spawn from.

    void Start()
    {
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating("SpawnMelee", meleeSpawnTime, meleeSpawnTime);
        InvokeRepeating("SpawnRanged", rangedSpawnTime, rangedSpawnTime);
        InvokeRepeating("SpawnSuicide", suicideSpawnTime, suicideSpawnTime);
    }


    void SpawnMelee()
    {
        // Find a random index between zero and one less than the number of spawn points.
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        melee.gameObject.SetActive(true);
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        Instantiate(melee, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }

    void SpawnRanged()
    {
        // Find a random index between zero and one less than the number of spawn points.
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        ranged.gameObject.SetActive(true);
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        Instantiate(ranged, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }

    void SpawnSuicide()
    {
        // Find a random index between zero and one less than the number of spawn points.
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);

        suicide.gameObject.SetActive(true);
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
        Instantiate(suicide, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}

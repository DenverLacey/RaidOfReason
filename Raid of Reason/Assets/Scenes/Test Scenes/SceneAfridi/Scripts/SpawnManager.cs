using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private BaseCharacter m_baseCharacter;
    public Transform[] m_spawnPoints;

    public GameObject m_Suicide;
    public GameObject m_Melee;
    public GameObject m_Ranged;
    public float m_suicideSpawnTime;
    public float m_meleeSpawnTime;
    public float m_rangedSpawnTime;

    void Start()
    {
        InvokeRepeating("SpawnMelee", m_meleeSpawnTime, m_meleeSpawnTime);
        InvokeRepeating("SpawnSuicide", m_suicideSpawnTime, m_suicideSpawnTime);
        InvokeRepeating("SpawnRanged", m_rangedSpawnTime, m_rangedSpawnTime);
    }

    void SpawnMelee() {
        int spawnPointIndex = Random.Range(0, m_spawnPoints.Length);
        m_Melee.SetActive(true);
        Instantiate(m_Melee, m_spawnPoints[spawnPointIndex].position, m_spawnPoints[spawnPointIndex].rotation);
    }

    void SpawnSuicide()
    {
        int spawnPointIndex = Random.Range(0, m_spawnPoints.Length);
        m_Suicide.SetActive(true);
        Instantiate(m_Suicide, m_spawnPoints[spawnPointIndex].position, m_spawnPoints[spawnPointIndex].rotation);
    }

    void SpawnRanged()
    {
        int spawnPointIndex = Random.Range(0, m_spawnPoints.Length);
        m_Ranged.SetActive(true);
        Instantiate(m_Ranged, m_spawnPoints[spawnPointIndex].position, m_spawnPoints[spawnPointIndex].rotation);
    }
}

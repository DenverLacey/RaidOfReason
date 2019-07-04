using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int numOfEnemies;
    private int m_currentEnemiesInRoom = 0;

    private Kenron m_Kenron;
    private Nashorn m_Nashorn;
    private Theá m_Thea;

    public GameObject m_Explosive;
    public int ExplosiveSpawnRate;

    public GameObject m_Melee;
    public int MeleeSpawnRate;

    public GameObject m_Ranged;
    public int RangedSpawnRate;

    public GameObject m_Horde;
    public int HordeSpawnRate;

    public Transform[] spawnPoints;
    private bool capReached = false;

    private void Start()
    {
        if (capReached == false)
        {
            InvokeRepeating("SpawnMelee", MeleeSpawnRate, MeleeSpawnRate);
            InvokeRepeating("SpawnRanged", RangedSpawnRate, RangedSpawnRate);
            InvokeRepeating("SpawnExplosive", ExplosiveSpawnRate, ExplosiveSpawnRate);
            InvokeRepeating("SpawnHorde", HordeSpawnRate, HordeSpawnRate);
        }
        m_currentEnemiesInRoom = 0;
    }

    private void Awake()
    {
        m_Kenron = FindObjectOfType<Kenron>();
        m_Nashorn = FindObjectOfType<Nashorn>();
        m_Thea = FindObjectOfType<Theá>();
    }

    private void Update()
    {
        if (m_currentEnemiesInRoom == numOfEnemies) {
            CancelInvoke("SpawnMelee");
            CancelInvoke("SpawnExplosive");
            CancelInvoke("SpawnRanged");
            CancelInvoke("SpawnHorde");
            capReached = true;
        }
        if (capReached == true && m_currentEnemiesInRoom == 0) {
            ClearedRoom();
        }
    }

    public void SpawnMelee()
    {
        if (m_currentEnemiesInRoom >= numOfEnemies)
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        GameObject temp = Instantiate(m_Melee, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        temp.AddComponent<EnemyCounter>().Init(this);
        m_currentEnemiesInRoom++;
    }

    public void SpawnExplosive()
    {
        if (m_currentEnemiesInRoom >= numOfEnemies)
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        GameObject temp = Instantiate(m_Explosive, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        temp.AddComponent<EnemyCounter>().Init(this);
        m_currentEnemiesInRoom++;
    }

    public void SpawnRanged()
    {
        if (m_currentEnemiesInRoom >= numOfEnemies)
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        GameObject temp = Instantiate(m_Ranged, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        temp.AddComponent<EnemyCounter>().Init(this);
        m_currentEnemiesInRoom++;
    }
    public void SpawnHorde()
    {
        if (m_currentEnemiesInRoom >= numOfEnemies)
        {
            return;
        }

        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        GameObject temp = Instantiate(m_Horde, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        temp.AddComponent<EnemyCounter>().Init(this);
        m_currentEnemiesInRoom++;
    }


    public void OnEnemyDestroyed(EnemyCounter enemy) {
        m_currentEnemiesInRoom--;
    }

    public void ClearedRoom() {
        capReached = false;
        m_Kenron.m_playerSkillPoints++;
        m_Nashorn.m_playerSkillPoints++;
        m_Thea.m_playerSkillPoints++;
    }
}

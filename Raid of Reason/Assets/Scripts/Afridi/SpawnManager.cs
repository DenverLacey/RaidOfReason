using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private BaseCharacter m_baseCharacter;
    public Transform m_spawnPoints;

    public GameObject m_Suicide;
    public GameObject m_Melee;
    public GameObject m_Ranged;

    public void SpawnMelee() {
        m_Melee.SetActive(true);
        Instantiate(m_Melee, m_spawnPoints.position, m_spawnPoints.rotation);
    }

    public void SpawnSuicide()
    {
        m_Suicide.SetActive(true);
        Instantiate(m_Suicide, m_spawnPoints.position, m_spawnPoints.rotation);
    }

    public void SpawnRanged()
    {
        m_Ranged.SetActive(true);
        Instantiate(m_Ranged, m_spawnPoints.position, m_spawnPoints.rotation);
    }
}

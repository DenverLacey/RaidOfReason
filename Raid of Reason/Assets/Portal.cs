using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour
{
    public GameObject m_dependentSpawner;
    public GameObject m_particleSystem;
    public BoxCollider m_collider;

    private int m_phase;

    private void Start()
    {
        m_particleSystem.SetActive(false);
    }

    private void Update()
    {
        if (m_phase == 0 && m_dependentSpawner == null)
        {
            m_particleSystem.SetActive(true);
            m_phase = 1;
        }

        if (m_phase == 1 && AllPlayersInPortal())
        {
            LevelManager.FadeLoadNextLevel();
            m_phase = 2;
        }
    }

    private bool AllPlayersInPortal()
    {
        float minx = m_collider.bounds.min.x;
        float maxx = m_collider.bounds.max.x;
        float minz = m_collider.bounds.min.z;
        float maxz = m_collider.bounds.max.z;

        foreach (var player in GameManager.Instance.AllPlayers)
        {
            if (!(player.transform.position.x <= maxx && player.transform.position.x >= minx &&
                player.transform.position.z <= maxz && player.transform.position.z >= minz))
            {
                return false;
            }
        }
        return true;
    }
}

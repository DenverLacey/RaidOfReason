using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    public GameManager m_Manager;

    public void Init(GameManager manager)
    {
        m_Manager = manager;
    }

    public void OnDestroy()
    {
        m_Manager.OnEnemyDestroyed(this);
    }

}
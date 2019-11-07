using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierManager : MonoBehaviour
{
    private ObjectiveManager m_ObjManager;
    public List<GameObject> Barriers = new List<GameObject>();

    void Awake()
    {
        m_ObjManager = FindObjectOfType<ObjectiveManager>();
        foreach (GameObject obj in Barriers)
        {
            obj.SetActive(true);
        }
    }

    public void ManageBarriers()
    {
        if (m_ObjManager.m_currentObjective.name == "Countdown To Destruction_1")
        {
            if (m_ObjManager.ObjectiveCompleted == true)
            {
                Barriers[0].SetActive(false);
            }
        }
    }

}

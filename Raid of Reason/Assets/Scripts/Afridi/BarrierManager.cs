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
        if (m_ObjManager.m_currentObjective.name == "Countdown To Destruction_1" && m_ObjManager.objectiveComplete)
        {
            Barriers[0].SetActive(false);
            Barriers[1].SetActive(false);
        }
        if (m_ObjManager.m_currentObjective.name == "Protect The Crystal_1")
        {
            Barriers[1].SetActive(true);
            if (m_ObjManager.objectiveComplete)
            {
                Barriers[1].SetActive(false);
                Barriers[2].SetActive(false);
            }
        }
        if (m_ObjManager.m_currentObjective.name == "Countdown To Destruction_2")
        {
            Barriers[2].SetActive(true);
            if (m_ObjManager.objectiveComplete)
            {
                Barriers[2].SetActive(false);
                Barriers[3].SetActive(false);
            }
        }
        if (m_ObjManager.m_currentObjective.name == "Protect The Crystal_2")
        {
            Barriers[4].SetActive(true);
            if (m_ObjManager.objectiveComplete)
            {
                Barriers[4].SetActive(false);
            }
        }
    }

}

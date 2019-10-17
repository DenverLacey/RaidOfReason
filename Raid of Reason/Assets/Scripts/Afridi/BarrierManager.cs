using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierManager : MonoBehaviour
{
    private ObjectiveManager m_ObjManager;
    public List<GameObject> Barriers = new List<GameObject>();

    void Awake()
    {
        foreach (GameObject obj in Barriers)
        {
            obj.SetActive(false);
        }
    }

    public void ManageBarriers()
    {
        if (m_ObjManager.m_currentObjective.name == "Countdown To Destruction_1" && m_ObjManager.objectiveComplete)
        {
            Barriers[0].SetActive(false);
        }
        if (m_ObjManager.m_currentObjective.name == "Protect The Crystal_1" && m_ObjManager.objectiveComplete)
        {
            Barriers[1].SetActive(false);
        }
        if (m_ObjManager.m_currentObjective.name == "Countdown To Destruction_2" && m_ObjManager.objectiveComplete)
        {
            Barriers[2].SetActive(false);
        }
    }

}

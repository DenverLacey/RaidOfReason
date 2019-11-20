using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Afridi Rahim
 * Description: Handles The Barriers via the completion of objectives
 * Last Edited: 15/11/2019
*/
public class BarrierManager : MonoBehaviour
{
    private ObjectiveManager m_ObjManager;
    public List<GameObject> Barriers = new List<GameObject>();

    void Awake()
    {
        // Initalise the barriers and Manager 
        m_ObjManager = FindObjectOfType<ObjectiveManager>();
        foreach (GameObject obj in Barriers)
        {
            obj.SetActive(true);
        }
    }

    /// <summary>
    /// This Function Makes sure that after the specific Objective is done it turns off the barriers
    /// </summary>
    public void ManageBarriers()
    {
       // Turn specified barrier off
       if (m_ObjManager.ObjectiveCompleted == true)
       {
           Barriers[0].SetActive(false);
       }
    }

}

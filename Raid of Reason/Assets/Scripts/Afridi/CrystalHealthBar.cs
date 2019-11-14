using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * Author: Afridi Rahim
 * Description: Handles the Health Ui
 * Last Edited: 15/11/2019
 */
public class CrystalHealthBar : MonoBehaviour
{
    public Image healthBar;
    private ObjectiveManager m_manager;
    public List<ProtectionObjective> Objects = new List<ProtectionObjective>();
    private void Awake()
    {
        // Init
        healthBar.gameObject.SetActive(false);
        m_manager = FindObjectOfType<ObjectiveManager>();
    }

    private void Update()
    {
        // if we have at least 1 Objective in the List
        if (Objects.Count == 1)
        {
            // if the current objective is this and has been triggered but completed
            if (m_manager.currentObjective == Objects[0] && !m_manager.currentObjective.Completed() && m_manager.ObjectiveTriggered == true)
            {
                // Set Health
                healthBar.gameObject.SetActive(true);
                healthBar.fillAmount = Objects[0].currentHealth / Objects[0].health;
            }

            // if the current objective is this and has failed
            if (m_manager.currentObjective == Objects[0] && m_manager.currentObjective.Failed())
            {
                // Turn off
                healthBar.gameObject.SetActive(false);
            }

            // if the current objective is this and has been completed
            if (m_manager.currentObjective == Objects[0] && m_manager.currentObjective.Completed())
            {
                // Turn off
                healthBar.gameObject.SetActive(false);
            }
        }
    }
}

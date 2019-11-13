using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalHealth : MonoBehaviour
{
    public Image healthBar;
    private ObjectiveManager manager;
    public List<ProtectionObjective> Objects = new List<ProtectionObjective>();
    private void Awake()
    {
        healthBar.gameObject.SetActive(false);
        manager = FindObjectOfType<ObjectiveManager>();
    }

    private void Update()
    {
        if (Objects.Count == 1)
        {
            if (manager.m_currentObjective == Objects[0] && !manager.m_currentObjective.IsDone() && manager.ObjectiveTriggered == true)
            {
                healthBar.gameObject.SetActive(true);
                healthBar.fillAmount = Objects[0].m_currentHealth / Objects[0].health;
            }

            if (manager.m_currentObjective == Objects[0] && manager.m_currentObjective.HasFailed())
            {
                healthBar.gameObject.SetActive(false);
            }
        }
    }
}

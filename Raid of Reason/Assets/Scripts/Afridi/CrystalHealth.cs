using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalHealth : MonoBehaviour
{
    public Image healthBar;
    private ObjectiveManager manager;
    public List<ProtectionObjective> Objects = new List<ProtectionObjective>();
    public float upAmount;
    private void Awake()
    {
        manager = FindObjectOfType<ObjectiveManager>();
    }
    private void Update()
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        screenPoint.y += upAmount;
        healthBar.transform.position = screenPoint;

        if (manager.m_currentObjective == Objects[0] && !manager.m_currentObjective.IsDone())
        {
            healthBar.fillAmount = Objects[0].m_currentHealth / Objects[0].health;
        }
        else if (manager.m_currentObjective == Objects[0] && manager.m_currentObjective.IsDone())
        {
            healthBar.fillAmount = 0;
            Objects.RemoveAt(0);
        }
    }
}

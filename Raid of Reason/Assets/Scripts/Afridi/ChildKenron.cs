using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildKenron : MonoBehaviour
{
    public Kenron m_Kenron;
    public ChildKenron m_Child;
    private float timer = 4.0f;
    public void Update()
    {
        if (m_Kenron.m_currentHealth <= 0.0f)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                m_Child.gameObject.SetActive(false);
                m_Kenron.enabled = true;
                m_Kenron.gameObject.SetActive(true);
                m_Kenron.SetHealth(40);
                m_Kenron.SetDamage(35);
                
            }
        }
    }

    public void CheckStatus() {

        if (m_Kenron.m_currentHealth <= 0.0f) {
            m_Kenron.enabled = false;
        }       
    }    
}

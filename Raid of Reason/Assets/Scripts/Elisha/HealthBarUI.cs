using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 
 * Author: Elisha_Anagnostakis
 * Description: Handles updating each characters’ health bar.
 */

public class HealthBarUI : MonoBehaviour
{
    [SerializeField]
    private BaseCharacter m_character;

    [SerializeField]
    private Image m_healthBar;

    [SerializeField]
    private Image m_shieldBar;

    [SerializeField]
    private Image m_overhealBar;

    void Awake()
    {
        m_healthBar.gameObject.SetActive(true);
        m_overhealBar.gameObject.SetActive(false);
        m_shieldBar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If player is true.
        if (m_character)
        {
            // This will output visually how much health the players have.
            m_healthBar.fillAmount = m_character.m_currentHealth / m_character.m_maxHealth;

            if (m_character.m_currentHealth > m_character.m_maxHealth)
            {
                m_overhealBar.gameObject.SetActive(true);
                m_overhealBar.fillAmount = m_character.m_currentHealth / m_character.m_maxHealth;
            }
            else
            {
                m_overhealBar.gameObject.SetActive(false);
            }

            if (m_character.currentShield > 0)
            {
                m_shieldBar.gameObject.SetActive(true);
                m_shieldBar.fillAmount = m_character.currentShield / m_character.m_maxShield;
            }
            else
            {
                m_shieldBar.gameObject.SetActive(false);
            }
        }
        else {
            // Players health is 0.
            m_healthBar.fillAmount = 0;
        }
    }
}
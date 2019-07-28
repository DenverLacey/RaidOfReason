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
    private Image m_healthBar;

    // Start is called before the first frame update
    void Start()
    {
        m_healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // If player is true.
        if (m_character)
        {
            // This will output visually how much health the players have.
            m_healthBar.fillAmount = m_character.m_currentHealth / m_character.m_maxHealth;
        }
        else {
            // Players health is 0.
            m_healthBar.fillAmount = 0;
        }
    }
}
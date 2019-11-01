using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/* 
 * Author: Elisha Anagnostakis, Afridi Rahim
 * Description: Handles updating each characters’ health bar.
 */

public class HealthBarUI : MonoBehaviour
{
    [Header("--Health Bar UI--")]

    [SerializeField]
    private BaseCharacter m_character;

    [SerializeField]
    private Image m_healthBar;

    [SerializeField]
    private Image m_healthUI;

    [SerializeField]
    private Image m_damagedHealth;

    [SerializeField]
    private Image m_shieldBar;

    [SerializeField]
    private GameObject[] m_playerUI;

    [SerializeField]
    [Tooltip("How much the damaged health sprite will lerp when damaged.")]
    private float m_lerpAmount = 0.2f;


    [Header("--Critical Health Image Settings--")]

    [SerializeField]
    private Image m_criticalHealthImage;
    [SerializeField]
    private float m_duration;
    [SerializeField]
    [Range(0f, 1f)]
    private float m_criticalPercentageThreshold = 0.5f;
    private Color imageWithAlpha;
    private float m_prevHealth;
    private bool m_isCritical = false;

    void Awake()
    {
        m_healthBar.gameObject.SetActive(true);
        m_shieldBar.gameObject.SetActive(false);
        m_prevHealth = m_character.m_currentHealth;
        m_character.onTakeDamage += HealthBarShake;
        imageWithAlpha = m_criticalHealthImage.color;
        m_criticalHealthImage.color = new Color(m_criticalHealthImage.color.r, m_criticalHealthImage.color.g, m_criticalHealthImage.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // If player is true.
        if (m_character)
        {
            if (m_character.m_currentHealth > 0)
            {
                // This will output visually how much health the players have.
                m_healthBar.fillAmount = m_character.m_currentHealth / m_character.m_maxHealth;
                if (m_prevHealth != m_character.m_currentHealth)
                {
                    OnDelayDone();
                }
            }
            else
            {
                m_healthBar.fillAmount = 0;
                m_damagedHealth.fillAmount = 0;
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

            if (m_healthBar.fillAmount <= m_criticalPercentageThreshold)
            {
                m_criticalHealthImage.enabled = true;
                //PlayerUIShake(m_character);
                if (!m_isCritical)
                {
                    m_isCritical = true;
                    m_criticalHealthImage.DOColor(imageWithAlpha, m_duration).SetLoops(-1, LoopType.Yoyo);
                }
                
                if (m_healthBar.fillAmount <= 0)
                {
                    m_criticalHealthImage.enabled = false;
                    m_criticalHealthImage.DOKill(true);
                }
            }
        }
    }

    /// <summary>
    /// When players health gets damaged, the damaged health sprite will slowly decrease fill amount.
    /// </summary>
    public void OnDelayDone()
    {
        m_damagedHealth.fillAmount = Mathf.Lerp(m_damagedHealth.fillAmount, m_healthBar.fillAmount, m_lerpAmount);

        if (Mathf.Abs(m_damagedHealth.fillAmount - m_healthBar.fillAmount) <= 0.01f)
        {
            m_damagedHealth.fillAmount = m_healthBar.fillAmount - 0.1f;
            m_prevHealth = m_character.m_currentHealth;
        }
    }

    private void HealthBarShake(BaseCharacter player)
    {
        transform.DOKill(true);
        transform.DOPunchPosition(Vector3.right * 3 * 3f, .3f, 10, 1);
    }

    private void PlayerUIShake(BaseCharacter player)
    {
        // checks for every object that exists in the players UI.
        foreach(GameObject i in m_playerUI)
        {
            i.transform.DOKill(true);
            i.transform.DOPunchPosition(Vector3.down * 3 * 3f, .3f, 10, 1);
        }
    }
}
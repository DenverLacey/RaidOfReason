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
    [SerializeField]
    private BaseCharacter m_character;

    [SerializeField]
    private Image m_healthBar;

    public Image m_healthUI;

    [SerializeField]
    private Image m_damagedHealth;

    [SerializeField]
    [Tooltip("How much the damaged health sprite will lerp when damaged.")]
    private float m_lerpAmount = 0.2f;

    [SerializeField]
    private Image m_shieldBar;

    [SerializeField]
    private Image m_criticalHealthImage;
    [SerializeField]
    private float m_duration;
    private Color imageColorNoAlpha;

    private float m_prevHealth;

    void Awake()
    {

        m_healthBar.gameObject.SetActive(true);
        m_shieldBar.gameObject.SetActive(false);
        m_prevHealth = m_character.m_currentHealth;
        m_character.onTakeDamage += HealthBarShake;

        m_criticalHealthImage.GetComponent<Image>();
        imageColorNoAlpha = m_criticalHealthImage.color;
        imageColorNoAlpha.a = 0;
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

            if (m_healthBar.fillAmount <= 0.3f)
            {
                m_criticalHealthImage.DOColor(imageColorNoAlpha, m_duration).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }

    /// <summary>
    /// When players health gets damaged, the damaged health sprite will slowly decrease fill amount.
    /// </summary>
    public void OnDelayDone()
    {
        m_damagedHealth.fillAmount = Mathf.Lerp(m_damagedHealth.fillAmount, m_healthBar.fillAmount, m_lerpAmount);

        if (m_damagedHealth.fillAmount == m_healthBar.fillAmount)
        {
            m_damagedHealth.fillAmount = m_healthBar.fillAmount;
            m_prevHealth = m_character.m_currentHealth;
        }
    }

    private void HealthBarShake(BaseCharacter player)
    {
        transform.DOKill(true);
        transform.DOPunchPosition(Vector3.right * 3 * 3f, .3f, 10, 1);
    }
}
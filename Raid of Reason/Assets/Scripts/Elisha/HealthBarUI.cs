/* 
 * Author: Elisha Anagnostakis
 * Description: This script handles all the mechanics that the player health bars do such as shield increase / damage,
 * health increase / damage, and critical health state
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    [Tooltip("How much the damaged health sprite will lerp when damaged.")]
    private float m_lerpAmount = 0.2f;
    
    [Header("--Critical Health Image Settings--")]

    [SerializeField]
    private Image m_criticalHealthImage;

    [SerializeField]
    private Image m_criticalHealthBarFlash;

    [SerializeField]
    private float m_duration;

    [SerializeField]
    [Range(0f, 1f)]
    private float m_criticalPercentageThreshold = 0.5f;

    private Color m_colourWithAlpha;
    private Color m_colourWithoutAlpha;

    private Color m_healthColourWithAlpha;
    private Color m_healthColourWithoutAlpha;

    private float m_prevHealth;
    private bool m_isCritical = false;

    void Awake()
    {
        m_healthBar.gameObject.SetActive(true);
        m_shieldBar.gameObject.SetActive(false);
        m_prevHealth = m_character.currentHealth;
        m_character.onTakeDamage += HealthBarShake;

        // critical red ring flash 
        m_colourWithAlpha = m_criticalHealthImage.color;
        m_criticalHealthImage.color = new Color(m_criticalHealthImage.color.r, m_criticalHealthImage.color.g, m_criticalHealthImage.color.b, 0f);
        m_colourWithoutAlpha = m_criticalHealthImage.color;

        // health bar flashes white 
        m_healthColourWithAlpha = m_criticalHealthBarFlash.color;
        m_criticalHealthBarFlash.color = new Color(m_criticalHealthBarFlash.color.r, m_criticalHealthBarFlash.color.g, m_criticalHealthBarFlash.color.b, 0f);
        m_healthColourWithoutAlpha = m_criticalHealthBarFlash.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_character)
        {
            // checks if the player has health
            if (m_character.currentHealth > 0)
            {
                // This will output visually how much health the players have
                m_healthBar.fillAmount = m_character.currentHealth / m_character.m_maxHealth;
                if (m_prevHealth != m_character.currentHealth)
                {
                    OnDelayDone();
                }
            }
            else
            {
                // if player is dead set the fill amounts to 0
                m_healthBar.fillAmount = 0;
                m_damagedHealth.fillAmount = 0;
            }

            // checks if the player has shield
            if (m_character.currentShield > 0)
            {
                // turns on the shield sprite
                m_shieldBar.gameObject.SetActive(true);
                // This will output visually how much shield the players have
                m_shieldBar.fillAmount = m_character.currentShield / m_character.m_maxShield;
            }
            else
            {
                // turns off the shield sprite
                m_shieldBar.gameObject.SetActive(false);
            }

            // checks if the health bar is greater than the set threshold that then becomes critical health
            if ((m_healthBar.fillAmount > m_criticalPercentageThreshold && m_isCritical == true) || m_character.playerState != BaseCharacter.PlayerState.ALIVE)
            {
                // Resets all critical heath attributes
                m_criticalHealthImage.enabled = false;
                m_isCritical = false;
                m_criticalHealthImage.DOKill();
                m_criticalHealthImage.color = m_colourWithoutAlpha;

                m_criticalHealthBarFlash.enabled = false;
                m_criticalHealthBarFlash.DOKill(true);
                m_criticalHealthBarFlash.color = m_healthColourWithoutAlpha;
            }
            else if (m_healthBar.fillAmount <= m_criticalPercentageThreshold && m_isCritical == false)
            {
                // enables the critical health sprite
                m_criticalHealthImage.enabled = true;
                // sets the flag to true 
                m_isCritical = true;
                // loops image alpha between 1 and 0
                m_criticalHealthImage.DOColor(m_colourWithAlpha, m_duration).SetLoops(-1, LoopType.Yoyo);

                m_criticalHealthBarFlash.enabled = true;
                // flashes the critical health image within a loop
                m_criticalHealthBarFlash.DOColor(m_healthColourWithAlpha, m_duration).SetLoops(-1, LoopType.Yoyo);
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
            m_prevHealth = m_character.currentHealth;
        }
    }

    /// <summary>
    /// Heath bar shake using DO Tweening to punch the bars transform when players get damaged.
    /// </summary>
    /// <param name="player"></param>
    private void HealthBarShake(BaseCharacter player)
    {
        transform.DOKill(true);
        transform.DOPunchPosition(Vector3.right * 3 * 3f, .3f, 10, 1);
    }
}
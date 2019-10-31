using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using XboxCtrlrInput;

/*
  * Author: Afridi Rahim, Denver Lacey
  *
  * Summary: Controls the Skills of All Players
  *          Includes thier cooldowns and skill icons
*/

[System.Serializable]
public class Skills
{
	public UnityEvent m_reset;

    [Tooltip("Time until Skill can be used again")]
	public float m_coolDown;

    [Tooltip("Duration of the skills usetime")]
    public float m_duration;

    [HideInInspector]

    public float m_currentCoolDown;
    [HideInInspector]
    public float m_currentDuration;

    [Tooltip("Player Ability Image")]
    public Image m_skillIcon;

    [HideInInspector]
    public bool active = false;

    [HideInInspector]
    public bool onCooldown = false;

    public void RunTimer()
	{
		m_currentCoolDown += Time.deltaTime;
        m_currentDuration += Time.deltaTime;
		m_skillIcon.fillAmount = m_currentCoolDown / m_coolDown;

        if (m_currentCoolDown >= m_coolDown)
        {
            onCooldown = false;          
        }
        if (m_currentDuration >= m_duration && active)
        {
            if (m_reset != null) m_reset.Invoke();
            active = false;
        }
	}
}

public class SkillManager : MonoBehaviour {

    [Tooltip("A List of How Many Main Skills the Players Have")]
    public List<Skills> m_mainSkills;

    private PauseMenu m_pauseInfo;

    protected void Awake()
    {
        m_pauseInfo = FindObjectOfType<PauseMenu>();
        // Intialisation 
        foreach (var skill in m_mainSkills)
        {
            // Sets the Cooldown
            skill.m_currentCoolDown = skill.m_coolDown;
            skill.m_currentDuration = skill.m_duration;
        }
    }

    /// <summary>
    /// Checks the specific player and thier button presses, According to the player each ability will
    /// activate, reset and the await another turn
    /// </summary>
    protected void Update()
    {
        if (m_pauseInfo.m_isPaused)
            return;

        // Empty Check
        if (GameManager.Instance.Kenron != null)
        {
            // If the first players left trigger is pressed
            if (XCI.GetAxis(XboxAxis.LeftTrigger, GameManager.Instance.Kenron.controller) > 0.1f)
            {
                // if the current cooldown is greater than the main
                if (m_mainSkills[0].m_currentCoolDown >= m_mainSkills[0].m_coolDown)
                {
                    // Activate Ability
                    GameManager.Instance.Kenron.ChaosFlame(m_mainSkills[0].m_currentDuration);

                    // Skill is Reset and has been Activated
                    m_mainSkills[0].m_currentCoolDown = 0;
                    m_mainSkills[0].m_currentDuration = 0;
                    m_mainSkills[0].active = true;
                    m_mainSkills[0].onCooldown = true;
                }
            }
        }

        // Empty Check
        if (GameManager.Instance.Kreiger != null)
        {
            // If the second players left trigger is pressed
            if (XCI.GetAxis(XboxAxis.LeftTrigger, GameManager.Instance.Kreiger.controller) > 0.1f)
            {
                // if the current cooldown is greater than the main
                if (m_mainSkills[1].m_currentCoolDown >= m_mainSkills[1].m_coolDown)
                {
                    // Activate Ability
                    GameManager.Instance.Kreiger.Spott(m_mainSkills[1].m_currentDuration);

                    // Skill is Reset and has been Activated
                    m_mainSkills[1].m_currentCoolDown = 0;
                    m_mainSkills[1].m_currentDuration = 0;
                    m_mainSkills[1].active = true;
                    m_mainSkills[1].onCooldown = true;
                }
            }
        }

        // Empty Check
        if (GameManager.Instance.Thea != null)
        {
            // If the third players left trigger is pressed
            if (XCI.GetAxis(XboxAxis.LeftTrigger, GameManager.Instance.Thea.controller) > 0.1f && GameManager.Instance.Thea.playerState == BaseCharacter.PlayerState.ALIVE)
            {
                if(m_mainSkills[2].m_currentCoolDown >= m_mainSkills[2].m_coolDown)
                {
                    // Activate Ability
                    GameManager.Instance.Thea.GiftOfPoseidon(m_mainSkills[2].m_currentDuration);
                    m_mainSkills[2].active = true;
                } 
            }
            else if(XCI.GetAxis(XboxAxis.LeftTrigger,GameManager.Instance.Thea.controller) < 0.1)
            {
                if (m_mainSkills[2].active)
                {
                    // Skill is Reset and has been Deactivated
                    m_mainSkills[2].m_currentCoolDown = 0;
                    m_mainSkills[2].m_currentDuration = 0;
                    m_mainSkills[2].active = false;
                    m_mainSkills[2].onCooldown = true;
                    GameManager.Instance.Thea.EndGIftOfPoseidon();
                }
            }
        }

        foreach (var skill in m_mainSkills)
        {
            // If a skill has been reset
            if (skill.onCooldown)
            {
                // Run the cooldown for that skill
                skill.RunTimer();
            }
        }
    }
}
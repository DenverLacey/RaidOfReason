using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using XboxCtrlrInput;

/*
  * Author: Afridi Rahim
  *
  * Summary: Controls the Skills of All Players
  *          Includes thier cooldowns and skill icons
*/

[System.Serializable]
public class Skills
{
	public UnityEvent m_reset;
	public float m_coolDown;
    public float m_currentCoolDown;
    public Image m_skillIcon;
    public bool active = false;
    public bool reset = false;

	public void RunTimer()
	{
		m_currentCoolDown += Time.deltaTime;
		m_skillIcon.fillAmount = m_currentCoolDown / m_coolDown;

		if (m_currentCoolDown >= m_coolDown)
		{
			if (m_reset != null) m_reset.Invoke();
            active = false;
            reset = false;
		}
	}
}

public class SkillManager : MonoBehaviour {

    [Tooltip("A List of How Many Main Skills the Players Have")]
    public List<Skills> m_Skills;

    // All Thre Players
    public Kenron m_Kenron;
    public Theá m_theá;
    public Nashorn m_Nashorn;

    protected void Awake()
    {
        // Intialisation 
        foreach (var skill in m_Skills)
        {
            // Sets the Cooldown
            skill.m_currentCoolDown = skill.m_coolDown;
        }

		m_theá = FindObjectOfType<Theá>();
		m_Kenron = FindObjectOfType<Kenron>();
        m_Nashorn = FindObjectOfType<Nashorn>();
    }

    /// <summary>
    /// Checks the specific player and thier button presses, According to the player each ability will
    /// activate, reset and the await another turn
    /// </summary>
    protected void Update()
    {
        // If the first players left trigger is pressed
		if (XCI.GetAxis(XboxAxis.LeftTrigger, XboxController.First) > 0.1f)
        {
            // if the current cooldown is greater than the main
			if (m_Skills[0].m_currentCoolDown >= m_Skills[0].m_coolDown)
			{
                // Activate Ability
				m_Kenron.ChaosFlame();

                // Skill is Reset and has been Activated
				m_Skills[0].m_currentCoolDown = 0;
				m_Skills[0].active = true;
                m_Skills[0].reset = true;
			}
        }

        // If the second players left trigger is pressed
        if (XCI.GetAxis(XboxAxis.LeftTrigger, XboxController.Second) > 0.1f)
        {
            // if the current cooldown is greater than the main
            if (m_Skills[1].m_currentCoolDown >= m_Skills[1].m_coolDown)
            {
                // Activate Ability
                m_Nashorn.Spott();

                // Skill is Reset and has been Activated
                m_Skills[1].m_currentCoolDown = 0;
                m_Skills[1].active = true;
                m_Skills[1].reset = true;
            }
        }

        // If the third players left trigger is pressed
        if (XCI.GetAxis(XboxAxis.LeftTrigger, XboxController.Third) > 0.1f)
        {
            // if the current cooldown is greater than the main
            if (m_Skills[2].m_currentCoolDown >= m_Skills[2].m_coolDown)
            {
                // Activate Ability
                m_theá.GiftOfPoseidon();
                // Skill is Active
                m_Skills[2].active = true;
            }
        } 
        // or else if the third players trigger is not pressed
        else if (XCI.GetAxis(XboxAxis.LeftTrigger, XboxController.Third) < 0.1f)
        {
            if (m_Skills[2].active)
            {
                // Skill is Reset and has been Deactivated
                m_Skills[2].m_currentCoolDown = 0;
                m_Skills[2].active = false;
                m_Skills[2].reset = true;

                // Grants health and resets
                m_theá.GiveHealth();
                m_theá.ResetGiftOfPoseidon();
            }
        }

        foreach (var skill in m_Skills)
        {
            // If a skill has been reset
            if (skill.reset)
            {
                // Run the cooldown for that skill
                skill.RunTimer();
            }
        }
    }

}

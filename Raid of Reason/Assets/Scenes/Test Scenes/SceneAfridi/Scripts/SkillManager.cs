using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using XboxCtrlrInput;

[System.Serializable]
public class Skills
{
	public UnityEvent m_reset;

	public float m_coolDown;
    public float m_currentCoolDown;
    public Image m_skillIcon;
    internal bool active = false;

	public void RunTimer()
	{
		m_currentCoolDown += Time.deltaTime;
		m_skillIcon.fillAmount = m_currentCoolDown / m_coolDown;

		if (m_currentCoolDown >= m_coolDown)
		{
			if (m_reset != null) m_reset.Invoke();
			active = false;
		}
	}
}

public class SkillManager : MonoBehaviour {

    public List<Skills> m_Skills;
    public _KenronMain m_Kenron;
    public Theá m_theá;
    protected XboxController controller;

    void Start()
    {
        foreach (var skill in m_Skills)
        {
            skill.m_currentCoolDown = skill.m_coolDown;
        }
		m_theá = FindObjectOfType<Theá>();
		m_Kenron = FindObjectOfType<_KenronMain>();
    }

    public void FixedUpdate()
    {
		if (XCI.GetButtonDown(XboxButton.LeftBumper, XboxController.Second))
		{
			if (m_Skills[0].m_currentCoolDown >= m_Skills[0].m_coolDown)
			{
				m_Kenron.FlashFire();
				m_Skills[0].m_currentCoolDown = 0;
				m_Skills[0].active = true;
			}

		}
		if (XCI.GetButtonDown(XboxButton.RightBumper, XboxController.Second))
		      {
				if (m_Skills[1].m_currentCoolDown >= m_Skills[1].m_coolDown)
				{
					m_Kenron.ChaosFlame();
					m_Skills[1].m_currentCoolDown = 0;
					m_Skills[1].active = true;
				}
        }
        if (XCI.GetButtonDown(XboxButton.RightBumper, XboxController.First))
        {
            if (m_Skills[2].m_currentCoolDown >= m_Skills[2].m_coolDown)
            {
                m_theá.UltimateAbility();
                m_Skills[2].m_currentCoolDown = 0;
                m_Skills[2].active = true;
            }
        }
    }

    public void Update()
    {
        foreach (var skill in m_Skills)
        {
            if (skill.active)
            {
				skill.RunTimer();          
            }
        }
    }
}

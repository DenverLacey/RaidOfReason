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

    public List<Skills> m_Skills;
    public Kenron m_Kenron;
    public Theá m_theá;
    public Nashorn m_Nashorn;

    void Start()
    {
        foreach (var skill in m_Skills)
        {
            skill.m_currentCoolDown = skill.m_coolDown;
        }
		m_theá = FindObjectOfType<Theá>();
		m_Kenron = FindObjectOfType<Kenron>();
        m_Nashorn = FindObjectOfType<Nashorn>();
    }

    public void FixedUpdate()
    {
		if (XCI.GetAxis(XboxAxis.LeftTrigger, XboxController.First) > 0.1f)
        {
			if (m_Skills[0].m_currentCoolDown >= m_Skills[0].m_coolDown)
			{
				m_Kenron.ChaosFlame();
				m_Skills[0].m_currentCoolDown = 0;
				m_Skills[0].active = true;
                m_Skills[0].reset = true;
			}
        }
        if (XCI.GetAxis(XboxAxis.LeftTrigger, XboxController.Second) > 0.1f)
        {
            if (m_Skills[1].m_currentCoolDown >= m_Skills[1].m_coolDown)
            {
                m_Nashorn.Spott();
                m_Skills[1].m_currentCoolDown = 0;
                m_Skills[1].active = true;
                m_Skills[1].reset = true;
            }
        }
        else if (XCI.GetAxis(XboxAxis.LeftTrigger, XboxController.Second) <= 0.1f)
        {
			if (m_Nashorn) {
				m_Nashorn.m_Collider.SetActive(false);
			}
        }
        if (XCI.GetAxis(XboxAxis.LeftTrigger, XboxController.Third) > 0.1f)
        {
            if (m_Skills[2].m_currentCoolDown >= m_Skills[2].m_coolDown)
            {
                m_theá.GiftOfPoseidon();
                m_Skills[2].active = true;
            }
        }
        else if (XCI.GetAxis(XboxAxis.LeftTrigger, XboxController.Third) < 0.1f)
        {
            if (m_Skills[2].active)
            {
                m_Skills[2].m_currentCoolDown = 0;
                m_Skills[2].active = false;
                m_Skills[2].reset = true;

                m_theá.GiveHealth();
                m_theá.ResetGiftOfPoseidon();
            }
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        foreach (var skill in m_Skills)
        {
            if (skill.reset)
            {
				skill.RunTimer();          
            }
        }
    }
}

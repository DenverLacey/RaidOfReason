using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using XboxCtrlrInput;

[System.Serializable]
public class Skills
{
    public float m_coolDown;
    //[HideInInspector]
    public float m_currentCoolDown;
    public Image m_skillIcon;
    internal bool active = false;
}

public class SkillManager : MonoBehaviour {

    public List<Skills> m_Skills;
    public _KenronMain m_Kenron;
    protected XboxController controller;

    void Start()
    {
        foreach (var skill in m_Skills)
        {
            skill.m_currentCoolDown = skill.m_coolDown;
        }
    }

    public void FixedUpdate()
    {
        if (XCI.GetButtonDown(XboxButton.B, controller)) { 
            if (m_Skills[0].m_currentCoolDown >= m_Skills[0].m_coolDown) {
                m_Kenron.FlashFire();
                m_Skills[0].m_currentCoolDown = 0;
                m_Skills[0].active = true;
            }         
        }
        if (XCI.GetButtonDown(XboxButton.Y, controller))
        {
            if (m_Skills[1].m_currentCoolDown >= m_Skills[1].m_coolDown)
            {
                m_Kenron.ChaosFlame();
                m_Skills[1].m_currentCoolDown = 0;
                m_Skills[1].active = true;
            }
        }
    }

    public void Update()
    {
        foreach (var skill in m_Skills)
        {
            if (skill.active)
            {
                if (skill.m_currentCoolDown < skill.m_coolDown)
                {
                    skill.m_currentCoolDown += Time.deltaTime;
                    skill.m_skillIcon.fillAmount = skill.m_currentCoolDown / skill.m_coolDown;
                }
                if (skill.m_currentCoolDown >= skill.m_coolDown)
                {
                    m_Kenron.ResetSkill();
                    m_Skills[0].active = false;
                    m_Skills[1].active = false;
                }
            }
        }
    }
}

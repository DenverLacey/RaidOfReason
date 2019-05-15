using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using XboxCtrlrInput;

[System.Serializable]
public class TheaSkills
{
    public float m_coolDown;
    [HideInInspector]
    public float m_currentCoolDown;
    public Image m_skillIcon;
}

public class TheaSkillManager : MonoBehaviour
{

    public List<Skills> m_Skills;
    public Theá m_thea;

    void Start()
    {
        foreach (var skill in m_Skills)
        {
            skill.m_currentCoolDown = skill.m_coolDown;
        }
    }

    public void FixedUpdate()
    {
        if (XCI.GetButtonDown(XboxButton.Y))
        {
            if (m_Skills[0].m_currentCoolDown >= m_Skills[0].m_coolDown)
            {
                m_thea.UltimateAbility();
                m_Skills[0].m_currentCoolDown = 0;
            }
        }
    }

    public void Update()
    {
        foreach (var skill in m_Skills)
        {
            if (skill.m_currentCoolDown < skill.m_coolDown)
            {
                skill.m_currentCoolDown += Time.deltaTime;
                skill.m_skillIcon.fillAmount = skill.m_currentCoolDown / skill.m_coolDown;
            }
        }
    }
}
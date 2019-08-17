using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpgrade : MonoBehaviour
{
    public SkillsAbilities skill;
    public ObjectiveManager objective;

    [SerializeField]
    private BaseCharacter m_Player;

    private void Awake()
    {
        m_Player.onGameChange += BoolChange;
        TurnOffSkillIcon();
    }

    private void Update()
    {
        GetSkill();
    }

    public void GetSkill()
    {
        if (skill.CheckSkill(m_Player, objective))
        {
            TurnOnSkillIcon();
        }
    }

    public void EnableSkills()
    {
        if (m_Player && skill & skill.EnableSkill(m_Player))
        {
            TurnOnSkillIcon();
        }
        else if (m_Player && skill && skill.CheckSkill(m_Player, objective))
        {
            this.transform.Find("IconParent").Find("Disabled").gameObject.SetActive(false);
        }
        else
        {
            TurnOffSkillIcon();
        }
    }


    private void TurnOnSkillIcon()
    {
        this.transform.Find("IconParent").Find("Available").gameObject.SetActive(false);
        this.transform.Find("IconParent").Find("Disabled").gameObject.SetActive(false);
    }

    private void TurnOffSkillIcon()
    {
        this.transform.Find("IconParent").Find("Available").gameObject.SetActive(true);
        this.transform.Find("IconParent").Find("Disabled").gameObject.SetActive(true);
    }

    void BoolChange()
    {
        EnableSkills();
    }
}

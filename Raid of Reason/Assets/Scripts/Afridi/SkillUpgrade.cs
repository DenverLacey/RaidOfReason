using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpgrade : MonoBehaviour
{
    public List<SkillsAbilities> m_characterSkills = new List<SkillsAbilities>();
    public ObjectiveManager objective;
    public BaseCharacter m_Player;

    private void Update()
    {
        EnableSkill();
    }

    void EnableSkill()
    {
        if (m_Player)
        {
            if (objective.tempCleared == true)
            {
                if (m_Player.m_skillUpgrades.Count == 0)
                {
                    m_Player.m_skillUpgrades.Add(m_characterSkills[0]);
                }
                else if (m_Player.m_skillUpgrades.Count == 1)
                {
                    m_Player.m_skillUpgrades.Add(m_characterSkills[1]);
                }
                else if (m_Player.m_skillUpgrades.Count == 2)
                {
                    m_Player.m_skillUpgrades.Add(m_characterSkills[2]);
                }
                else if (m_Player.m_skillUpgrades.Count == 3)
                {
                    m_Player.m_skillUpgrades.Add(m_characterSkills[3]);
                }
            }
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
}

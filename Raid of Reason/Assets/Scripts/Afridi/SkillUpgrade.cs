using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpgrade : MonoBehaviour
{
    public List<SkillsAbilities> m_characterSkills = new List<SkillsAbilities>();
    public List<GameObject> m_UIObjects = new List<GameObject>();
    public ObjectiveManager objective;
    public BaseCharacter player;
    private bool WaitTillNextScene;

    private void Awake()
    {
        WaitTillNextScene = true;
    }

    private void Update()
    {
        EnableSkill();
    }

    void EnableSkill()
    {
        if (player)
        {
            if (objective.ObjectiveCompleted == true)
            {
                if (player.m_skillUpgrades.Count == 0 && WaitTillNextScene)
                {
                    player.AddSkillUpgrade(m_characterSkills[0]);
                    m_UIObjects[0].gameObject.SetActive(true);
                    objective.ObjectiveCompleted = false;
                    WaitTillNextScene = false;
                }
                else if (player.m_skillUpgrades.Count == 1 && WaitTillNextScene)
                {
                    player.AddSkillUpgrade(m_characterSkills[1]);
                    m_UIObjects[1].gameObject.SetActive(true);
                    objective.ObjectiveCompleted = false;
                    WaitTillNextScene = false;
                }
                else if (player.m_skillUpgrades.Count == 2 && WaitTillNextScene)
                {
                    player.AddSkillUpgrade(m_characterSkills[2]);
                    m_UIObjects[2].gameObject.SetActive(true);
                    objective.ObjectiveCompleted = false;
                    WaitTillNextScene = false;
                }
                else if (player.m_skillUpgrades.Count == 3 && WaitTillNextScene)
                {
                    player.AddSkillUpgrade(m_characterSkills[3]);
                    m_UIObjects[3].gameObject.SetActive(true);
                    objective.ObjectiveCompleted = false;
                    WaitTillNextScene = false;
                }
            }
        }
    }
}

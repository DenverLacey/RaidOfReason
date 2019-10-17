using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpgrade : MonoBehaviour
{
    public List<SkillsAbilities> m_characterSkills = new List<SkillsAbilities>();
    public List<GameObject> m_UIObjects = new List<GameObject>();
    public ObjectiveManager objective;
    public BaseCharacter player;

    public GameObject[] m_lockedUpgrades;
    public GameObject[] m_unlockedUpgrades;

    public void EnableSkill()
    {
        if (player)
        {
            if (player.m_skillUpgrades.Count == 0 && objective.ObjectiveCompleted == true)
            {
                player.m_skillUpgrades.Add(m_characterSkills[0]);
                m_UIObjects[0].gameObject.SetActive(true);
                objective.ObjectiveCompleted = false;

                m_lockedUpgrades[0].SetActive(false);
                m_unlockedUpgrades[0].SetActive(true);
            }
            else if (player.m_skillUpgrades.Count == 1 && objective.ObjectiveCompleted == true)
            {
                player.m_skillUpgrades.Add(m_characterSkills[1]);
                m_UIObjects[1].gameObject.SetActive(true);
                objective.ObjectiveCompleted = false;
                m_lockedUpgrades[1].SetActive(false);
                m_unlockedUpgrades[1].SetActive(true);
            }
            else if (player.m_skillUpgrades.Count == 2 && objective.ObjectiveCompleted == true)
            {
                player.m_skillUpgrades.Add(m_characterSkills[2]);
                m_UIObjects[2].gameObject.SetActive(true);
                objective.ObjectiveCompleted = false;
                m_lockedUpgrades[2].SetActive(false);
                m_unlockedUpgrades[2].SetActive(true);
            }
            else if (player.m_skillUpgrades.Count == 3 && objective.ObjectiveCompleted == true)
            {
                player.m_skillUpgrades.Add(m_characterSkills[3]);
                m_UIObjects[3].gameObject.SetActive(true);
                objective.ObjectiveCompleted = false;
                m_lockedUpgrades[3].SetActive(false);
                m_unlockedUpgrades[3].SetActive(true);
            }
        }
    }
}

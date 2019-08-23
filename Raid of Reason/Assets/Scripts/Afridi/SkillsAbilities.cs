using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skill Tree/Add Skill")]
public class SkillsAbilities : ScriptableObject 
{
    public string Name;
    public string Description;
    public Sprite Icon;
    
    public int upgradeIndex = 0;


    public bool CheckSkill(BaseCharacter character, ObjectiveManager objective, bool gameCleared)
    {
        if (objective.tempCleared == true)
        {
            if (gameCleared)
            {
                switch (upgradeIndex)
                {
                    case 0:
                        character.m_skillUpgrades.Add(this);
                        upgradeIndex++;
                        break;
                    case 1:
                        character.m_skillUpgrades.Add(this);
                        upgradeIndex++;
                        break;
                    case 2:
                        character.m_skillUpgrades.Add(this);
                        upgradeIndex++;
                        break;
                    case 3:
                        character.m_skillUpgrades.Add(this);
                        upgradeIndex++;
                        break;
                }
            }
        }
        return false;
    }
}

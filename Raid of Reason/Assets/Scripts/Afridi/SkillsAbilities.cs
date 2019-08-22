using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skill Tree/Add Skill")]
public class SkillsAbilities : ScriptableObject 
{
    public string Name;
    public string Description;
    public Sprite Icon;


    public bool CheckSkill(BaseCharacter character, ObjectiveManager objective, bool gameCleared)
    {
        if (objective.tempCleared == true)
        {
            if (!gameCleared)
            {
                character.m_skillUpgrades.Add(this);
                return true;
            }
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skill Tree/Add Skill")]
public class SkillsAbilities : ScriptableObject 
{
    public string Name;
    public string Description;
    public Sprite Icon;

    public bool CheckSkill(BaseCharacter character, ObjectiveManager objective)
    {
        if (objective.tempCleared == true)
        {
            character.m_skillUpgrades.Add(this);
            return true;
        }
        return false;
    }


    public bool EnableSkill(BaseCharacter character)
    {
        List<SkillsAbilities>.Enumerator Skills = character.m_skillUpgrades.GetEnumerator();
        while (Skills.MoveNext())
        {
            var CurrSkill = Skills.Current;
            if (CurrSkill.name == this.name)
            {
                return true;
            }
        }
        return false;
    }

}

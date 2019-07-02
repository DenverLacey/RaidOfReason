using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skill Tree/Add Skill")]
public class SkillsAbilities : ScriptableObject 
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public int pointsNeeded;

    private BaseCharacter Base;


   // public static SkillsAbilities s_default = new SkillsAbilities();

    public void SetValues(GameObject skillDisplayObject, BaseCharacter Player) {
        if (skillDisplayObject) {
            SkillDisplay SD = skillDisplayObject.GetComponent<SkillDisplay>();
            SD.skillName.text = name;
            if (SD.skillDescription)
                SD.skillDescription.text = Description;
            if (SD.skillIcon)
                SD.skillIcon.sprite = Icon;
            if (SD.skillPointsNeeded)
                SD.skillPointsNeeded.text = pointsNeeded.ToString();
        }
    }

    public bool CheckSkills(BaseCharacter character) {

        if (character.m_playerSkillPoints < pointsNeeded)
            return false;

        return true;
    }

    public bool EnableSkill(BaseCharacter character) {
        List<SkillsAbilities>.Enumerator Skills = character.playerSkills.GetEnumerator();
        while (Skills.MoveNext()) {
            var CurrSkill = Skills.Current;
            if (CurrSkill.name == this.name) {
                return true;
            }
        }
        return false;
    }

    public bool GetSkill(BaseCharacter character) {

        character.m_playerSkillPoints -= this.pointsNeeded;
        character.playerSkills.Add(this);
        return true;

    }
    
}

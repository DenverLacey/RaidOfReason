using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUpgrade : MonoBehaviour
{
    public SkillsAbilities skill;
    public ObjectiveManager objective;
    private bool toReset;

    [SerializeField]
    private BaseCharacter m_Player;

    private void Start()
    {
        GetSkill();
    }

    private void Awake()
    {
        TurnOffSkillIcon();
        toReset = true;
    }

    public void GetSkill()
    {
        if (skill.CheckSkill(m_Player, objective, toReset))
        {
            toReset = false;
            TurnOnSkillIcon();
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

    public void UnlockSkills(SkillsAbilities Skills)
    {
        if (Skills)
        {



        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SkillDisplay : MonoBehaviour
{

    public SkillsAbilities skill;
    public TextMeshProUGUI skillName;
    public Text skillDescription;
    public Image skillIcon;
    public Text skillPointsNeeded;

    [SerializeField]
    private BaseCharacter m_PlayerHandler;

    // Start is called before the first frame update
    void Start()
    {
        m_PlayerHandler = this.GetComponentInParent<PlayerHandler>().Player;
        m_PlayerHandler.onPointChange += ReactToChange;
        if (skill)
            skill.SetValues(this.gameObject, m_PlayerHandler);
    }

    // Update is called once per frame
    public void EnableSkills() {
        if (m_PlayerHandler && skill & skill.EnableSkill(m_PlayerHandler))
        {
            TurnOnSkillIcon();
        }
        else if (m_PlayerHandler && skill && skill.CheckSkills(m_PlayerHandler))
        {
            this.transform.Find("IconParent").Find("Disabled").gameObject.SetActive(false);
        }
        else {
            TurnOffSkillIcon();
        }
    }

    private void OnEnable()
    {
        EnableSkills();
    }

    public void GetSkill() {
        if (skill.GetSkill(m_PlayerHandler)) {
            TurnOnSkillIcon();
        }
    }

    private void TurnOnSkillIcon() {
        this.GetComponent<Button>().interactable = false;
        this.transform.Find("IconParent").Find("Available").gameObject.SetActive(false);
        this.transform.Find("IconParent").Find("Disabled").gameObject.SetActive(false);
    }

    private void TurnOffSkillIcon() {
        this.GetComponent<Button>().interactable = false;
        this.transform.Find("IconParent").Find("Available").gameObject.SetActive(true);
        this.transform.Find("IconParent").Find("Disabled").gameObject.SetActive(true);
    }

    void ReactToChange() {
        EnableSkills();
    }
}

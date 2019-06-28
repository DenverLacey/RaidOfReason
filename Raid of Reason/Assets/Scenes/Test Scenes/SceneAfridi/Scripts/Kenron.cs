using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

/*
 * Author: Afridi Rahim
  Kenron's Skeleton that includes
  - What he does
  - How he does it
  - How he interacts with others     
*/

public class Kenron : BaseCharacter {

    //Kenron himself
    public Kenron m_Kenron;
    public GameObject m_Amaterasu;
    public Rigidbody m_KenronSkeleton;

    [SerializeField]
    private GameObject particle;
    [SerializeField]
    private GameObject swordParticle;
    private BaseEnemy enemy;

    public SkillManager manager;
    public bool isActive = false;

    protected override void Awake () {
        base.Awake();
        m_Kenron = FindObjectOfType<Kenron>();
        enemy = FindObjectOfType<BaseEnemy>();
        m_Amaterasu = GameObject.FindGameObjectWithTag("Amaterasu");
        m_KenronSkeleton = GetComponent<Rigidbody>();
	}

    protected override void FixedUpdate() {
        if (m_Kenron != null)
        {
            Slash();
            SkillChecker();
			base.FixedUpdate();
        }
	}

    public void ChaosFlame() {
        if (m_Kenron != null && m_Amaterasu != null)
        {
            isActive = true;
            GameObject temp = Instantiate(swordParticle, m_Amaterasu.transform.position + Vector3.zero * 0.5f, Quaternion.Euler(-90, 0, 0), m_Amaterasu.transform);
            SetDamage(50);
            SetSpeed(20.0f);
            Destroy(temp, 10);
        }
    }

    public void Slash() {
        if (m_Amaterasu != null)
        {
            if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.Second) > 0.1)
            {
                m_Amaterasu.transform.localPosition = new Vector3(-0.65f, 0.0f, 0.8f);
            }
            else if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.Second) < 0.1)
            {
                m_Amaterasu.transform.localPosition = new Vector3(-0.65f, 0.0f, 0);
            }
        }
    }

    public void SkillChecker() {
        if (isActive == true && playerSkills.Find(skill => skill.Name == "Vile Infusion") != new SkillsAbilities())
        {
            if (enemy.isDeadbByKenron == true)
            {
                this.m_maxHealth = m_maxHealth + 10;
            }
        }
        if (isActive == true && playerSkills.Find(skill => skill.Name == "Blood Lust") != new SkillsAbilities())
        {
            if (enemy.isDeadbByKenron == true)
            {
                manager.m_Skills[0].m_currentCoolDown = manager.m_Skills[0].m_currentCoolDown - 2;
            }
        }
    }

    public void ResetSwordSkill()
    {
        if (m_Kenron != null)
        {
            SetDamage(40);
            SetSpeed(10.0f);
            isActive = false;
        }
    }


}
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

    public ChildKenron c_Kenron;
    public GameObject m_Amaterasu;
    public Rigidbody m_KenronSkeleton;

    [SerializeField]
    private GameObject particle;
    [SerializeField]
    private GameObject swordParticle;
    private BaseEnemy enemy;

    public SkillManager manager;
    public bool isActive = false;
    public float timer = 4.0f;

    protected override void Awake () {
        base.Awake();
        enemy = FindObjectOfType<BaseEnemy>();
        m_Amaterasu = GameObject.FindGameObjectWithTag("Amaterasu");
        m_KenronSkeleton = GetComponent<Rigidbody>();
        c_Kenron.gameObject.SetActive(false);
	}

    protected override void FixedUpdate() {
        if (this.gameObject != null)
        {
			base.FixedUpdate();
            Slash();
            SkillChecker();
        }
	}

    public void ChaosFlame() {
        if (this.gameObject != null && m_Amaterasu != null)
        {
            isActive = true;
            GameObject temp = Instantiate(swordParticle, m_Amaterasu.transform.position + Vector3.zero * 0.5f, Quaternion.Euler(-90, 0, 0), m_Amaterasu.transform);
            SetDamage(60);
            SetHealth(m_currentHealth / 2);
            SetSpeed(20.0f);
            Destroy(temp, 10);
        }
    }

    public void Slash() {
        if (m_Amaterasu != null)
        {
            if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.First) > 0.1)
            {
                m_Amaterasu.transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
            else if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.First) < 0.1)
            {
                m_Amaterasu.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public void SkillChecker() {
        if (this.gameObject != null)
        {
            if (playerSkills.Find(skill => skill.Name == "Vile Infusion"))
            {
                if (enemy.isDeadbByKenron == true)
                {
                    this.m_maxHealth = m_maxHealth + 10;
                }
            }
            if (isActive == true && playerSkills.Find(skill => skill.Name == "Blood Lust"))
            {
                if (enemy.isDeadbByKenron == true)
                {
                    manager.m_Skills[0].m_currentCoolDown = manager.m_Skills[0].m_currentCoolDown - 2;
                }
            }
            if (playerSkills.Find(skill => skill.Name == "Curse of Amaterasu")) {
                if (m_currentHealth <= 0.0f) {
                    c_Kenron.CheckStatus();
                    c_Kenron.transform.position = this.gameObject.transform.position;
                    c_Kenron.gameObject.SetActive(true);
                }
            }
        }
    }

    public void ResetSwordSkill()
    {
        if (this.gameObject != null)
        {
            SetDamage(40);
            SetSpeed(10.0f);
            isActive = false;
        }
    }
}
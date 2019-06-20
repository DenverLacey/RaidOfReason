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

    public GameObject m_SkillTree;

    // Use this for initialization
    protected override void Awake () {
        base.Awake();
        m_Kenron = FindObjectOfType<Kenron>();
        m_Amaterasu = GameObject.FindGameObjectWithTag("Amaterasu");
        m_KenronSkeleton = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    protected override void FixedUpdate() {
        if (m_Kenron != null)
        {
            Slash();
            SkillMenu();
			base.FixedUpdate();
        }
	}

    //Abilty 1: Flash Fire
    public void FlashFire() {
        if (m_Kenron != null)
        {
            GameObject temp = Instantiate(particle, transform.position + Vector3.down * 0.5f, Quaternion.Euler(270, 0, 0), transform);
            Destroy(temp, 7);
            SetDamage(60);
            SetSpeed(15.0f);
        }
    }

    public void ChaosFlame() {
        if (m_Kenron != null && m_Amaterasu != null)
        {
            GameObject temp = Instantiate(swordParticle, m_Amaterasu.transform.position + Vector3.zero * 0.5f, Quaternion.Euler(-90, 0, 0), m_Amaterasu.transform);
            Destroy(temp, 30);
            SetDamage(90);
            SetHealth(40);         
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

    public void ResetSkill()
    {
        if (m_Kenron != null)
        {
            SetDamage(50);
            SetSpeed(10.0f);
        }
    }

    public void ResetSwordSkill()
    {
        if (m_Kenron != null)
        {
            SetDamage(50);
            SetSpeed(10.0f);
        }
    }

    public void SkillMenu()
    {
        if (Input.GetKey(KeyCode.O)) {
            m_SkillTree.SetActive(true);
        }
        else
        {
            m_SkillTree.SetActive(false);
        }
    }
}
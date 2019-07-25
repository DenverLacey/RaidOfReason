﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

/*
  * Author: Afridi Rahim
  *
  * Summary: The Stats and Management of Kenrons Core Mechanics.
           Manages his abilites and his skill tree as he improves within the
           game
*/

public class Kenron : BaseCharacter {

    [Tooltip("The Collider of Kenrons Sword")]
    public Collider Sword;

    [Tooltip("The Aethereal Kenron that Spawns on Death")]
    public ChildKenron c_Kenron;

    [Tooltip("The Skill Manager that manages the skills of the players")]
    public SkillManager manager;

    // Kenrons Rigid Body
    private Rigidbody m_KenronSkeleton;
    private BaseEnemy enemy;

    private GameObject particle;

    [Tooltip("The Collider of Kenrons Sword")]
    public GameObject m_Amaterasu;
    private GameObject swordParticle;

    [Tooltip("The Collider of Kenrons Sword")]
    public bool isActive = false;
    private bool isDashing = false;
    private bool m_triggerDown;

    public float dashTime;
    public float timer = 4.0f;

    protected override void Awake () {
        base.Awake();
        enemy = FindObjectOfType<BaseEnemy>();
        m_Amaterasu = GameObject.FindGameObjectWithTag("Amaterasu");
        m_KenronSkeleton = GetComponent<Rigidbody>();
        c_Kenron.gameObject.SetActive(false);
        Sword.enabled = false;
	}

    protected override void FixedUpdate() {
        if (this.gameObject != null)
        {
			base.FixedUpdate();
            SkillChecker();
            Slash();
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
            if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.First) > 0.1 && !m_triggerDown)
            {
                Sword.enabled = true;
                m_triggerDown = true;

                m_Amaterasu.transform.localRotation = Quaternion.Euler(0, 90, 0);
                StartCoroutine(Dash());
            }
            else if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.First) < 0.1)
            {
                Sword.enabled = false;
                m_triggerDown = false;
                m_Amaterasu.transform.localRotation = Quaternion.Euler(0, 0, 0);
                m_KenronSkeleton.velocity = Vector3.zero;
            }
        }
    }

    IEnumerator Dash()
    {
        if (!isDashing)
        {
            m_KenronSkeleton.AddForce(GetSpeed() * m_KenronSkeleton.transform.forward, ForceMode.Impulse);
            isDashing = true;
        }

        yield return new WaitForSeconds(dashTime);
        isDashing = false;
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
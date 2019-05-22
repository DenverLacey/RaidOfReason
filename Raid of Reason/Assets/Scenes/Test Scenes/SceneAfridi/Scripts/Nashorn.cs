﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class Nashorn : BaseCharacter
{
    public GameObject m_Nashorn;
    public Rigidbody m_NashornSkeleton;
    public List<GameObject> m_gauntlets;

    public GameObject m_Particle;
    public GameObject m_Particle_2;
    private GameObject m_Instaniate;
    public GameObject m_Collider;

    public float m_explosiveForce;
    public float m_explosiveRadius;
    

    // 1 = Left Fist / 0 = Right Fist
    private uint m_gauntletIndex;

    private void Awake()
    {
        SetHealth(150);
        SetSpeed(20.0f);
        SetDamage(5);
        m_gauntletIndex = 0;
        m_Collider.SetActive(false);
        m_Nashorn = GameObject.FindGameObjectWithTag("Nashorn");
    }

    private void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            // Ignores the collider on the player
            Physics.IgnoreCollision(GetComponent<Collider>(), m_gauntlets[i].GetComponent<Collider>(), true);
        }
    }

    protected override void FixedUpdate()
    {
        if (m_Nashorn != null)
        {
            base.FixedUpdate();
        }
    }

    protected override void Update() {
        if (m_Nashorn != null) {
            Punch();
        }
    }

    public void Punch() {
        if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.Third) > 0.1)
        {
            if (m_gauntletIndex == 0)
            {
                m_gauntlets[0].gameObject.transform.localPosition = new Vector3(-0.75f, 0, 0.8f);
                m_Instaniate = Instantiate(m_Particle, m_gauntlets[0].transform.position + Vector3.forward * (m_gauntlets[0].transform.localScale.y / 2), Quaternion.Euler(-180, 0, 0), m_gauntlets[0].transform);
            }
            else if (m_gauntletIndex == 1)
            {
                m_gauntlets[1].gameObject.transform.localPosition = new Vector3(0.75f, 0, 0.8f);
                m_Instaniate = Instantiate(m_Particle, m_gauntlets[1].transform.position + Vector3.forward * (m_gauntlets[1].transform.localScale.y / 2), Quaternion.Euler(-180, 0, 0), m_gauntlets[1].transform);
            }
        }
        else if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.Third) < 0.1) {
            if (m_gauntletIndex == 0)
            {
                m_gauntlets[0].gameObject.transform.localPosition = new Vector3(-0.75f, 0, 0.0f);
                Destroy(m_Instaniate);
                m_gauntletIndex++;
            }
            else if (m_gauntletIndex == 1) {
                m_gauntlets[1].gameObject.transform.localPosition = new Vector3(0.75f, 0, 0.0f);
                Destroy(m_Instaniate);
                m_gauntletIndex--;
            }
        }
    }

    //Base Ability
    public void Spott() {
        //Taunt
    }

    //Ultimate
    public void MachtDesSturms()
    {
        //Power of the Storm
        if (m_Nashorn != null)
        {
            GameObject temp = Instantiate(m_Particle_2, transform.position + Vector3.down * 0.5f, Quaternion.Euler(270, 0, 0), transform);
            m_Collider.SetActive(true);
            Collider[] colliders = Physics.OverlapSphere(m_Collider.transform.position, m_explosiveRadius);

            foreach (Collider nearbyEnemy in colliders)
            {
                Rigidbody rb = nearbyEnemy.GetComponent<Rigidbody>();
                if (nearbyEnemy.tag == "Enemy")
                {
                    rb.AddExplosionForce(m_explosiveForce, m_Collider.transform.position, m_explosiveRadius);
                }
            }

            Destroy(temp, 12);
        }
    }   

    public void ResetGauntlet() {

    }
}
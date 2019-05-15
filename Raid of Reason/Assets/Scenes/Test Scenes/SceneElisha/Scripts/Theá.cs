//*
// @Brief: Thea's main class, which handles her basic attacks, abilities and her ult which heals all players in the game to max health
// Author: Elisha Anagnostakis
// Date: 14/05/19 
//*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Theá : BaseCharacter
{
    public GameObject projectile;
    private Vector3 hitLocation;
    private LayerMask layerMask;
    public ParticleSystem healEffect;
    private float delay;
    private float shotCounter;
    private float counter;
    public Rigidbody m_rigid;
    public GameObject m_thea;
    bool isActive;

    private int m_fullHealth = 100;

    public void Awake()
    {
        isActive = false;
        m_rigid = GetComponent<Rigidbody>();
        m_thea = GameObject.FindGameObjectWithTag("Thea");
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if(m_thea != null)
        {
            Player();
            Projectile();
        } 
    }

    public void Projectile()
    {
        counter += Time.deltaTime;

        if (XCI.GetButtonDown(XboxButton.X, controller))
        {
            shotCounter += Time.deltaTime;

            if (counter > delay)
            {
                GameObject temp = Instantiate(projectile, transform.position, transform.rotation);
                Debug.Log("instiantiated");
                counter = 0f;
            }

            //// Ray cast mechanic
            //RaycastHit hit;
            //Ray rayCast = new Ray(transform.position, transform.forward);
            //if (Physics.Raycast(rayCast, out hit, layerMask))
            //{
            //    // enemy health damaged
            //    m_enemy = hit.transform.GetComponent<BaseEnemy>();
            //    if (m_enemy != null)
            //    {
            //        m_enemy.TakeDamage(5);

            //    }

            //}
        }
        else if (XCI.GetButtonUp(XboxButton.X, controller))
        {
            shotCounter = 0f;
        }
    }

    public void UltimateAbility()
    {
        isActive = true;
        int m_coolDown = 20;
        if(isActive == true && m_coolDown == 20)
        {
            for(int i = 0; i < 20; i++)
            {
                SetHealth(m_fullHealth);
                Debug.Log("Ult ability works");
                healEffect.Play();
                m_coolDown--;
            }
            isActive = false;
            healEffect.Stop();
        }

        if(isActive == false && m_coolDown <= 0)
        {
            return;
        }
    }
}
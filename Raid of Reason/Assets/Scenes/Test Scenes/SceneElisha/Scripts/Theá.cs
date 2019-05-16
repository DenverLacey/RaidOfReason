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
    [SerializeField] private GameObject waterPrefab;
    private GameObject temp;
    public GameObject m_thea;
    public _KenronMain m_kenron;
    private float delay;
    private float shotCounter;
    private float counter;
    bool isActive;

    public void Awake()
    {
        isActive = false;
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

        if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.First) > 0.1)
        {
            shotCounter += Time.deltaTime;

            if (counter > delay)
            {
                GameObject temp = Instantiate(projectile, transform.position + projectile.transform.position, transform.rotation);
                Debug.Log("instiantiated");
                counter = 0f;
            }
        }
        else if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.First) < 0.1)
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
                SetHealth(100);
                GetHealth();
                m_kenron.SetHealth(100);
                m_kenron.GetHealth();
                temp = Instantiate(waterPrefab, transform.position, Quaternion.identity);
                m_coolDown--;
            }
            isActive = false;
            Destroy(temp);
        }

        if(isActive == false && m_coolDown <= 0)
        {
            return;
        }
    }
}
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
    [SerializeField] private GameObject waterPrefab;
    public GameObject projectile;
    private Vector3 hitLocation;
    private LayerMask layerMask;
    private GameObject temp;
    public Theá m_thea;
    public _KenronMain m_kenron;
    [SerializeField]private float delay;
    private float shotCounter;
    private float counter;
    bool isActive;

    public void Awake()
    {
        isActive = false;
		m_thea = FindObjectOfType<Theá>();
		m_kenron = FindObjectOfType<_KenronMain>();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if(m_thea != null)
        {
			base.FixedUpdate();
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
                GameObject temp = Instantiate(projectile, transform.position + transform.forward * 2, transform.rotation);
				temp.GetComponent<ProjectileMove>().SetDamage(m_damage);
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
				temp = Instantiate(waterPrefab, transform.position + Vector3.down * (transform.localScale.y / 2), Quaternion.Euler(90, 0, 0));
				if (m_kenron != null)
				{
					temp = Instantiate(waterPrefab, m_kenron.transform.position + Vector3.down * (m_kenron.transform.localScale.y / 2), Quaternion.Euler(90, 0, 0), m_kenron.transform);
				}
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
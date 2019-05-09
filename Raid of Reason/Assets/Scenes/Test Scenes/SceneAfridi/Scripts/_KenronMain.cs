﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Author: Afridi Rahim
  Kenron's Skeleton that includes
  - What he does
  - How he does it
  - How he interacts with others     
*/

public class _KenronMain : MonoBehaviour {

    //Kenron himself
    public GameObject m_Kenron;
    public Rigidbody m_KenronSkeleton;

    //Variables will be in its own abstract class
    private int m_Health;
    private int m_Armor;
    public int m_DamageDealt;
    public float m_Speed;
    private float m_JumpForce;
    //Ability Variables

    //UI
    public Text m_Abiliy1_CD;
    //public Text m_Ult_CD;

    public Text m_Ability1_Use;
   //public Text m_Ult_Use;

    // Use this for initialization
    void Awake () {
        m_Health = 100;
        m_Armor = 100;
        m_DamageDealt = 50;
        m_Speed = 30.0f;
        m_JumpForce = 0.0f;
        m_Kenron = GameObject.FindGameObjectWithTag("Kenron");
        m_KenronSkeleton = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (m_Kenron != null)
        {
            KenronMove();
        }
	}

    //Abilty 1: Flash Fire
    void FlashFire() {
        if (Input.GetKeyDown(KeyCode.K))
        {
            //
        }
    }

    //This must be converted to Xbox controller
    void KenronMove() {
        if (m_Kenron != null)
        {
            float mH = Input.GetAxis("Horizontal");
            float mV = Input.GetAxis("Vertical");
            m_KenronSkeleton.velocity = new Vector3(mH * m_Speed, m_KenronSkeleton.velocity.y, mV * m_Speed);
        }
    }
    
}
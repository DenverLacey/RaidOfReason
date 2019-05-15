//*
// @Brief: Thea's main class, which handles her basic attacks, abilities and her ult which heals all players in the game to max health
// Author: Elisha Anagnostakis
// Date: 14/05/19 
//*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Theá : BaseCharacter {

    [SerializeField]
    GameObject projectile;
    [SerializeField]
    private float delay;
    [SerializeField]
    private float range;
    private float shotCounter;
    private float counter;
    private Vector3 hitLocation;
    private LayerMask layerMask;

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        Player();
        Projectile();
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
               
        }
        else if(XCI.GetButtonUp(XboxButton.X, controller))
        {
            shotCounter = 0f;
        }
    }
}
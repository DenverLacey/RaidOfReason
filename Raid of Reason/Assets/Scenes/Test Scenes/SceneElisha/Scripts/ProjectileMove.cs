﻿//*
// @Brief: This class handles the speed at which thea's projectiles travel in the forward direction of the player
// Author: Elisha Anagnostakis
// Date: 15/05/19 
//*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour {

    [SerializeField]
    private float projectileLife;
    [SerializeField]
    private float projectileSpeed;

	// Update is called once per frame
	void Update () {
		if(projectileSpeed != 0)
        {
            transform.position += transform.forward * (projectileSpeed * Time.deltaTime);
            projectileLife -= Time.deltaTime;
            if(projectileLife <= 0f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("no projectile speed");
        }
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("attack successful");
        }
        else
        {
            Debug.Log("attack unsuccessful");
        }
    }
}
//*
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

    private float m_damage;

    public void SetDamage(float damage)
    {
        this.m_damage = damage;
    }

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

    public void OnCollisionEnter(Collision other)
    {
        string tag = other.gameObject.tag;
        if (tag == "Enemy")
        {
            BaseEnemy enemy = other.gameObject.GetComponent<BaseEnemy>();

            if (enemy)
            {
                enemy.TakeDamage(m_damage);
            }

            Debug.Log("attack successful " + tag);
        }
        else
        {
            Debug.Log("attack unsuccessful " + tag);
        }
		Destroy(gameObject);
    }
}
//*
// @Brief: This class handles the speed at which thea's projectiles travel in the forward direction of the player
// Author: Elisha Anagnostakis
// Date: 15/05/19 
//*

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour {

    [SerializeField] private float m_projectileLife;
    [SerializeField] private float m_projectileSpeed;
    [SerializeField] private float m_healAmount;
    [SerializeField] private float m_damage;

    public void SetDamage(float damage)
    {
        this.m_damage = damage;
    }

    // Update is called once per frame
    void Update () {
		if(m_projectileSpeed != 0)
        {
            transform.position += transform.forward * (m_projectileSpeed * Time.deltaTime);
            m_projectileLife -= Time.deltaTime;
            if(m_projectileLife <= 0f)
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
                enemy.GetComponent<MeshRenderer>().material.color = Color.red;
                StartCoroutine(ResetMaterialColour(enemy, .2f));
            }

            Debug.Log("attack successful " + tag);
        }
        else 
        {
            switch (other.gameObject.tag)
            {
                case "Kenron":
                    Kenron m_kenron = other.gameObject.GetComponent<Kenron>();
                    m_kenron.m_currentHealth += m_healAmount;
                    break;
                case "Nashorn":
                    Nashorn m_nashorn = other.gameObject.GetComponent<Nashorn>();
                    m_nashorn.m_currentHealth += m_healAmount;
                    break;
                default:
                    Debug.Log("projectile missed player");
                    break;
            }
        }
		Destroy(gameObject);
    }

    IEnumerator ResetMaterialColour(BaseEnemy enemy, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (enemy)
        {
            GetComponent<MeshRenderer>().material.color = Color.clear;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class SmashDamage : MonoBehaviour
{ 
    private bool haveSkill = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
			EnemyData enemy = other.gameObject.GetComponent<EnemyData>();
            // Knock back enemies with every punch Nashorn lands.
            Rigidbody rb = other.GetComponent<Rigidbody>();

            GameManager.Instance.Nashorn.currentShield += GameManager.Instance.Nashorn.shieldGain;

            if (GameManager.Instance.Nashorn != null && GameManager.Instance.Nashorn.m_skillUpgrades.Find(skill => skill.Name == "Kinetic Discharge"))
            {
                haveSkill = true;
                // TODO: Knockback enemies deal damage to other enemies 

                if (rb != null && haveSkill == true)
                {
                    Vector3 direction = other.transform.position - GameManager.Instance.Nashorn.transform.position;
                    direction.y = 0;

                    rb.AddForce(direction.normalized * GameManager.Instance.Nashorn.KDForce, ForceMode.Impulse);
                    enemy.KnockBack(GameManager.Instance.Nashorn.KDStun);
                }
            }
           
            if (rb != null && haveSkill == false)
            {
                Vector3 direction = other.transform.position - GameManager.Instance.Nashorn.transform.position;
                direction.y = 0;

                rb.AddForce(direction.normalized * GameManager.Instance.Nashorn.knockBackForce, ForceMode.Impulse);
            }

            enemy.TakeDamage(GameManager.Instance.Nashorn.GetDamage(), GameManager.Instance.Nashorn);
        }
    }
}
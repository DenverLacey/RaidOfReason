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
            // Knock back enemies with every punch Kreiger lands.
            Rigidbody rb = other.GetComponent<Rigidbody>();

            GameManager.Instance.Kreiger.currentShield += GameManager.Instance.Kreiger.shieldGain;

            if (GameManager.Instance.Kreiger != null && GameManager.Instance.Kreiger.m_skillUpgrades.Find(skill => skill.Name == "Kinetic Discharge"))
            {
                haveSkill = true;
                // TODO: Knockback enemies deal damage to other enemies 

                if (rb != null && haveSkill == true)
                {
                    Vector3 direction = other.transform.position - GameManager.Instance.Kreiger.transform.position;
                    direction.y = 0;

                    rb.AddForce(direction.normalized * GameManager.Instance.Kreiger.KDForce, ForceMode.Impulse);
                    enemy.KnockBack(GameManager.Instance.Kreiger.KDStun);
                }
            }
           
            if (rb != null && haveSkill == false)
            {
                Vector3 direction = other.transform.position - GameManager.Instance.Kreiger.transform.position;
                direction.y = 0;

                rb.AddForce(direction.normalized * GameManager.Instance.Kreiger.knockBackForce, ForceMode.Impulse);
            }

            enemy.TakeDamage(GameManager.Instance.Kreiger.GetDamage(), GameManager.Instance.Kreiger);
        }
    }
}
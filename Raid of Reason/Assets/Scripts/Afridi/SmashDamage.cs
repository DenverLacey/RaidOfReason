using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashDamage : MonoBehaviour
{
    [SerializeField] private int m_damage;
    public Nashorn Nashorn;
    public void SetDamage(int damage)
    {
        this.m_damage = damage;
    }
    public void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        if (tag == "Enemy")
        {
            BaseEnemy enemy = other.gameObject.GetComponent<BaseEnemy>();

            if (enemy)
            {
                SetDamage(m_damage);
                enemy.TakeDamage(m_damage);
                if (Nashorn.playerSkills.Find(skill => skill.Name == "Shockwave"))
                { 
                    Vector3 direction = this.gameObject.transform.position - enemy.transform.position;
                    other.GetComponent<StatusEffectManager>().ApplyKnockBack(enemy.gameObject, direction, 1, 0.3f);
                }
            }

            Debug.Log("attack successful " + tag);
        }
        else
        {
            Debug.Log("attack unsuccessful " + tag);
        }
    }
}

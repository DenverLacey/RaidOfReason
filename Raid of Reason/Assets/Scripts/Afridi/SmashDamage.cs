using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class SmashDamage : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How much damage the punches deal")]
    private int m_damage;
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

            if (enemy && XCI.GetAxis(XboxAxis.RightTrigger, XboxController.Second) > 0.1)
            {
                enemy.TakeDamage(m_damage);
                enemy.GetComponent<MeshRenderer>().material.color = Color.red;
                StartCoroutine(ResetMaterialColour(enemy, .2f));
                if (Nashorn.m_playerSkills.Find(skill => skill.Name == "Shockwave"))
                { 
                    Vector3 direction = this.transform.position - enemy.transform.position;
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

    IEnumerator ResetMaterialColour(BaseEnemy enemy, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (enemy)
        {
            enemy.GetComponent<MeshRenderer>().material.color = Color.clear;
        }
    }
}

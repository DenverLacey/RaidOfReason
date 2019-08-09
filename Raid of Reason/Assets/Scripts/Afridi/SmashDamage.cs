using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class SmashDamage : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyData enemy = other.gameObject.GetComponent<EnemyData>();

            if (enemy && XCI.GetAxis(XboxAxis.RightTrigger, GameManager.Instance.Nashorn.m_controller) > 0.1)
            {
                enemy.TakeDamage(GameManager.Instance.Nashorn.GetDamage());
                enemy.GetComponent<MeshRenderer>().material.color = Color.red;
                StartCoroutine(ResetMaterialColour(enemy, .2f));
                if (GameManager.Instance.Nashorn.m_playerSkills.Find(skill => skill.Name == "Shockwave"))
                { 
                    Vector3 direction = this.transform.position - enemy.transform.position;
                    other.GetComponent<StatusEffectManager>().ApplyKnockBack(enemy.gameObject, direction, 1, 0.3f);
                }
                if (GameManager.Instance.Kenron.m_playerSkills.Find(skill => skill.Name == "Shuras Reckoning") && GameManager.Instance.Kenron.isActive == true) {
                    other.GetComponent<StatusEffectManager>().ApplyBurn(4);
                }
            }

            Debug.Log("attack successful " + tag);
        }
        else
        {
            Debug.Log("attack unsuccessful " + tag);
        }
    }

    IEnumerator ResetMaterialColour(EnemyData enemy, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (enemy)
        {
            enemy.GetComponent<MeshRenderer>().material.color = Color.clear;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class SmashDamage : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Normalised Direction Vector for force")]
    private Vector3 m_forceVector;

    [SerializeField]
    [Tooltip("How much force will be applied")]
    private float m_knockBackForce;

    private void Start()
    {
        m_forceVector.Normalize();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyData enemy = other.gameObject.GetComponent<EnemyData>();

            if (enemy && XCI.GetAxis(XboxAxis.RightTrigger, GameManager.Instance.Nashorn.controller) > 0.1)
            {
                enemy.TakeDamage(GameManager.Instance.Nashorn.GetDamage());
                if (GameManager.Instance.Nashorn.m_skillUpgrades.Find(skill => skill.Name == "Shockwave"))
                { 
                    Vector3 direction = this.transform.position - enemy.transform.position;
                    other.GetComponent<StatusEffectManager>().ApplyKnockBack(m_forceVector, m_knockBackForce);
                }
                if (GameManager.Instance.Kenron.m_skillUpgrades.Find(skill => skill.Name == "Shuras Reckoning") && GameManager.Instance.Kenron.isActive == true) {
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
}

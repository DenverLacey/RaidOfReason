using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class SmashDamage : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How much force will be applied")]
    private float m_knockBackForce;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyData enemy = other.gameObject.GetComponent<EnemyData>();

            if (enemy && XCI.GetAxis(XboxAxis.RightTrigger, GameManager.Instance.Nashorn.controller) > 0.1)
            {
                GameManager.Instance.Nashorn.currentShield += GameManager.Instance.Nashorn.shieldGain;
                // Knock back enemies with every punch Nashorn lands.
                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 direction = other.transform.position - GameManager.Instance.Nashorn.transform.position;
                    direction.y = 0;

                    rb.AddForce(direction.normalized * m_knockBackForce, ForceMode.Impulse);
                }
                enemy.TakeDamage(GameManager.Instance.Nashorn.GetDamage());
                if (GameManager.Instance.Nashorn.m_skillUpgrades.Find(skill => skill.Name == "Shockwave"))
                {

                }
                if (GameManager.Instance.Kenron.m_skillUpgrades.Find(skill => skill.Name == "Shuras Reckoning") && GameManager.Instance.Kenron.isActive == true)
                {
                    other.GetComponent<StatusEffectManager>().ApplyBurn(4);
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class SmashDamage : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How much force will be applied")]
    private float m_knockBackForce;

    [SerializeField]
    [Tooltip("How much knockback nashorn deals with his upgrade")]
    private float m_kineticDischargeForce;

    [SerializeField]
	[Tooltip("How long enemies will be stunned for")]
	private float m_stunTime = 1.0f;

    private bool haveSkill = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
			EnemyData enemy = other.gameObject.GetComponent<EnemyData>();
            // Knock back enemies with every punch Nashorn lands.
            Rigidbody rb = other.GetComponent<Rigidbody>();

            GameManager.Instance.Nashorn.currentShield += GameManager.Instance.Nashorn.shieldGain;

            if (GameManager.Instance.Nashorn.m_skillUpgrades.Find(skill => skill.Name == "Kinetic Discharge"))
            {
                haveSkill = true;
                // TODO: Knockback enemies deal damage to other enemies 

                if (rb != null && haveSkill == true)
                {
                    Vector3 direction = other.transform.position - GameManager.Instance.Nashorn.transform.position;
                    direction.y = 0;

                    rb.AddForce(direction.normalized * m_kineticDischargeForce, ForceMode.Impulse);
                    enemy.Stun(m_stunTime);
                }
            }
           
            if (rb != null && haveSkill == false)
            {
                Vector3 direction = other.transform.position - GameManager.Instance.Nashorn.transform.position;
                direction.y = 0;

                rb.AddForce(direction.normalized * m_knockBackForce, ForceMode.Impulse);
            }

            enemy.TakeDamage(GameManager.Instance.Nashorn.GetDamage(), GameManager.Instance.Nashorn);
		
			if (GameManager.Instance.Kenron != null)
			{
			    if (GameManager.Instance.Kenron.m_skillUpgrades.Find(skill => skill.Name == "Shuras Reckoning") && GameManager.Instance.Kenron.isActive == true)
			    {
			        other.GetComponent<StatusEffectManager>().ApplyBurn(4);
			    }
			}
        }
    }
}
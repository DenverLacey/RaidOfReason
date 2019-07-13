using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    private BaseEnemy enemy;
    private int remainingTicks;
    public int burnAmount;
    private bool burnOn = false;
    private Rigidbody m_rigidBody;

    void Start() {
        enemy = GetComponent<BaseEnemy>();
        m_rigidBody = GetComponent<Rigidbody>();
    }

    public void ApplyBurn(int ticks) {
        if (remainingTicks <= 0)
        {
            remainingTicks = ticks;
            StartCoroutine(Burn());
        }
        else
        {
            remainingTicks += ticks;
        }
    }

    public void ApplyKnockBack(GameObject target, Vector3 direction, float length, float overtime) {
        direction = direction.normalized;
        StartCoroutine(KnockBack(target, direction, length, overtime));
    }

    public void KnockBackEnemy(GameObject target, Vector3 direction)
    {
        direction = direction.normalized;
        StartCoroutine(Knock(target, direction));
    }

    IEnumerator Burn() {
        for (; remainingTicks != 0; remainingTicks--)
        {
            enemy.TakeDamage(burnAmount);
            enemy.GetComponent<MeshRenderer>().material.color = Color.red;
            StartCoroutine(ResetMaterialColour(enemy, 0.75f));
            yield return new WaitForSeconds(0.75f);
        }
    }

    IEnumerator KnockBack(GameObject target, Vector3 direction, float length, float overtime) {
        float timeleft = overtime;
        while (timeleft > 0) {

            if (timeleft > Time.deltaTime)
            {
                target.transform.Translate(direction * Time.deltaTime / overtime * length);
            }
            else {
                target.transform.Translate(direction * timeleft / overtime * length);
            }
            timeleft -= Time.deltaTime;
        }
        yield return null;
    }

    IEnumerator Knock(GameObject target, Vector3 direction)
    {
        m_rigidBody.AddForce(direction.normalized * -500f);
        yield return null;
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

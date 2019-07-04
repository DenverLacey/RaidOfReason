using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    private BaseEnemy enemy;
    private int remainingTicks;
    public int burnAmount;

    void Start() {
        enemy = GetComponent<BaseEnemy>();
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

    IEnumerator Burn() {
        for (; remainingTicks != 0; remainingTicks--)
        {
            enemy.TakeDamage(burnAmount);
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
}

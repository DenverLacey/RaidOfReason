using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    private BaseEnemy enemy;
    public List<int> burnTick = new List<int>();
    public int burnAmount;

    void Start() {
        enemy = GetComponent<BaseEnemy>();
    }

    public void ApplyBurn(int ticks) {
        if (burnTick.Count <= 0)
        {
            burnTick.Add(ticks);
            StartCoroutine(Burn());
        }
        else {
            burnTick.Add(ticks);
        }
    }

    public void ApplyKnockBack(GameObject target, Vector3 direction, float length, float overtime) {
        direction = direction.normalized;
        StartCoroutine(KnockBack(target, direction, length, overtime));
    }

    IEnumerator Burn() {
        while (burnTick.Count > 0) {
            for (int i = 0; i < burnTick.Count; i++) {
                burnTick[i]--;
            }
            enemy.m_maxHealth -= burnAmount;
            burnTick.RemoveAll(i => i == 0);
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

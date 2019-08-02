﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    private EnemyData enemy;
    private int remainingTicks;
    private bool burnOn = false;
    private Rigidbody m_rigidBody;

    public int burnAmount;

    void Start()
    {
        enemy = GetComponent<EnemyData>();
        m_rigidBody = GetComponent<Rigidbody>();
    }

    public void ApplyBurn(int ticks)
    {
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

    public void ApplyStun(float duration) {
        // enemy.Stun();
        StartCoroutine(Stun());
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

    IEnumerator Stun() {
        yield return null;

        yield return new WaitForSeconds(0.2f);
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

    IEnumerator ResetMaterialColour(EnemyData enemy, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (enemy)
        {
            enemy.GetComponent<MeshRenderer>().material.color = Color.clear;
        }
    }
}

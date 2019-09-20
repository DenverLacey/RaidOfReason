using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    private EnemyData enemy;
    private Rigidbody m_rigidBody;
   
    void Start()
    {
        enemy = GetComponent<EnemyData>();
        m_rigidBody = GetComponent<Rigidbody>();
    }

    public void ApplyStun(float duration) {
        StartCoroutine(Stun());
    }

    IEnumerator Stun() {
        yield return null;

        yield return new WaitForSeconds(0.2f);
    }

    IEnumerator ResetMaterialColour(EnemyData enemy, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (enemy)
        {
            enemy.Renderer.material.color = Color.clear;
        }
    }
}

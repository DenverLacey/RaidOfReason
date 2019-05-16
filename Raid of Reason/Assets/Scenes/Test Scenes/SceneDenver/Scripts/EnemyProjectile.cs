using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Tooltip("Time until projectile despawns.")]
    [SerializeField] private float m_lifetime;

    [Tooltip("How fast the projectile will travel.")]
    [SerializeField] private float m_speed;

    private float m_timer;
    private int m_damage;

    // Start is called before the first frame update
    void Start() {
        m_timer = m_lifetime;
    }

    // Update is called once per frame
    void Update() {
        transform.Translate(Vector3.forward * m_speed * Time.deltaTime);
        m_timer -= Time.deltaTime;

        if (m_timer <= 0.0f) {
            Destroy(gameObject);
        }
    }

    public void SetDamage(int damage) {
        m_damage = damage;
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Kenron" || other.tag == "Thea") {
            BaseCharacter player = other.GetComponent<BaseCharacter>();
            player.TakeDamage(m_damage);
            Destroy(gameObject);
        }
    }
}

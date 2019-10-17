/*
 * Author: Judy, Denver
 * Description:	Controls lava position using timer and deals damage to players that are colliding
 *				with the lava.
 */

using UnityEngine;

public class Lava : MonoBehaviour
{
	[Tooltip("Damage dealt by lava every game tick")]
	[SerializeField]
    private float m_damage = 2;

	[Tooltip("Time it takes for lava to engulf level")]
	[SerializeField]
    private float m_timer = 30f;

	private Kenron m_kenron;
	private Kreiger m_nashorn;
	private Thea m_thea;

    // Start is called before the first frame update
    void Start()
    {
		m_kenron = GameManager.Instance.Kenron;
		m_nashorn = GameManager.Instance.Kreiger;
		m_thea = GameManager.Instance.Thea;
	}

    // Update is called once per frame
    void Update()
    {
        m_timer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
		DealDamage(other.tag);
    }

    private void OnTriggerStay(Collider other)
    {
		DealDamage(other.tag);
		m_timer = 2f;
	}

	void DealDamage(string tag)
	{
		switch (tag)
		{
			case "Kenron":
				m_kenron.TakeDamage(m_damage);
				break;

			case "Kreiger":
				m_nashorn.TakeDamage(m_damage);
				break;

			case "Thea":
				m_thea.TakeDamage(m_damage);
				break;

			default:
				break;
		}
	}
}

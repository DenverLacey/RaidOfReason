using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class Nashorn : BaseCharacter
{
    public Nashorn m_Nashorn;
    public Rigidbody m_NashornSkeleton;
    public List<GameObject> m_gauntlets;

    public GameObject m_Particle;
    public GameObject m_Particle_2;
    private GameObject m_Instaniate;
    public GameObject m_Collider;

    public float m_explosiveForce;
    public float m_explosiveRadius;
    
    [Tooltip("Size of the area of effect for taunt ability")]
    [SerializeField] float m_tauntRadius;

    [Tooltip("How vulnerable Nashorn is while taunting (1.0 is default)")]
    [SerializeField] float m_tauntVulnerability;

    private List<BaseEnemy> m_nearbyEnemies = new List<BaseEnemy>();

    // 1 = Left Fist / 0 = Right Fist
    private uint m_gauntletIndex = 0;

    protected override void Awake()
    {
        base.Awake();
        m_Collider.SetActive(false);
        m_Nashorn = FindObjectOfType<Nashorn>();
        m_NashornSkeleton = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            // Ignores the collider on the player
            Physics.IgnoreCollision(GetComponent<Collider>(), m_gauntlets[i].GetComponent<Collider>(), true);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update() {
        if (m_Nashorn != null) {
            Punch();
        }
    }

    public void Punch() {
        if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.Third) > 0.1)
        {
            if (m_gauntletIndex == 0)
            {
                m_gauntlets[0].gameObject.transform.localPosition = new Vector3(-0.75f, 0, 0.8f);
                SetDamage(10);
                m_Instaniate = Instantiate(m_Particle, m_gauntlets[0].transform.position + Vector3.forward * (m_gauntlets[0].transform.localScale.y / 2), Quaternion.Euler(-180, 0, 0), m_gauntlets[0].transform);
            }
            else if (m_gauntletIndex == 1)
            {
                m_gauntlets[1].gameObject.transform.localPosition = new Vector3(0.75f, 0, 0.8f);
                SetDamage(10);
                m_Instaniate = Instantiate(m_Particle, m_gauntlets[1].transform.position + Vector3.forward * (m_gauntlets[1].transform.localScale.y / 2), Quaternion.Euler(-180, 0, 0), m_gauntlets[1].transform);
            }
        }
        else if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.Third) < 0.1) {
            if (m_gauntletIndex == 0)
            {
                m_gauntlets[0].gameObject.transform.localPosition = new Vector3(-0.75f, 0, 0.0f);
                m_gauntletIndex = 1;
                Destroy(m_Instaniate);
            }
            else if (m_gauntletIndex == 1) {
                m_gauntlets[1].gameObject.transform.localPosition = new Vector3(0.75f, 0, 0.0f);
                m_gauntletIndex = 0;
                Destroy(m_Instaniate);
            }
        }
    }

    //Base Ability
    public void Spott() {
        // set vulnerability
        m_vulnerability = m_tauntVulnerability;

        // get reference to all nearby enemies
        m_nearbyEnemies.AddRange(FindObjectsOfType<BaseEnemy>());
        m_nearbyEnemies.RemoveAll(e => (transform.position - e.transform.position).sqrMagnitude <= m_tauntRadius * m_tauntRadius);

        // taunt all nearby enemies
        foreach (BaseEnemy enemy in m_nearbyEnemies) {
            enemy.Taunt();
        }
    }

    //Ultimate
    public void MachtDesSturms()
    {
        //Power of the Storm
        if (m_Nashorn != null)
        {
            GameObject temp = Instantiate(m_Particle_2, transform.position + Vector3.down * 0.5f, Quaternion.Euler(270, 0, 0), transform);
            m_Collider.SetActive(true);
            Collider[] colliders = Physics.OverlapSphere(m_Collider.transform.position, m_explosiveRadius);

            foreach (Collider nearbyEnemy in colliders)
            {
                Rigidbody rb = nearbyEnemy.GetComponent<Rigidbody>();
                if (nearbyEnemy.tag == "Enemy")
                {
                    rb.AddExplosionForce(m_explosiveForce, m_Collider.transform.position, m_explosiveRadius);
                    rb.GetComponent<BaseEnemy>().Stun(3f);
                }
            }

            m_Collider.SetActive(false);

            Destroy(temp, 12);
        }
    }   

    public void ResetGauntlet() {

    }

    public void ResetSpott() {
        ResetVulernability();
        foreach (BaseEnemy enemy in m_nearbyEnemies) {
            enemy.StopTaunt();
        }
        m_nearbyEnemies.Clear();
    }
}

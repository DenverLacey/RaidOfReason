using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class Nashorn : BaseCharacter
{
    public Rigidbody m_NashornSkeleton;
    public List<GameObject> m_gauntlets;

    public GameObject m_Particle;
    public GameObject m_Particle_2;
    private GameObject m_Instaniate;
    public GameObject m_Collider;

    public float m_explosiveForce;
    public float m_explosiveRadius;

    public Collider LeftGauntlet;
    public Collider RightGauntlet;
    
    [Tooltip("Size of the area of effect for taunt ability")]
    [SerializeField] float m_tauntRadius;

    [Tooltip("How vulnerable Nashorn is while taunting (1.0 is default)")]
    [SerializeField] float m_tauntVulnerability;

    private List<BaseEnemy> m_nearbyEnemies = new List<BaseEnemy>();
    public bool isActive = false;
    private bool skillCheck = false;

    // 1 = Left Fist / 0 = Right Fist
    private uint m_gauntletIndex = 0;
    public SkillManager manager;

    protected override void Awake()
    {
        base.Awake();
        LeftGauntlet.enabled = false;
        RightGauntlet.enabled = false;
        m_Collider.SetActive(false);
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
        SkillChecker();
    }

    protected override void Update() {
        if (this.gameObject != null) {
            Punch();
        }
    }

    public void SkillChecker() {
        if (this.gameObject != null)
        {
            if (isActive == true && playerSkills.Find(skill => skill.Name == "Roaring Thunder")) {
                if (skillCheck == true)
                {
                    this.m_tauntRadius = m_tauntRadius + 5;
                    manager.m_Skills[1].m_coolDown = manager.m_Skills[1].m_coolDown / 2;
                    skillCheck = false;
                }
            }
        }
    }
    public void Punch() {
        if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.Second) > 0.1)
        {
            LeftGauntlet.enabled = true;
            RightGauntlet.enabled = true;
            SetSpeed(0.0f);
            if (m_gauntletIndex == 0)
            {
                SetSpeed(0.0f);
                m_gauntlets[0].gameObject.transform.localPosition = new Vector3(-0.75f, 0, 0.8f);             
                m_Instaniate = Instantiate(m_Particle, m_gauntlets[0].transform.position + Vector3.forward * (m_gauntlets[0].transform.localScale.y / 2), Quaternion.Euler(-180, 0, 0), m_gauntlets[0].transform);
            }
            else if (m_gauntletIndex == 1)
            {
                SetSpeed(0.0f);
                m_gauntlets[1].gameObject.transform.localPosition = new Vector3(0.75f, 0, 0.8f);
                m_Instaniate = Instantiate(m_Particle, m_gauntlets[1].transform.position + Vector3.forward * (m_gauntlets[1].transform.localScale.y / 2), Quaternion.Euler(-180, 0, 0), m_gauntlets[1].transform);
            }
        }
        else if (XCI.GetAxis(XboxAxis.RightTrigger, XboxController.Second) < 0.1)
        {
            LeftGauntlet.enabled = false;
            RightGauntlet.enabled = false;
            SetSpeed(0.0f);
            if (m_gauntletIndex == 0)
            {
                SetSpeed(15.0f);
                m_gauntlets[0].gameObject.transform.localPosition = new Vector3(-0.75f, 0, 0.0f);                
                m_gauntletIndex = 1;
                Destroy(m_Instaniate);
            }
            else if (m_gauntletIndex == 1) {
                SetSpeed(15.0f);
                m_gauntlets[1].gameObject.transform.localPosition = new Vector3(0.75f, 0, 0.0f);
                m_gauntletIndex = 0;
                Destroy(m_Instaniate);
            }
        }
    }

    /// <summary>
	/// Nashorn's base ability.
	/// </summary>
    public void Spott()
    {
        if (this.gameObject != null)
        {
            isActive = true;
            skillCheck = true;
            // set vulnerability
            m_vulnerability = m_tauntVulnerability;

            m_nearbyEnemies = new List<BaseEnemy>(FindObjectsOfType<BaseEnemy>());

            // taunt all nearby enemies
            foreach (BaseEnemy enemy in m_nearbyEnemies)
            {
                if ((transform.position - enemy.transform.position).sqrMagnitude <= m_tauntRadius * m_tauntRadius)
                {
                    enemy.Taunt();
                }
            }
        }
    }
   
	/// <summary>
	/// Reset's Nashorn's base ability.
	/// </summary>
    public void ResetSpott() {
        ResetVulernability();
        isActive = false;
        skillCheck = true;

        foreach (BaseEnemy enemy in m_nearbyEnemies) {
            if (enemy.State == BaseEnemy.AI_STATE.TAUNTED)
            {
                enemy.StopTaunt();
            }
        }
    }

	protected override void OnDeath() {
		foreach (BaseEnemy enemy in m_nearbyEnemies) {
			if (enemy.State == BaseEnemy.AI_STATE.TAUNTED) {
				enemy.StopTaunt();
			}
		}
	}
}

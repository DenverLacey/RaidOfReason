using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

/*
 * Author: Afridi Rahim
  Kenron's Skeleton that includes
  - What he does
  - How he does it
  - How he interacts with others     
*/

public class _KenronMain : BaseCharacter {

    //Kenron himself
    public GameObject m_Kenron;
    public GameObject m_Amaterasu;
    public Rigidbody m_KenronSkeleton;

    //DEMO STUFF
    public Material FFire;
    public Material Base;
    public Material Steel;
    private Vector3 Draw;
    private Vector3 Sheath;

    // Use this for initialization
    void Awake () {
        SetArmor(100);
        SetDamage(50);
        SetSpeed(10.0f);
        Draw = new Vector3(0.0f, 0.0f, 0.8f);
        Sheath = new Vector3(0.0f, 0.0f, 0.0f);
        m_Kenron = GameObject.FindGameObjectWithTag("Kenron");
        m_Amaterasu = GameObject.FindGameObjectWithTag("Amaterasu");
        m_KenronSkeleton = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    protected override void FixedUpdate() {
        if (m_Kenron != null)
        {
            Player();
            Slash();
        }
	}

    protected override void Update()
    {
        if (m_Kenron != null && m_Amaterasu != null) {
        }
    }

    //Abilty 1: Flash Fire
    public void FlashFire() {
        if (m_Kenron != null)
        {
            SetDamage(60);
            SetSpeed(15.0f);
            m_Kenron.gameObject.GetComponent<Renderer>().material = FFire;
        }
    }

    public void ChaosFlame() {
        if (m_Kenron != null && m_Amaterasu != null)
        {
            SetDamage(90);
            SetHealth(40);         
            m_Amaterasu.gameObject.GetComponent<Renderer>().material = FFire;
        }
    }

    public void Slash() {
        if (m_Amaterasu != null)
        {
            if (XCI.GetButtonDown(XboxButton.X, controller))
            {
                m_Amaterasu.transform.localPosition = new Vector3(-0.65f, 0.0f, 0.8f);
            }
            else if (XCI.GetButtonUp(XboxButton.X, controller))
            {
                m_Amaterasu.transform.localPosition = new Vector3(-0.65f, 0.0f, 0);
            }
        }
    }

    public void ResetSkill()
    {
        if (m_Kenron != null)
        {
            SetDamage(50);
            SetSpeed(10.0f);
            m_Kenron.gameObject.GetComponent<Renderer>().material = Base;
            m_Amaterasu.gameObject.GetComponent<Renderer>().material = Steel;
        }
    }

}

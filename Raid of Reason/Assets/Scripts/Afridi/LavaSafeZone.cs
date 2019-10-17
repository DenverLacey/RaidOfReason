using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSafeZone : MonoBehaviour
{
    public bool m_KenronSafe;
    public bool m_MachinaSafe;
    public bool m_TheaSafe;

    private void Awake()
    {
        m_KenronSafe = false;
        m_MachinaSafe = false;
        m_TheaSafe = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Kenron")
        {
            m_KenronSafe = true;
        }
        if (other.tag == "Nashorn")
        {
            m_MachinaSafe = true;
        }
        if (other.tag == "Thea")
        {
            m_TheaSafe = true;
        }
    }
}

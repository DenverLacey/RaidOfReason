using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public GameObject nash;
    public GameObject thea;
    public GameObject ken;

    public float damage = -34;
    public float timer = 30f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        timer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Kenron" )
        {
            ken.GetComponent<Kenron>().m_currentHealth += damage;
        }

        if(other.tag == "Nashorn")
        {
            nash.GetComponent<Nashorn>().m_currentHealth += damage;
        }

        if (other.tag == "Thea")
        {
            thea.GetComponent<Thea>().m_currentHealth += damage;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        {
            if (other.tag == "Kenron")
            {
                ken.GetComponent<Kenron>().m_currentHealth += damage;
            }

            if (other.tag == "Nashorn")
            {
                nash.GetComponent<Nashorn>().m_currentHealth += damage;
            }

            if (other.tag == "Thea")
            {
                thea.GetComponent<Thea>().m_currentHealth += damage;
            }

            timer = 2f;
        }
    }
}

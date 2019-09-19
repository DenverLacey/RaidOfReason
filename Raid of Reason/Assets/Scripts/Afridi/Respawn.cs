using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class Respawn : MonoBehaviour
{
    public float timetillrespawn;

    private void Update()
    {
        if (GameManager.Instance.Kenron.gameObject.activeSelf && GameManager.Instance.Kenron.m_currentHealth <= 0)
        {
            GameManager.Instance.Kenron.gameObject.SetActive(false);
            StartCoroutine(WaitTillISaySo());
        }

        if (!GameManager.Instance.Nashorn.gameObject.activeSelf && GameManager.Instance.Nashorn.m_currentHealth <= 0)
        {
            GameManager.Instance.Nashorn.gameObject.SetActive(false);
            StartCoroutine(WaitTillISaySo1());
        }

        if (!GameManager.Instance.Thea.gameObject.activeSelf && GameManager.Instance.Thea.m_currentHealth <= 0)
        { 
            GameManager.Instance.Thea.gameObject.SetActive(false);
            StartCoroutine(WaitTillISaySo2());
        }
        if (!GameManager.Instance.Nashorn.gameObject.activeSelf && !GameManager.Instance.Thea.gameObject.activeSelf && !GameManager.Instance.Kenron.gameObject.activeSelf) {
            if (GameManager.Instance.Kenron.m_currentHealth <= 0 && GameManager.Instance.Nashorn.m_currentHealth <= 0 && GameManager.Instance.Thea.m_currentHealth <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        IEnumerator WaitTillISaySo()
        {
            yield return new WaitForSeconds(timetillrespawn);
            GameManager.Instance.Kenron.gameObject.SetActive(true);
            GameManager.Instance.Kenron.m_currentHealth = GameManager.Instance.Kenron.m_maxHealth;
            GameManager.Instance.Kenron.m_currentCharges = GameManager.Instance.Kenron.m_charges;
            GameManager.Instance.Kenron.m_controllerOn = true;
        }

        IEnumerator WaitTillISaySo1()
        {
            yield return new WaitForSeconds(timetillrespawn);
            GameManager.Instance.Nashorn.gameObject.SetActive(true);
            GameManager.Instance.Nashorn.m_currentHealth = GameManager.Instance.Nashorn.m_maxHealth;
            GameManager.Instance.Nashorn.m_controllerOn = true;
        }

        IEnumerator WaitTillISaySo2()
        {
            yield return new WaitForSeconds(timetillrespawn);
            GameManager.Instance.Thea.gameObject.SetActive(true);
            GameManager.Instance.Thea.m_currentHealth = GameManager.Instance.Thea.m_maxHealth;
            GameManager.Instance.Thea.m_controllerOn = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class Respawn : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.Instance.Kenron != null && GameManager.Instance.Kenron.m_currentHealth <= 0)
        {
            GameManager.Instance.Kenron.gameObject.SetActive(false);
            GameManager.Instance.Kenron.m_controllerOn = false;
            StartCoroutine(WaitTillISaySo());
        }

        if (GameManager.Instance.Nashorn != null && GameManager.Instance.Nashorn.m_currentHealth <= 0)
        {
            GameManager.Instance.Nashorn.gameObject.SetActive(false);
            GameManager.Instance.Nashorn.m_controllerOn = false;
            StartCoroutine(WaitTillISaySo1());
        }


        if (GameManager.Instance.Thea != null && GameManager.Instance.Thea.m_currentHealth <= 0)
        {
            GameManager.Instance.Thea.gameObject.SetActive(false);
            GameManager.Instance.Thea.m_controllerOn = false;
            StartCoroutine(WaitTillISaySo2());
        }

        if (GameManager.Instance.Nashorn.m_currentHealth <= 0 && GameManager.Instance.Thea.m_currentHealth <= 0 && GameManager.Instance.Kenron.m_currentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
          
        }

        IEnumerator WaitTillISaySo()
        {
            yield return new WaitForSeconds(5);
            GameManager.Instance.Kenron.gameObject.SetActive(true);
            GameManager.Instance.Kenron.m_currentHealth = 100;
            GameManager.Instance.Kenron.m_controllerOn = true;
        }

        IEnumerator WaitTillISaySo1()
        {
            yield return new WaitForSeconds(5);
            GameManager.Instance.Nashorn.gameObject.SetActive(true);
            GameManager.Instance.Nashorn.m_currentHealth = 100;
            GameManager.Instance.Nashorn.m_controllerOn = true;
        }

        IEnumerator WaitTillISaySo2()
        {
            yield return new WaitForSeconds(5);
            GameManager.Instance.Thea.gameObject.SetActive(true);
            GameManager.Instance.Thea.m_currentHealth = 100;
            GameManager.Instance.Thea.m_controllerOn = true;
        }
    }
}

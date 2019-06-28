using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    Image healthBar;
    [SerializeField]
    private BaseCharacter character;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (character)
            healthBar.fillAmount = character.GetComponent<BaseCharacter>().m_currentHealth / character.GetComponent<BaseCharacter>().m_maxHealth;
        else
            healthBar.fillAmount = 0;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatTrackingManager : MonoBehaviour
{
    public TextMeshProUGUI K_Damage_Dealt_Count;
    public TextMeshProUGUI K_HellFire_Dash_Count;
    public TextMeshProUGUI K_Chaos_Flame_Count;
    public TextMeshProUGUI K_Damage_In_A_Single_Dash_Count;

    public TextMeshProUGUI N_Damage_Taken_Count;
    public TextMeshProUGUI N_Enemies_Taunted_Count;
    public TextMeshProUGUI N_Sheilds_Charged_Count;
    public TextMeshProUGUI N_Enemies_Taunted_At_Once_Count;

    public TextMeshProUGUI T_Damage_Healed_Count;
    public TextMeshProUGUI T_Total_Number_Of_Heals_Count;
    public TextMeshProUGUI T_Fully_Charged_GOP_Count;
    public TextMeshProUGUI T_All_Three_Players_Healed_Count;

    [HideInInspector]
    public int dashesUsed;
    [HideInInspector]
    public int chaosFlameUsed;
    [HideInInspector]
    public float damageInTotal;
    [HideInInspector]
    public float mostDamageInASingleDash;

    [HideInInspector]
    public float damageTaken;
    [HideInInspector]
    public int enemiesTaunted;
    [HideInInspector]
    public float highestTaunted;
    [HideInInspector]
    public float totalSheildsCharged;

    [HideInInspector]
    public int gopUsed;
    [HideInInspector]
    public int gopHitThree;

    private GameManager m_gameManager;

    public void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        dashesUsed = 0;
        chaosFlameUsed = 0;
        enemiesTaunted = 0;
        highestTaunted = 0;
        gopUsed = 0;
        gopHitThree = 0;
    }

    public void Update()
    {
        if (m_gameManager)
        {
            if (m_gameManager.Kenron)
            {
                K_HellFire_Dash_Count.text = dashesUsed.ToString("f0");
                K_Chaos_Flame_Count.text = chaosFlameUsed.ToString("f0");
                K_Damage_Dealt_Count.text = damageInTotal.ToString("f0");
                K_Damage_In_A_Single_Dash_Count.text = mostDamageInASingleDash.ToString("f0");
            }
            if (m_gameManager.Nashorn)
            {               
                N_Damage_Taken_Count.text = damageTaken.ToString("f0");
                N_Sheilds_Charged_Count.text = totalSheildsCharged.ToString("f0");
                N_Enemies_Taunted_Count.text = enemiesTaunted.ToString("f0");
                N_Enemies_Taunted_At_Once_Count.text = highestTaunted.ToString("f0");
                DebugTools.LogVariable("Shields Taken", totalSheildsCharged);
            }
            if (m_gameManager.Thea)
            {
                T_Total_Number_Of_Heals_Count.text = gopUsed.ToString("f0");
                T_All_Three_Players_Healed_Count.text = gopHitThree.ToString("f0");
            }

        }
    }

    public void ResetStats()
    {
        dashesUsed = 0;
        chaosFlameUsed = 0;
        enemiesTaunted = 0;
        highestTaunted = 0;
        gopHitThree = 0;
        gopUsed = 0;
    }

  
}

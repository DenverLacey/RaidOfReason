using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatTrackingManager : MonoBehaviour
{
    public TextMeshPro K_Damage_Dealt_Count;
    public TextMeshPro K_HellFire_Dash_Count;
    public TextMeshPro K_Chaos_Flame_Count;
    public TextMeshPro K_Damage_In_A_Single_Dash_Count;

    public TextMeshPro N_Damage_Taken_Count;
    public TextMeshPro N_Enemies_Taunted_Count;
    public TextMeshPro N_Sheilds_Charged_Count;
    public TextMeshPro N_Enemies_Taunted_At_Once_Count;

    public TextMeshPro T_Damage_Healed_Count;
    public TextMeshPro T_Total_Number_Of_Heals_Count;
    public TextMeshPro T_Fully_Charged_GOP_Count;
    public TextMeshPro T_All_Three_Players_Healed_Count;

    [HideInInspector]
    public int m_dashesUsed;
    [HideInInspector]
    public int m_chaosFlameUsed;

    [HideInInspector]
    public int m_enemiesTaunted;
    [HideInInspector]
    public int m_highestTaunted;

    [HideInInspector]
    public int m_gopUsed;
    [HideInInspector]
    public int m_gopHitThree;

    private GameManager m_gameManager;

    public void Awake()
    {
        m_gameManager = FindObjectOfType<GameManager>();
        m_dashesUsed = 0;
        m_chaosFlameUsed = 0;
        m_enemiesTaunted = 0;
        m_highestTaunted = 0;
        m_gopUsed = 0;
        m_gopHitThree = 0;
    }

    public void Update()
    {
        if (m_gameManager)
        {
            if (m_gameManager.Kenron)
            {
                K_HellFire_Dash_Count.text = m_dashesUsed.ToString("f0");
                K_Chaos_Flame_Count.text = m_chaosFlameUsed.ToString("f0");                
            }
            if (m_gameManager.Nashorn)
            {
                N_Enemies_Taunted_Count.text = m_enemiesTaunted.ToString("f0");
                N_Enemies_Taunted_At_Once_Count.text = m_highestTaunted.ToString("f0");
            }
            if (m_gameManager.Thea)
            {
                T_Total_Number_Of_Heals_Count.text = m_gopUsed.ToString("f0");
                T_All_Three_Players_Healed_Count.text = m_gopHitThree.ToString("f0");
            }

        }
    }

    public void ResetStats()
    {
        m_dashesUsed = 0;
        m_chaosFlameUsed = 0;
        m_enemiesTaunted = 0;
        m_highestTaunted = 0;
        m_gopHitThree = 0;
        m_gopUsed = 0;
    }

  
}

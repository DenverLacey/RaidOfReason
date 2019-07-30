using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class ReviveAlly : MonoBehaviour
{
    private float m_timeToRevive;
    private bool m_isRevived = false;
    private bool m_isReviving = false;
    [SerializeField]
    private float m_healthUponRevive;
    [SerializeField]
    private float m_AOERevive;
    private SphereCollider m_AOEReviveCollider;


    private void Awake()
    {

        m_AOEReviveCollider.enabled = false;
        m_timeToRevive = 5f;
    }

    private void Update()
    {
        // pops off the player thats dead from the camera to then focus on the remaining player alive
        if (m_camera.m_targets.Count > 0)
        {
            m_camera.m_targets.Remove(this.gameObject.transform);
        }
    }


    /// <summary>
    /// Checks if the downed player is in REVIVE state which then gives allies a chance to revive them.
    /// </summary>
    public void ReviveTeamMate()
    {
        float sqrDistance1 = (this.gameObject.transform.position - other.transform.position).sqrMagnitude;

        if (m_playerStates == PlayerState.REVIVE)
        {
            m_AOEReviveCollider.enabled = true;
            m_AOEReviveCollider.radius = m_AOERevive;

            if (sqrDistance1 <= m_AOERevive * m_AOERevive)
            {
                // TODO: Show in UI the player can hold B to revive
                print("Hold B to revive");

                if (XCI.GetButton(XboxButton.B, m_controller))
                {
                    m_isReviving = true;

                    if (m_isReviving)
                    {
                        m_timeToRevive -= Time.deltaTime;

                        // TODO: Start revive particle effect.
                        if (m_timeToRevive <= 0f && m_isReviving)
                        {
                            m_isRevived = true;
                            m_isReviving = false;

                            m_playerStates = PlayerState.ALIVE;
                            m_currentHealth = m_healthUponRevive;
                            // TODO: Stop revive particle effect.
                            m_timeToRevive = 5f;
                        }
                    }
                }
            }
            else
            {
                m_isReviving = false;
                m_timeToRevive = 5f;
            }
        }
    }
}
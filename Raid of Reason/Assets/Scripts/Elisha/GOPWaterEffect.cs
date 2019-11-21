/*
* Author: Elisha_Anagnostakis
* Description: This script handles Theas water texture effect when the player releases the trigger to heal her allies.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GOPWaterEffect : MonoBehaviour
{
    public GameObject GOPWater;
    public SpriteRenderer GOPWaterTex;

    [SerializeField]
    [Tooltip("The duration of the fade out of the water effect sprite")]
    private float m_duration;

    [SerializeField]
    [Tooltip("How fast the effect will rotate")]
    private float m_rotateSpeed = 5f;

    private float m_timer = 0;
    private float m_lerpTime = .5f;
    private int m_phase = 0;
    private float m_AOERadius;
    private Vector3 m_AOEScale;

    // Start is called before the first frame update
    void Start()
    {
        // Texture isnt active at the start of the scene
        GOPWater.SetActive(false);
        GOPWaterTex = GOPWater.GetComponent<SpriteRenderer>();
        // Sets the sprites alpha to 1 by default
        GOPWaterTex.color = new Color(GOPWaterTex.color.r, GOPWaterTex.color.g, GOPWaterTex.color.b, 1);
    }

    /// <summary>
    /// Updates the different phases thea is in when activating her ultimate ability.
    /// </summary>
    public void Update()
    {
        // if Thea activates her ability phase will increase
        if (m_phase > 0)
        {
            // starts to increment the timer 
            m_timer += Time.deltaTime;
            // water sprite rotates on the spot
            GOPWater.transform.rotation = Quaternion.AngleAxis(m_rotateSpeed, Vector3.up) * GOPWater.transform.rotation;
        }

        // If Theas phase is equal to 1
        if (m_phase == 1)
        {
            // the water sprites scale will increase over time 
            GOPWater.transform.localScale = Vector3.Lerp(GOPWater.transform.localScale, m_AOEScale, m_timer / m_lerpTime);

            // if the water sprite has reached the max aoe scale the player has released the trigger on
            if((GOPWater.transform.localScale - m_AOEScale).sqrMagnitude <= 0.1f)
            {
                // increase to pahse 2
                m_phase = 2;
                // fade the texture out of the scene by the duration that is set
                GOPWaterTex.DOFade(0, m_duration);
            }

        }
        else if (m_phase == 2) // if Theas phase is equal to 2
        {
            // if Theas water sprite has faded to 0 alpha
            if (GOPWaterTex.color.a == 0)
            {
                // turn off the sprite
                GOPWater.SetActive(false);
                // Reset phases and timer back to 0
                m_phase = 0;
                m_timer = 0;
            }
        }
    }

    /// <summary>
    /// Activates the water sprite into existence when player releases the trigger.
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="position"></param>
    public void Activate(float radius, Vector3 position)
    {
        m_phase = 1;
        m_AOERadius = radius;
        m_AOEScale = new Vector3(m_AOERadius, m_AOERadius, 0f);
        GOPWater.transform.position = position;
        GOPWaterTex.color = new Color(GOPWaterTex.color.r, GOPWaterTex.color.g, GOPWaterTex.color.b, 1);
        GOPWater.SetActive(true);
    }
}
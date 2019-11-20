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
        GOPWater.SetActive(false);
        GOPWaterTex = GOPWater.GetComponent<SpriteRenderer>();
        GOPWaterTex.color = new Color(GOPWaterTex.color.r, GOPWaterTex.color.g, GOPWaterTex.color.b, 1);
    }
    public void Update()
    {
        if (m_phase > 0)
        {
            m_timer += Time.deltaTime;
            GOPWater.transform.rotation = Quaternion.AngleAxis(m_rotateSpeed, Vector3.up) * GOPWater.transform.rotation;
        }

        if (m_phase == 1)
        {
            GOPWater.transform.localScale = Vector3.Lerp(GOPWater.transform.localScale, m_AOEScale, m_timer / m_lerpTime);

            if((GOPWater.transform.localScale - m_AOEScale).sqrMagnitude <= 0.1f)
            {
                m_phase = 2;
                GOPWaterTex.DOFade(0, m_duration);
            }

        }
        else if (m_phase == 2)
        {
            if (GOPWaterTex.color.a == 0)
            {
                GOPWater.SetActive(false);
                m_phase = 0;
                m_timer = 0;
            }
        }
    }

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

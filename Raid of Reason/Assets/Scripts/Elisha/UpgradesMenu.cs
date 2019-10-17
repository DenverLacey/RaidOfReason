using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class UpgradesMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject m_KenronUpgrades;

    [SerializeField]
    private GameObject m_MachinaUpgrades;

    [SerializeField]
    private GameObject m_TheaUpgrades;

    [HideInInspector]
    public bool m_isPressed { get; set; }

    private GameObject m_miniMap;

    [Tooltip("All game objects that need to be removed off the screen when in the upgrade menu.")]
    public GameObject[] m_InvalidGameObjects;

    // Start is called before the first frame update
    void Start()
    {
        m_KenronUpgrades.SetActive(false);
        m_MachinaUpgrades.SetActive(false);
        m_TheaUpgrades.SetActive(false);
        m_isPressed = false;
        m_miniMap = GameObject.Find("Minimap_Outline");
    }

    // Update is called once per frame
    void Update()
    {
        if (XCI.GetButton(XboxButton.Back, XboxController.Any))
        {
            m_isPressed = true;
            Time.timeScale = 0f;
            m_miniMap.SetActive(false);

            foreach (GameObject objects in m_InvalidGameObjects)
            {
                objects.SetActive(false);
            }

            foreach ( BaseCharacter player in GameManager.Instance.Players)
            {
                if (GameManager.Instance.Kenron)
                {
                    m_KenronUpgrades.SetActive(true);
                }

                if (GameManager.Instance.Kreiger)
                {
                    m_MachinaUpgrades.SetActive(true);
                }

                if (GameManager.Instance.Thea)
                {
                    m_TheaUpgrades.SetActive(true);
                }
            }
        }
        else
        {
            m_miniMap.SetActive(true);
            m_isPressed = false;
            Time.timeScale = 1;
            m_KenronUpgrades.SetActive(false);
            m_MachinaUpgrades.SetActive(false);
            m_TheaUpgrades.SetActive(false);
            foreach (GameObject objects in m_InvalidGameObjects)
            {
                objects.SetActive(true);
            }
        }
    }
}

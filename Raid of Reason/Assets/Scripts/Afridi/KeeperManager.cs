using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using XboxCtrlrInput;

public class KeeperManager : MonoBehaviour
{
    public Canvas shopCanvas;
    
    public GameObject kenronList;
    public GameObject nashornList;
    public GameObject theaList;

    public List<Text> itemCost = new List<Text>();
    public List<Text> skillPoints = new List<Text>();
    public float distance;

    [SerializeField]
    private Kenron m_Kenron;
    [SerializeField]
    private Nashorn m_Nashorn;
    [SerializeField]
    private Theá m_Thea;

    void Awake()
    {
        shopCanvas.gameObject.SetActive(false);
        kenronList.SetActive(false);
        nashornList.SetActive(false);
        theaList.SetActive(false);
    }

    void Update()
    {
        if (this.gameObject != null)
        {
            if (this.gameObject.activeInHierarchy == true)
            {
                float ken_Dist = Vector3.Distance(m_Kenron.transform.position, this.gameObject.transform.position);
                float nas_Dist = Vector3.Distance(m_Nashorn.transform.position, this.gameObject.transform.position);
                float the_Dist = Vector3.Distance(m_Thea.transform.position, this.gameObject.transform.position);
                if (ken_Dist >= distance)
                {
                    //The sprite to press A should appear
                    if (Input.GetKeyDown(KeyCode.J) || XCI.GetButtonDown(XboxButton.A, XboxController.First))
                    {
                        shopCanvas.gameObject.SetActive(true);
                        kenronList.SetActive(true);
                    }
                    //The sprite to press A should appear
                    if (Input.GetKeyUp(KeyCode.K) || XCI.GetButtonUp(XboxButton.A, XboxController.Second))
                    {
                        shopCanvas.gameObject.SetActive(false);
                        nashornList.SetActive(false);
                    }
                }
                if (nas_Dist >= distance) {
                    //The sprite to press A should appear
                    if (Input.GetKeyDown(KeyCode.K) || XCI.GetButtonDown(XboxButton.A, XboxController.Second))
                    {
                        shopCanvas.gameObject.SetActive(true);
                        nashornList.SetActive(true);
                    }
                    //The sprite to press A should appear
                    if (Input.GetKeyUp(KeyCode.K) || XCI.GetButtonUp(XboxButton.A, XboxController.Second))
                    {
                        shopCanvas.gameObject.SetActive(false);
                        nashornList.SetActive(false);
                    }
                }
                if (the_Dist >= distance)
                {
                    //The sprite to press A should appear
                    if (Input.GetKeyDown(KeyCode.L) || XCI.GetButtonDown(XboxButton.A, XboxController.Third))
                    {
                        shopCanvas.gameObject.SetActive(true);
                        theaList.SetActive(true);
                    }
                    //The sprite to press A should appear
                    if (Input.GetKeyUp(KeyCode.K) || XCI.GetButtonUp(XboxButton.A, XboxController.Second))
                    {
                        shopCanvas.gameObject.SetActive(false);
                        nashornList.SetActive(false);
                    }
                }

            }
        }
    }

    //Used to check final item in the shop
    void CheckPlayerSkills() {

    }


    //Gives the player the item requested
    void GiveItem()
    {

    }

    //Removes Item from the Player
    void RemoveItem() {

    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Author: Afridi Rahim
 * Description: Handles The UI placement depending on the players available 
 * Last Edited: 15/11/2019
     */
public class DeactivateUI : MonoBehaviour
{
    public List<GameObject> playerUI = new List<GameObject>();

    public void Start()
    {
        // Turn off the unavailable players UI 
        if (!Utility.IsPlayerAvailable(CharacterType.KENRON))
        {
            playerUI[0].SetActive(false);
        }
        if (!Utility.IsPlayerAvailable(CharacterType.KREIGER))
        {
            playerUI[1].SetActive(false);
        }
        if (!Utility.IsPlayerAvailable(CharacterType.THEA))
        {
            playerUI[2].SetActive(false);
        }
    }
}

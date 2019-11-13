using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateUI : MonoBehaviour
{
    public List<GameObject> playerUI = new List<GameObject>();

    public void Start()
    {
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

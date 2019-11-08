using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public int buildIndex;
    public float transitonTime;

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.Players.Count > 1) {

            #region Single Player
            if (Utility.IsPlayerAvailable(CharacterType.KENRON))
            {
                StartCoroutine(MovetoScene(transitonTime));
            }
            if (Utility.IsPlayerAvailable(CharacterType.KREIGER))
            {
                StartCoroutine(MovetoScene(transitonTime));
            }
            if (Utility.IsPlayerAvailable(CharacterType.THEA))
            {
                StartCoroutine(MovetoScene(transitonTime));
            }
            #endregion

            #region Two Player
            if (Utility.IsPlayerAvailable(CharacterType.KENRON) && Utility.IsPlayerAvailable(CharacterType.THEA))
            {
                StartCoroutine(MovetoScene(transitonTime));
            }
            if (Utility.IsPlayerAvailable(CharacterType.KENRON) && Utility.IsPlayerAvailable(CharacterType.KREIGER))
            {
                StartCoroutine(MovetoScene(transitonTime));
            }

            if (Utility.IsPlayerAvailable(CharacterType.KREIGER) && Utility.IsPlayerAvailable(CharacterType.THEA))
            {
                StartCoroutine(MovetoScene(transitonTime));
            }
            if (Utility.IsPlayerAvailable(CharacterType.KREIGER) && Utility.IsPlayerAvailable(CharacterType.KENRON))
            {
                StartCoroutine(MovetoScene(transitonTime));
            }

            if (Utility.IsPlayerAvailable(CharacterType.THEA) && Utility.IsPlayerAvailable(CharacterType.KREIGER))
            {
                StartCoroutine(MovetoScene(transitonTime));
            }
            if (Utility.IsPlayerAvailable(CharacterType.THEA) && Utility.IsPlayerAvailable(CharacterType.KENRON))
            {
                StartCoroutine(MovetoScene(transitonTime));
            }
            #endregion

            #region Three Player
            if (Utility.IsPlayerAvailable(CharacterType.KENRON) && Utility.IsPlayerAvailable(CharacterType.THEA) && Utility.IsPlayerAvailable(CharacterType.KREIGER))
            {
                StartCoroutine(MovetoScene(transitonTime));
            }
            #endregion
        }
    }

    IEnumerator MovetoScene(float time)
    {
        yield return new WaitForSeconds(time);
		LevelManager.FadeLoadLevel(buildIndex);
    }
}

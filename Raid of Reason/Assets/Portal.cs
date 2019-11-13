using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public int buildIndex;
    public float transitonTime;

    private bool kenPass = false;
    private bool nasPass = false;
    private bool thePass = false;


    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.Players.Count > 1) {

            #region Two Player
            if (Utility.IsPlayerAvailable(CharacterType.KENRON) && Utility.IsPlayerAvailable(CharacterType.THEA))
            {
                if (other.tag == "Kenron")
                {
                    kenPass = true;
                }
                if (other.tag == "Thea")
                {
                    thePass = true;
                }
                if (kenPass && thePass)
                {
                    StartCoroutine(MovetoScene(transitonTime));
                }
            }
            if (Utility.IsPlayerAvailable(CharacterType.KENRON) && Utility.IsPlayerAvailable(CharacterType.KREIGER))
            {
                if (other.tag == "Kenron")
                {
                    kenPass = true;
                }
                if (other.tag == "Kreiger")
                {
                    nasPass = true;
                }
                if (kenPass && nasPass)
                {
                    StartCoroutine(MovetoScene(transitonTime));
                }
            }
            if (Utility.IsPlayerAvailable(CharacterType.KREIGER) && Utility.IsPlayerAvailable(CharacterType.THEA))
            {
                if (other.tag == "Thea")
                {
                    thePass = true;
                }
                if (other.tag == "Kreiger")
                {
                    nasPass = true;
                }
                if (thePass && nasPass)
                {
                    StartCoroutine(MovetoScene(transitonTime));
                }
            }
            #endregion

            #region Three Player
            if (Utility.IsPlayerAvailable(CharacterType.KENRON) && Utility.IsPlayerAvailable(CharacterType.THEA) && Utility.IsPlayerAvailable(CharacterType.KREIGER))
            {
                if (other.tag == "Thea")
                {
                    thePass = true;
                }
                if (other.tag == "Kreiger")
                {
                    nasPass = true;
                }
                if (other.tag == "Kenron")
                {
                    kenPass = true;
                }
                if (thePass && nasPass && kenPass)
                {
                    StartCoroutine(MovetoScene(transitonTime));
                }
            }
            #endregion
        }
    }

    IEnumerator MovetoScene(float time)
    {
        kenPass = false;
        nasPass = false;
        thePass = false;
        yield return new WaitForSeconds(time);
		LevelManager.FadeLoadLevel(buildIndex);
    }
}

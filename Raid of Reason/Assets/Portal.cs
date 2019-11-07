using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public int buildIndex;
    public float transitonTime;

    private bool KenronCheck = false;
    private bool NashornCheck = false;
    private bool TheaCheck = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Kenron")
        {
            KenronCheck = true;
        }
        else if (other.tag == "Kreiger")
        {
            NashornCheck = true;
        }
        else if (other.tag == "Thea")
        {
            TheaCheck = true;
        }
        if (KenronCheck && NashornCheck && TheaCheck)
        {
            StartCoroutine(MovetoScene(transitonTime));
        }
    }

    IEnumerator MovetoScene(float time)
    {
        yield return new WaitForSeconds(time);
        KenronCheck = false;
        NashornCheck = false;
        TheaCheck = false;
		LevelManager.FadeLoadLevel(buildIndex);
    }
}

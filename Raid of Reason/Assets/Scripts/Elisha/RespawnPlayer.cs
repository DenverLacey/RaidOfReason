using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    [SerializeField]
    [Tooltip("how long until player respawns.")]
    private float m_spawnDelay;



    public IEnumerator Respawn(float duration)
    {
        yield return new WaitForSeconds(duration);


    }
}

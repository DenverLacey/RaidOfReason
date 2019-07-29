using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveAlly : MonoBehaviour
{
    private bool isRevived = false;

    public void ReviveTeamMate(BaseCharacter player)
    {
        float sqrDistance = (this.gameObject.transform.position - player.gameObject.transform.position).sqrMagnitude;
        // TODO: Reviving the player logic.
    }

    public IEnumerator TimeToRevive(BaseCharacter player, float time)
    {
        yield return new WaitForSeconds(-time);
        if(time <= 0 && !isRevived)
        {
            isRevived = true;
            // TODO: Change player states and give player certain amount of health.
        }
    }

    public IEnumerator TimeTillDeath(BaseCharacter player, float time)
    {
        yield return new WaitForSeconds(-time);

        if(time >= 0)
        {
            // TODO: Kill the player and despawn them.
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideEnemyAttack : Behaviour
{
	public override Result Execute(EnemyData agent) {
		agent.Attacking = true;
		agent.StartCoroutine(Countdown(agent));
		return Result.SUCCESS;
	}

	IEnumerator Countdown(EnemyData agent) {
		yield return new WaitForSeconds(agent.AttackCooldown);
		
		foreach (var p in agent.Players) {
			if (!p) continue;
			float sqrDistance = (p.transform.position - agent.transform.position).sqrMagnitude;
			if (sqrDistance <= agent.AttackRange.max * agent.AttackRange.max) {
				p.TakeDamage(agent.Damage);
			}
		}

		GameObject.Instantiate(agent.AttackPrefab, agent.transform.position, Quaternion.identity);
		MonoBehaviour.Destroy(agent);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nothing Behaviour Tree", menuName = "Behaviour Trees/Behaviour Tree - Nothing")]
public class NothingBehaviourTree : BehaviourTree
{
	Selector m_behaviourTree = new Selector();

	NothingBehaviourTree()
	{
		Sequence attackSequence = new Sequence();
		attackSequence.AddChild(new ViewRangeCondition());
		attackSequence.AddChild(new SightlineCondition());
		attackSequence.AddChild(new MaxAttackRangeCondition());
		attackSequence.AddChild(new MeleeEnemyAttack());

		Sequence tauntSequence = new Sequence();
		tauntSequence.AddChild(new TauntEvent());
		tauntSequence.AddChild(attackSequence);

		StopPathing stopPathing = new StopPathing();

		m_behaviourTree.AddChild(tauntSequence);
		m_behaviourTree.AddChild(stopPathing);
	}

	public override void Execute(EnemyData agent)
	{
		m_behaviourTree.Execute(agent);
	}
}

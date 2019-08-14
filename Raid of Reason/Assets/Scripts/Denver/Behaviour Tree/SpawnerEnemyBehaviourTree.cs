using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawner Enemy Behaviour Tree", menuName = "Behaviour Trees/Behaviour Tree - Spawner")]
public class SpawnerEnemyBehaviourTree : BehaviourTree
{
	private Selector m_behaviourTree = new Selector();

    SpawnerEnemyBehaviourTree() 
    {
		// create components for behaviour tree
		StunnedCondition stunned = new StunnedCondition();

		Selector tauntSelector = new Selector();
		tauntSelector.AddChild(new TauntEvent());
		tauntSelector.AddChild(new ViewRangeCondition());

		Selector outsideRangeSelector = new Selector();
		outsideRangeSelector.AddChild(new MinAttackRangeCondition());
		outsideRangeSelector.AddChild(new Not(new MaxAttackRangeCondition()));

		Sequence getIntoPositionSequence = new Sequence();
		getIntoPositionSequence.AddChild(outsideRangeSelector);
		getIntoPositionSequence.AddChild(new GetIntoPosition());

		Selector attackSelector = new Selector();
		attackSelector.AddChild(getIntoPositionSequence);
		attackSelector.AddChild(new SpawnerEnemyAttack());

		Sequence attackSequence = new Sequence();
		attackSequence.AddChild(tauntSelector);
		attackSequence.AddChild(new SightlineCondition());
		attackSequence.AddChild(attackSelector);

		Wander wander = new Wander();

		// add compoenents to behaviour tree
		m_behaviourTree.AddChild(stunned);
		m_behaviourTree.AddChild(attackSequence);
		m_behaviourTree.AddChild(wander);
    }

    public override void Execute(EnemyData agent) 
    {
		m_behaviourTree.Execute(agent);
    }
}

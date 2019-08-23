/*
 * Author: Denver
 * Description:	Spawner enemy's scriptable behaviour tree object
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[CreateAssetMenu(fileName = "Spawner Enemy Behaviour Tree", menuName = "Behaviour Trees/Behaviour Tree - Spawner")]
public class SpawnerEnemyBehaviourTree : BehaviourTree
{
	private Selector m_behaviourTree = new Selector();

	/// <summary>
	/// Builds Spawner Type Enemy Behaviour Tree
	/// </summary>
	SpawnerEnemyBehaviourTree() 
    {
		// create components for behaviour tree
		StunnedCondition stunned = new StunnedCondition();

		Sequence withinRangeSequence = new Sequence();
		withinRangeSequence.AddChild(new Not(new MinAttackRangeCondition()));
		withinRangeSequence.AddChild(new MaxAttackRangeCondition());

		Sequence tauntSequence = new Sequence();
		tauntSequence.AddChild(new TauntEvent());
		tauntSequence.AddChild(new SetDestination());
		tauntSequence.AddChild(new SightlineCondition());
		tauntSequence.AddChild(withinRangeSequence);
		tauntSequence.AddChild(new StopPathing());

		Sequence canSeePlayerSequence = new Sequence();
		canSeePlayerSequence.AddChild(new ViewRangeCondition());
		canSeePlayerSequence.AddChild(new SightlineCondition());

		Selector calculateTargetSelector = new Selector();
		calculateTargetSelector.AddChild(tauntSequence);
		calculateTargetSelector.AddChild(canSeePlayerSequence);

		Sequence attackSequence = new Sequence();
		attackSequence.AddChild(new SightlineCondition());
		attackSequence.AddChild(new TurnManualSteeringOn());
		attackSequence.AddChild(new GetIntoPosition());
		attackSequence.AddChild(new SpawnerEnemyAttack());

		Sequence middleSequence = new Sequence();
		middleSequence.AddChild(calculateTargetSelector);
		middleSequence.AddChild(attackSequence);

		Sequence wanderSequence = new Sequence();
		wanderSequence.AddChild(new TurnManualSteeringOff());
		wanderSequence.AddChild(new Wander());

		// add components to behaviour tree
		m_behaviourTree.AddChild(stunned);
		m_behaviourTree.AddChild(middleSequence);
		m_behaviourTree.AddChild(wanderSequence);
	}

    public override void Execute(EnemyData agent) 
    {
		m_behaviourTree.Execute(agent);
    }
}

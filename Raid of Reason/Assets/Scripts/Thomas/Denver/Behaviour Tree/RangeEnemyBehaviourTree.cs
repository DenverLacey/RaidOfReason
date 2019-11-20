/*
 * Author: Denver
 * Description: Behaviour Tree Scriptable Object for Range Enemy Type
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Range Enemy's Behaviour Tree Scriptable Object
/// </summary>
[CreateAssetMenu(fileName = "Range Enemy Behaviour Tree", menuName = "Behaviour Trees/Behaviour Tree - Range")]
public class RangeEnemyBehaviourTree : BehaviourTree
{
    private Selector m_behaviourTree = new Selector();

    /// <summary>
	/// Builds Range Type Enemy Behaviour Tree
	/// </summary>
    RangeEnemyBehaviourTree() 
    {	
		StunnedCondition stunnedCondition = new StunnedCondition();

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
		attackSequence.AddChild(calculateTargetSelector);
		attackSequence.AddChild(new TurnManualSteeringOn());
		attackSequence.AddChild(new GetIntoPosition());
		attackSequence.AddChild(new RangeEnemyAttack());

		TurnManualSteeringOff turnOffManualSteering = new TurnManualSteeringOff();

		Wander wander = new Wander();

		// add components to behaviour tree
		m_behaviourTree.AddChild(stunnedCondition);
		m_behaviourTree.AddChild(attackSequence);
		m_behaviourTree.AddChild(turnOffManualSteering);
		m_behaviourTree.AddChild(wander);
	}

    /// <summary>
	/// Executes behaviour tree on an agent
	/// </summary>
	/// <param name="agent">
	/// The agent to perfom the behaviour tree on
	/// </param>
    public override void Execute(EnemyData agent) 
    {
        m_behaviourTree.Execute(agent);
    }
}

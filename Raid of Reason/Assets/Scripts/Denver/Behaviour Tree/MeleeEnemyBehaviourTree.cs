/*
 * Author: Denver
 * Description:	Behaviour Tree Scriptable Object for Melee Enemy Type
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Melee Enemy's Behaviour Tree Scriptable Object
/// </summary>
[CreateAssetMenu(fileName = "Melee Enemy Behaviour Tree", menuName = "Behaviour Trees/Behaviour Tree - Melee")]
public class MeleeEnemyBehaviourTree : BehaviourTree
{
	private Selector m_behaviourTree = new Selector();

	/// <summary>
	/// Builds Behaviour Tree
	/// </summary>
	MeleeEnemyBehaviourTree()
	{
		// create components for the behaviour tree
		StunnedCondition stunned = new StunnedCondition();

		Sequence sightlineSequence = new Sequence();
		sightlineSequence.AddChild(new ViewRangeCondition());
		sightlineSequence.AddChild(new SightlineCondition());

		Selector tauntSelector = new Selector();
		tauntSelector.AddChild(new TauntEvent());
		tauntSelector.AddChild(sightlineSequence);

		Sequence attackSequence = new Sequence();
		attackSequence.AddChild(sightlineSequence);
		attackSequence.AddChild(new MaxAttackRangeCondition());
		attackSequence.AddChild(new MeleeEnemyAttack());

		Sequence tryToAttackSequence = new Sequence();
		tryToAttackSequence.AddChild(tauntSelector);
		tryToAttackSequence.AddChild(new SetDestination());
		tryToAttackSequence.AddChild(attackSequence);

		Wander wander = new Wander();

		// add components to behaviour tree
		m_behaviourTree.AddChild(stunned);
		m_behaviourTree.AddChild(tryToAttackSequence);
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

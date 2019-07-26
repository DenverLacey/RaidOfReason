/*
 * Author: Denver
 * Description:	Behaviour Tree Scriptable Object for Melee Eenemy Type
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
		// create the attack behaviour sequence
		Sequence abs = new Sequence();
		abs.AddChild(new MaxAttackRangeCondition());
		abs.AddChild(new MeleeEnemyAttack());

		StunnedCondition stunned = new StunnedCondition();

		Sequence tauntSequence = new Sequence();
		tauntSequence.AddChild(new TauntEvent());
		tauntSequence.AddChild(abs);

		Sequence viewRangeAttackSequence = new Sequence();
		viewRangeAttackSequence.AddChild(new ViewRangeCondition());
		viewRangeAttackSequence.AddChild(abs);

		Wander wander = new Wander();

		// add components to behaviour tree
		m_behaviourTree.AddChild(stunned);
		m_behaviourTree.AddChild(tauntSequence);
		m_behaviourTree.AddChild(viewRangeAttackSequence);
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

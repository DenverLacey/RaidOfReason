/*
 * Author: Denver
 * Description:	Behaviour Tree Scriptable Oject for the Suicide Enemy Type
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Suicide Enemy's Behaviour Tree Scriptable Object
/// </summary>
[CreateAssetMenu(fileName = "Suicide Enemy Behaviour Tree", menuName = "Behaviour Trees/Behaviour Tree - Suicide")]
public class SuicideEnemyBehaviourTree : BehaviourTree
{
    private Selector m_behaviourTree = new Selector();

	/// <summary>
	/// Builds Behaviour Tree
	/// </summary>
    SuicideEnemyBehaviourTree()
	{
		// create behaviour tree components
		SuicideEnemyAttack attackBehaviour = new SuicideEnemyAttack();

		// attack behaviour sequence
		Sequence abs = new Sequence();
		abs.AddChild(new MinAttackRangeCondition());
		abs.AddChild(attackBehaviour);

		Sequence alreadyAttackingSequqnce = new Sequence();
		alreadyAttackingSequqnce.AddChild(new AttackingCondition());
		alreadyAttackingSequqnce.AddChild(attackBehaviour);

		Sequence tauntSequence = new Sequence();
		tauntSequence.AddChild(new TauntEvent());
		tauntSequence.AddChild(abs);

		Sequence viewRangeAttackSequence = new Sequence();
		viewRangeAttackSequence.AddChild(new ViewRangeCondition());
		viewRangeAttackSequence.AddChild(abs);

		Wander wander = new Wander();

		// add components to behaviour tree
		m_behaviourTree.AddChild(alreadyAttackingSequqnce);
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

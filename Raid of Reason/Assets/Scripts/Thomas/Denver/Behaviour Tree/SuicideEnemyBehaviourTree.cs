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
		// create components for behaviour tree
		StunnedCondition stunned = new StunnedCondition();

		Selector tryToAttack = new Selector();
		tryToAttack.AddChild(new TauntEvent());
		tryToAttack.AddChild(new ViewRangeCondition());

		Sequence attackSequence = new Sequence();
		attackSequence.AddChild(new MinAttackRangeCondition());
		attackSequence.AddChild(new SuicideEnemyAttack());

		Sequence canSeePlayerSoTryToAttack = new Sequence();
		canSeePlayerSoTryToAttack.AddChild(tryToAttack);
		canSeePlayerSoTryToAttack.AddChild(new SetDestination());
		canSeePlayerSoTryToAttack.AddChild(attackSequence);

		Wander wander = new Wander();

		// add components to behaviour tree
		m_behaviourTree.AddChild(stunned);
		m_behaviourTree.AddChild(canSeePlayerSoTryToAttack);
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

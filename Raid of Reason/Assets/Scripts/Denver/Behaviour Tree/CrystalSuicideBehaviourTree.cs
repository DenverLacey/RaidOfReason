/*
 * Author: Denver
 * Description:	Behaviour Tree Scriptable Object for the Crystal Suicide Enemy Type
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Crystal Suicide Enemy's Behaviour Tree Scriptable Object
/// </summary>
[CreateAssetMenu(fileName = "Crystal Suicide Enemy Behaviour Tree", menuName = "Behaviour Trees/Behaviour Tree - Crystal Suicide")]
public class CrystalSuicideBehaviourTree : BehaviourTree
{
	[Tooltip("Position of the crystal")]
	[SerializeField]
	private Vector3 m_crystalPosition;

	private Selector m_behaviourTree = new Selector();

	private void OnEnable()
	{
		// create components for behaviour tree
		StunnedCondition stunned = new StunnedCondition();

		Selector setTarget = new Selector();
		setTarget.AddChild(new TauntEvent());
		setTarget.AddChild(new SetTarget(m_crystalPosition));

		SetDestination setDestination = new SetDestination();

		Sequence attackSequence = new Sequence();
		attackSequence.AddChild(new MinAttackRangeCondition());
		attackSequence.AddChild(new CrystalSuicideAttack());

		Wander wander = new Wander();

		// add components to behaviour tree
		m_behaviourTree.AddChild(stunned);
		m_behaviourTree.AddChild(setTarget);
		m_behaviourTree.AddChild(setDestination);
		m_behaviourTree.AddChild(attackSequence);
		m_behaviourTree.AddChild(wander);
	}

	public override void Execute(EnemyData agent)
	{
		m_behaviourTree.Execute(agent);
	}
}

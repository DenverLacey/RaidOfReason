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
	[Tooltip("Name of Crystal Object")]
	[SerializeField]
	private string m_objectName;

	private GameObject m_crystal;

	private Selector m_behaviourTree = new Selector();

	private void OnEnable()
	{
		m_crystal = GameObject.Find(m_objectName);

		if (!m_crystal)
		{
			Debug.LogErrorFormat("{0} could not find an object with name: {1}", name, m_objectName);
		}

		// create components for behaviour tree
		StunnedCondition stunned = new StunnedCondition();

		Selector setTarget = new Selector();
		setTarget.AddChild(new TauntEvent());
		setTarget.AddChild(new SetTarget(m_crystal));

		SetDestination setDestination = new SetDestination();

		Sequence attackSequence = new Sequence();
		attackSequence.AddChild(new MinAttackRangeCondition());
		attackSequence.AddChild(new CrystalSuicideAttack());

		m_behaviourTree.AddChild(stunned);
		m_behaviourTree.AddChild(setTarget);
		m_behaviourTree.AddChild(setDestination);
		m_behaviourTree.AddChild(attackSequence);
	}

	public override void Execute(EnemyData agent)
	{
		m_behaviourTree.Execute(agent);
	}
}

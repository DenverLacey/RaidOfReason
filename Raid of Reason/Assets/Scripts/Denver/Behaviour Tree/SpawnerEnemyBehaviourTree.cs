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
		// add components to behaviour tree
		m_behaviourTree.AddChild(new StunnedCondition());
		m_behaviourTree.AddChild(new SpawnerEnemyAttack());
	}

    public override void Execute(EnemyData agent) 
    {
		m_behaviourTree.Execute(agent);
    }
}

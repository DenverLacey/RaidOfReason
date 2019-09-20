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
		attackSequence.AddChild(canSeePlayerSequence);
		attackSequence.AddChild(new TurnManualSteeringOn());
		attackSequence.AddChild(new GetIntoPosition());
		attackSequence.AddChild(new RangeEnemyAttack());

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

    /// <summary>
	/// Finds the closest point on the nav mesh to the source.
	/// </summary>
	/// <param name="source">
	/// The source position that you want to know the closest point on the nav mesh.
	/// </param>
    /// <param name="tolerance">
    /// How close the source can be to being on the Nav Mesh and succeed
    /// </param>
	/// <returns>
	/// The closest point on the nav mesh to the source position.
	/// </returns>
    public static Vector3 FindClosestPoint(Vector3 source, float tolerance) 
    {
		NavMeshHit hit;
        var area = NavMesh.GetAreaFromName("Walkable");

		if (NavMesh.SamplePosition(source, out hit, tolerance, area))
		{
			if (hit.hit)
			{
				return source;
			}
		}

		if (NavMesh.FindClosestEdge(source, out hit, area))
		{
			if (hit.hit)
			{
				return hit.position;
			}
		}
		return Vector3.positiveInfinity;
	}
}

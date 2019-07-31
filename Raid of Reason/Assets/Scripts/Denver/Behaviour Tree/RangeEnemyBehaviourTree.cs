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
	/// Builds Behaviour Tree
	/// </summary>
    RangeEnemyBehaviourTree() 
    {
        // create compoenents for behaviour tree
        // Attack Behaviour Selector
        Sequence fallbackSeqeunce = new Sequence();
        fallbackSeqeunce.AddChild(new MinAttackRangeCondition());
        fallbackSeqeunce.AddChild(new Fallback());

        Sequence advanceSequence = new Sequence();
        advanceSequence.AddChild(new Not(new MaxAttackRangeCondition()));
        advanceSequence.AddChild(new Advance());

        RangeEnemyAttack attack = new RangeEnemyAttack();

        Selector abs = new Selector();
        abs.AddChild(fallbackSeqeunce);
        abs.AddChild(advanceSequence);
        abs.AddChild(attack);

        // other components
        StunnedCondition stunned = new StunnedCondition();

        Sequence tauntSequence = new Sequence();
        tauntSequence.AddChild(new TauntEvent());
        tauntSequence.AddChild(abs);

        Sequence sightlineSequence = new Sequence();
        sightlineSequence.AddChild(new SightlineCondition());
        sightlineSequence.AddChild(abs);

        Wander wander = new Wander();

        m_behaviourTree.AddChild(stunned);
        m_behaviourTree.AddChild(tauntSequence);
        m_behaviourTree.AddChild(sightlineSequence);
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

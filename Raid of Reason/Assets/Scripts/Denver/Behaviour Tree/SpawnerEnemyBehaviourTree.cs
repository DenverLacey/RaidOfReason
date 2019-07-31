using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawner Enemy Behaviour Tree", menuName = "Behaviour Trees/Behaviour Tree - Spawner")]
public class SpawnerEnemyBehaviourTree : BehaviourTree
{
	private Selector m_behaviourTree = new Selector();

    SpawnerEnemyBehaviourTree() 
    {
		// create components for behaviour tree
		// attack behaviour selector
		Sequence fallbackSequence = new Sequence();
		fallbackSequence.AddChild(new MinAttackRangeCondition());
		fallbackSequence.AddChild(new Fallback());

		Sequence advanceSequence = new Sequence();
		advanceSequence.AddChild(new Not(new MaxAttackRangeCondition()));
		advanceSequence.AddChild(new Advance());

		SpawnerEnemyAttack attack = new SpawnerEnemyAttack();

		Selector abs = new Selector();
		abs.AddChild(fallbackSequence);
		abs.AddChild(advanceSequence);
		abs.AddChild(attack);

		StunnedCondition stunned = new StunnedCondition();

		Sequence tauntSequence = new Sequence();
		tauntSequence.AddChild(new TauntEvent());
		tauntSequence.AddChild(abs);

		Sequence sightlineSequence = new Sequence();
		sightlineSequence.AddChild(new SightlineCondition());
		sightlineSequence.AddChild(abs);

		Wander wander = new Wander();

		// add components to behaviour tree
		m_behaviourTree.AddChild(stunned);
		m_behaviourTree.AddChild(tauntSequence);
		m_behaviourTree.AddChild(sightlineSequence);
		m_behaviourTree.AddChild(wander);
    }

    public override void Execute(EnemyData agent) 
    {
		m_behaviourTree.Execute(agent);
    }
}

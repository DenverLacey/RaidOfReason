using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Suicide Enemy Behaviour Tree", menuName = "Behaviour Trees/Behaviour Tree - Suicide")]
public class SuicideEnemyBehaviourTree : BehaviourTree
{
    private Selector m_behaviourTree = new Selector();

    SuicideEnemyBehaviourTree() {
		Selector stunnedOrAttacking = new Selector();
		stunnedOrAttacking.AddChild(new StunnedCondition());
		stunnedOrAttacking.AddChild(new AttackingCondition());

		Sequence tauntAttack = new Sequence();
		tauntAttack.AddChild(new TauntEvent());
		tauntAttack.AddChild(new SuicideEnemyAttack());

		Sequence attack = new Sequence();
		attack.AddChild(new ProximityCondition());
		attack.AddChild(new SuicideEnemyAttack());

		Wander wander = new Wander();

		m_behaviourTree.AddChild(stunnedOrAttacking);
		m_behaviourTree.AddChild(tauntAttack);
		m_behaviourTree.AddChild(attack);
		m_behaviourTree.AddChild(wander);
    }

    public override void Execute(EnemyData agent) {
        m_behaviourTree.Execute(agent);
    }
}

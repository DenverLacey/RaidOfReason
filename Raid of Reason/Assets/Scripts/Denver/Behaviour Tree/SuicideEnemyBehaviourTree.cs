using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Suicide Enemy Behaviour Tree", menuName = "Behaviour Trees/Behaviour Tree - Suicide")]
public class SuicideEnemyBehaviourTree : BehaviourTree
{
    private Sequence m_behaviourTree = new Sequence();

    SuicideEnemyBehaviourTree() {
        m_behaviourTree.AddChild(new ProximityCondition());
        m_behaviourTree.AddChild(new Pathfinder());
    }

    public override void Execute(EnemyData agent) {
        m_behaviourTree.Execute(agent);
    }
}

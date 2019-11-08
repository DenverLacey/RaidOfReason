/*
 * Author: Denver
 * Description:	Behaviour Tree Scriptable Object for the Crystal Suicide Enemy Type
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Crystal Suicide Enemy's Behaviour Tree Scriptable Object
/// </summary>
[CreateAssetMenu(fileName = "Crystal Suicide Enemy Behaviour Tree", menuName = "Behaviour Trees/Behaviour Tree - Crystal Suicide")]
public class CrystalSuicideBehaviourTree : BehaviourTree
{
	[Tooltip("Name of Crystal Object")]
	[SerializeField]
	private string m_objectName;
	private Vector3 m_objectPosition;

	private Selector m_behaviourTree = new Selector();

	private void OnEnable()
	{
        SceneManager.sceneLoaded += Init;
	}

    void Init(Scene s, LoadSceneMode l)
    {
        m_objectPosition = GameObject.Find(m_objectName).transform.position;

        // create components for behaviour tree
        StunnedCondition stunned = new StunnedCondition();

        Sequence tauntSequence = new Sequence();
        tauntSequence.AddChild(new TauntEvent());
        tauntSequence.AddChild(new SetDestination());

        Sequence targetSequence = new Sequence();
        targetSequence.AddChild(new SetTarget(m_objectPosition));
        targetSequence.AddChild(new SetDestinationToNearestEdge());

        Selector pathfindingSelector = new Selector();
        pathfindingSelector.AddChild(tauntSequence);
        pathfindingSelector.AddChild(targetSequence);

        Sequence attackSequence = new Sequence();
        attackSequence.AddChild(new MinAttackRangeCondition());
        attackSequence.AddChild(new CrystalSuicideAttack());

        Sequence doStuffSequence = new Sequence();
        doStuffSequence.AddChild(pathfindingSelector);
        doStuffSequence.AddChild(attackSequence);

        Wander wander = new Wander();

        // add components to behaviour tree
        m_behaviourTree.AddChild(stunned);
        m_behaviourTree.AddChild(doStuffSequence);
    }

	public override void Execute(EnemyData agent)
	{
		m_behaviourTree.Execute(agent);
	}
}

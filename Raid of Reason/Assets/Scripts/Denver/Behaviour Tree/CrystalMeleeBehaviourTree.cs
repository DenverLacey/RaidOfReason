/*
 * Author: Denver
 * Description: Behaviour tree for cystal melee type enemies
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Crystal Melee Enemy Behaviour Tree", menuName = "Behaviour Trees/Behaviour Tree - Crystal Melee")]
public class CrystalMeleeBehaviourTree : BehaviourTree
{
    [Tooltip("Name of Crystal Object")]
    [SerializeField]
    private string m_objectName;
    private Vector3 m_objectPosition;

    Selector m_behaviourTree = new Selector();

    private void OnEnable()
    {
        SceneManager.sceneLoaded += Init;
    }

    private void Init(Scene s, LoadSceneMode l)
    {
        if (s.name != "The level")
            return;

        m_objectPosition = GameObject.Find(m_objectName).transform.position;

        // create components for behaviour tree
        StunnedCondition stunned = new StunnedCondition();

        
    }

    public override void Execute(EnemyData agent)
    {
        m_behaviourTree.Execute(agent);
    }
}

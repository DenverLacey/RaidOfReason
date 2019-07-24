using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourTree : ScriptableObject
{
    public abstract void Execute(EnemyData agent);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Behaviour
{
    public enum Result
    {
        FAILURE,
        SUCCESS,
        RUNNING
    }

    abstract public Result Execute(EnemyData agent);
}

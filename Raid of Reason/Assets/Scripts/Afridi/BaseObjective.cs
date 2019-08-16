using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObjective : ScriptableObject
{
    public abstract void Update();
    public abstract bool IsDone();
}

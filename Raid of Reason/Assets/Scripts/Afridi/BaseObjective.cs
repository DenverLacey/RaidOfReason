using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObjective : ScriptableObject
{
    public abstract void Init();
    public abstract void Update();
    public abstract bool IsDone();
    public abstract bool HasFailed();
    public abstract float Timer();
    public abstract string GrabDescription();
    public abstract string GrabTitle();
    public abstract GameObject SpawnPoints();

    public abstract GameObject ActivatePortal();
}

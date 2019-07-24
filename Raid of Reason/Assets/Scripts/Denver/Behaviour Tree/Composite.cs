using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Composite : Behaviour
{
    protected List<Behaviour> m_children = new List<Behaviour>();
    
    public void AddChild(Behaviour child) {
        m_children.Add(child);
    }
}

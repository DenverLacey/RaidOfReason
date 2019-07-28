/*
 * Author: Denver
 * Description: Composite Behaviour class that handles many child behaviours
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract Composite Behaviour class
/// </summary>
public abstract class Composite : Behaviour
{
    protected List<Behaviour> m_children = new List<Behaviour>();
    
    /// <summary>
    /// Adds behaviour as a child
    /// </summary>
    /// <param name="child">
    /// Behaviour to add to the composite
    /// </param>
    public void AddChild(Behaviour child) 
    {
        m_children.Add(child);
    }
}

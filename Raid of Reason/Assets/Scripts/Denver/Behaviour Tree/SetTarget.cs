/*
 * Author: Denver
 * Description:	SetTarget Behaviour which will set an agent's target to a predetermined Object
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Behaviour.Result;

/// <summary>
/// Sets an enemy's target to an object's position
/// </summary>
public class SetTarget : Behaviour
{
	GameObject m_object;

	public SetTarget(GameObject a_object)
	{
		m_object = a_object;
	}

	public override Result Execute(EnemyData agent)
	{
		agent.Target = m_object.transform.position;
		return CONTINUE;
	}
}

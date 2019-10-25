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
	Vector3 m_position;

	public SetTarget(Vector3 position)
	{
		m_position = position;
	}

	public override Result Execute(EnemyData agent)
	{
		agent.Target = m_position;
		return CONTINUE;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class CheckPlayerInRange : Node
{
	private Transform _transform;
	private NewMonster monster;
	public CheckPlayerInRange(Transform transform)
	{
		_transform = transform;
		monster = transform.GetComponent<NewMonster>();
	}
	public override NodeState Evaluate()
	{
		var target = GameManager.Instance.player;
		if(target != null)
		{
			float distance = Vector3.Distance(target.transform.position, _transform.position);
			if (distance < monster.searchRange)
			{
				state = NodeState.SUCCESS;
				return state;
			}
		}
		state = NodeState.FAILURE;
		return state;
	}

}

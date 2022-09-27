using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class CheckPlayerInAtkRange : Node
{
	private int IsMoveId = Animator.StringToHash("IsMove");

	private Transform _transform;
	private NewMonster monster;
	private Animator animator;
	public CheckPlayerInAtkRange(Transform transform)
	{
		_transform = transform;
		monster = transform.GetComponent<NewMonster>();
		animator = monster.animator;
	}
	public override NodeState Evaluate()
	{
		var target = GameManager.Instance.player;
		if (target != null)
		{
			if (Vector3.Distance(target.transform.position, _transform.position) < monster.atkRange)
			{
				state = NodeState.SUCCESS;
				return state;
			}
		}
		state = NodeState.FAILURE;
		return state;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;
public class MoveTargetNode : Node
{
	private int IsMoveId = Animator.StringToHash("IsMove");

	private Transform _transform;
	private NewMonster monster;
	private NavMeshAgent navMesh;
	private Animator animator;
	public MoveTargetNode(Transform transform)
	{
		_transform = transform;
		monster = transform.GetComponent<NewMonster>();
		navMesh = transform.GetComponent<NavMeshAgent>();
		animator = monster.animator;
	}
	public override NodeState Evaluate()
	{
		var target = GameManager.Instance.player;
		if (Vector3.Distance(_transform.position, target.transform.position) > monster.atkRange)
		{
			animator.SetBool(IsMoveId, true);
			navMesh.SetDestination(target.transform.position);
		}
		else
		{
			animator.SetBool(IsMoveId, false);
			navMesh.ResetPath();
		}
		state = NodeState.RUNNING;
		return state;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;
public class PatrolNode : Node
{
	private int IsMoveId = Animator.StringToHash("IsMove");

	private Transform _transform;

	private float waitTime = 1f;
	private float counter = 0f;
	private bool waiting = false;
	private float searchRange;
	private NewMonster monster;
	private Spawner spawner;
	private Animator animator;
	private Vector3 movePoint;
	private NavMeshAgent navMesh;
	public PatrolNode(Transform transform)
	{
		_transform = transform;
		monster = transform.GetComponent<NewMonster>();
		navMesh = transform.GetComponent<NavMeshAgent>();
		spawner = monster.spawner;
		searchRange = monster.searchRange;
		animator = monster.animator;
		movePoint = RandomPointInSphere(spawner.radius);
	}

	public override NodeState Evaluate()
	{
		if(waiting)
		{
			counter += Time.deltaTime;
			if(counter >= waitTime)
			{
				waiting = false;
				navMesh.ResetPath();
				movePoint = RandomPointInSphere(spawner.radius);
				animator.SetBool(IsMoveId, true);
			}
		}
		else
		{
			if (Vector3.Distance(_transform.position, movePoint) < 1.5f)
			{
				waiting = true;
				counter = 0f;
				animator.SetBool(IsMoveId, false);
			}
			else
			{
				navMesh.SetDestination(movePoint);
			}
		}
		state = NodeState.RUNNING;
		return state;
	}
	private Vector3 RandomPointInSphere(float radius)
	{
		Vector3 getPoint = Random.onUnitSphere;
		getPoint.y = 0.0f;
		float r = Random.Range(0.0f, radius);
		return getPoint * r;
	}
}

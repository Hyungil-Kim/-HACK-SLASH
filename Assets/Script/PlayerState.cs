using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Enum;

public class PStateIdle : IState
{
	Player fsm;
	public PStateIdle(Player _fsm)
	{
		fsm = _fsm;
	}
	public void StateEnter()
	{
		Debug.Log("idle");
	}

	public void StateExit()
	{
	}

	public void StateFixedUpdate()
	{
		fsm.ChangeMoveState();
	}
	public void StateUpdate()
	{
		fsm.ChangeAttackState();
	}

}
public class PStateMove : IState
{
	Player fsm;
	public PStateMove(Player _fsm)
	{
		fsm = _fsm;
	}
	public void StateEnter()
	{
		Debug.Log("move");
	}

	public void StateExit()
	{
		fsm.ISMove(false);
	}
	public void StateFixedUpdate()
	{
		float xAxis = Input.GetAxis("Horizontal");
		float yAxis = Input.GetAxis("Vertical");
		fsm.MovePlayer(xAxis, yAxis);
	}
	public void StateUpdate()
	{
		fsm.ChangeAttackState();
	}
}

public class PStateAtk : IState
{
	Player fsm; 
	private int IsAttackId = Animator.StringToHash("IsAttack");
	public PStateAtk(Player _fsm)
	{
		fsm = _fsm;
	}
	public void StateEnter()
	{
		
	}
	public void StateExit()
	{
		fsm.IsAttack(false);
		fsm.animator.ResetTrigger(IsAttackId);
	}

	public void StateFixedUpdate()
	{
		if (fsm.canMove)
		{
			float xAxis = Input.GetAxis("Horizontal");
			float yAxis = Input.GetAxis("Vertical");
			Vector3 newPos = Vector3.right * -xAxis + Vector3.forward * -yAxis;
			newPos.Normalize();
			fsm.transform.position += newPos * Time.deltaTime * fsm.Speed;
		}
	}
	public void StateUpdate()
	{
		foreach (var key in fsm.atkKeyCode)
		{
			if (Input.GetKey(key.Key))
			{
				key.Value();
			}
		}
	}
}

public class PStateDie : IState
{
	Player fsm;
	public PStateDie(Player _fsm)
	{
		fsm = _fsm;
	}
	public void StateEnter()
	{

	}

	public void StateExit()
	{

	}
	public void StateFixedUpdate()
	{

	}
	public void StateUpdate()
	{

	}
}

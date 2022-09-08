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
	Animator animator;
	Dictionary<KeyCode, Action> atkKeyCodeKey;
	private int KeyDownZId = Animator.StringToHash("SkillZ");
	private int KeyDownXId = Animator.StringToHash("SkillX");
	private int KeyDownCId = Animator.StringToHash("SkillC");
	private bool canMove =false;
	public PStateAtk(Player _fsm)
	{
		fsm = _fsm;
		animator = fsm.animator;
		atkKeyCodeKey = new Dictionary<KeyCode, Action>
		{
			{KeyCode.Q, KeyDown_Z },
			{KeyCode.W, KeyDown_X },
			{KeyCode.E, KeyDown_C }
		};
	}
	public void StateEnter()
	{
		foreach (var dic in atkKeyCodeKey)
		{
			if (dic.Key == fsm.pressedAtkKey)
			{
				dic.Value();
			}
		}
		
	}

	public void StateExit()
	{
		fsm.IsAttack(false);
	}
	public void StateFixedUpdate()
	{
		if (canMove)
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
		if (Input.anyKeyDown)
		{
			foreach (var dic in atkKeyCodeKey)
			{
				if (Input.GetKeyDown(dic.Key))
				{
					dic.Value();
				}
			}
		}
	}
	private void KeyDown_Z()
	{
		Debug.Log("SkillZ");
		animator.SetTrigger(KeyDownZId);
		Attack();
	}
	private void KeyDown_X() 
	{
		Debug.Log("SkillX");
		animator.SetTrigger(KeyDownXId);
		Attack();
	}
	private void KeyDown_C() 
	{
		Debug.Log("SkillC");
		animator.SetBool(KeyDownCId,true);
		CoroutineHelper.StartCoroutine(HoldAttack(KeyDownCId,KeyCode.E,true));
	}

	private void Attack()
	{
		Debug.Log("Attack");
		CoroutineHelper.StartCoroutine(CheckAttackAnimationEnd(0.8f));

	}
	private IEnumerator HoldAttack(int id,KeyCode keyCode,bool isMove = false)
	{
		canMove = isMove;
		while (true)
		{
			if (!Input.GetKey(keyCode))
			{
				animator.SetBool(id, false);
				CoroutineHelper.StartCoroutine(CheckAttackAnimationEnd(0.5f));
				canMove = false; 
				yield break;
			}
			yield return new WaitForEndOfFrame();
		}
	}
	private IEnumerator CheckAttackAnimationEnd(float exitTime)
	{
		while ( animator.GetCurrentAnimatorStateInfo(0).normalizedTime < exitTime)
		{
			yield return null;
		}
		fsm.ChangeState(PlayerState.Idle);
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

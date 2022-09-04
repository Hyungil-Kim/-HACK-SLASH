using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MStateIdle : IState
{
	Monster fsm;
	Animator animator;
	public MStateIdle(Monster _fsm)
	{
		fsm = _fsm;
		animator = fsm.animator;
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
		fsm.IdleAction();
	}
}
public class MStateSearch : IState
{
	Monster fsm;
	public MStateSearch(Monster _fsm)
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
		fsm.Search();
	}

	public void StateUpdate()
	{
	}
}
public class MStateAtk : IState
{
	Monster fsm;
	public MStateAtk(Monster _fsm)
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
		fsm.Attack();
	}

	public void StateUpdate()
	{
	}
}
public class MStateDie : IState
{
	Monster fsm;
	public MStateDie(Monster _fsm)
	{
		fsm = _fsm;
	}
	public void StateEnter()
	{
		fsm.deadEvent();
	}

	public void StateExit()
	{
	}

	public void StateFixedUpdate()
	{
	}

	public void StateUpdate()
	{
		if(fsm.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
			fsm.DelayTime(() => ObjectPoolManager.Instance.DeSpawn(fsm.gameObject));
	}
}
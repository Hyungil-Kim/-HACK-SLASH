using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Mimic : Monster
{
	private int A_isWake = Animator.StringToHash("IsWake");
	private int A_attackRange = Animator.StringToHash("AttackRange");
	private bool isWake = false;

	protected override void OnEnable()
	{
		base.OnEnable();
		isWake = false;
		SetWakeAni(isWake);
		SetSight(false);
	}
	protected override void Update()
	{
		base.Update();
		if (isWake) { return; }

		if (CurrentState != Enum.MonsterState.Idle)
		{
			isWake = true;
			SetSight(true);
			SetWakeAni(isWake);
		}
	}
	public override void Attack(Action action = null)
	{
		base.Attack(() => selectAttackRange());
	}
	public void SetWakeAni(bool boolean)
	{
		animator.SetBool(A_isWake, boolean);
	}
	public override void Search()
	{
		if (!isWake) return;
		if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) { return; }

		base.Search();
	}
	private void selectAttackRange()
	{
		float distance = Vector3.Distance(target.transform.position, transform.position);
		animator.SetFloat(A_attackRange, distance * 0.35f);
	}
	public override void IdleAction()
	{
		if (CheckDistanceToPlayer(1f))
		{
			ChangeState(Enum.MonsterState.Search);
		}
	}
}

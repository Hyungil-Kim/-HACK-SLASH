using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class M_Magician : Monster
{
	private int A_Speed = Animator.StringToHash("Speed");
	private int A_AttackEnd = Animator.StringToHash("AttackEnd");

	protected override void Start()
	{
		base.Start();
		transform.position = new Vector3(transform.position.x, -0.4f, transform.position.z);
	}
	protected override void FixedUpdate()
	{
		animator.SetFloat(A_Speed, speed * 0.25f);
		base.FixedUpdate();
	}

	public override void Search()
	{
		ChangeSpeed(1f);
		base.Search();
	}
	public override void Attack(Action action = null)
	{
		ChangeSpeed(4f);
		base.Attack(action);
	}

	protected override void AddAtkList()
	{
		atkDic.Add(() => Attack1(), 0.6f);
		atkDic.Add(() => Attack2(), 0.4f);
		atkDic.OrderBy(x => x.Value);
	}

	private void Attack1()
	{
		IsAttackId = Animator.StringToHash("IsAttack1");
	}

	private void Attack2()
	{
		IsAttackId = Animator.StringToHash("IsAttack2");
	}


	protected override void EndAttack()
	{
		base.EndAttack();
		animator.SetTrigger(A_AttackEnd);
	}
}

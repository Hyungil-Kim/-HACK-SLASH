using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Beholder : Monster
{
	private int A_Speed = Animator.StringToHash("Speed");

	protected override void FixedUpdate()
	{
		animator.SetFloat(A_Speed,speed * 0.25f);
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
}

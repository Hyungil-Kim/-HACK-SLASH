using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Orc : NewMonster
{
	protected int Attack1Id = Animator.StringToHash("Attack1");
	protected int Attack2Id = Animator.StringToHash("Attack2");
	protected override void AddAtkList()
	{
		atkDic.Add(() => Attack1(),0.4f);
		atkDic.Add(() => Attack2(),0.6f);
	}
	private void Attack1()
	{
		animator.SetTrigger(Attack1Id);
	}
	private void Attack2()
	{
		animator.SetTrigger(Attack2Id);
	}
}

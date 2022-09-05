using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class M_Beholder : Monster
{
	private int A_Speed = Animator.StringToHash("Speed");
	private int A_AttackEnd = Animator.StringToHash("AttackEnd");
	
	[SerializeField]
	private GameObject laser;
	private B_bullet[] bullet = new B_bullet[5];
	protected override void Start()
	{
		base.Start();
		for(int i = 0;  i< 5;i++)
		{
			GameObject spawnBullet = ObjectPoolManager.Instance.Spawn("Beholder_Bullet",gameObject);
			bullet[i] = spawnBullet.GetComponent<B_bullet>();
		}
	}
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

	protected override void AddAtkList()
	{
		atkDic.Add(() =>Attack1(),0.6f);
		atkDic.Add(() =>Attack2(),0.4f);
		atkDic.OrderBy(x => x.Value);
	}

	private void Attack1()
	{
		IsAttackId = Animator.StringToHash("IsAttack1");
		for(int i = 0; i < 5;i++)
		{ 
			bullet[i].Init(transform, target.transform, 6.0f, 3.0f);
			bullet[i].gameObject.SetActive(true);
			StartCoroutine(bullet[i].ShootBullet());
		}
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
	private void StartLaser()
	{
		laser.SetActive(true);
	}
	private void EndLaser()
	{
		laser.SetActive(false);
	}
}

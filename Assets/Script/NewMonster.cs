using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
public class NewMonster : Unit
{
	[SerializeField]
	public float atkRange;
	[SerializeField]
	public float searchRange;
	[SerializeField]
	private float atkAngle;

	private NavMeshAgent navMesh;
	protected Player target;
	[NonSerialized]
	public Spawner spawner;
	public Dictionary<Action, float> atkDic = new();
	[NonSerialized]
	public Animator animator;
	protected virtual void Awake()
	{
		animator = GetComponent<Animator>();
		navMesh = GetComponent<NavMeshAgent>();
		spawner = GetComponentInParent<Spawner>();
	}
	protected virtual void Start()
	{
		target = GameManager.Instance.player.GetComponent<Player>();
	}
	protected virtual void OnEnable()
	{
		navMesh.speed = speed;
		Reset();
		AddAtkList();
	}
	protected virtual void Reset()
	{
		hp = maxHp;
		ResetTimer();
		animator.Rebind();
		animator.Update(0f);
	}
	protected virtual void AddAtkList() {; }

	public void SelectAttack()
	{
		if (atkDic.Count <= 0) return;
		var percent = UnityEngine.Random.Range(0f, 1f);
		var total = 0f;
		Action action = null;
		foreach (var elem in atkDic)
		{
			total += elem.Value;
			if (total > percent)
			{
				action = elem.Key;
				break;
			}
		}
		action();
	}
	
}

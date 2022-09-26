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
	private Vector3 destination;
	protected Player target;
	[NonSerialized]
	public Spawner spawner;

	private bool isMove;
	private bool isAttack;

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
	}
	protected virtual void Reset()
	{
		hp = maxHp;
		ResetTimer();
		animator.Rebind();
		animator.Update(0f);
	}
}

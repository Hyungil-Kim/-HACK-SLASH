using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Enum;
using Random = UnityEngine.Random;
using UnityEditor;

public class Monster : Unit
{
	[SerializeField]
	private float atkrange;
	[SerializeField]
	private float searchrange;
	[SerializeField]
	private float atkAngle;

	private NavMeshAgent navMesh;
	private Vector3 destination;
	protected Player target;
	protected FieldOfView sight;
	private Spawner spawner;

	private bool isMove;
	private bool isAttack;

	private int IsMoveId = Animator.StringToHash("IsMove");
	protected int IsAttackId = Animator.StringToHash("IsAttack");
	private int IsDieId = Animator.StringToHash("IsDie");
	
	[NonSerialized]
	public Animator animator;
	[SerializeField]
	protected Dictionary<Action,float> atkDic = new();
	private Dictionary<MonsterState, IState> dicState = new Dictionary<MonsterState, IState>();
	private StateMachine stateMachine;
	[SerializeField]
	public MonsterState CurrentState;

	#region MonoBehaviour
	protected virtual void Awake()
	{
		animator = GetComponent<Animator>();
		navMesh = GetComponent<NavMeshAgent>();
		sight = GetComponent<FieldOfView>();
		spawner = GetComponentInParent<Spawner>();
		if (sight != null)
		{
			sight.viewRadius = atkrange;
			sight.viewAngle = atkAngle;
		}
		
		AddState();
		AddAtkList();
	}
	protected virtual void OnEnable()
	{
		navMesh.speed = speed;
		Reset();
	}
	protected virtual void Start()
	{
		target = GameManager.Instance.player.GetComponent<Player>();
	}
	protected virtual void Update()
	{
		IsAlive();
		stateMachine.StateUpdate();
		foreach (var state in dicState)
		{
			if (state.Value == stateMachine.CurrentState)
			{
				CurrentState = state.Key;
			}
		}
	}
	protected virtual void FixedUpdate()
	{
		stateMachine.StateFixedUpdate();
	}
	#endregion MonoBehaviour

	#region Virtual Function
	protected virtual void Reset()
	{
		hp = maxHp;
		ChangeState(MonsterState.Idle);
		ResetTimer();
		animator.Rebind();
		animator.Update(0f);
	}

	protected virtual void AddAtkList()
	{

	}

	public virtual void Search()
	{
		if (CheckDistanceToPlayer(searchrange))
		{
			MoveDone();
			ChangeState(MonsterState.Attack);
			return;
		}
		MovePosition(RandomPointInSphere(spawner.radius), false);

	}
	protected virtual void MoveDone()
	{
		navMesh.ResetPath();
		StartCoroutine(CheckRotation(target.gameObject));
		IsMove(false);
	}
	public virtual void Attack(Action action = null)
	{
		if (CheckDistanceToPlayer(atkrange) || isAttack)
		{
			if (isMove)
			{
				MoveDone();
			}
			if (!isAttack)
			{
				if (!sight.isCollision)
				{
					StartCoroutine(CheckRotation(target.gameObject));
					return;
				}
				isAttack = true;
				navMesh.updateRotation = false;

				StopAllCoroutines();

				if (action != null)
					action();

				SelectAttack();
				animator.SetTrigger(IsAttackId);
			}
			if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
			{
				DelayTime(() => EndAttack());
			}
		}
		else if (!CheckDistanceToPlayer(atkrange))
		{
			if (!isAttack)
				MovePosition(target.transform.position, true);
		}
	}

	protected virtual void EndAttack()
	{
		isAttack = false;
		navMesh.updateRotation = true;
	}
	#endregion Virtual Function

	private void AddState()
	{
		IState idle = new MStateIdle(this);
		IState search = new MStateSearch(this);
		IState attack = new MStateAtk(this);
		IState die = new MStateDie(this);

		dicState.Add(MonsterState.Idle, idle);
		dicState.Add(MonsterState.Search, search);
		dicState.Add(MonsterState.Attack, attack);
		dicState.Add(MonsterState.Dead, die);

		stateMachine = new StateMachine(idle);
	}
	
	public bool CheckDistanceToPlayer(float range)
	{
		if (target == null) { return false; }

		float distance = Vector3.Distance(target.transform.position, transform.position);

		if(distance < range) { return true;}
	
		return false;
	}
	public void ChangeState(MonsterState state)
	{
		stateMachine.SetState(dicState[state]);
	}
	public void IsMove(bool boolean)
	{
		isMove = boolean;
		animator.SetBool(IsMoveId, boolean);
	}


	private void MovePosition(Vector3 des,bool update)
	{
		if (!isMove || update)
		{
			destination = des;
			IsMove(true);
		}
		navMesh.SetDestination(destination);

		var dis = Vector3.Distance(transform.position, destination);
		if (dis < 1f)
		{
			MoveDone();
		}
	}
	

	public Vector3 RandomPointInSphere(float radius)
	{
		Vector3 getPoint = Random.onUnitSphere;
		getPoint.y = 0.0f;
		float r = Random.Range(0.0f, radius);
		return getPoint * r;
	}
	
	private IEnumerator CheckRotation(GameObject target)
	{
		while (transform.rotation != Quaternion.LookRotation(target.transform.position - transform.position))
		{
			Vector3 dir = target.transform.position - transform.position;
			var nextRot = Quaternion.LookRotation(dir);
			transform.rotation = Quaternion.Slerp(transform.rotation,nextRot, Time.deltaTime * rotateSpeed);
			yield return new WaitForEndOfFrame();
		}
	}
	public void deadEvent()
	{
		SetSight(false);
		navMesh.ResetPath();
		animator.SetTrigger(IsDieId);
		spawner.monsterList.Remove(gameObject);
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (CurrentState == MonsterState.Attack)
		{
			if (collision.gameObject.tag == "Player")
			{
				DoDamage(atk,target.Hp);
			}
		}
	}
	private void IsAlive()
	{
		if (CheckDeath())
		{
			ChangeState(MonsterState.Dead);
		}
	}
	protected void SetSight(bool boolean)
	{
		sight.viewMeshFilter.gameObject.SetActive(boolean);
	}

	public virtual void IdleAction()
	{
		ChangeState(Enum.MonsterState.Search);	
	}
	protected void ChangeSpeed(float changeSpeed)
	{
		speed = Mathf.Lerp(speed, changeSpeed, Time.deltaTime * 3);
		navMesh.speed = speed;
	}
	private void SelectAttack()
	{
		if (atkDic.Count <= 0) return;
		var percent = Random.Range(0f, 1f);
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Enum;
public class Player : Unit
{
	private bool isMove;
	private bool isAttack;
	private int IsMoveId = Animator.StringToHash("IsMove");
	private int IsAttackId = Animator.StringToHash("IsAttack");
	private int WeaponTypeId = Animator.StringToHash("WeaponType");

	[System.NonSerialized]
	public Animator animator;
	[System.NonSerialized]
	public Dictionary<KeyCode, Action> atkKeyCode = new Dictionary<KeyCode, Action>();
	private Dictionary<PlayerState, IState> dicState = new();

	private StateMachine stateMachine;
	[SerializeField]
	public PlayerState CurrentState;
	[SerializeField]
	private GameObject hand;
	[SerializeField]
	private PlayerWeapon playerWeapon;
	private int weaponType = 0;
	[System.NonSerialized]
	public bool canMove;
	[System.NonSerialized]
	public KeyCode curAtkCode;
	private void OnEnable()
	{
		Speed = speed;
		animator = GetComponent<Animator>();
		GameManager.Instance.player = gameObject;
	}
	private void Start()
	{
		AddState();
		SetWeapon(playerWeapon);
	}
	private void AddState()
	{
		IState idle = new PStateIdle(this);
		IState move = new PStateMove(this);
		IState attack = new PStateAtk(this);
		IState die = new PStateDie(this);

		dicState.Add(PlayerState.Idle, idle);
		dicState.Add(PlayerState.Move, move);
		dicState.Add(PlayerState.Attack, attack);
		dicState.Add(PlayerState.Dead, die);

		stateMachine = new StateMachine(idle);
	}
	private void Update()
	{
		stateMachine.StateUpdate();
		foreach (var state in dicState)
		{
			if (state.Value == stateMachine.CurrentState)
			{
				CurrentState = state.Key;
			}
		}
	}
	private void FixedUpdate()
	{
		stateMachine.StateFixedUpdate();
	}
	public void MovePlayer(float xAxis, float yAxis)
	{
		if (xAxis != 0 || yAxis != 0)
		{
			Vector3 newPos = Vector3.right * -xAxis + Vector3.forward * -yAxis;
			newPos.Normalize();
			transform.position += newPos * Time.deltaTime * Speed;
			CheckRotation(newPos);
		}
		else
		{
			ChangeState(PlayerState.Idle);
		}
		isMove = xAxis != 0 || yAxis != 0 ? true : false;
		animator.SetBool(IsMoveId, isMove);
	}
	private void CheckRotation(Vector3 newPos)
	{
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newPos), Time.deltaTime * rotateSpeed);
	}

	public void ChangeAttackState()
	{
		foreach (var key in atkKeyCode)
		{
			if (Input.GetKeyDown(key.Key))
			{
				curAtkCode = key.Key;
				animator.SetTrigger(IsAttackId);
				ChangeState(PlayerState.Attack);
			}
		}
	}

	public void ChangeMoveState()
	{
		float xAxis = Input.GetAxis("Horizontal");
		float yAxis = Input.GetAxis("Vertical");

		bool check = stateMachine.CurrentState == dicState[PlayerState.Move];
		if (!check && (xAxis != 0 || yAxis != 0))
		{
			ChangeState(PlayerState.Move);
		}
	}
	public void ChangeState(PlayerState state)
	{
		stateMachine.SetState(dicState[state]);
	}
	public void IsAttack(bool boolean)
	{
		isAttack = boolean;
	}
	public void ISMove(bool boolean)
	{
		isMove = boolean;
		animator.SetBool(IsMoveId, boolean);
	}
	private void IsAlive()
	{
		if (CheckDeath())
		{
			ChangeState(PlayerState.Dead);
		}
	}
	private void SetWeapon(PlayerWeapon weapon)
	{
		GameObject handWeapon = null;
		for (int i = 0; i < hand.transform.childCount; i++)
		{
			if (hand.transform.GetChild(i).gameObject.tag == "Weapon")
			{
				handWeapon = hand.transform.GetChild(i).gameObject;

			}
		}
		if (handWeapon != null)
		{
			GameObject.Destroy(handWeapon);
		}
		Instantiate(weapon.weaponObject, hand.transform);
		weaponType = (int)weapon.weaponType;
		SetWeaponSkill();
		animator.SetInteger(WeaponTypeId, weaponType);
	}
	private void SetWeaponSkill()
	{
		atkKeyCode.Clear();
		playerWeapon.SetSkill();
		atkKeyCode = playerWeapon.atkKeyCode;
	}
	private void EndWeaponAni()
	{
		playerWeapon.weapon.EndAni();
	}
	//private void OnCollisionEnter(Collision collision)
	//{
	//	if (collision.gameObject.tag == "Monster")
	//	{
	//		if (CurrentState == PlayerState.Attack)
	//		{
	//			var enemy = collision.gameObject.GetComponent<Monster>();
	//			DoDamage(atk, enemy.Hp);

	//		}
	//	}
	//}
}
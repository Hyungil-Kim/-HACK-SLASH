using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;
public class Weapon_Staff : MonoBehaviour, IWeapon
{
	private int KeyDownQId = Animator.StringToHash("SkillQ");
	private int KeyDownWId = Animator.StringToHash("SkillW");
	private int KeyDownEId = Animator.StringToHash("SkillE");
	private int KeyDownRId = Animator.StringToHash("SkillR");
	private int currentKey = 0;

	public void Skill_Q()
	{
		currentKey = KeyDownQId;
		Attack();
	}
	public void Skill_W()
	{
		currentKey = KeyDownWId;
		Attack();
	}

	public void Skill_E()
	{
		currentKey = KeyDownEId;
		Attack();
	}

	public void Skill_R()
	{

	}
	private void Attack()
	{
		GameManager.Instance.player.GetComponent<Player>().animator.SetTrigger(currentKey);
	}
	public void ResetAni(int curKey)
	{
		GameManager.Instance.player.GetComponent<Player>().animator.ResetTrigger(currentKey);
	}
	public void EndAni()
	{
		ResetAni(currentKey);
		GameManager.Instance.player.GetComponent<Player>().ChangeState(PlayerState.Idle);
	}
}

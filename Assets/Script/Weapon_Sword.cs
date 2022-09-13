using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;
public class Weapon_Sword : MonoBehaviour , IWeapon
{
	private int KeyDownQId = Animator.StringToHash("SkillQ");
	private int KeyDownWId = Animator.StringToHash("SkillW");
	private int KeyDownEId = Animator.StringToHash("SkillE");
	private int KeyDownRId = Animator.StringToHash("SkillR");
	private int currentKey = 0;

	public void Skill_Q()
	{
		Debug.Log("SkillQ");
		currentKey = KeyDownQId;
		Attack();
	}
	public void Skill_W()
	{
		Debug.Log("SkillW");
		currentKey = KeyDownWId;
		Attack();
	}

	public void Skill_E()
	{
		Debug.Log("SkillE");
		CoroutineHelper.StartCoroutine(HoldAttack(KeyDownEId, KeyCode.E, true));
	}

	public void Skill_R()
	{
		
	}
	private void Attack()
	{
		GameManager.Instance.player.GetComponent<Player>().animator.SetTrigger(currentKey);
		CoroutineHelper.StartCoroutine(CheckAttackAnimationEnd(0.9f));
	}
	private IEnumerator CheckAttackAnimationEnd(float exitTime)
	{
		while (GameManager.Instance.player.GetComponent<Player>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
		{
			Debug.Log("End");
			ResetAni(currentKey);
			GameManager.Instance.player.GetComponent<Player>().ChangeState(PlayerState.Idle);
			yield return new WaitForEndOfFrame();
		}
			Debug.Log("End2");
	}
	private IEnumerator HoldAttack(int id, KeyCode keyCode, bool isMove = false)
	{
		GameManager.Instance.player.GetComponent<Player>().canMove = isMove;
		while (true)
		{
			if (!Input.GetKey(keyCode))
			{
				GameManager.Instance.player.GetComponent<Player>().animator.SetBool(id, false);
				CoroutineHelper.StartCoroutine(CheckAttackAnimationEnd(0.9f));
				GameManager.Instance.player.GetComponent<Player>().canMove = false;
				yield break;
			}
			yield return new WaitForEndOfFrame();
		}
	}

	public void ResetAni(int curKey)
	{
		GameManager.Instance.player.GetComponent<Player>().animator.ResetTrigger(currentKey);
	}
}

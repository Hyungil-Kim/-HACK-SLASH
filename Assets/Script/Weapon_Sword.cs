using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enum;
public class Weapon_Sword : MonoBehaviour , Weapon
{
	private int KeyDownQId = Animator.StringToHash("SkillZ");
	private int KeyDownWId = Animator.StringToHash("SkillX");
	private int KeyDownEId = Animator.StringToHash("SkillC");
	private int KeyDownRId = Animator.StringToHash("SkillC");

	public void Skill_Q()
	{
		Debug.Log("SkillQ");
		GameManager.Instance.player.GetComponent<Player>().animator.SetTrigger(KeyDownQId);
		Attack();
	}
	public void Skill_W()
	{
		Debug.Log("SkillW");
		GameManager.Instance.player.GetComponent<Player>().animator.SetTrigger(KeyDownWId);
		Attack();
	}

	public void Skill_E()
	{
		Debug.Log("SkillE");
		GameManager.Instance.player.GetComponent<Player>().animator.SetBool(KeyDownEId, true);
		CoroutineHelper.StartCoroutine(HoldAttack(KeyDownEId, KeyCode.E, true));
	}

	public void Skill_R()
	{
		
	}
	private void Attack()
	{
		Debug.Log("Attack");
		CoroutineHelper.StartCoroutine(CheckAttackAnimationEnd(0.8f));
	}
	private IEnumerator CheckAttackAnimationEnd(float exitTime)
	{
		while (GameManager.Instance.player.GetComponent<Player>().animator.GetCurrentAnimatorStateInfo(0).normalizedTime < exitTime)
		{
			yield return null;
		}
		GameManager.Instance.player.GetComponent<Player>().ChangeState(PlayerState.Idle);
	}
	private IEnumerator HoldAttack(int id, KeyCode keyCode, bool isMove = false)
	{
		GameManager.Instance.player.GetComponent<Player>().canMove = isMove;
		while (true)
		{
			if (!Input.GetKey(keyCode))
			{
				GameManager.Instance.player.GetComponent<Player>().animator.SetBool(id, false);
				CoroutineHelper.StartCoroutine(CheckAttackAnimationEnd(0.5f));
				GameManager.Instance.player.GetComponent<Player>().canMove = false;
				yield break;
			}
			yield return new WaitForEndOfFrame();
		}
	}

}

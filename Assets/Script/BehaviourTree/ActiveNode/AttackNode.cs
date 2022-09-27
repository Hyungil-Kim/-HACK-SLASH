using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class AttackNode : Node
{
    private Transform _transform;
    private Animator animator;
    private NewMonster monster;

    private float counter = 0f;
    private int IsMoveId = Animator.StringToHash("IsMove");

    public AttackNode(Transform transform)
	{
        _transform = transform;
        monster = transform.GetComponent<NewMonster>();
        animator = transform.GetComponent<Animator>();
	}

	public override NodeState Evaluate()
	{
        Player target = GameManager.Instance.player.GetComponent<Player>();
        animator.SetBool(IsMoveId, false);
        if(target ==null)
		{
            state = NodeState.FAILURE;
            return state;
		}
		if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
		{
            counter += Time.deltaTime;
		}
        if(counter >= monster.watingTime)
		{
            monster.SelectAttack();
            //monster.DoDamage(target.Hp, monster.Atk);
            counter = 0f;
		}
        state = NodeState.RUNNING;
        return state;
	}
}

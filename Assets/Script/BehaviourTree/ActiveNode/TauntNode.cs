using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class TauntNode : Node
{
    private Transform _transform;
    private Animator animator;
    private NewMonster monster;

    private int TauntingId = Animator.StringToHash("Taunting");

    public TauntNode(Transform transform)
    {
        _transform = transform;
        monster = transform.GetComponent<NewMonster>();
        animator = monster.animator;
    }
    public override NodeState Evaluate()
    {
        animator.SetTrigger(TauntingId);
        state = NodeState.RUNNING;
        return state;
    }

}

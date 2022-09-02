using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public IState CurrentState { get; private set; }

    public StateMachine(IState defaultState)
	{
		CurrentState = defaultState;
	}
	public void SetState(IState state)
	{
		if (CurrentState == state) return;

		CurrentState.StateExit();

		CurrentState = state;

		CurrentState.StateEnter();
	}
	public void StateUpdate()
	{
		CurrentState.StateUpdate();
	}
	public void StateFixedUpdate()
	{
		CurrentState.StateFixedUpdate();
	}
}

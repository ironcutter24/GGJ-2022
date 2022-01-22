using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMController : MonoBehaviour
{
	[SerializeField] Enemy targetUnit;

    State currentState;
	State oldState = null;

    void Update()
    {
		if (StateHasChanged)
		{
			currentState.Enter();
			oldState = currentState;
		}

		currentState.Process();

		if(StateHasChanged)
			oldState.Exit();
    }

	bool StateHasChanged { get { return currentState != oldState; } }

	public void ChangeState(State newState)
    {
		oldState = currentState;
		currentState = newState;
    }
}

public abstract class State
{
	protected Enemy controller;

	protected State(Enemy controller)
	{
		this.controller = controller;
	}

	public virtual void Enter() { }

	public abstract void Process();

	public virtual void Exit() { }
}

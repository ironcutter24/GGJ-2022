using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
	public abstract class FSMController : MonoBehaviour
	{
		[SerializeField] public Enemy targetUnit;

		State currentState = null;
		State newState;

		private void Awake()
		{
			InitializeStates();
			newState = States.Idle;
		}

		protected StateCollection validStates;
		public StateCollection States { get { return validStates; } }

		protected abstract void InitializeStates();

		private void Update()
		{
			if (StateHasChanged)
			{
				currentState = newState;
				currentState.Enter();
			}

			currentState.Process();
			currentState.LateProcess();

			if (StateHasChanged)
            {
				currentState.Exit();
			}
		}

		bool StateHasChanged { get { return newState != currentState; } }

		public void SetState(State newState)
		{
			this.newState = newState;
		}

		public class StateCollection
		{
			public State Idle;
			public State Patrol;
			public State Chase;
			public State Attack;
			public State RunAway;
			public State SafeZone;

			public StateCollection(State idle, State patrol, State chase, State attack, State runAway, State safeZone)
			{
				this.Idle = idle;
				this.Patrol = patrol;
				this.Chase = chase;
				this.Attack = attack;
				this.RunAway = runAway;
				this.SafeZone = safeZone;
			}
		}
	}

	public abstract class State
	{
		private FSMController controller;
		protected Enemy Actor { get { return controller.targetUnit; } }
		protected FSMController.StateCollection States { get { return controller.States; } }

		protected State(FSMController controller)
		{
			this.controller = controller;
		}

		protected void SetState(FSM.State newState)
        {
			controller.SetState(newState);
        }

		public virtual void Enter() { }

		public abstract void Process();

		public virtual void LateProcess()
        {
			if (PlayerState.IsHunter)
				SetState(States.RunAway);
        }

		public virtual void Exit() { }
	}
}

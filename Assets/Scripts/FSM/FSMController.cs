using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
	public class FSMController : MonoBehaviour
	{
		[SerializeField] public Enemy targetUnit;

		State currentState;
		State oldState = null;


        #region States definition and initialization

		public struct StateCollection
        {
			public State Idle;
			public State Patrol;
			public State Chase;
			public State RunAway;
			public State Attack;
		}

		StateCollection validStates;
		public StateCollection States { get { return validStates; } }

		private void Awake()
		{
			validStates.Idle = new IdleState(this);
			validStates.Patrol = new PatrolState(this);
			validStates.Chase = new ChaseState(this);
			validStates.RunAway = new RunAwayState(this);
			validStates.Attack = new AttackState(this);
		}

		#endregion


		private void Update()
		{
			if (StateHasChanged)
			{
				currentState.Enter();
				oldState = currentState;
			}

			currentState.Process();

			if (StateHasChanged)
				oldState.Exit();
		}

		bool StateHasChanged { get { return currentState != oldState; } }

		public void SetState(State newState)
		{
			oldState = currentState;
			currentState = newState;
		}
	}

	public abstract class State
	{
		private FSMController controller;
		protected Enemy TargetUnit { get { return controller.targetUnit; } }
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

		public virtual void Exit() { }
	}
}

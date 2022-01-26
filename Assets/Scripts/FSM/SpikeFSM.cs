using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class SpikeFSM : FSMController
    {
        protected override void InitializeStates()
        {
            validStates = new StateCollection(
                new IdleState(this),
                new PatrolState(this),
                new ChaseState(this),
                new AttackState(this),
                new RunAwayState(this),
                new SafeZoneState(this)
                );
        }

        private class RunAwayState : State
        {
            public RunAwayState(FSMController controller) : base(controller) { }

            public override void Process()
            {
                throw new System.NotImplementedException();
            }

            public override void LateProcess() { }
        }

        private class SafeZoneState : State
        {
            public SafeZoneState(FSMController controller) : base(controller) { }

            public override void Enter()
            {
                // Enter lair
                // Disable collider
            }

            public override void Process()
            {
                if (PlayerState.IsPrey)
                    SetState(States.Patrol);
            }
        }
    }
}

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

            public override void Enter()
            {
                Actor.SetDestination(Actor.spikeLair.transform.position);
            }

            public override void Process()
            {
                if (Actor.HasReachedDestination())
                    SetState(States.SafeZone);

                if (PlayerState.IsPrey)
                    SetState(States.Patrol);
            }

            public override void LateProcess() { }
        }

        private class SafeZoneState : State
        {
            public SafeZoneState(FSMController controller) : base(controller) { }

            public override void Enter()
            {
                // Enter lair

                Actor.DisableGraphics();
            }

            public override void Process()
            {
                if (PlayerState.IsPrey)
                    SetState(States.Patrol);
            }

            public override void Exit()
            {
                // Exit lair

                Actor.EnableGraphics();
            }

            public override void LateProcess() { }
        }
    }
}

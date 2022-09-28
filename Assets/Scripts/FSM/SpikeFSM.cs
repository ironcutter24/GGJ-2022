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
                new StunnedState(this),
                new RunAwayState(this),
                new SafeZoneState(this)
                );
        }

        public class ChaseState : State
        {
            public ChaseState(FSMController controller) : base(controller) { }

            public override void Enter()
            {
                Actor.SetSpeedToRun();
                Actor.SetDestination(Controller3D.Instance.Pos);
            }

            public override void Process()
            {
                if (Actor.CanSeePlayer())
                {
                    if (PlayerState.IsPrey)
                        Actor.SetDestination(Controller3D.Instance.Pos);
                    else
                        SetState(States.RunAway);
                }

                if (Actor.HasReachedDestination())
                {
                    if (Actor.CanSeePlayer())
                    {
                        if (Actor.IsPlayerInAttackRange())
                            SetState(States.Attack);
                    }
                    else
                        SetState(States.Patrol);
                }
            }
        }

        private class RunAwayState : State
        {
            public RunAwayState(FSMController controller) : base(controller) { }

            public override void Enter()
            {
                Actor.SetSpeedToRun();
                Actor.SetDestination(Actor.GetNearestCoffin());
            }

            public override void Process()
            {
                if (PlayerState.IsHunter)
                {
                    if (Actor.HasReachedDestination())
                        SetState(States.SafeZone);
                }
                else 
                    SetState(States.Patrol);
            }

            public override void LateProcess()
            {
                if (Actor.IsStunned)
                    SetState(States.Stunned);
            }
        }

        private class SafeZoneState : State
        {
            public SafeZoneState(FSMController controller) : base(controller) { }

            public override void Enter()
            {
                Actor.PauseMovement();
                Actor.SetCollisionsAndGraphics(false);  // Enter lair
            }

            public override void Process()
            {
                if (PlayerState.IsPrey)
                    SetState(States.Patrol);
            }

            public override void Exit()
            {
                Actor.SetCollisionsAndGraphics(true);  // Exit lair
                Actor.ResumeMovement();
            }

            public override void LateProcess() { }
        }
    }
}

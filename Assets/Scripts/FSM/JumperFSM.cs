using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public class JumperFSM : FSMController
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
                Actor.SetDestination(Actor.GetTeleportDestination());
            }

            public override void Process()
            {
                if (Actor.HasReachedDestination())
                {
                    if (Actor.CanSeePlayer())
                    {
                        if (Actor.IsPlayerInAttackRange())
                            SetState(States.Attack);
                    }
                    else
                        SetState(States.Idle);
                }
            }
        }

        private class RunAwayState : State
        {
            public RunAwayState(FSMController controller) : base(controller) { }

            public override void Process()
            {
                
            }

            public override void LateProcess() { }
        }

        private class SafeZoneState : State
        {
            public SafeZoneState(FSMController controller) : base(controller) { }

            public override void Process()
            {
                
            }

            public override void LateProcess() { }
        }
    }

    public abstract class JumperFSMState : State
    {
        public JumperFSMState(JumperFSM controller) : base(controller) { }
    }
}

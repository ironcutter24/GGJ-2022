using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    /*
     * METHODS EXAMPLE
     * 
     * 

     public class StateClassDocumentation : State
     {
         public StateClassDocumentation(FSMController controller) : base(controller) { }

         public override void Enter()
         {
             Actor.PerformAction();
         }

         public override void Process()
         {
             if(Actor.ActionIsDone())
                SetState(States.NextState);
         }

         public override void Exit() { }
     }

     */

    public class IdleState : State
    {
        public IdleState(FSMController controller) : base(controller) { }

        public override void Enter()
        {
            Actor.SetSpeedToWalk();
        }

        public override void Process()
        {
            if (Actor.HasWaypoints)
            {
                SetState(States.Patrol);
            }
            else
            {
                if (Actor.CanSeePlayer())
                {
                    if (PlayerState.IsPrey)
                        SetState(States.Chase);
                    else
                        SetState(States.RunAway);
                }
            }
        }
    }

    public class PatrolState : State
    {
        public PatrolState(FSMController controller) : base(controller) { }

        public override void Enter()
        {
            Actor.SetSpeedToWalk();

            if (Actor.HasWaypoints)
                Actor.SetDestination(Actor.GetNearestWaypoint());
        }

        public override void Process()
        {
            if (Actor.HasWaypoints)
            {
                if (Actor.HasReachedDestination())
                    Actor.SetDestination(Actor.GetNextWaypoint());

                if (Actor.CanSeePlayer())
                {
                    if (PlayerState.IsPrey)
                        SetState(States.Chase);
                    else
                        SetState(States.RunAway);
                }
            }
            else
                SetState(States.Idle);
        }
    }

    public class AttackState : State
    {
        public AttackState(FSMController controller) : base(controller) { }

        public override void Enter()
        {
            Actor.PauseMovement();
            Actor.StartAttack();
        }

        public override void Process()
        {
            if (!Actor.IsAttacking)
                SetState(States.Chase);
        }

        public override void LateProcess() { }

        public override void Exit()
        {
            Actor.ResumeMovement();
        }
    }

    public class StunnedState : State
    {
        public StunnedState(FSMController controller) : base(controller) { }

        public override void Enter()
        {
            Actor.PauseMovement();
            Actor.StartStunCountdown();
        }

        public override void Process()
        {
            if (!Actor.IsStunned)
            {
                if (PlayerState.IsPrey)
                    SetState(States.Patrol);
                else
                    SetState(States.RunAway);
            }
        }

        public override void LateProcess() { }

        public override void Exit()
        {
            Actor.ResumeMovement();
        }
    }
}

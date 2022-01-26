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

        public override void Process()
        {
            throw new System.NotImplementedException();
        }
    }

    public class PatrolState : State
    {
        public PatrolState(FSMController controller) : base(controller) { }

        public override void Enter()
        {
            Actor.SetDestination(Actor.PeekNextWaypoint());
        }

        public override void Process()
        {
            if (Actor.HasReachedDestination())
                Actor.SetDestination(Actor.PeekNextWaypoint());

            if (Actor.CanSeePlayer())
                SetState(States.Chase);
        }
    }

    public class ChaseState : State
    {
        public ChaseState(FSMController controller) : base(controller) { }

        public override void Process()
        {
            Actor.SetDestination(Controller3D.Instance.Pos);
        }
    }

    public class AttackState : State
    {
        public AttackState(FSMController controller) : base(controller) { }

        public override void Process()
        {
	        Actor.Attack();
        }
    }
}

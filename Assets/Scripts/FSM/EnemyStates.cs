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
             TargetUnit.PerformAction();
         }

         public override void Process()
         {
             if(TargetUnit.ActionIsDone())
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

        public override void Process()
        {
            throw new System.NotImplementedException();
        }
    }

    public class ChaseState : State
    {
        public ChaseState(FSMController controller) : base(controller) { }

        public override void Process()
        {
            throw new System.NotImplementedException();
        }
    }

    public class RunAwayState : State
    {
        public RunAwayState(FSMController controller) : base(controller) { }

        public override void Process()
        {
            throw new System.NotImplementedException();
        }
    }

    public class AttackState : State
    {
        public AttackState(FSMController controller) : base(controller) { }

        public override void Process()
        {
            throw new System.NotImplementedException();
        }
    }

}

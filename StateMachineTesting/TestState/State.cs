using System;
using StateMachineCore;

namespace StateMachineTesting.TestState
{
    public class State : AState
    {
        public Guid Id { get; protected set; }
        public int UpdateIterations { get; protected set; }
        public int StartIterations { get; protected set; }
        public int EndIterations { get; protected set; }

        private bool shouldComplete = false;


        public State(StateMachine machine) : base(machine)
        {
            Id = Guid.NewGuid();
        }

        protected override bool UpdateState()
        {
            ++UpdateIterations;

            return shouldComplete;
        }

        protected override void Start()
        {
            ++StartIterations;
            shouldComplete = false;
        }

        protected override void End()
        {
            ++EndIterations;
        }

        public void ShouldCompleteAfterNextUpdate(bool complete)
        {
            shouldComplete = complete;
        }
    }
}

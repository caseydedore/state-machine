
using StateMachineCore;
using System;

namespace StateMachineTesting
{
    public class TestState : State
    {
        public int UpdateIterations { get; protected set; }
        public int StartIterations { get; protected set; }
        public int EndIterations { get; protected set; }
        public Action<Blackboard> UpdateAction { get; set; } = (Blackboard b) => { };

        public TestState(Blackboard b = null) : base(b) { }

        protected override void UpdateState()
        {
            ++UpdateIterations;
            UpdateAction(Blackboard);
        }

        public override void Start()
        {
            ++StartIterations;
        }

        public override void End()
        {
            ++EndIterations;
        }
    }
}

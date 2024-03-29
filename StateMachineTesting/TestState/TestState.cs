
using StateMachineCore;
using System;

namespace StateMachineTesting
{
    public class TestState : State
    {
        public int UpdateIterations { get; protected set; }
        public int StartIterations { get; protected set; }
        public int EndIterations { get; protected set; }

        public TestState
        (
            Action start = null, Action update = null, Action end = null
        ) : base
        (
            start, update, end
        )
        {
            UpdateState += () => ++UpdateIterations;
            StartState += () => ++StartIterations;
            EndState += () => ++EndIterations;
        }
    }
}

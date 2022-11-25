
using StateMachineCore;
using System;

namespace StateMachineTesting
{
    public class TestState : State
    {
        public int UpdateIterations { get; protected set; }
        public int StartIterations { get; protected set; }
        public int EndIterations { get; protected set; }
        public int OptionalUpdateIterations { get; protected set; }
        public int OptionalStartIterations { get; protected set; }
        public int OptionalEndIterations { get; protected set; }

        public TestState
        (
            Action start = null, Action update = null, Action end = null,
            Action optionalStart = null, Action optionalUpdate = null, Action optionalEnd = null
        ) : base
        (
            start, update, end,
            optionalStart, optionalUpdate, optionalEnd
        )
        {
            UpdateState += () => ++UpdateIterations;
            StartState += () => ++StartIterations;
            EndState += () => ++EndIterations;
            OptionalUpdateState += () => ++OptionalUpdateIterations;
            OptionalStartState += () => ++OptionalStartIterations;
            OptionalEndState += () => ++OptionalEndIterations;
        }
    }
}

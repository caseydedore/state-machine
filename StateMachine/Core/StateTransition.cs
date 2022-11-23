
using System;

namespace StateMachineCore
{
    public class StateTransition
    {
        public Func<bool> Condition { get; }
        public IState State { get; }
        public TransitionMode Mode { get; }

        public StateTransition(Func<bool> condition, IState state, TransitionMode mode)
        {
            Condition = condition;
            State = state;
            Mode = mode;
        }

        public StateTransition(Func<bool> condition, IState state) 
            : this(condition, state, TransitionMode.Immediate) {}
    }
}

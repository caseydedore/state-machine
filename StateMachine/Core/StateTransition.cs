
using System;

namespace StateMachineCore
{
    public class StateTransition
    {
        public Func<bool> Condition { get; }
        public IState State { get; }

        public StateTransition(Func<bool> condition, IState state)
        {
            Condition = condition;
            State = state;
        }
    }
}

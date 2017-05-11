
using System;

namespace StateMachineCore
{
    public class StateTransition
    {
        public Func<bool> Condition { get; set; }
        public IState State { get; set; }
        public TransitionBehavior Behavior { get; set; }
    }

    public enum TransitionBehavior { EndState, DoNotEndState, WaitForEndState };
}

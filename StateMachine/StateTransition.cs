
using System;

namespace StateMachineCore
{
    public class StateTransition
    {
        public Func<bool> Condition { get; set; }
        public IState State { get; set; }
    }
}

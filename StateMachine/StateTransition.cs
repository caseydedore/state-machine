
using System;
using System.Collections.Generic;

namespace StateMachineCore
{
    public class StateTransition
    {
        public Func<bool> Condition { get; set; }
        public IState[] States { get; set; }

        public StateTransition()
        {
            States = new IState[] { };
        }
    }
}

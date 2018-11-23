
using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachineCore
{
    public abstract class AState : IState
    {
        private List<StateTransition> Transitions { get; set; } = new List<StateTransition>();

        public StateTransition Update()
        {
            UpdateState();
            return GetFirstSuccessfulTransition();
        }

        public virtual void Start() { }
        public virtual void End() { }

        protected abstract void UpdateState();

        public void AddTransition(Func<bool> checkCondition, IState transitionState)
        {
            var transition = new StateTransition(checkCondition, transitionState);
            AddTransition(transition);
        }

        public void AddTransition(StateTransition transition)
        {
            Transitions.Add(transition);
        }

        private StateTransition GetFirstSuccessfulTransition()
        {
            return Transitions.Where(t => t.Condition()).FirstOrDefault();
        }
    }
}

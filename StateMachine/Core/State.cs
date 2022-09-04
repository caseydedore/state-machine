
using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachineCore
{
    public class State : IState
    {
        List<StateTransition> Transitions { get; set; } = new List<StateTransition>();

        public State(Action start = null, Action update = null, Action end = null)
        {
            if (start != null) StartState = start;
            if (update != null) UpdateState = update;
            if (end != null) EndState = end;
        }

        public StateTransition Update()
        {
            UpdateState();
            return GetFirstSuccessfulTransition();
        }

        public void Start() => StartState();

        public void End() => EndState();

        public void AddTransition(Func<bool> checkCondition, IState transitionState)
        {
            var transition = new StateTransition(checkCondition, transitionState);
            AddTransition(transition);
        }

        public void AddTransition(StateTransition transition) =>
            Transitions.Add(transition);

        protected StateTransition GetFirstSuccessfulTransition() =>
            Transitions.Where(t => t.Condition()).FirstOrDefault();

        protected event Action StartState = () => { };
        protected event Action EndState = () => { };
        protected event Action  UpdateState = () => { };
    }
}

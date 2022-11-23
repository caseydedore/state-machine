
using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachineCore
{
    public class State : IState
    {
        List<StateTransition> Transitions { get; set; } = new List<StateTransition>();
        uint iterations = 0;

        public State(Action start = null, Action update = null, Action end = null)
        {
            if (start != null) StartState = start;
            if (update != null) UpdateState = update;
            if (end != null) EndState = end;
        }

        public StateTransition Update()
        {
            var transition = GetFirstSuccessfulTransition();
            if (transition == null)
            {
                UpdateState();
                ++iterations;
            }
            return transition;
        }

        public StateTransition Start()
        {
            var transition = GetFirstSuccessfulTransition();
            if (transition == null)
                StartState();
            return transition;
        }

        public void End()
        {
            EndState();
            iterations = 0;
        }

        public void AddTransition(Func<bool> checkCondition, IState transitionState, TransitionMode mode)
        {
            var transition = new StateTransition(checkCondition, transitionState, mode);
            AddTransition(transition);
        }

        public void AddTransition(Func<bool> checkCondition, IState transitionState)
        {
            var transition = new StateTransition(checkCondition, transitionState);
            AddTransition(transition);
        }

        public void AddTransition(StateTransition transition) =>
            Transitions.Add(transition);

        StateTransition GetFirstSuccessfulTransition() =>
            Transitions
                .Where(t => iterations > 0 || t.Mode == TransitionMode.Immediate)
                .Where(t => t.Condition())
                .FirstOrDefault();

        protected event Action StartState = () => { };
        protected event Action EndState = () => { };
        protected event Action  UpdateState = () => { };
    }
}

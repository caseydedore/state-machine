
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
            UpdateState();
            ++iterations;
            var transition = GetFirstSuccessfulTransition();
            return transition;
        }

        public void Start() => StartState();

        public void End()
        {
            EndState();
            iterations = 0;
        }

        public void AddTransitionAfter(uint numberOfUpdates, IState transitionState)
        {
            var transition = new StateTransition(numberOfUpdates, transitionState);
            AddTransition(transition);
        }

        public void AddTransitionAfter(uint numberOfUpdates, Func<bool> checkCondition, IState transitionState)
        {
            var transition = new StateTransition(numberOfUpdates, checkCondition, transitionState);
            AddTransition(transition);
        }

        public void AddTransition(Func<bool> checkCondition, IState transitionState)
        {
            var transition = new StateTransition(checkCondition, transitionState);
            AddTransition(transition);
        }

        public void AddTransition(StateTransition transition) => Transitions.Add(transition);

        StateTransition GetFirstSuccessfulTransition() =>
            Transitions
                .Where(t => t.MinimumUpdates <= iterations)
                .Where(t => t.Condition())
                .FirstOrDefault();

        protected event Action StartState = () => { };
        protected event Action EndState = () => { };
        protected event Action  UpdateState = () => { };
    }
}

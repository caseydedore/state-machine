
using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachineCore
{
    public class State : IState
    {
        List<StateTransition> Transitions { get; set; } = new List<StateTransition>();
        List<StateTransition> PostTransitions { get; set; } = new List<StateTransition>();

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
                UpdateState();
            if (transition == null)
                transition = GetFirstSuccessfulPostTransition();
            return transition;
        }

        public StateTransition Start()
        {
            var transition = GetFirstSuccessfulTransition();
            if (transition == null)
                StartState();
            return transition;
        }

        public void End() => EndState();

        public void AddTransition(Func<bool> checkCondition, IState transitionState)
        {
            var transition = new StateTransition(checkCondition, transitionState);
            AddTransition(transition);
        }

        public void AddTransition(StateTransition transition) =>
            Transitions.Add(transition);

        public void AddPostTransition(Func<bool> checkCondition, IState transitionState)
        {
            var transition = new StateTransition(checkCondition, transitionState);
            AddPostTransition(transition);
        }

        public void AddPostTransition(StateTransition transition) =>
            PostTransitions.Add(transition);

        StateTransition GetFirstSuccessfulTransition() =>
            Transitions.Where(t => t.Condition()).FirstOrDefault();

        StateTransition GetFirstSuccessfulPostTransition() =>
            PostTransitions.Where(t => t.Condition()).FirstOrDefault();

        protected event Action StartState = () => { };
        protected event Action EndState = () => { };
        protected event Action  UpdateState = () => { };
    }
}

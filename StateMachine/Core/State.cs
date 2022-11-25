
using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachineCore
{
    public class State : IState
    {
        List<StateTransition> Transitions { get; set; } = new List<StateTransition>();
        uint iterations = 0;

        public State
        (
            Action start = null, Action update = null, Action end = null,
            Action optionalStart = null, Action optionalUpdate = null, Action optionalEnd = null
        )
        {
            if (start != null) StartState = start;
            if (update != null) UpdateState = update;
            if (end != null) EndState = end;
            if (optionalStart != null) OptionalStartState = optionalStart;
            if (optionalUpdate != null) OptionalUpdateState = optionalUpdate;
            if (optionalEnd != null) OptionalEndState = optionalEnd;
        }

        public StateTransition Update()
        {
            UpdateState();
            ++iterations;
            var ignoreIterationTransition = GetFirstSuccessfulTransitionBeforeCurrentIteration();
            if (ignoreIterationTransition == null)
                OptionalUpdateState();
            var transition = GetFirstSuccessfulTransition();
            return transition;
        }

        public void Start()
        {
            StartState();
            var transition = GetFirstSuccessfulTransition();
            if (transition == null)
                OptionalStartState();
        }

        public void End()
        {
            EndState();
            var transition = GetFirstSuccessfulTransition();
            if (transition == null)
                OptionalEndState();
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

        StateTransition GetFirstSuccessfulTransitionBeforeCurrentIteration() =>
            Transitions
                .Where(t => t.MinimumUpdates <= iterations - 1)
                .Where(t => t.Condition())
                .FirstOrDefault();

        protected event Action StartState = () => { };
        protected event Action EndState = () => { };
        protected event Action  UpdateState = () => { };

        protected event Action OptionalStartState = () => { };
        protected event Action OptionalUpdateState = () => { };
        protected event Action OptionalEndState = () => { };
    }
}

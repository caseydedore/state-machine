
using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachineCore
{
    public class State : IState
    {
        List<StateTransition> Transitions { get; set; } = new List<StateTransition>();

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

        StateTransition GetFirstSuccessfulTransition() =>
            Transitions.Where(t => t.Condition()).FirstOrDefault();

        public event Action StartState = () => { };
        public event Action EndState = () => { };
        public event Action UpdateState = () => { };
    }
}

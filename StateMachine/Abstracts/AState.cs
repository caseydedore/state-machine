
using System;
using System.Collections.Generic;

namespace StateMachineCore
{

    public abstract class AState : IState
    {
        public IStateMachine StateMachine { get; private set; }
        public Status Status { get; protected set; }

        private List<StateTransition> Transitions { get; set; }
        private IState transitionStateWaitingForEnd = null;

        private bool hasTransitionStateWaitingForEnd = true;


        public AState(IStateMachine machine)
        {
            StateMachine = machine;
            Transitions = new List<StateTransition>();
            Status = Status.Inactive;
        }

        public void Update()
        {
            if (Status != Status.Active)
            {
                Status = Status.Active;
                hasTransitionStateWaitingForEnd = false;
                Start();
            }

            Status = UpdateState();
            if (Status == Status.Inactive)
            {
                End();
            }

            if (ExecuteTransitionIfAvailable())
            {
                if (Status != Status.Inactive)
                {
                    Status = Status.Inactive;
                    End();
                }
            }
        }

        protected virtual void Start() { }
        protected virtual void End() { }

        protected abstract Status UpdateState();

        public void AddTransition(Func<bool> checkCondition)
        {
            AddTransition(checkCondition, null);
        }

        public void AddTransition(Func<bool> checkCondition, IState transitionState)
        {
            var transition =
                new StateTransition() { Condition = checkCondition, State = transitionState };
            AddTransition(transition);
        }

        public void AddTransition(StateTransition transition)
        {
            Transitions.Add(transition);
        }

        private bool ExecuteTransitionIfAvailable()
        {
            foreach (var transition in Transitions)
            {
                if (transition.Condition())
                {
                    if(transition.State != null)
                    {
                        StateMachine.AddState(transition.State);
                    }
                    StateMachine.RemoveState(this);
                    return true;
                }
            }

            return false;
        }
    }
}

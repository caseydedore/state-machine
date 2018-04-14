
using System;
using System.Collections.Generic;
using System.Linq;

namespace StateMachineCore
{

    public abstract class AState : IState
    {
        public IStateMachine StateMachine { get; private set; }
        public Status Status { get; protected set; }

        private List<StateTransition> Transitions { get; set; }
        private IState transitionStateWaitingForEnd = null;


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
            AddTransition(checkCondition, new IState[] { });
        }

        public void AddTransition(Func<bool> checkCondition, IState transitionState)
        {
            var transitionStates = new IState[] { transitionState };
            AddTransition(checkCondition, transitionStates);
        }

        public void AddTransition(Func<bool> checkCondition, IState[] transitionStates)
        {
            var transition =
                new StateTransition()
                {
                    Condition = checkCondition,
                    States = transitionStates
                };
            AddTransition(transition);
        }

        public void AddTransition(StateTransition transition)
        {
            Transitions.Add(transition);
        }

        private bool ExecuteTransitionIfAvailable()
        {
            var firstSuccessfulTransition = Transitions.Where(t => t.Condition()).FirstOrDefault();
            if(firstSuccessfulTransition != null)
            {
                StateMachine.RemoveState(this);
                firstSuccessfulTransition.States.ToList().ForEach(state => { StateMachine.AddState(state); } );
                return true;
            }
            return false;
        }
    }
}

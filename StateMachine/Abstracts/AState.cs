
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

        private bool 
            hasTransitionedOut = false,
            hasTransitionStateWaitingForEnd = true;


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

            if (UpdateState())
            {
                Status = Status.Inactive;
                End();
            }

            if (CheckForTransitions())
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

        protected abstract bool UpdateState();

        public void AddTransitionToEnd(Func<bool> checkCondition)
        {
            AddTransition(checkCondition, null, TransitionBehavior.EndState);
        }

        public void AddTransitionToEnd(Func<bool> checkCondition, TransitionBehavior behavior)
        {
            AddTransition(checkCondition, null, behavior);
        }

        public void AddTransition(Func<bool> checkCondition, IState transitionState)
        {
            AddTransition(checkCondition, transitionState, TransitionBehavior.EndState);
        }

        public void AddTransition(Func<bool> checkCondition, IState transitionState, TransitionBehavior behavior)
        {
            var transition =
                new StateTransition() { Condition = checkCondition, State = transitionState, Behavior = behavior };
            AddTransition(transition);
        }

        public void AddTransition(StateTransition transition)
        {
            Transitions.Add(transition);
        }

        private bool CheckForTransitions()
        {
            hasTransitionedOut = false;

            if (!CheckForWaitingTransition())
            {
                hasTransitionedOut = CheckPotentialTransitions();
            }

            return hasTransitionedOut;
        } 

        private bool CheckPotentialTransitions()
        {
            foreach (var t in Transitions)
            {
                if (t.Condition())
                {
                    if (t.Behavior == TransitionBehavior.DoNotEndState)
                    {
                        if (t.State != null)
                        {
                            StateMachine.AddState(t.State);
                        }
                    }
                    else if (t.Behavior == TransitionBehavior.EndState)
                    {
                        if (t.State != null)
                        {
                            StateMachine.AddState(t.State);
                        }

                        StateMachine.RemoveState(this);
                        return true;
                    }
                    else if (t.Behavior == TransitionBehavior.WaitForEndState)
                    {
                        hasTransitionStateWaitingForEnd = true;
                        if (t.State != null)
                        {
                            transitionStateWaitingForEnd = t.State;
                        }
                    }
                }
            }

            return false;
        }

        private void HandleSuccessfulTransitionBasedOnBehavior(StateTransition t)
        {
            if (t.Behavior == TransitionBehavior.DoNotEndState)
            {
                if (t.State != null)
                {
                    StateMachine.AddState(t.State);
                }
            }
            else if (t.Behavior == TransitionBehavior.EndState)
            {
                if (t.State != null)
                {
                    StateMachine.AddState(t.State);
                }

                StateMachine.RemoveState(this);
                hasTransitionedOut = true;
            }
            else if (t.Behavior == TransitionBehavior.WaitForEndState)
            {
                hasTransitionStateWaitingForEnd = true;
                if (t.State != null)
                {
                    transitionStateWaitingForEnd = t.State;
                }
            }
        }

        private bool CheckIsTransitionExclusive(StateTransition transition)
        {
            if(transition.Behavior == TransitionBehavior.DoNotEndState)
            {
                return false;
            }

            return true;
        }

        private bool CheckForWaitingTransition()
        {
            if (hasTransitionStateWaitingForEnd && Status == Status.Inactive)
            {
                hasTransitionedOut = true;
                StateMachine.RemoveState(this);
                if(transitionStateWaitingForEnd != null)
                {
                    StateMachine.AddState(transitionStateWaitingForEnd);
                }
                return true;
            }
            else if (hasTransitionStateWaitingForEnd)
            {
                return true;
            }

            return false;
        }
    }
}

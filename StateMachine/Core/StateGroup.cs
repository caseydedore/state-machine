
using System;

namespace StateMachineCore
{
	public class StateGroup : State
	{
        IState currentState;
        IState nextState;

        public StateGroup
        (
            Action start = null, Action end = null,
            Action optionalStart = null, Action optionalEnd = null
        )
        {
            UpdateState += () =>
            {
                if (nextState != null)
                {
                    currentState = nextState;
                    nextState = null;
                    currentState.Start();
                }
                var transition = currentState?.Update() ?? Any.Update();
                if (transition != null)
                {
                    currentState?.End();
                    currentState = null;
                    nextState = transition.State;
                }
            };

            StartState += () =>
            {
                start?.Invoke();
                nextState = Entry;
            };

            EndState += () =>
            {
                currentState?.End();
                nextState = null;
                currentState = null;
                end?.Invoke();
            };

            OptionalStartState += optionalStart;
            OptionalEndState += optionalEnd;
        }

        public IState Entry { get; set; }

        public IState Any { get; } = new State();
    }
}


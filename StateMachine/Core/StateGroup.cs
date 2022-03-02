
namespace StateMachineCore
{
	public class StateGroup : State
	{
        IState currentState;
        IState nextState;

        public StateGroup(IState entry = null)
        {
            if (entry != null)
                Entry = entry;

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
                nextState = Entry;

            EndState += () =>
            {
                currentState?.End();
                nextState = null;
                currentState = null;
            };
        }

        public IState Entry { get; set; }

        public IState Any { get; } = new State();
    }
}


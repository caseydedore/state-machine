
namespace StateMachineCore
{
	public class StateMachine : IStateMachine
	{
        private IState currentState;
        private IState entry;
        private IState nextState;

        public void Update()
        {
            if (nextState != null)
            {
                currentState = nextState;
                nextState = null;
                currentState?.Start();
            }
            var transition = currentState?.Update();
            if (transition != null)
            {
                currentState?.End();
                currentState = null;
                nextState = transition.State;
            }
        }

        public IState Entry
        {
            get { return entry; }
            set
            {
                entry = value;
                nextState = entry;
            }
        }
    }

}


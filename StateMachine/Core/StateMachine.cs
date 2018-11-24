
namespace StateMachineCore
{
	public class StateMachine : IStateMachine
	{
        private IState entry;
        private IState any = new State();
        private IState currentState;
        private IState nextState;

        public void Update()
        {
            if (nextState != null)
            {
                currentState = nextState;
                nextState = null;
                currentState.Start();
            }
            var transition = currentState?.Update() ?? any.Update();
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

        public IState Any
        {
            get { return any; }
        }
    }
}


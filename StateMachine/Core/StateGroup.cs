
namespace StateMachineCore
{
	public abstract class StateGroup : State
	{
        private IState currentState;
        private IState nextState;

        protected sealed override void UpdateState()
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
        }

        public sealed override void Start()
        {
            StartState();
            nextState = Entry;
        }

        public sealed override void End()
        {
            EndState();
            currentState?.End();
            nextState = null;
            currentState = null;
        }

        protected IState Entry { get; set; }
        protected IState Any { get; } = new BlankState();
        protected virtual void StartState() { }
        protected virtual void EndState() { }
    }
}


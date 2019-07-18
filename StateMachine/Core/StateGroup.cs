
namespace StateMachineCore
{
	public abstract class StateGroup : StateCore
	{
        private IState currentState;
        private IState nextState;

        protected override void UpdateState()
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

        public override void Start()
        {
            nextState = Entry;
        }

        public override void End()
        {
            currentState?.End();
            nextState = null;
            currentState = null;
        }

        protected IState Entry { get; set; }
        protected IState Any { get; } = new BlankState();
    }
}


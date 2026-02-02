
namespace StateMachine.Core
{
	public class StateGroup : State
	{
        IState? currentState;
        IState? nextState;
        uint currentStateIterations = 0;
        List<StateTransition> Transitions { get; set; } = [];
        IState Any { get; } = new State();

        public StateGroup
        (
            Action? start = null, Action? update = null, Action? end = null,
            Action? optionalStart = null, Action? optionalUpdate = null, Action? optionalEnd = null
        )
        {
            OptionalUpdateState += () =>
            {
                if (nextState != null)
                {
                    currentState = nextState;
                    nextState = null;
                    var allowOptionalStart = GetFirstSuccessfulTransition() == null;
                    if (allowOptionalStart)
                    {
                        currentState?.Start();
                    }
                    else
                    {
                        currentState?.StartSkipOptional();
                    }
                }
                currentStateIterations++;
                var allowOptionalUpdate =
                    GetFirstSuccessfulTransitionBeforeCurrentIteration() == null;
                if (allowOptionalUpdate)
                {
                    currentState?.Update();
                }
                else
                {
                    currentState?.UpdateSkipOptional();
                }
                var transition = GetFirstSuccessfulTransition() ?? GetFirstSuccessfulAnyTransition();
                if (transition != null)
                {
                    currentState?.EndSkipOptional();
                    currentState = null;
                    currentStateIterations = 0;
                    nextState = transition.To;
                }
                optionalUpdate?.Invoke();
            };

            StartState += () =>
            {
                nextState = Entry;
                start?.Invoke();
            };

            EndState += () =>
            {
                var allowOptionalEnd = GetFirstSuccessfulTransition() == null;
                if (allowOptionalEnd)
                {
                    currentState?.End();
                }
                else
                {
                   currentState?.EndSkipOptional();
                }
                currentState = null;
                currentStateIterations = 0;
                nextState = null;
                end?.Invoke();
            };

            UpdateState += update;
            OptionalStartState += optionalStart;
            OptionalEndState += optionalEnd;
        }

        public void AddTransitionAfter(uint numberOfUpdates, IState from, IState to)
        {
            var transition = new StateTransition(numberOfUpdates, from, to);
            AddTransition(transition);
        }

        public void AddTransitionAfter(uint numberOfUpdates, Func<bool> checkCondition, IState from, IState to)
        {
            var transition = new StateTransition(numberOfUpdates, checkCondition, from, to);
            AddTransition(transition);
        }

        public void AddTransition(Func<bool> checkCondition, IState from, IState to)
        {
            var transition = new StateTransition(checkCondition, from, to);
            AddTransition(transition);
        }

        public void AddTransition(StateTransition transition) => Transitions.Add(transition);

        public void AddAnyTransitionAfter(uint numberOfUpdates, IState to)
        {
            var transition = new StateTransition(numberOfUpdates, Any, to);
            AddTransition(transition);
        }

        public void AddAnyTransitionAfter(uint numberOfUpdates, Func<bool> checkCondition, IState to)
        {
            var transition = new StateTransition(numberOfUpdates, checkCondition, Any, to);
            AddTransition(transition);
        }

        public void AddAnyTransition(Func<bool> checkCondition, IState to)
        {
            var transition = new StateTransition(checkCondition, Any, to);
            AddTransition(transition);
        }

        StateTransition? GetFirstSuccessfulTransition() =>
            Transitions
                .Where(t => ReferenceEquals(t.From, currentState))
                .Where(t => t.MinimumUpdates <= currentStateIterations)
                .Where(t => t.Condition())
                .FirstOrDefault();

        StateTransition? GetFirstSuccessfulAnyTransition() =>
            Transitions
                .Where(t => ReferenceEquals(t.From, Any))
                .Where(t => t.Condition())
                .FirstOrDefault();

        StateTransition? GetFirstSuccessfulTransitionBeforeCurrentIteration() =>
            Transitions
                .Where(t => ReferenceEquals(t.From, currentState))
                .Where(t => t.MinimumUpdates <= currentStateIterations - 1)
                .Where(t => t.Condition())
                .FirstOrDefault();

        public IState? Entry { get; set; }
    }
}
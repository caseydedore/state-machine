
namespace StateMachine.Core
{
	public class StateGroup : State
	{
        IState? currentState;
        IState? nextState;
        uint currentStateIterations = 0;
        List<StateTransition> Transitions { get; set; } = [];

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
                    currentState?.Start();
                    var allowOptionalStart = GetFirstSuccessfulTransition() == null;
                    if (allowOptionalStart)
                    {
                        currentState?.OptionalStart();
                    }
                }
                currentState?.Update();
                currentStateIterations++;
                var allowOptionalUpdate =
                    GetFirstSuccessfulTransitionBeforeCurrentIteration() == null;
                if (allowOptionalUpdate)
                {
                    currentState?.OptionalUpdate();
                }
                var transition = GetFirstSuccessfulTransition() ?? GetFirstSuccessfulAnyTransition();
                if (transition != null)
                {
                    currentState?.End();
                    var allowOptionalEnd = GetFirstSuccessfulTransition() == null;
                    if (allowOptionalEnd)
                    {
                        currentState?.OptionalEnd();
                    }
                    currentState = null;
                    currentStateIterations = 0;
                    nextState = transition.From;
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
                currentState?.End();
                var allowOptionalEnd = GetFirstSuccessfulTransition() == null;
                if (allowOptionalEnd)
                {
                    currentState?.OptionalEnd();
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

        StateTransition? GetFirstSuccessfulTransition() =>
            Transitions
                .Where(t => t.From == currentState)
                .Where(t => t.MinimumUpdates <= currentStateIterations)
                .Where(t => t.Condition())
                .FirstOrDefault();

        StateTransition? GetFirstSuccessfulAnyTransition() =>
            Transitions
                .Where(t => t.From == Any)
                .Where(t => t.Condition())
                .FirstOrDefault();

        StateTransition? GetFirstSuccessfulTransitionBeforeCurrentIteration() =>
            Transitions
                .Where(t => t.From == currentState)
                .Where(t => t.MinimumUpdates <= currentStateIterations - 1)
                .Where(t => t.Condition())
                .FirstOrDefault();

        public IState? Entry { get; set; }

        public IState Any { get; } = new State();
    }
}



// Previously in State
/*
        public void Update()
        {
            UpdateState();
            ++iterations;
            var ignoreIterationTransition = GetFirstSuccessfulTransitionBeforeCurrentIteration();
            if (ignoreIterationTransition == null)
                OptionalUpdateState();
            var transition = GetFirstSuccessfulTransition();
            return transition;
        }

        public void Start()
        {
            StartState();
            var transition = GetFirstSuccessfulTransition();
            if (transition == null)
                OptionalStartState();
        }

        public void End()
        {
            EndState();
            var transition = GetFirstSuccessfulTransition();
            if (transition == null)
                OptionalEndState();
            iterations = 0;
        }
*/

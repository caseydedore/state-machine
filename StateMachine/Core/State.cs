namespace StateMachine.Core
{
    public class State : IState
    {
        public State
        (
            Action? start = null, Action? update = null, Action? end = null,
            Action? optionalStart = null, Action? optionalUpdate = null, Action? optionalEnd = null
        )
        {
            if (start != null) StartState = start;
            if (update != null) UpdateState = update;
            if (end != null) EndState = end;
            if (optionalStart != null) OptionalStartState = optionalStart;
            if (optionalUpdate != null) OptionalUpdateState = optionalUpdate;
            if (optionalEnd != null) OptionalEndState = optionalEnd;
        }

        public void Start() => StartState();

        public void End() => EndState();

        public void Update() => UpdateState();

        public void OptionalStart() => OptionalStartState();

        public void OptionalEnd() => OptionalEndState();

        public void OptionalUpdate() => OptionalUpdateState();

        protected event Action StartState = () => { };
        protected event Action EndState = () => { };
        protected event Action UpdateState = () => { };
        protected event Action OptionalStartState = () => { };
        protected event Action OptionalUpdateState = () => { };
        protected event Action OptionalEndState = () => { };
    }
}

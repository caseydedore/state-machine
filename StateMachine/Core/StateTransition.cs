
namespace StateMachine.Core
{
    public class StateTransition
    {
        public Func<bool> Condition { get; } = () => true;
        public IState From { get; }
        public IState To { get; }
        public uint MinimumUpdates { get; } = 0;

        public StateTransition(IState from, IState to, Func<bool> condition)
        {
            Condition = condition;
            From = from;
            To = to;
        }

        public StateTransition(uint minimumUpdates, IState from, IState to, Func<bool> condition)
        {
            MinimumUpdates = minimumUpdates;
            Condition = condition;
            From = from;
            To = to;
        }

        public StateTransition(uint minimumUpdates, IState from, IState to)
        {
            MinimumUpdates = minimumUpdates;
            From = from;
            To = to;
        }
    }
}

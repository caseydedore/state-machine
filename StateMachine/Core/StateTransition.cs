
namespace StateMachine.Core
{
    public class StateTransition
    {
        public Func<bool> Condition { get; } = () => true;
        public IState From { get; }
        public IState To { get; }
        public uint MinimumUpdates { get; } = 0;

        public StateTransition(Func<bool> condition, IState from, IState to)
        {
            Condition = condition;
            From = from;
            To = to;
        }
        public StateTransition(uint minimumUpdates, Func<bool> condition, IState from, IState to)
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

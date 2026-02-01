
namespace StateMachine.Core
{
    public class StateTransition
    {
        public Func<bool> Condition { get; } = () => true;
        public IState State { get; }
        public uint MinimumUpdates { get; } = 0;

        public StateTransition(Func<bool> condition, IState state)
        {
            Condition = condition;
            State = state;
        }
        public StateTransition(uint minimumUpdates, Func<bool> condition, IState state)
        {
            MinimumUpdates = minimumUpdates;
            Condition = condition;
            State = state;
        }

        public StateTransition(uint minimumUpdates, IState state)
        {
            MinimumUpdates = minimumUpdates;
            State = state;
        }
    }
}

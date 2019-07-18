
namespace StateMachineCore
{
    public class StateContext
    {
        Blackboard Blackboard { get; }

        public StateContext(Blackboard blackboard)
        {
            Blackboard = blackboard;
        }
    }
}

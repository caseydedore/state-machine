
namespace StateMachineCore
{
    public abstract class State : StateCore
    {
        protected Blackboard Blackboard { get; }

        public State(Blackboard blackboard = null)
        {
            Blackboard = blackboard ?? new Blackboard();
        }
    }
}

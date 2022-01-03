
using StateMachineCore;

namespace StateMachineTesting
{
    public class TestState : State
    {
        public int UpdateIterations { get; protected set; }
        public int StartIterations { get; protected set; }
        public int EndIterations { get; protected set; }

        public TestState()
        {
            UpdateState += () => ++UpdateIterations;
            StartState += () => ++StartIterations;
            EndState += () => ++EndIterations;
        }
    }
}

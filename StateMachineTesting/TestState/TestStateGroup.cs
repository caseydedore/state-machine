using StateMachineCore;

namespace StateMachineTesting
{
    public class TestStateGroup : StateGroup
    {
        public int StartIterations { get; protected set; }
        public int EndIterations { get; protected set; }

        public int UpdateIterations { get; protected set; }

        public TestStateGroup()
        {
            StartState += () => ++StartIterations;
            EndState += () => ++EndIterations;
            UpdateState += () => ++UpdateIterations;
        }
    }
}

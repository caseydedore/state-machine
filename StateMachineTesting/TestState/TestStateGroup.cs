using StateMachineCore;

namespace StateMachineTesting
{
    public class TestStateGroup : StateGroup
    {
        public new IState Entry
        {
            get
            {
                return base.Entry;
            }
            
            set
            {
                base.Entry = value;
            }
        }

        public new IState Any
        {
            get
            {
                return base.Any;
            }
        }

        public int StartIterations { get; protected set; }
        public int EndIterations { get; protected set; }

        protected override void StartState() => ++StartIterations;

        protected override void EndState() => ++EndIterations;
    }
}

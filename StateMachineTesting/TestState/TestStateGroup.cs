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
    }
}

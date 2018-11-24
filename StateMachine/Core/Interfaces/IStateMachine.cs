
namespace StateMachineCore
{
	public interface IStateMachine
	{
        void Update();
        IState Entry { get; set; }
	}
}

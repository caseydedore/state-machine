
namespace StateMachine.Core
{
	public interface IState
	{
        void Start();
        void End();
        void Update();
        void OptionalStart();
        void OptionalEnd();
        void OptionalUpdate();
    }
}

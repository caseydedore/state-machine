
namespace StateMachineCore
{
	public interface IState
	{
        void Start();
        void End();
        StateTransition Update();
        void AddTransition(Func<bool> checkCondition, IState transitionState);
        void AddTransitionAfter(uint afterUpdates, IState transitionState);
        void AddTransitionAfter(uint afterUpdates, Func<bool> checkCondition, IState transitionState);
        void AddTransition(StateTransition transition);
    }
}

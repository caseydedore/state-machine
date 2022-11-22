
using System;

namespace StateMachineCore
{
	public interface IState
	{
        void Start();
        StateTransition StartOptional();
        void End();
        StateTransition Update();
        void AddTransition(Func<bool> checkCondition, IState transitionState);
        void AddTransition(StateTransition transition);
    }
}

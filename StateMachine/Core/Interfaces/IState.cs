
using System;

namespace StateMachineCore
{
	public interface IState
	{
        StateTransition Start();
        void End();
        StateTransition Update();
        void AddTransition(Func<bool> checkCondition, IState transitionState);
        void AddTransition(Func<bool> checkCondition, IState transitionState, TransitionMode mode);
        void AddTransition(StateTransition transition);
    }
}

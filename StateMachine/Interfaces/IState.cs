
using StateMachineCore;
using System;

namespace StateMachineCore
{
	public interface IState
	{
        IStateMachine StateMachine { get; }
        Status Status { get; }


        void Update();
        void AddTransitionToEnd(Func<bool> checkCondition);
        void AddTransitionToEnd(Func<bool> checkCondition, TransitionBehavior behavior);
        void AddTransition(Func<bool> checkCondition, IState transitionState);
        void AddTransition(Func<bool> checkCondition, IState transitionState, TransitionBehavior behavior);
        void AddTransition(StateTransition transition);
    }
}

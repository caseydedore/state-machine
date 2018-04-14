
using System;
using System.Collections.Generic;

namespace StateMachineCore
{
	public interface IState
	{
        IStateMachine StateMachine { get; }
        Status Status { get; }


        void Update();
        void AddTransition(Func<bool> checkCondition);
        void AddTransition(Func<bool> checkCondition, IState transitionState);
        void AddTransition(Func<bool> checkCondition, IState[] transitionStates);
        void AddTransition(StateTransition transition);
    }
}

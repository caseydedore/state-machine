
using System.Collections.Generic;

namespace StateMachineCore
{
	public interface IStateMachine
	{
		List<IState> CurrentStates { get; }

        void Update();
        void AddState(IState state);
        void RemoveState(IState state);
	}
}


using System.Collections.Generic;

namespace StateMachineCore
{

	public class StateMachine : IStateMachine
	{
        public List<IState> CurrentStates { get { return currentStates; } protected set { currentStates = value; } }
        private List<IState> currentStates = new List<IState>();
        private List<IState> statesToAdd = new List<IState>();
        private List<IState> statesToRemove = new List<IState>();

        private bool isInUpdate = false;


        public void Update()
        {
            isInUpdate = true;
            foreach (var s in currentStates)
            {
                s.Update();
            }
            isInUpdate = false;

            UpdateCurrentStates();
        }

        private void UpdateCurrentStates()
        {
            foreach(var s in statesToRemove)
            {
                currentStates.Remove(s);
            }
            statesToRemove.Clear();

            foreach(var s in statesToAdd)
            {
                currentStates.Add(s);
            }
            statesToAdd.Clear();
        }

        public void AddState(IState state)
        {
            if (isInUpdate)
            {
                statesToAdd.Add(state);
            }
            else
            {
                currentStates.Add(state);
            }
        }

        public void RemoveState(IState state)
        {
            if (isInUpdate)
            {
                statesToRemove.Add(state);
            }
            else
            {
                currentStates.Remove(state);
            }
        }
    }

}


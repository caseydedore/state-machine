
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCore;
using StateMachineTesting.TestState;

namespace StateMachineTesting
{
    [TestClass]
    public class StateMachineTest
    {
        private StateMachine machine = null;


        [TestMethod]
        public void Transition()
        {
            machine = new StateMachine();
            var state = new State(machine);
            var nextState = new State(machine);

            machine.AddState(state);
            state.AddTransition(() => { return true; }, nextState);

            machine.Update();

            Assert.AreEqual(1, machine.CurrentStates.Count);
            Assert.AreEqual(nextState, machine.CurrentStates[0]);
        }

        [TestMethod]
        public void UpdateIterations()
        {
            var numberOfIterations = 10;

            machine = new StateMachine();
            var state = new State(machine);
            machine.AddState(state);

            for(var i = 0; i < numberOfIterations; i++)
            {
                machine.Update();
            }

            Assert.AreEqual(numberOfIterations, state.UpdateIterations);
        }

        [TestMethod]
        public void Start()
        {
            machine = new StateMachine();
            var state = new State(machine);
            machine.AddState(state);

            machine.Update();

            Assert.AreEqual(1, state.StartIterations);
        }

        [TestMethod]
        public void StateEvents()
        {
            machine = new StateMachine();
            var state = new State(machine);
            var nextState = new State(machine);
            machine.AddState(state);
            state.AddTransition(() => { return true; }, nextState);

            machine.Update();

            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(1, state.UpdateIterations);
            Assert.AreEqual(1, state.EndIterations);
        }

        [TestMethod]
        public void TransitionedStateEvents()
        {
            machine = new StateMachine();
            var state = new State(machine);
            var nextState = new State(machine);
            machine.AddState(state);
            state.AddTransition(() => { return true; }, nextState);
            nextState.AddTransition(() => { return true; }, state);

            machine.Update();
            machine.Update();

            Assert.AreEqual(1, nextState.StartIterations);
            Assert.AreEqual(1, nextState.UpdateIterations);
            Assert.AreEqual(1, nextState.EndIterations);
        }
    }
}

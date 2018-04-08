
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCore;
using StateMachineTesting.TestState;

namespace StateMachineTesting
{
    [TestClass]
    public class StateMachineTest
    {
        [TestMethod]
        public void TransitionSucceeds()
        {
            var machine = new StateMachine();
            var state = new State(machine);
            var nextState = new State(machine);

            machine.AddState(state);
            state.AddTransition(() => { return true; }, nextState);

            machine.Update();

            Assert.AreEqual(1, machine.CurrentStates.Count);
            Assert.AreSame(nextState, machine.CurrentStates[0]);
        }

        [TestMethod]
        public void TransitionFails()
        {
            var machine = new StateMachine();
            var state = new State(machine);
            var transitionDestination = new State(machine);

            machine.AddState(state);
            state.AddTransition(() => { return false; }, transitionDestination);

            machine.Update();

            Assert.AreEqual(1, machine.CurrentStates.Count);
            Assert.AreSame(state, machine.CurrentStates[0]);
        }

        [TestMethod]
        public void UpdateIterations()
        {
            var numberOfIterations = 10;

            var machine = new StateMachine();
            var state = new State(machine);
            machine.AddState(state);

            for(var i = 0; i < numberOfIterations; i++)
            {
                machine.Update();
            }

            Assert.AreEqual(numberOfIterations, state.UpdateIterations);
        }

        [TestMethod]
        public void StartEvent()
        {
            var machine = new StateMachine();
            var state = new State(machine);
            machine.AddState(state);

            machine.Update();

            Assert.AreEqual(1, state.StartIterations);
        }

        [TestMethod]
        public void StartEventOnlyOnce()
        {
            var machine = new StateMachine();
            var state = new State(machine);
            machine.AddState(state);

            machine.Update();
            machine.Update();

            Assert.AreEqual(1, state.StartIterations);
        }

        [TestMethod]
        public void StateEvents()
        {
            var machine = new StateMachine();
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
            var machine = new StateMachine();
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

        [TestMethod]
        public void TransitionFirstToSucceedIsUsed()
        {
            var machine = new StateMachine();
            var runningState = new State(machine);
            var firstPossibleState = new State(machine);
            var nextState = new State(machine);
            var lastPossibleState = new State(machine);
            machine.AddState(runningState);
            runningState.AddTransition(() => { return false; }, firstPossibleState);
            runningState.AddTransition(() => { return true; }, nextState);
            runningState.AddTransition(() => { return false; }, lastPossibleState);

            machine.Update();

            Assert.AreEqual(1, machine.CurrentStates.Count);
            Assert.AreSame(nextState, machine.CurrentStates[0]);
        }
    }
}

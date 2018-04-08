using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCore;
using StateMachineTesting.TestState;

namespace StateMachineTesting
{
    [TestClass]
    public class StateMachineConcurrentStateTest
    {
        [TestMethod]
        public void ConcurrentStateUpdate()
        {
            var machine = new StateMachine();
            var stateA = new State(machine);
            var stateB = new State(machine);
            machine.AddState(stateA);
            machine.AddState(stateB);

            machine.Update();
            machine.Update();

            Assert.AreEqual(2, machine.CurrentStates.Count);
            Assert.AreSame(stateA, machine.CurrentStates[0]);
            Assert.AreSame(stateB, machine.CurrentStates[1]);
        }

        [TestMethod]
        public void ConcurrentStateSingleTransition()
        {
            var machine = new StateMachine();
            var stateA = new State(machine);
            var stateB = new State(machine);
            var destinationState = new State(machine);
            machine.AddState(stateA);
            machine.AddState(stateB);
            stateA.AddTransition(() => { return true; }, destinationState);

            machine.Update();

            Assert.AreEqual(2, machine.CurrentStates.Count);
            Assert.AreSame(stateB, machine.CurrentStates[0]);
            Assert.AreSame(destinationState, machine.CurrentStates[1]);
        }

        [TestMethod]
        public void ConcurrentStateAllTransition()
        {
            var machine = new StateMachine();
            var stateA = new State(machine);
            var stateB = new State(machine);
            var destinationStateA = new State(machine);
            var destinationStateB = new State(machine);
            machine.AddState(stateA);
            machine.AddState(stateB);
            stateA.AddTransition(() => { return true; }, destinationStateA);
            stateB.AddTransition(() => { return true; }, destinationStateB);

            machine.Update();

            Assert.AreEqual(2, machine.CurrentStates.Count);
            Assert.AreSame(destinationStateA, machine.CurrentStates[0]);
            Assert.AreSame(destinationStateB, machine.CurrentStates[1]);
        }

        [TestMethod]
        public void ConcurrentStateSingleTransitionEnd()
        {
            var machine = new StateMachine();
            var stateA = new State(machine);
            var stateB = new State(machine);
            machine.AddState(stateA);
            machine.AddState(stateB);
            stateA.AddTransition(() => { return true; });

            machine.Update();

            Assert.AreEqual(1, machine.CurrentStates.Count);
            Assert.AreSame(stateB, machine.CurrentStates[0]);
        }

        [TestMethod]
        public void ConcurrentStateAllTransitionEnd()
        {
            var machine = new StateMachine();
            var stateA = new State(machine);
            var stateB = new State(machine);
            machine.AddState(stateA);
            machine.AddState(stateB);
            stateA.AddTransition(() => { return true; });
            stateB.AddTransition(() => { return true; });

            machine.Update();

            Assert.AreEqual(0, machine.CurrentStates.Count);
        }
    }
}

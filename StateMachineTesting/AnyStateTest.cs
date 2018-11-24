using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCore;

namespace StateMachineTesting
{
    [TestClass]
    public class AnyStateTest
    {
        [TestMethod]
        public void AnyStateTransitionFailure()
        {
            var machine = new StateMachine();
            var start = new TestState();
            var attemptedDestination = new TestState();
            var transition = new StateTransition(() => { return false; }, attemptedDestination);
            machine.Any.AddTransition(transition);
            machine.Entry = start;

            machine.Update();

            Assert.AreEqual(0, start.EndIterations);
        }

        [TestMethod]
        public void AnyStateTransitionSuccess()
        {
            var machine = new StateMachine();
            var start = new TestState();
            var destination = new TestState();
            var transition = new StateTransition(() => { return true; }, destination);
            machine.Any.AddTransition(transition);
            machine.Entry = start;

            machine.Update();
            machine.Update();

            Assert.AreEqual(1, start.EndIterations);
            Assert.AreEqual(1, destination.StartIterations);
        }

        [TestMethod]
        public void StateTransitionPriorityOverAnyStateTransition()
        {
            var machine = new StateMachine();
            var start = new TestState();
            var destination = new TestState();
            var anyDestination = new TestState();
            var transition = new StateTransition(() => { return true; }, destination);
            var anyTransition = new StateTransition(() => { return true; }, anyDestination);
            start.AddTransition(transition);
            machine.Any.AddTransition(anyTransition);
            machine.Entry = start;

            machine.Update();
            machine.Update();

            Assert.AreEqual(1, destination.StartIterations);
            Assert.AreEqual(0, anyDestination.StartIterations);
        }
    }
}

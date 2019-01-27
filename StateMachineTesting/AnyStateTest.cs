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
            var group = new TestStateGroup();
            var start = new TestState();
            var attemptedDestination = new TestState();
            var transition = new StateTransition(() => { return false; }, attemptedDestination);
            group.Any.AddTransition(transition);
            group.Entry = start;

            group.Start();
            group.Update();

            Assert.AreEqual(0, start.EndIterations);
        }

        [TestMethod]
        public void AnyStateTransitionSuccess()
        {
            var group = new TestStateGroup();
            var start = new TestState();
            var destination = new TestState();
            var transition = new StateTransition(() => { return true; }, destination);
            group.Any.AddTransition(transition);
            group.Entry = start;

            group.Start();
            group.Update();
            group.Update();

            Assert.AreEqual(1, start.EndIterations);
            Assert.AreEqual(1, destination.StartIterations);
        }

        [TestMethod]
        public void StateTransitionPriorityOverAnyStateTransition()
        {
            var group = new TestStateGroup();
            var start = new TestState();
            var destination = new TestState();
            var anyDestination = new TestState();
            var transition = new StateTransition(() => { return true; }, destination);
            var anyTransition = new StateTransition(() => { return true; }, anyDestination);
            start.AddTransition(transition);
            group.Any.AddTransition(anyTransition);
            group.Entry = start;

            group.Start();
            group.Update();
            group.Update();

            Assert.AreEqual(1, destination.StartIterations);
            Assert.AreEqual(0, anyDestination.StartIterations);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StateMachineTesting
{
    [TestClass]
    public class AnyStateTransitionTest
    {
        [TestMethod]
        public void AnyStateTransitionFailure()
        {
            var group = new TestStateGroup();
            var start = new TestState();
            var attemptedDestination = new TestState();
            group.Any.AddTransition(() => false, attemptedDestination);
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
            group.Any.AddTransition(() => true, destination);
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
            start.AddTransition(() => true, destination);
            group.Any.AddTransition(() => true, anyDestination);
            group.Entry = start;

            group.Start();
            group.Update();
            group.Update();

            Assert.AreEqual(1, destination.StartIterations);
            Assert.AreEqual(0, anyDestination.StartIterations);
        }
    }
}

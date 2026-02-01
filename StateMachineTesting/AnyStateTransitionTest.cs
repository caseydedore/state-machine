
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
            group.AddTransition(() => false, group.Any, attemptedDestination);
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
            group.AddTransition(() => true, group.Any, destination);
            group.Entry = start;

            group.Start();
            group.Update();
            group.Update();

            Assert.AreEqual(1, start.EndIterations);
            Assert.AreEqual(1, destination.StartIterations);
        }

        [TestMethod]
        public void AnyStateTransitionNotAffectGroup()
        {
            var group = new TestStateGroup();
            var start = new TestState();
            var destination = new TestState();
            group.AddTransition(() => true, group.Any, destination);
            group.Entry = start;

            group.Start();
            group.Update();
            group.Update();

            Assert.AreEqual(2, group.UpdateIterations);
            Assert.AreEqual(0, group.EndIterations);
        }

        [TestMethod]
        public void StateTransitionPriorityOverAnyStateTransition()
        {
            var group = new TestStateGroup();
            var start = new TestState();
            var destination = new TestState();
            var anyDestination = new TestState();
            group.AddTransition(() => true, start, destination);
            group.AddTransition(() => true, group.Any, anyDestination);
            group.Entry = start;

            group.Start();
            group.Update();
            group.Update();

            Assert.AreEqual(1, destination.StartIterations);
            Assert.AreEqual(0, anyDestination.StartIterations);
        }
    }
}

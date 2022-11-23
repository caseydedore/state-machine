using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StateMachineTesting
{
    [TestClass]
    public class StateTest
    {
        [TestMethod]
        public void OneStateUpdate()
        {
            var group = new TestStateGroup();
            var state = new TestState();
            group.Entry = state;

            group.Start();
            group.Update();

            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(1, state.StartOptionalIterations);
            Assert.AreEqual(1, state.UpdateIterations);
            Assert.AreEqual(0, state.EndIterations);
        }

        [TestMethod]
        public void OneStateUpdates()
        {
            var group = new TestStateGroup();
            var state = new TestState();
            var iterations = 42;
            group.Entry = state;

            group.Start();
            var current = 0;
            while (current < iterations)
            {
                current++;
                group.Update();
            }

            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(1, state.StartOptionalIterations);
            Assert.AreEqual(iterations, state.UpdateIterations);
            Assert.AreEqual(0, state.EndIterations);
        }
    }
}

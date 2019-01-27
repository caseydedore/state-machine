
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StateMachineTesting
{
    [TestClass]
    public class StateGroupTest
    {
        [TestMethod]
        public void SubstateLifecycle()
        {
            var group = new TestStateGroup();
            var state = new TestState();
            group.Entry = state;

            group.Start();
            group.Update();
            group.End();

            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(1, state.UpdateIterations);
            Assert.AreEqual(1, state.EndIterations);
        }

        [TestMethod]
        public void SubstateLifecycleRestart()
        {
            var group = new TestStateGroup();
            var state = new TestState();
            group.Entry = state;

            group.Start();
            group.Update();
            group.End();
            group.Start();
            group.Update();
            group.End();

            Assert.AreEqual(2, state.StartIterations);
            Assert.AreEqual(2, state.UpdateIterations);
            Assert.AreEqual(2, state.EndIterations);
        }

        [TestMethod]
        public void Start()
        {
            var group = new TestStateGroup();
            var state = new TestState();
            group.Entry = state;

            group.Start();

            Assert.AreEqual(0, state.StartIterations);
            Assert.AreEqual(0, state.UpdateIterations);
            Assert.AreEqual(0, state.EndIterations);
        }

        [TestMethod]
        public void ErrantStart()
        {
            var group = new TestStateGroup();
            var state = new TestState();
            group.Entry = state;

            group.Start();
            group.Update();
            group.Start();

            Assert.AreEqual(1, state.StartIterations);
        }

        [TestMethod]
        public void ErrantEnd()
        {
            var group = new TestStateGroup();
            var state = new TestState();
            group.Entry = state;

            group.Start();
            group.Update();
            group.End();
            group.End();

            Assert.AreEqual(1, state.EndIterations);
        }

        [TestMethod]
        public void EndWithoutStart()
        {
            var group = new TestStateGroup();
            var state = new TestState();
            group.Entry = state;

            group.End();

            Assert.AreEqual(0, state.StartIterations);
            Assert.AreEqual(0, state.UpdateIterations);
            Assert.AreEqual(0, state.EndIterations);
        }

        [TestMethod]
        public void UpdateWithoutStart()
        {
            var group = new TestStateGroup();
            var state = new TestState();
            group.Entry = state;

            group.Update();

            Assert.AreEqual(0, state.StartIterations);
            Assert.AreEqual(0, state.UpdateIterations);
            Assert.AreEqual(0, state.EndIterations);
        }
    }
}

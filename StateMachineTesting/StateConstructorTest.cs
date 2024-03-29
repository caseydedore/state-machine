
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCore;

namespace StateMachineTesting
{
    [TestClass]
    public class StateConstructorTest
    {
        [TestMethod]
        public void EmptyConstructor()
        {
            var state = new State();

            state.Start();
            state.Update();
            state.End();

            //success if no exception thrown
        }

        [TestMethod]
        public void AllConstructor()
        {
            bool startFired = false,
                updateFired = false,
                endFired = false;

            var state = new State
            (
                start: () => startFired = true,
                update: () => updateFired = true,
                end: () => endFired = true
            );

            state.Start();
            state.Update();
            state.End();

            Assert.IsTrue(startFired);
            Assert.IsTrue(updateFired);
            Assert.IsTrue(endFired);
        }

        [TestMethod]
        public void StartOnly()
        {
            var startFired = false;
            var state = new State(
                start: () => startFired = true
            );

            state.Start();
            state.Update();
            state.End();

            Assert.IsTrue(startFired);
        }

        [TestMethod]
        public void UpdateOnly()
        {
            var updateFired = false;
            var state = new State(
                update: () => updateFired = true
            );

            state.Start();
            state.Update();
            state.End();

            Assert.IsTrue(updateFired);
        }

        [TestMethod]
        public void EndOnly()
        {
            var endFired = false;
            var state = new State(
                end: () => endFired = true
            );

            state.Start();
            state.Update();
            state.End();

            Assert.IsTrue(endFired);
        }
    }
}

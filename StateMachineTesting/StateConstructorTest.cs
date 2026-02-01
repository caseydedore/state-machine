
using StateMachine.Core;

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
                endFired = false,
                optionalStartFired = false,
                optionalUpdateFired = false,
                optionalEndFired = false;

            var state = new State
            (
                start: () => startFired = true,
                update: () => updateFired = true,
                end: () => endFired = true,
                optionalStart: () => optionalStartFired = true,
                optionalUpdate: () => optionalUpdateFired = true,
                optionalEnd: () => optionalEndFired = true
            );

            state.Start();
            state.Update();
            state.End();

            Assert.IsTrue(startFired);
            Assert.IsTrue(updateFired);
            Assert.IsTrue(endFired);
            Assert.IsTrue(optionalStartFired);
            Assert.IsTrue(optionalUpdateFired);
            Assert.IsTrue(optionalEndFired);
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

        [TestMethod]
        public void OptionalStartOnly()
        {
            var didExecute = false;
            var state = new State
            (
                optionalStart: () => didExecute = true
            );

            state.Start();
            state.Update();
            state.End();

            Assert.IsTrue(didExecute);
        }

        [TestMethod]
        public void OptionalUpdateOnly()
        {
            var didExecute = false;
            var state = new State
            (
                optionalUpdate: () => didExecute = true
            );

            state.Start();
            state.Update();
            state.End();

            Assert.IsTrue(didExecute);
        }

        [TestMethod]
        public void OptionalEndOnly()
        {
            var didExecute = false;
            var state = new State
            (
                optionalEnd: () => didExecute = true
            );

            state.Start();
            state.Update();
            state.End();

            Assert.IsTrue(didExecute);
        }
    }
}


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
            state.OptionalStart();
            state.Update();
            state.OptionalUpdate();
            state.End();
            state.OptionalEnd();

            Assert.IsTrue(startFired);
            Assert.IsTrue(updateFired);
            Assert.IsTrue(endFired);
            Assert.IsTrue(optionalStartFired);
            Assert.IsTrue(optionalUpdateFired);
            Assert.IsTrue(optionalEndFired);
        }
    }
}

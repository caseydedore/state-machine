
using StateMachine.Core;

namespace StateMachineTesting
{
    [TestClass]
    public class StateGroupTest
    {
        [TestMethod]
        public void LifecycleEvents()
        {
            var group = new TestStateGroup();
            var state = new TestState();
            group.Entry = state;

            group.Start();
            group.Update();
            group.End();

            Assert.AreEqual(1, group.StartIterations);
            Assert.AreEqual(1, state.UpdateIterations);
            Assert.AreEqual(1, group.EndIterations);
            Assert.AreEqual(1, group.OptionalStartIterations);
            Assert.AreEqual(1, group.OptionalUpdateIterations);
            Assert.AreEqual(1, group.OptionalEndIterations);
        }

        [TestMethod]
        public void SubstateLifecycleEvents()
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
            Assert.AreEqual(1, state.OptionalStartIterations);
            Assert.AreEqual(1, state.OptionalUpdateIterations);
            Assert.AreEqual(1, state.OptionalEndIterations);
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
            Assert.AreEqual(2, state.OptionalStartIterations);
            Assert.AreEqual(2, state.OptionalUpdateIterations);
            Assert.AreEqual(2, state.OptionalEndIterations);
        }

        [TestMethod]
        public void Start()
        {
            var group = new TestStateGroup();

            group.Start();

            Assert.AreEqual(1, group.StartIterations);
            Assert.AreEqual(0, group.UpdateIterations);
            Assert.AreEqual(0, group.EndIterations);
        }

        [TestMethod]
        public void StartNoSubstateEvents()
        {
            var group = new TestStateGroup();
            var state = new TestState();
            group.Entry = state;

            group.Start();

            Assert.AreEqual(0, state.StartIterations);
            Assert.AreEqual(0, state.UpdateIterations);
            Assert.AreEqual(0, state.EndIterations);
            Assert.AreEqual(0, state.OptionalStartIterations);
            Assert.AreEqual(0, state.OptionalUpdateIterations);
            Assert.AreEqual(0, state.OptionalEndIterations);
        }

        [TestMethod]
        public void ErrantStartNoSubstateStart()
        {
            var group = new TestStateGroup();
            var state = new TestState();
            group.Entry = state;

            group.Start();
            group.Update();
            group.Start();

            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(1, state.OptionalStartIterations);
        }

        [TestMethod]
        public void ErrantEndNoSubstateEndAgain()
        {
            var group = new TestStateGroup();
            var state = new TestState();
            group.Entry = state;

            group.Start();
            group.Update();
            group.End();
            group.End();

            Assert.AreEqual(1, state.EndIterations);
            Assert.AreEqual(1, state.OptionalEndIterations);
        }

        [TestMethod]
        public void EndWithoutStartNoSubstateEvents()
        {
            var group = new TestStateGroup();
            var state = new TestState();
            group.Entry = state;

            group.End();

            Assert.AreEqual(0, state.StartIterations);
            Assert.AreEqual(0, state.UpdateIterations);
            Assert.AreEqual(0, state.EndIterations);
            Assert.AreEqual(0, state.OptionalStartIterations);
            Assert.AreEqual(0, state.OptionalUpdateIterations);
            Assert.AreEqual(0, state.OptionalEndIterations);
        }

        [TestMethod]
        public void UpdateWithoutStartNoSubstateEvents()
        {
            var group = new TestStateGroup();
            var state = new TestState();
            group.Entry = state;

            group.Update();

            Assert.AreEqual(0, state.StartIterations);
            Assert.AreEqual(0, state.UpdateIterations);
            Assert.AreEqual(0, state.EndIterations);
            Assert.AreEqual(0, state.OptionalStartIterations);
            Assert.AreEqual(0, state.OptionalUpdateIterations);
            Assert.AreEqual(0, state.OptionalEndIterations);
        }

        [TestMethod]
        public void StartBeforeChild()
        {
            var didChildStart = false;
            var didStartBeforeChild = false;
            var group = new StateGroup
            (
                start: () => didStartBeforeChild = !didChildStart
            );
            var child = new State
            (
                start: () => didChildStart = true
            );
            group.Entry = child;

            group.Start();
            group.Update();
            group.End();

            Assert.IsTrue(didStartBeforeChild);
        }

        [TestMethod]
        public void EndAfterChild()
        {
            var didChildEnd = false;
            var didEndAfterChild = false;
            var group = new StateGroup
            (
                end: () => didEndAfterChild = didChildEnd
            );
            var child = new State
            (
                end: () => didChildEnd = true
            );
            group.Entry = child;

            group.Start();
            group.Update();
            group.End();

            Assert.IsTrue(didEndAfterChild);
        }

        [TestMethod]
        public void OptionalUpdateAfterChildUpdate()
        {
            var didChildUpdate = false;
            var didGroupOptionalUpdateAfterChild = false;
            var group = new StateGroup
            (
                optionalUpdate: () => didGroupOptionalUpdateAfterChild = didChildUpdate
            );
            var state = new TestState
            (
                update: () => didChildUpdate = true
            );
            group.Entry = state;

            group.Start();
            group.Update();

            Assert.IsTrue(didGroupOptionalUpdateAfterChild);
        }
    }
}

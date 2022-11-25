
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCore;

namespace StateMachineTesting
{
    [TestClass]
    public class TransitionTest
    {
        [TestMethod]
        public void TransitionIsChecked()
        {
            var state = new TestState();
            var didCheckTransition = false;
            var transition = new StateTransition(() =>
            {
                didCheckTransition = true;
                return true;
            },
            state);
            state.AddTransition(transition);

            state.Start();
            state.Update();

            Assert.IsTrue(didCheckTransition);
        }

        [TestMethod]
        public void TransitionSuccess()
        {
            var group = new TestStateGroup();
            var first = new TestState();
            var next = new TestState();
            first.AddTransition(() => true, next);
            group.Entry = first;

            group.Start();
            group.Update();

            Assert.AreEqual(1, first.StartIterations);
            Assert.AreEqual(1, first.UpdateIterations);
            Assert.AreEqual(1, first.EndIterations);
            Assert.AreEqual(0, first.OptionalStartIterations);
            Assert.AreEqual(0, first.OptionalUpdateIterations);
            Assert.AreEqual(0, first.OptionalEndIterations);
            Assert.AreEqual(0, next.StartIterations);
            Assert.AreEqual(0, next.UpdateIterations);
        }

        [TestMethod]
        public void TransitionSuccessNextStateUpdate()
        {
            var group = new TestStateGroup();
            var first = new TestState();
            var next = new TestState();
            first.AddTransition(() => true, next);
            group.Entry = first;

            group.Start();
            group.Update();
            group.Update();

            Assert.AreEqual(1, first.EndIterations);
            Assert.AreEqual(1, next.StartIterations);
            Assert.AreEqual(1, next.UpdateIterations);
            Assert.AreEqual(1, next.OptionalStartIterations);
            Assert.AreEqual(1, next.OptionalUpdateIterations);
        }

        [TestMethod]
        public void TransitionFailure()
        {
            var group = new TestStateGroup();
            var first = new TestState();
            var next = new TestState();
            first.AddTransition(() => false, next);
            group.Entry = first;

            group.Start();
            group.Update();

            Assert.AreEqual(0, first.EndIterations);
            Assert.AreEqual(0, next.StartIterations);
        }

        [TestMethod]
        public void TransitionFailures()
        {
            var group = new TestStateGroup();
            var first = new TestState();
            var next = new TestState();
            first.AddTransition(() => false, next);
            group.Entry = first;

            group.Start();
            var iteration = 0;
            while (iteration < 13)
            {
                iteration++;
                group.Update();
            }

            Assert.AreEqual(0, first.EndIterations);
            Assert.AreEqual(0, next.StartIterations);
        }

        [TestMethod]
        public void TransitionFirstPrioritySuccess()
        {
            var group = new TestStateGroup();
            var start = new TestState();
            var destination = new TestState();
            var secondDestination = new TestState();
            start.AddTransition(() => true, destination);
            start.AddTransition(() => false, secondDestination);
            group.Entry = start;

            group.Start();
            group.Update();
            group.Update();

            Assert.AreEqual(1, destination.StartIterations);
            Assert.AreEqual(1, destination.UpdateIterations);
            Assert.AreEqual(0, secondDestination.StartIterations);
            Assert.AreEqual(0, secondDestination.UpdateIterations);
        }

        [TestMethod]
        public void TransitionSecondPrioritySuccess()
        {
            var group = new TestStateGroup();
            var start = new TestState();
            var attemptedDestination = new TestState();
            var destination = new TestState();
            start.AddTransition(() => false, attemptedDestination);
            start.AddTransition(() => true, destination);
            group.Entry = start;

            group.Start();
            group.Update();
            group.Update();

            Assert.AreEqual(0, attemptedDestination.StartIterations);
            Assert.AreEqual(0, attemptedDestination.UpdateIterations);
            Assert.AreEqual(1, destination.StartIterations);
            Assert.AreEqual(1, destination.UpdateIterations);
        }

        [TestMethod]
        public void TransitionBackAndForth()
        {
            var shouldTransition = false;
            var group = new TestStateGroup();
            var first = new TestState
            (
                update: () => shouldTransition = true,
                end: () => shouldTransition = false
            );
            var second = new TestState
            (
                update: () => shouldTransition = true,
                end: () => shouldTransition = false
            );
            first.AddTransition(() => shouldTransition, second);
            second.AddTransition(() => shouldTransition, first);
            group.Entry = first;

            group.Start();
            group.Update();
            group.Update();
            group.Update();
            group.Update();

            Assert.AreEqual(2, first.StartIterations);
            Assert.AreEqual(2, first.UpdateIterations);
            Assert.AreEqual(2, second.StartIterations);
            Assert.AreEqual(2, second.UpdateIterations);
        }

        [TestMethod]
        public void MultipleTransitionsSucceed()
        {
            var group = new TestStateGroup();
            var first = new TestState();
            var destination = new TestState();
            var lastDestination = new TestState();
            first.AddTransition(() => true, destination);
            first.AddTransition(() => true, lastDestination);
            group.Entry = first;

            group.Start();
            group.Update();
            group.Update();

            Assert.AreEqual(1, destination.StartIterations);
            Assert.AreEqual(1, destination.UpdateIterations);
            Assert.AreEqual(0, lastDestination.StartIterations);
            Assert.AreEqual(0, lastDestination.UpdateIterations);
        }

        [TestMethod]
        public void TransitionSuccessAfterFailure()
        {
            var group = new TestStateGroup();
            var state = new TestState();
            var destination = new TestState();
            var willTransitionSucceed = false;
            state.AddTransition(() => willTransitionSucceed, destination);
            group.Entry = state;

            group.Start();
            group.Update();
            willTransitionSucceed = true;
            group.Update();
            group.Update();

            Assert.AreEqual(1, destination.StartIterations);
        }
    }
}

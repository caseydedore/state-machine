
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCore;

namespace StateMachineTesting
{
    [TestClass]
    public class StateTransitionTest
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
            Assert.AreEqual(iterations, state.UpdateIterations);
            Assert.AreEqual(0, state.EndIterations);
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
            group.Update();

            Assert.AreEqual(1, first.EndIterations);
            Assert.AreEqual(1, next.StartIterations);
            Assert.AreEqual(1, next.UpdateIterations);
        }

        [TestMethod]
        public void TransitionIsChecked()
        {
            var group = new TestStateGroup();
            var state = new TestState();
            var didCheckTransition = false;
            var transition = new StateTransition(() =>
                {
                    didCheckTransition = true;
                    return true;
                },
                state);
            state.AddTransition(transition);
            group.Entry = state;

            group.Start();
            group.Update();

            Assert.IsTrue(didCheckTransition);
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

            Assert.AreEqual(1, first.StartIterations);
            Assert.AreEqual(0, first.EndIterations);
            Assert.AreEqual(0, next.StartIterations);
        }

        [TestMethod]
        public void TransitionFirstSuccess()
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
        public void TransitionSecondSuccess()
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
            var group = new TestStateGroup();
            var first = new TestState();
            var second = new TestState();
            var shouldTransitionToSecond = false;
            var shouldTransitionToFirst = false;
            first.AddTransition(() => shouldTransitionToSecond, second);
            second.AddTransition(() => shouldTransitionToFirst, first);
            group.Entry = first;

            group.Start();
            group.Update();
            shouldTransitionToSecond = true;
            group.Update();
            shouldTransitionToSecond = false;
            group.Update();
            shouldTransitionToFirst = true;
            group.Update();

            Assert.AreEqual(1, first.UpdateIterations);
            Assert.AreEqual(1, second.UpdateIterations);
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

            Assert.AreEqual(1, state.UpdateIterations);
            Assert.AreEqual(0, destination.StartIterations);
        }
    }
}

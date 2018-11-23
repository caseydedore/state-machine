
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCore;
using StateMachineTesting.TestState;

namespace StateMachineTesting
{
    [TestClass]
    public class StateMachineTest
    {
        [TestMethod]
        public void OneStateUpdate()
        {
            var machine = new StateMachine();
            var state = new State();
            machine.Entry = state;

            machine.Update();

            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(1, state.UpdateIterations);
            Assert.AreEqual(0, state.EndIterations);
        }

        [TestMethod]
        public void OneStateUpdates()
        {
            var machine = new StateMachine();
            var state = new State();
            var iterations = 42;
            machine.Entry = state;

            var current = 0;
            while (current < iterations)
            {
                current++;
                machine.Update();
            }

            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(iterations, state.UpdateIterations);
            Assert.AreEqual(0, state.EndIterations);
        }

        [TestMethod]
        public void TransitionSuccess()
        {
            var machine = new StateMachine();
            var first = new State();
            var next = new State();
            var transition = new StateTransition(() => { return true; }, next);
            first.AddTransition(transition);
            machine.Entry = first;

            machine.Update();
            machine.Update();

            Assert.AreEqual(1, first.EndIterations);
            Assert.AreEqual(1, next.StartIterations);
            Assert.AreEqual(1, next.UpdateIterations);
        }

        [TestMethod]
        public void TransitionIsChecked()
        {
            var machine = new StateMachine();
            var state = new State();
            var didCheckTransition = false;
            var transition = new StateTransition(() =>
                {
                    didCheckTransition = true;
                    return true;
                },
                state);
            state.AddTransition(transition);
            machine.Entry = state;

            machine.Update();

            Assert.IsTrue(didCheckTransition);
        }

        [TestMethod]
        public void TransitionFailure()
        {
            var machine = new StateMachine();
            var first = new State();
            var next = new State();
            var transition = new StateTransition(() => { return false; }, next);
            first.AddTransition(transition);
            machine.Entry = first;

            machine.Update();

            Assert.AreEqual(0, first.EndIterations);
        }

        [TestMethod]
        public void TransitionFailures()
        {
            var machine = new StateMachine();
            var first = new State();
            var next = new State();
            var transition = new StateTransition(() => { return false; }, next);
            first.AddTransition(transition);
            machine.Entry = first;

            var iteration = 0;
            while (iteration < 13)
            {
                iteration++;
                machine.Update();
            }

            Assert.AreEqual(1, first.StartIterations);
            Assert.AreEqual(0, first.EndIterations);
            Assert.AreEqual(0, next.StartIterations);
        }

        [TestMethod]
        public void TransitionFirstSuccess()
        {
            var machine = new StateMachine();
            var start = new State();
            var destination = new State();
            var secondDestination = new State();
            var fail = new StateTransition(() => { return true; }, destination);
            var succeed = new StateTransition(() => { return false; }, secondDestination);
            start.AddTransition(fail);
            start.AddTransition(succeed);
            machine.Entry = start;

            machine.Update();
            machine.Update();

            Assert.AreEqual(1, start.UpdateIterations);
            Assert.AreEqual(1, destination.UpdateIterations);
            Assert.AreEqual(0, secondDestination.UpdateIterations);
        }

        [TestMethod]
        public void TransitionSecondSuccess()
        {
            var machine = new StateMachine();
            var start = new State();
            var attemptedDestination = new State();
            var destination = new State();
            var fail = new StateTransition(() => { return false; }, attemptedDestination);
            var succeed = new StateTransition(() => { return true; }, destination);
            start.AddTransition(fail);
            start.AddTransition(succeed);
            machine.Entry = start;

            machine.Update();
            machine.Update();

            Assert.AreEqual(0, attemptedDestination.StartIterations);
            Assert.AreEqual(1, destination.StartIterations);
            Assert.AreEqual(1, destination.UpdateIterations);
        }

        [TestMethod]
        public void TransitionBackAndForth()
        {
            var machine = new StateMachine();
            var first = new State();
            var second = new State();
            var firstTransition = new StateTransition(() => { return true; }, second);
            var secondTransition = new StateTransition(() => { return true; }, first);
            first.AddTransition(firstTransition);
            second.AddTransition(secondTransition);
            machine.Entry = first;

            machine.Update();
            machine.Update();
            machine.Update();
            machine.Update();

            Assert.AreEqual(2, first.UpdateIterations);
            Assert.AreEqual(2, second.UpdateIterations);
        }

        [TestMethod]
        public void MultipleTransitionsSucceed()
        {
            var machine = new StateMachine();
            var first = new State();
            var destination = new State();
            var lastDestination = new State();
            var firstTransition = new StateTransition(() => { return true; }, destination);
            var secondTransition = new StateTransition(() => { return true; }, lastDestination);
            first.AddTransition(firstTransition);
            first.AddTransition(secondTransition);
            machine.Entry = first;

            machine.Update();
            machine.Update();

            Assert.AreEqual(1, destination.StartIterations);
            Assert.AreEqual(1, destination.UpdateIterations);
            Assert.AreEqual(0, lastDestination.StartIterations);
        }

        [TestMethod]
        public void TransitionSucceedsAfterFailure()
        {
            var machine = new StateMachine();
            var state = new State();
            var destination = new State();
            var willTransitionSucceed = false;
            var transition = new StateTransition(() => { return willTransitionSucceed; }, destination);
            state.AddTransition(transition);
            machine.Entry = state;

            machine.Update();
            willTransitionSucceed = true;
            machine.Update();

            Assert.AreEqual(2, state.UpdateIterations);
            Assert.AreEqual(0, destination.StartIterations);
        }
    }
}

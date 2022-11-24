using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StateMachineTesting
{
    [TestClass]
    public class TransitionAfterTest
    {
        [TestMethod]
        public void TransitionAfterNoUpdate()
        {
            var state = new TestState();
            var nextState = new TestState();
            state.AddTransitionAfter(0, nextState);

            var transition = state.Start();

            Assert.IsNotNull(transition);
            Assert.AreEqual(0, state.StartIterations);
            Assert.AreEqual(0, state.UpdateIterations);
        }

        [TestMethod]
        public void TransitionAfterOneUpdate()
        {
            var state = new TestState();
            var nextState = new TestState();
            state.AddTransitionAfter(1, nextState);

            var first = state.Start();
            var second = state.Update();
            var third = state.Update();

            Assert.IsNull(first);
            Assert.IsNull(second);
            Assert.IsNotNull(third);
        }

        [TestMethod]
        public void TransitionAfterTwoUpdates()
        {
            var state = new TestState();
            var nextState = new TestState();
            state.AddTransitionAfter(2, nextState);

            state.Start();
            var first = state.Update();
            var second = state.Update();
            var third = state.Update();

            Assert.IsNull(first);
            Assert.IsNull(second);
            Assert.IsNotNull(third);
        }

        [TestMethod]
        public void TransitionAfterConditionSuccess()
        {
            var state = new TestState();
            var nextState = new TestState();
            state.AddTransitionAfter(0, () => true, nextState);

            var first = state.Start();

            Assert.IsNotNull(first);
        }

        [TestMethod]
        public void TransitionAfterConditionFailure()
        {
            var state = new TestState();
            var nextState = new TestState();
            state.AddTransitionAfter(0, () => false, nextState);

            var first = state.Start();

            Assert.IsNull(first);
        }

        [TestMethod]
        public void TransitionAfterOneUpdateConditionSuccess()
        {
            var state = new TestState();
            var nextState = new TestState();
            state.AddTransitionAfter(1, () => true, nextState);

            var first = state.Start();
            var second = state.Update();
            var third = state.Update();

            Assert.IsNull(first);
            Assert.IsNull(second);
            Assert.IsNotNull(third);
        }

        [TestMethod]
        public void TransitionAfterOneUpdateConditionFailure()
        {
            var state = new TestState();
            var nextState = new TestState();
            state.AddTransitionAfter(1, () => false, nextState);

            var first = state.Start();
            var second = state.Update();
            var third = state.Update();

            Assert.IsNull(first);
            Assert.IsNull(second);
            Assert.IsNull(third);
        }
    }
}

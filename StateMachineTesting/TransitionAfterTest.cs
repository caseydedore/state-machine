
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

            state.Start();
            var transition = state.Update();

            Assert.IsNotNull(transition);
            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(1, state.UpdateIterations);
            Assert.AreEqual(0, state.OptionalStartIterations);
            Assert.AreEqual(0, state.OptionalUpdateIterations);
        }

        [TestMethod]
        public void TransitionAfterOneUpdate()
        {
            var state = new TestState();
            var nextState = new TestState();
            state.AddTransitionAfter(1, nextState);

            state.Start();
            var first = state.Update();

            Assert.IsNotNull(first);
            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(1, state.UpdateIterations);
            Assert.AreEqual(1, state.OptionalStartIterations);
            Assert.AreEqual(1, state.OptionalUpdateIterations);
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

            Assert.IsNull(first);
            Assert.IsNotNull(second);
            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(2, state.UpdateIterations);
            Assert.AreEqual(1, state.OptionalStartIterations);
            Assert.AreEqual(2, state.OptionalUpdateIterations);
        }

        [TestMethod]
        public void TransitionAfterConditionSuccess()
        {
            var state = new TestState();
            var nextState = new TestState();
            state.AddTransitionAfter(0, () => true, nextState);

            state.Start();
            var first = state.Update();

            Assert.IsNotNull(first);
            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(1, state.UpdateIterations);
            Assert.AreEqual(0, state.OptionalStartIterations);
            Assert.AreEqual(0, state.OptionalUpdateIterations);
        }

        [TestMethod]
        public void TransitionAfterConditionFailure()
        {
            var state = new TestState();
            var nextState = new TestState();
            state.AddTransitionAfter(0, () => false, nextState);

            state.Start();
            var first = state.Update();

            Assert.IsNull(first);
            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(1, state.UpdateIterations);
            Assert.AreEqual(1, state.OptionalStartIterations);
            Assert.AreEqual(1, state.OptionalUpdateIterations);
        }

        [TestMethod]
        public void TransitionAfterOneUpdateConditionSuccess()
        {
            var state = new TestState();
            var nextState = new TestState();
            state.AddTransitionAfter(1, () => true, nextState);

            state.Start();
            var first = state.Update();

            Assert.IsNotNull(first);
            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(1, state.UpdateIterations);
            Assert.AreEqual(1, state.OptionalStartIterations);
            Assert.AreEqual(1, state.OptionalUpdateIterations);
        }

        [TestMethod]
        public void TransitionAfterOneUpdateConditionFailure()
        {
            var state = new TestState();
            var nextState = new TestState();
            state.AddTransitionAfter(1, () => false, nextState);

            state.Start();
            var first = state.Update();
            var second = state.Update();

            Assert.IsNull(first);
            Assert.IsNull(second);
            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(2, state.UpdateIterations);
            Assert.AreEqual(1, state.OptionalStartIterations);
            Assert.AreEqual(2, state.OptionalUpdateIterations);
        }
    }
}

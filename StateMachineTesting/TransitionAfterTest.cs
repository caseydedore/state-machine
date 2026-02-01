
namespace StateMachineTesting
{
    [TestClass]
    public class TransitionAfterTest
    {
        [TestMethod]
        public void TransitionAfterNoUpdate()
        {
            var root = new TestStateGroup();
            var state = new TestState();
            var nextState = new TestState();
            root.AddTransitionAfter(0, state, nextState);
            root.Entry = state;

            root.Start();
            root.Update();

            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(1, state.UpdateIterations);
            Assert.AreEqual(0, state.OptionalStartIterations);
            Assert.AreEqual(0, state.OptionalUpdateIterations);
        }

        [TestMethod]
        public void TransitionAfterOneUpdate()
        {
            var root = new TestStateGroup();
            var state = new TestState();
            var nextState = new TestState();
            root.AddTransitionAfter(1, state, nextState);
            root.Entry = state;

            root.Start();
            root.Update();

            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(1, state.UpdateIterations);
            Assert.AreEqual(1, state.OptionalStartIterations);
            Assert.AreEqual(1, state.OptionalUpdateIterations);
        }

        [TestMethod]
        public void TransitionAfterTwoUpdates()
        {
            var root = new TestStateGroup();
            var state = new TestState();
            var nextState = new TestState();
            root.AddTransitionAfter(2, state, nextState);
            root.Entry = state;

            root.Start();
            root.Update();
            root.Update();

            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(2, state.UpdateIterations);
            Assert.AreEqual(1, state.OptionalStartIterations);
            Assert.AreEqual(2, state.OptionalUpdateIterations);
        }

        [TestMethod]
        public void TransitionAfterConditionSuccess()
        {
            var root = new TestStateGroup();
            var state = new TestState();
            var nextState = new TestState();
            root.AddTransitionAfter(0, () => true, state, nextState);
            root.Entry = state;

            root.Start();
            root.Update();

            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(1, state.UpdateIterations);
            Assert.AreEqual(0, state.OptionalStartIterations);
            Assert.AreEqual(0, state.OptionalUpdateIterations);
        }

        [TestMethod]
        public void TransitionAfterConditionFailure()
        {
            var root = new TestStateGroup();
            var state = new TestState();
            var nextState = new TestState();
            root.AddTransitionAfter(0, () => false, state, nextState);
            root.Entry = state;

            root.Start();
            root.Update();

            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(1, state.UpdateIterations);
            Assert.AreEqual(1, state.OptionalStartIterations);
            Assert.AreEqual(1, state.OptionalUpdateIterations);
        }

        [TestMethod]
        public void TransitionAfterOneUpdateConditionSuccess()
        {
            var root = new TestStateGroup();
            var state = new TestState();
            var nextState = new TestState();
            root.AddTransitionAfter(1, () => true, state, nextState);
            root.Entry = state;

            root.Start();
            root.Update();

            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(1, state.UpdateIterations);
            Assert.AreEqual(1, state.OptionalStartIterations);
            Assert.AreEqual(1, state.OptionalUpdateIterations);
        }

        [TestMethod]
        public void TransitionAfterOneUpdateConditionFailure()
        {
            var root = new TestStateGroup();
            var state = new TestState();
            var nextState = new TestState();
            root.AddTransitionAfter(1, () => false, state, nextState);
            root.Entry = state;

            root.Start();
            root.Update();
            root.Update();

            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(2, state.UpdateIterations);
            Assert.AreEqual(1, state.OptionalStartIterations);
            Assert.AreEqual(2, state.OptionalUpdateIterations);
        }
    }
}

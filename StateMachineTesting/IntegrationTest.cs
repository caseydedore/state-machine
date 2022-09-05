using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StateMachineTesting
{
    [TestClass]
    public class IntegrationTest
    {
        [TestMethod]
        public void GroupTransitionSuccess()
        {
            var shouldTransition = false;
            var rootGroup = new TestStateGroup();
            var startGroup = new TestStateGroup();
            var destGroup = new TestStateGroup();
            rootGroup.Entry = startGroup;
            startGroup.AddTransition(() => shouldTransition, destGroup);
            var startState = new TestState(
                update: () => shouldTransition = true
            );
            startGroup.Entry = startState;
            var destState = new TestState();
            destGroup.Entry = destState;

            rootGroup.Start();
            rootGroup.Update();
            rootGroup.Update();

            Assert.AreEqual(1, startState.StartIterations);
            Assert.AreEqual(1, startState.UpdateIterations);
            Assert.AreEqual(1, startState.EndIterations);
            Assert.AreEqual(0, destState.StartIterations);
            Assert.AreEqual(0, destState.UpdateIterations);
        }

        [TestMethod]
        public void GroupTransitionSuccessNextStateFollow()
        {
            var shouldTransition = false;
            var rootGroup = new TestStateGroup();
            var startGroup = new TestStateGroup();
            var destGroup = new TestStateGroup();
            rootGroup.Entry = startGroup;
            startGroup.AddTransition(() => shouldTransition, destGroup);
            var startState = new TestState(
                update: () => shouldTransition = true
            );
            startGroup.Entry = startState;
            var destState = new TestState();
            destGroup.Entry = destState;

            rootGroup.Start();
            rootGroup.Update();
            rootGroup.Update();
            rootGroup.Update();

            Assert.AreEqual(1, startState.StartIterations);
            Assert.AreEqual(1, startState.UpdateIterations);
            Assert.AreEqual(1, startState.EndIterations);
            Assert.AreEqual(1, destState.StartIterations);
            Assert.AreEqual(1, destState.UpdateIterations);
        }

        [TestMethod]
        public void SharedSubstate()
        {
            var root = new TestStateGroup();
            var start = new TestStateGroup();
            var dest = new TestStateGroup();
            root.Entry = start;
            var willTransition = false;
            start.AddTransition(() => willTransition, dest);
            var sharedSubstate = new TestState();
            start.Entry = sharedSubstate;
            dest.Entry = sharedSubstate;

            root.Start();
            root.Update();
            willTransition = true;
            root.Update();
            root.Update();

            Assert.AreEqual(2, sharedSubstate.StartIterations);
            Assert.AreEqual(2, sharedSubstate.UpdateIterations);
            Assert.AreEqual(1, sharedSubstate.EndIterations);
        }
    }
}

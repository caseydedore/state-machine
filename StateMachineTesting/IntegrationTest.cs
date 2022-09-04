using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCore;

namespace StateMachineTesting
{
    [TestClass]
    public class IntegrationTest
    {
        [TestMethod]
        public void GroupTransition()
        {
            var rootGroup = new TestStateGroup();
            var startGroup = new TestStateGroup();
            var destGroup = new TestStateGroup();
            rootGroup.Entry = startGroup;
            var toDest = new StateTransition(() => { return true; }, destGroup);
            startGroup.AddTransition(toDest);
            var startState = new TestState();
            startGroup.Entry = startState;
            var destState = new TestState();
            destGroup.Entry = destState;

            rootGroup.Start();
            rootGroup.Update();
            rootGroup.Update();

            Assert.AreEqual(0, startState.StartIterations);
            Assert.AreEqual(0, startState.UpdateIterations);
            Assert.AreEqual(0, startState.EndIterations);
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
            var toDest = new StateTransition(() => { return willTransition; }, dest);
            start.AddTransition(toDest);
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

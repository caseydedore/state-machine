using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCore;

namespace StateMachineTesting
{
    [TestClass]
    public class StateGroupConstructorTest
    {
        [TestMethod]
        public void EmptyConstructor()
        {
            var group = new StateGroup();

            group.Start();
            group.Update();
            group.End();

            //success is no exception thrown
        }

        [TestMethod]
        public void Start()
        {
            var entry = new TestState();
            var group = new StateGroup(entry: entry);

            group.Start();
            group.Update();
            group.End();

            Assert.AreEqual(1, entry.StartIterations);
            Assert.AreEqual(1, entry.UpdateIterations);
            Assert.AreEqual(1, entry.EndIterations);
        }

        [TestMethod]
        public void StartOverriddenByProperty()
        {
            var entry = new TestState();
            var overrideState = new TestState();
            var group = new StateGroup(entry);
            group.Entry = overrideState;

            group.Start();
            group.Update();
            group.End();

            Assert.AreEqual(0, entry.UpdateIterations);
            Assert.AreEqual(1, overrideState.UpdateIterations);
        }
    }
}

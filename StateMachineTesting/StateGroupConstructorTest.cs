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
        public void StartConstructor()
        {
            var startIterations = 0;
            var group = new StateGroup
            (
                start: () => startIterations++
            );

            group.Start();
            group.Update();
            group.End();

            Assert.AreEqual(1, startIterations);
        }

        [TestMethod]
        public void EndConstructor()
        {
            var endIterations = 0;
            var group = new StateGroup
            (
                end: () => endIterations++
            );

            group.Start();
            group.Update();
            group.End();

            Assert.AreEqual(1, endIterations);
        }

        [TestMethod]
        public void OptionalStartConstructor()
        {
            var didExecute = false;
            var group = new StateGroup
            (
                optionalStart: () => didExecute = true
            );

            group.Start();
            group.Update();
            group.End();

            Assert.IsTrue(didExecute);
        }

        [TestMethod]
        public void OptionalEndConstructor()
        {
            var didExecute = false;
            var group = new StateGroup
            (
                optionalEnd: () => didExecute = true
            );

            group.Start();
            group.Update();
            group.End();

            Assert.IsTrue(didExecute);
        }

        [TestMethod]
        public void AllConstructor()
        {
            var startIterations = 0; 
            var endIterations = 0;
            var updateIterations = 0;
            var optionalStartIterations = 0;
            var optionalEndIterations = 0;
            var optionalUpdateIterations = 0;
            var group = new StateGroup
            (
                start: () => startIterations++,
                end: () => endIterations++,
                update: () => updateIterations++,
                optionalStart: () => optionalStartIterations++,
                optionalEnd: () => optionalEndIterations++,
                optionalUpdate: () => optionalUpdateIterations++
            );

            group.Start();
            group.Update();
            group.End();

            Assert.AreEqual(1, startIterations);
            Assert.AreEqual(1, endIterations);
            Assert.AreEqual(1, updateIterations);
            Assert.AreEqual(1, optionalStartIterations);
            Assert.AreEqual(1, optionalEndIterations);
            Assert.AreEqual(1, optionalUpdateIterations);
        }
    }
}

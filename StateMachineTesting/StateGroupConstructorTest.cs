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
            var group = new StateGroup
            (
                start: () => startIterations++,
                end: () => endIterations++
            );

            group.Start();
            group.Update();
            group.End();

            Assert.AreEqual(1, startIterations);
            Assert.AreEqual(1, endIterations);
        }

        [TestMethod]
        public void StartBeforeChild()
        {
            var didChildStart = false;
            var didStartBeforeChild = false;
            var group = new StateGroup
            (
                start: () => didStartBeforeChild = !didChildStart
            );
            var child = new State
            (
                start: () => didChildStart = true
            );
            group.Entry = child;

            group.Start();
            group.Update();
            group.End();

            Assert.IsTrue(didStartBeforeChild);
        }

        [TestMethod]
        public void EndAfterChild()
        {
            var didChildEnd = false;
            var didEndAfterChild = false;
            var group = new StateGroup
            (
                end: () => didEndAfterChild = didChildEnd
            );
            var child = new State
            (
                end: () => didChildEnd = true
            );
            group.Entry = child;

            group.Start();
            group.Update();
            group.End();

            Assert.IsTrue(didEndAfterChild);
        }
    }
}

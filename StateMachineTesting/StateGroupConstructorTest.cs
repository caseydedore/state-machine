
using StateMachine.Core;

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
            group.OptionalStart();
            group.Update();
            group.OptionalUpdate();
            group.End();
            group.OptionalEnd();

            Assert.AreEqual(1, startIterations);
            Assert.AreEqual(1, endIterations);
            Assert.AreEqual(1, updateIterations);
            Assert.AreEqual(1, optionalStartIterations);
            Assert.AreEqual(1, optionalEndIterations);
            Assert.AreEqual(1, optionalUpdateIterations);
        }
    }
}

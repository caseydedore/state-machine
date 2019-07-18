
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCore;

namespace StateMachineTesting
{
    [TestClass]
    public class StateBlackboardTest
    {
        [TestMethod]
        public void InternalBlackboardExists()
        {
            Blackboard blackboard = null;
            var state = new TestState
            {
                UpdateAction = (Blackboard b) =>
                {
                    blackboard = b;
                }
            };

            state.Update();

            Assert.IsNotNull(blackboard);
        }

        [TestMethod]
        public void ReadFromExternalBlackboard()
        {
            var blackboard = new Blackboard();
            var key = "bbk";
            var value = 595;
            var retrievedValue = 0;
            blackboard.Set(key, value);
            var state = new TestState(blackboard)
            {
                UpdateAction = (Blackboard b) =>
                {
                    retrievedValue = int.Parse(b.Get(key).ToString());
                }
            };

            state.Update();

            Assert.AreEqual(value, retrievedValue);
        }

        [TestMethod]
        public void WriteToExternalBlackboard()
        {
            var blackboard = new Blackboard();
            var key = "wrt";
            var value = "writtenbystate";
            var state = new TestState(blackboard)
            {
                UpdateAction = (Blackboard b) =>
                {
                    blackboard.Set(key, value);
                }
            };

            state.Update();

            var retrievedValue = blackboard.Get(key).ToString();
            Assert.AreEqual(value, retrievedValue);
        }

        [TestMethod]
        public void StateReadStateWriteToBlackboard()
        {
            var blackboard = new Blackboard();
            var key = "shared";
            var value = "0293402";
            var retrievedValue = "";
            var stateWriter = new TestState(blackboard)
            {
                UpdateAction = (Blackboard b) =>
                {
                    b.Set(key, value);
                }
            };
            var stateReader = new TestState(blackboard)
            {
                UpdateAction = (Blackboard b) =>
                {
                    retrievedValue = b.Get(key).ToString();
                }
            };

            stateWriter.Update();
            stateReader.Update();

            Assert.AreEqual(value, retrievedValue);
        }
    }
}

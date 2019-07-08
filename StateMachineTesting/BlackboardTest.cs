using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace StateMachineTesting
{
    [TestClass]
    public class BlackboardTest
    {
        [TestMethod]
        public void GetNonexistentValue()
        {
            var board = new TestBlackboard();

            var value = board.Get("key");

            Assert.IsNull(value);
        }

        [TestMethod]
        public void SetValue()
        {
            var board = new TestBlackboard();

            board.Set("key", 9);

            Assert.AreEqual(1, board.Values.Count);
        }

        [TestMethod]
        public void GetValue()
        {
            var key = "key";
            var value = 29;
            var board = new TestBlackboard();
            board.Values.Add(key, value);

            var retrieved = board.Get(key);

            Assert.AreEqual(value, (int)retrieved);
        }

        [TestMethod]
        public void GetValueFromMany()
        {
            var key = "key";
            var value = 5;
            var board = new TestBlackboard();
            board.Values.Add("another", "someValue");
            board.Values.Add(key, value);
            board.Values.Add("bool", false);

            var retrieved = board.Get(key);

            Assert.AreEqual(value, (int)retrieved);
        }

        [TestMethod]
        public void SetValueFromMany()
        {
            var key = "key";
            var value = true;
            var board = new TestBlackboard();
            board.Values.Add("another", 392.3);

            board.Set(key, value);

            Assert.AreEqual(2, board.Values.Count);
        }

        [TestMethod]
        public void SetValueAlreadyExisting()
        {
            var key = "key";
            var value = 4476;
            var board = new TestBlackboard();
            board.Values.Add("key", 392.3);

            board.Set(key, value);

            Assert.AreEqual(1, board.Values.Count);
            Assert.AreEqual(value, (int)board.Values.Values.First());
        }
    }
}

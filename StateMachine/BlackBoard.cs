
using System.Collections.Generic;

namespace FSM
{
    public class BlackBoard : IBlackBoard
    {
        private Dictionary<string, BlackBoardValue> values = new Dictionary<string, BlackBoardValue>();


        public BlackBoardValue Get(string name)
        {
            var value = new BlackBoardValue();
            values.TryGetValue(name, out value);
            return value;
        }

        public void Set(string name, BlackBoardValue value)
        {
            values.Add(name, value);
        }

        public void Set(string name, object value)
        {
            values.Add(name, new BlackBoardValue(value, value.GetType()));
        }
    }
}

using System.Collections.Generic;

namespace StateMachineCore
{
    public class Blackboard
    {
        protected Dictionary<string, object> Values { get; set; } = new Dictionary<string, object>();

        public object Get(string name)
        {
            Values.TryGetValue(name, out object value);
            return value;
        }

        public void Set(string name, object value)
        {
            Values.Remove(name);
            Values.Add(name, value);
        }
    }
}

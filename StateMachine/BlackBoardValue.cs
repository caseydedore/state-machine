
using System;

namespace FSM
{
    public struct BlackBoardValue
    {
        public object Value { get; private set; }
        public Type ValueType { get; private set; }


        public BlackBoardValue(object value, Type type)
        {
            Value = value;
            ValueType = type;
        }
    }
}

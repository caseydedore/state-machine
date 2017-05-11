
namespace FSM
{
    public interface IBlackBoard
    {
        void Set(string name, object value);
        void Set(string name, BlackBoardValue value);
        BlackBoardValue Get(string name);
    }
}

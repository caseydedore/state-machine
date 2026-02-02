
namespace StateMachine.Core;

public interface IState
{
    void Start();
    void End();
    void Update();
    void StartSkipOptional();
    void EndSkipOptional();
    void UpdateSkipOptional();
}

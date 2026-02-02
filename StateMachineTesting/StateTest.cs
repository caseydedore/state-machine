
namespace StateMachineTesting;

[TestClass]
public class StateTest
{
    [TestMethod]
    public void StateSkipOptionals()
    {
        var state = new TestState();

        state.StartSkipOptional();
        state.UpdateSkipOptional();
        state.EndSkipOptional();

        Assert.AreEqual(1, state.StartIterations);
        Assert.AreEqual(0, state.OptionalStartIterations);
        Assert.AreEqual(1, state.UpdateIterations);
        Assert.AreEqual(0, state.OptionalUpdateIterations);
        Assert.AreEqual(1, state.EndIterations);
        Assert.AreEqual(0, state.OptionalEndIterations);
    }
}

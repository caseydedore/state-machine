
using StateMachine.Core;

namespace StateMachineTesting;

[TestClass]
public class TransitionTest
{
    [TestMethod]
    public void TransitionIsChecked()
    {
        var root = new StateGroup();
        var state = new TestState();
        var didCheckTransition = false;
        var transition = new StateTransition(state, state, () => {
            didCheckTransition = true;
            return true;
        });
        root.AddTransition(transition);
        root.Entry = state;

        root.Start();
        root.Update();

        Assert.IsTrue(didCheckTransition);
    }

    [TestMethod]
    public void TransitionSuccess()
    {
        var group = new TestStateGroup();
        var first = new TestState();
        var next = new TestState();
        group.AddTransition(first, next, () => true);
        group.Entry = first;

        group.Start();
        group.Update();

        Assert.AreEqual(1, first.StartIterations);
        Assert.AreEqual(1, first.UpdateIterations);
        Assert.AreEqual(1, first.EndIterations);
        Assert.AreEqual(0, first.OptionalStartIterations);
        Assert.AreEqual(0, first.OptionalUpdateIterations);
        Assert.AreEqual(0, first.OptionalEndIterations);
        Assert.AreEqual(0, next.StartIterations);
        Assert.AreEqual(0, next.UpdateIterations);
    }

    [TestMethod]
    public void TransitionSuccessNextStateUpdate()
    {
        var group = new TestStateGroup();
        var first = new TestState();
        var next = new TestState();
        group.AddTransition(first, next, () => true);
        group.Entry = first;

        group.Start();
        group.Update();
        group.Update();

        Assert.AreEqual(1, first.EndIterations);
        Assert.AreEqual(1, next.StartIterations);
        Assert.AreEqual(1, next.UpdateIterations);
        Assert.AreEqual(1, next.OptionalStartIterations);
        Assert.AreEqual(1, next.OptionalUpdateIterations);
    }

    [TestMethod]
    public void TransitionFailure()
    {
        var group = new TestStateGroup();
        var first = new TestState();
        var next = new TestState();
        group.AddTransition(first, next, () => false);
        group.Entry = first;

        group.Start();
        group.Update();

        Assert.AreEqual(0, first.EndIterations);
        Assert.AreEqual(0, next.StartIterations);
    }

    [TestMethod]
    public void TransitionFailures()
    {
        var group = new TestStateGroup();
        var first = new TestState();
        var next = new TestState();
        group.AddTransition(first, next, () => false);
        group.Entry = first;

        group.Start();
        var iteration = 0;
        while (iteration < 13)
        {
            iteration++;
            group.Update();
        }

        Assert.AreEqual(0, first.EndIterations);
        Assert.AreEqual(0, next.StartIterations);
    }

    [TestMethod]
    public void TransitionFirstPrioritySuccess()
    {
        var group = new TestStateGroup();
        var start = new TestState();
        var destination = new TestState();
        var secondDestination = new TestState();
        group.AddTransition(start, destination, () => true);
        group.AddTransition(start, secondDestination, () => false);
        group.Entry = start;

        group.Start();
        group.Update();
        group.Update();

        Assert.AreEqual(1, destination.StartIterations);
        Assert.AreEqual(1, destination.UpdateIterations);
        Assert.AreEqual(0, secondDestination.StartIterations);
        Assert.AreEqual(0, secondDestination.UpdateIterations);
    }

    [TestMethod]
    public void TransitionSecondPrioritySuccess()
    {
        var group = new TestStateGroup();
        var start = new TestState();
        var attemptedDestination = new TestState();
        var destination = new TestState();
        group.AddTransition(start, attemptedDestination, () => false);
        group.AddTransition(start, destination, () => true);
        group.Entry = start;

        group.Start();
        group.Update();
        group.Update();

        Assert.AreEqual(0, attemptedDestination.StartIterations);
        Assert.AreEqual(0, attemptedDestination.UpdateIterations);
        Assert.AreEqual(1, destination.StartIterations);
        Assert.AreEqual(1, destination.UpdateIterations);
    }

    [TestMethod]
    public void TransitionPreventsStateOptionalEnd()
    {
        var root = new TestStateGroup();
        var state = new TestState();
        var dest = new TestState();
        root.Entry = state;
        root.AddTransition(state, dest, () => true);

        root.Start();
        root.Update();

        Assert.AreEqual(1, state.EndIterations);
        Assert.AreEqual(0, state.OptionalEndIterations);
    }

    [TestMethod]
    public void TransitionBackAndForth()
    {
        var shouldTransition = false;
        var group = new TestStateGroup();
        var first = new TestState
        (
            update: () => shouldTransition = true,
            end: () => shouldTransition = false
        );
        var second = new TestState
        (
            update: () => shouldTransition = true,
            end: () => shouldTransition = false
        );
        group.AddTransition(first, second, () => shouldTransition);
        group.AddTransition(second, first, () => shouldTransition);
        group.Entry = first;

        group.Start();
        group.Update();
        group.Update();
        group.Update();
        group.Update();

        Assert.AreEqual(2, first.StartIterations);
        Assert.AreEqual(2, first.OptionalStartIterations);
        Assert.AreEqual(2, first.UpdateIterations);
        Assert.AreEqual(2, first.OptionalUpdateIterations);
        Assert.AreEqual(2, first.EndIterations);
        Assert.AreEqual(0, first.OptionalEndIterations);
        Assert.AreEqual(2, second.StartIterations);
        Assert.AreEqual(2, second.OptionalStartIterations);
        Assert.AreEqual(2, second.UpdateIterations);
        Assert.AreEqual(2, second.OptionalUpdateIterations);
        Assert.AreEqual(2, second.EndIterations);
        Assert.AreEqual(0, second.OptionalEndIterations);
    }

    [TestMethod]
    public void MultipleTransitionsSucceed()
    {
        var group = new TestStateGroup();
        var first = new TestState();
        var destination = new TestState();
        var lastDestination = new TestState();
        group.AddTransition(first, destination, () => true);
        group.AddTransition(first, lastDestination, () => true);
        group.Entry = first;

        group.Start();
        group.Update();
        group.Update();

        Assert.AreEqual(1, destination.StartIterations);
        Assert.AreEqual(1, destination.UpdateIterations);
        Assert.AreEqual(0, lastDestination.StartIterations);
        Assert.AreEqual(0, lastDestination.UpdateIterations);
    }

    [TestMethod]
    public void TransitionSuccessAfterFailure()
    {
        var group = new TestStateGroup();
        var state = new TestState();
        var destination = new TestState();
        var willTransitionSucceed = false;
        group.AddTransition(state, destination, () => willTransitionSucceed);
        group.Entry = state;

        group.Start();
        group.Update();
        willTransitionSucceed = true;
        group.Update();
        group.Update();

        Assert.AreEqual(1, destination.StartIterations);
    }

    [TestMethod]
    public void GroupTransitionPreventsSubstateEvents()
    {
        var root = new TestStateGroup();
        var group = new TestStateGroup();
        var destGroup = new TestStateGroup();
        var state = new TestState();
        group.Entry = state;
        root.AddTransition(group, destGroup, () => true);
        root.Entry = group;

        root.Start();
        root.Update();

        Assert.AreEqual(0, state.StartIterations);
        Assert.AreEqual(0, state.UpdateIterations);
        Assert.AreEqual(0, state.EndIterations);
        Assert.AreEqual(0, state.OptionalStartIterations);
        Assert.AreEqual(0, state.OptionalUpdateIterations);
        Assert.AreEqual(0, state.OptionalEndIterations);
    }

    [TestMethod]
    public void TransitionEvalsStopUponFirstSuccess()
    {
        var group = new TestStateGroup();
        var state = new TestState();
        var stateDest = new TestState();
        bool didSecondEvaluate = false;
        bool secondTransitionShouldNotEvaluate()
        {
            didSecondEvaluate = true;
            return true;
        }
        group.AddTransition(state, stateDest, () => true);
        group.AddTransition(state, stateDest, secondTransitionShouldNotEvaluate);
        group.Entry = state;

        group.Start();
        group.Update();

        Assert.IsFalse(didSecondEvaluate);
    }
}

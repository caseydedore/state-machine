using Microsoft.VisualStudio.TestTools.UnitTesting;
using StateMachineCore;
using StateMachineTesting.TestState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachineTesting
{
    [TestClass]
    public class TransitionBehaviorTest
    {
        private StateMachine machine = null;


        [TestMethod]
        public void EndState()
        {
            machine = new StateMachine();
            var state = new State(machine);

            machine.AddState(state);
            state.AddTransitionToEnd(() => { return true; }, TransitionBehavior.EndState);

            machine.Update();

            Assert.AreEqual(0, machine.CurrentStates.Count);
            Assert.AreEqual(1, state.EndIterations);
        } 

        [TestMethod]
        public void WaitToEndState()
        {
            machine = new StateMachine();
            var state = new State(machine);
            var iterations = 5;

            machine.AddState(state);
            state.AddTransitionToEnd(() => { return true; }, TransitionBehavior.WaitForEndState);

            for(var i = 1; i <= iterations; i++)
            {
                machine.Update();

                if (i == iterations - 1)
                {
                    state.ShouldCompleteAfterNextUpdate(true);
                }
            }

            machine.Update();

            Assert.AreEqual(iterations, state.UpdateIterations);
            Assert.AreEqual(1, state.StartIterations);
            Assert.AreEqual(1, state.EndIterations);
        }

        [TestMethod]
        public void DoNotEndState()
        {
            Assert.Fail();
        }
    }
}

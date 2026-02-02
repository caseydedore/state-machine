# StateMachine
Finite State Machine (FSM) in C#

This FSM library is intended to provide an easy-to-use FSM for developing reactive systems. It consists of a few basic components detailed below. **View the unit test project for examples of API usage**.

States are decoupled from one another. Transitions between states are declared externally instead of within the state logic itself. This allows state reusability as states are not dependent on the FSM structure as a whole. The transitions inform the FSM when to change active states based on conditional checks against some portion of the system. Transitions form the main relationships within the FSM.

### StateGroup
StateGroup is the manager of child States and the sole facilitator of hierarchy in the FSM. These are responsible for the lifecycle execution of child States.

A single State is designated the Entry point for a StateGroup. This State will be made initially active as the StateGroup is made initially active.

StateGroups manage transitions between child States. Transitions between States are assigned to StateGroups, forming the implicit connection between parent StateGroup and children States.

StateGroup is also a State and has the same lifecycle. StateGroups may be placed anywhere in the FSM hierarchy as children of other StateGroups.

One StateGroup should be root of the FSM. Lifecycle updates for the FSM should be executed on the root. Any number of States or StateGroups may be children of the root.

### State
States are the core unit of the FSM. States have lifecycle events corresponding to their status.

States are the drivers for client logic. Logic may be executed in any of the State's lifecycle events, `Start`, `OptionalStart`, `Update`, `OptionalUpdate`, `End`, `OptionalEnd`. These are executed via a parent StateGroup.

In an FSM with a StateGroup root, only a single plain State may be active at one time. StateGroups are also States, so the parent StateGroup and any up the hierarchy to the root are also active.

State lifecycle events are always executed at least once when a State is active. Optionals are the exception. Optionals will execute as long as any transition, defined from the active State to another State, has not yet succeeded. Optionals will not execute once a transition from the active State succeeds.

### StateTransition
StateTransitions are defined by the state the transition is from, the State the transition is to, the condition delegate to evaluate if the transition succeeds. Transitions are processed by the StateGroup, and the minimum update iterations the State should run before the condition is valid. If a transition condition is successful, the managing StateGroup will end the active State and start another as specified by the successful transition.

### Quick Start
```csharp
// Create the root node, which should be a StateGroup.
var root = new StateGroup();

// Create States. These have no logic for lifecycle events for this example.
var stateOne = new State();
var stateTwo = new State();

// Assign a State to the StateGroup Entry point. This State is active when the StateGroup begins activity.
root.Entry = stateOne;

// Setup transitions for the States. Transitions are managed by a parent StateGroup. These conditionals immediately return true and these States will perform a single Update before the StateGroup transitions to the next.
root.AddTransition(from: stateOne, to: stateTwo, condition: () => true);
root.AddTransition(from: stateTwo, to: stateOne, condition: () => true);

// Begin by calling Start on the root. While a StateGroup may exist anywhere in the hierarchy, as root, it requires manual execution.
root.Start();

// Proceed with manual calls to Update as desired to advance system state.
root.Update();

// End should be called on root when terminating the FSM and proper State lifecycle events are expected.
root.End();
```

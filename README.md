# StateMachine
Finite State Machine (FSM) in C#

This FSM library is intended to provide an easy-to-use FSM for developing reactive systems. It consists of a few basic components detailed below. **View the unit test project for examples of API usage**.

States are decoupled from one another. Transitions between states are declared externally instead of within the state logic itself. This allows state reusability as states are not dependent on the FSM structure as a whole. The transitions inform the FSM when to change active states based on conditional checks against some portion of the system. Transitions form the main relationships within the FSM.

### StateMachine
StateMachine is the manager of states and the root of the FSM. It is responsible for `Update()`, `Start()`, and `End()` execution for States. It also is responsible for handling transitions when transition conditions are successful after an `Update()`.

### State
The State bears the most responsibility within the FSM. Behavioral logic is defined in the State. States store their own eligible StateTransitions and are responsible for testing transitions after an `Update()`. These are transitions from the current State to another, as defined in the transition.

### StateTransition
StateTransitions store a reference to the destination State and are stored on the running State. They also store a conditional check (function) to determine whether or not the transition is valid. If valid, then the transition will be executed and the destination State will replace the current State as active.

### Quick Start
```csharp
// Create the root node, which should be a StateGroup.
var root = new StateGroup();

// Create States. These have no logic for lifecycle events for this example.
var stateOne = new State();
var stateTwo = new State();

// Assign a State to the StateGroup Entry point. This State is active when the StateGroup begins activity.
root.Entry = stateOne;

// Setup transitions for the States. Transitions are managed by a parent StateGroup, thus States with transitions always have an implied parent. These conditionals immediately return true and these States will perform a single Update before the StateGroup transitions to the next.
root.AddTransition(from: stateOne, to: stateTwo, condition: () => true);
root.AddTransition(from: stateTwo, to: stateOne, condition: () => true);

// Begin by calling Start on the root. While a StateGroup may exist anywhere in the hierarchy, as root, it requires manual execution.
root.Start();

// Proceed with manual calls to Update as desired to advance system state.
root.Update();

// End should be called on root when terminating the FSM and proper State lifecycle events are expected.
root.End();
```

# StateMachine
Finite State Machine (FSM) in C#

This FSM library is intended to provide an easy-to-use FSM for developing reactive systems. It consists of a few basic pieces detailed below. **View the unit test project for examples of API usage**.

States are decoupled from one another by enforcing that the transitions between states are declared externally instead of within the state logic itself. This allows state reusability as states are not dependent on the FSM structure as a whole. The transitions inform the FSM when to change active states based on conditional checks against some portion of the system. Transitions as a result might rely on the system as a whole so that states don't need to be.

### StateMachine
StateMachine is the manager of states and the root of the FSM. It is responsible for `Update()`, `Start()`, and `End()` execution for States. It also is responsible for handling transitions when transition conditions are successful after an `Update()`.
### State
The State bears the most responsibility within the FSM. Behavioral logic is defined in the State. States store their own eligible StateTransitions and are responsible for testing transitions after an `Update()`. These are transitions from the current State to another, as defined in the transition.
### StateTransition
StateTransitions store a reference to the destination State and are stored on the running State. They also store a conditional check (function) to determine whether or not the transition is valid. If valid, then the transition will be executed and the destination State will replace the current State as active.

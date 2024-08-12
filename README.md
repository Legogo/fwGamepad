# fwGamepad

Library meant to give some tools to have multiple controllers working without too much setup
This won't be as flexible as what unity InputSystem is doing. It's meant to have a quick&dirty setup game jam (or short projects).

## Next steps

I'm currently working on integrating this library (on my spare time) to older projects to make sure it's generic enough for everyone.

A "how to" will come when it's stable enough.

## Structure

Each entity (controlled by players) is linked to a specific controller using : InputSysGamepad It will provide a "subs" container.
The GamepadWatcher will provide the "selection" paradigm. Hard linked with InputSysGamepad.
Each InputSysGamepad will be setup with a target controller (one, two, ...)

You then need to create your own implementation of inputs by inheriting input interfaces.

    Iselectable + ISelectable[input]

At runtime this object must be injected into the watcher using the "select" method.
Watcher will "select" the object and call interface method when controller is performing.

## Basic setup

1. create a dedicated transform to carry gamepad link
    - PlayerInput (unity input system component)
    - InputSysGamepad
    - GamepadWatcher (selector : manage what object to send input to)

2. create an avatar and attach a script that own needed ISelectable + ISelectable(input) interfaces
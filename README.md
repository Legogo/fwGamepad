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

# Gamepad nomenclature

Directional (dpad)
Action (XYBA)
Stick/Joystick Left/Right
Stick press     Pressing the joystick   R3/L3

Select/Start
    (PS) create/options
    (XBOX) view/menu
    (Nin) - / +

# Culture

https://www.techspot.com/article/2182-anatomy-gamepad/
https://infinitalab.medium.com/anatomy-of-a-gamepad-design-materials-assembly-and-the-future-8e5e7d54aae8

# Constructor diagram

PS      : https://controller.dl.playstation.net/controller/lang/en/2100002.html
Xbox    : https://support.xbox.com/en-US/help/hardware-network/controller/get-to-know-elite-series-2
Switch  : https://www.nintendo.com/en-gb/Support/Nintendo-Switch/Joy-Con-Controller-Diagram-1518877.html

# Medias

Credits to : https://assetstore.unity.com/packages/2d/gui/icons/ux-flat-icons-free-202525?srsltid=AfmBOoo5wUfYFZGeRH8nGiMVuUBL6-vFhdm8tAn15FQTdxipddc6wz9g
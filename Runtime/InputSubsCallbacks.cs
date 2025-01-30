namespace fwp.gamepad
{
    using UnityEngine;
    using System;

    /// <summary>
    /// used by various elements
    /// </summary>
    public class InputSubsCallbacks
    {
        public Action<InputJoystickSide, Vector2> onJoystickPunchDirection; // when joystick pressed a direction and returned to neutral "quickly"
        public Action<InputJoystickSide, Vector2> onJoystickDirection; // only when value change beyond a threshold
        public Action<InputJoystickSide, Vector2> onJoystickPerformed; // every frame the value changes
        public Action<InputJoystickSide> onJoystickReleased; // joystick went back to neutral

        public Action<InputButtons, bool> onButtonPerformed;
        public Action<InputTriggers, float> onTriggerPerformed;
        public Action<InputDPad, bool> onDPadPerformed;
    }

}
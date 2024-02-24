using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.gamepad.blueprint
{
    using state;

    /// <summary>
    /// dual stick mapping
    /// </summary>
    [System.Serializable]
    public class BlueprintXbox : BlueprintSnes
    {
        public ControllerJoystickState leftJoy;
        public ControllerJoystickState rightJoy;

        public ControllerTriggerState trigLeft;
        public ControllerTriggerState trigRight;

        public void mimic(InputTriggers side, float value)
        {
            switch (side)
            {
                case InputTriggers.LT:
                    trigLeft.inject(value);
                    break;
                case InputTriggers.RT:
                    trigRight.inject(value);
                    break;
                default:
                    break;
            }
        }

        public void mimic(InputJoystickSide side, Vector2 value)
        {
            switch (side)
            {
                case InputJoystickSide.LEFT:
                    leftJoy.inject(value);
                    break;
                case InputJoystickSide.RIGHT:
                    rightJoy.inject(value);
                    break;
                default:
                    break;
            }
        }

    }

}

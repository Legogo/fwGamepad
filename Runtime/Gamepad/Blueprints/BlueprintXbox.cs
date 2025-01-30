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

        //public Vector2 LeftRaw => leftJoy.joystick;
        //public Vector2 RightRaw => rightJoy.joystick;

        public ControllerTriggerState trigLeft;
        public ControllerTriggerState trigRight;

        /// <summary>
        /// triggers
        /// </summary>
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

        public void mimic(InputJoystickSide side, Vector2 raw)
        {
            switch (side)
            {
                case InputJoystickSide.LEFT:
                    leftJoy.inject(raw);
                    break;
                case InputJoystickSide.RIGHT:
                    rightJoy.inject(raw);
                    break;
                default:
                    break;
            }
        }

        public void mimic(InputJoystickSide side, Vector2 raw, Vector2 dir)
        {
            switch (side)
            {
                case InputJoystickSide.LEFT:
                    leftJoy.inject(raw, dir);
                    break;
                case InputJoystickSide.RIGHT:
                    rightJoy.inject(raw, dir);
                    break;
                default:
                    break;
            }
        }

        public void mimicDirection(InputJoystickSide side, Vector2 dir)
        {
            switch (side)
            {
                case InputJoystickSide.LEFT:
                    leftJoy.injectDirection(dir);
                    break;
                case InputJoystickSide.RIGHT:
                    rightJoy.injectDirection(dir);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// some button have timings
        /// </summary>
        public void update(float dt)
        {
            leftJoy.update(dt);
            rightJoy.update(dt);
        }
    }

}

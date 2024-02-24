using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.inputeer.state
{
    [System.Serializable]
    public struct ControllerJoystickState
    {
        public Vector2 joystick;
        public float lastAngle; // last movement angle

        public void inject(Vector2 value)
        {
            lastAngle = Vector2.Angle(value, joystick);
            joystick = value;
        }
    }

}
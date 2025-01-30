using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.gamepad.state
{
    [System.Serializable]
    public struct ControllerJoystickState
    {
        public Vector2 joystick; // raw signal

        /// <summary>
        ///    N (0,1)
        /// W     E (1,0)
        ///    S
        /// </summary>
        public Vector2 joystickDirection; // last "direction" NESW

        float timerValid; // internal timer

        public float lastAngle; // last movement angle

        /// <summary>
        /// pressed a direction and quickly returned to neutral
        /// </summary>
        public bool Punch => timerValid > 0f;

        public void injectDirection(Vector2 value)
        {
            if(joystickDirection.sqrMagnitude != 0f)
            {
                if (!Punch || value != joystickDirection)
                {
                    timerValid = 0.33f;
                }
            }

            joystickDirection = value;
        }

        public void inject(Vector2 value)
        {
            lastAngle = Vector2.Angle(value, joystick);
            joystick = value;
        }

        public void inject(Vector2 value, Vector2 dir)
        {
            inject(value);

            injectDirection(dir);
        }

        public void onValidMotion()
        {
            timerValid = 0.5f;
        }

        public bool update(float dt)
        {
            if (timerValid > 0f)
            {
                timerValid -= dt;
                //Debug.Log(timerValid);
                if (timerValid <= 0f)
                {
                    return true;
                }
            }

            return false;
        }
    }

}
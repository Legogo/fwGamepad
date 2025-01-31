using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.gamepad.state
{
    [System.Serializable]
    public class ControllerJoystickState
    {
        /// <summary>
        /// 1/X seconds
        /// 30f ~= 10 frames
        /// </summary>
        public const float keyboardKeyGravity = 30f;

        public Vector2 joystick; // acutal solved gameplay value

        Vector2 joystickRaw; // raw signal, last known real input value

        /// <summary>
        /// when using keyboard we simulate progressive pressing of the joystick
        /// instead of jumping to max value instantly
        /// to avoid issue when canceling joystick usage because humans usualy don't release two keys during the same frame 
        /// </summary>
        Vector2 joystickIntention; // used for keyboard

        /// <summary>
        ///    N (0,1)
        /// W     E (1,0)
        ///    S
        /// </summary>
        public Vector2 joystickDirection; // last "direction" NESW

        public float lastAngle; // last movement angle

        const float timerPunchTime = 0.33f;
        float timerPunch; // internal timer

        public float PunchTimer => timerPunch;

        /// <summary>
        /// pressed a direction and quickly returned to neutral
        /// </summary>
        public bool Punch => timerPunch > 0f;

        void setDirection(Vector2 value)
        {
            // in value has SQR magnitude :
            // 0 = cancel
            // 1 = one direction
            // 2 = two directions

            //Debug.Log(value + " & " + value.sqrMagnitude + " <=> " + joystickDirection);

            if (value.sqrMagnitude != 0f)
            {
                if (!Punch || value != joystickDirection)
                {
                    timerPunch = timerPunchTime;
                    //Debug.Log("+punch");
                }
            }

            joystickDirection = value;
        }

        public bool injectIntention(Vector2 value, bool snap)
        {
            if (joystickIntention == value) return false;

            joystickIntention = value;

            joystickRaw = value;
            lastAngle = Vector2.Angle(value, joystickRaw);

            if (snap) joystick = joystickIntention;

            return true;
        }

        /// <summary>
        /// true : direction changed
        /// </summary>
        public bool injectDirection(Vector2 value)
        {
            // get diff in integer
            Vector2 intDirection = value.get6D();

            if (intDirection.x != joystickDirection.x || intDirection.y != joystickDirection.y) // any changes ?
            {
                setDirection(intDirection);
                return true;
            }
            return false;
        }

        public bool updateIntention(float dt)
        {
            if (joystick != joystickIntention)
            {
                joystick = Vector2.MoveTowards(joystick, joystickIntention, dt * keyboardKeyGravity);
                return true;
            }
            return false;
        }

        public bool updateTimer(float dt)
        {
            if (timerPunch > 0f)
            {
                timerPunch -= dt;
                if (timerPunch <= 0f)
                {
                    //Debug.Log("-punch");
                    return true;
                }
            }

            return false;
        }


    }

}
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
        public ControllerJoystickState leftJoy = new ControllerJoystickState();
        public ControllerJoystickState rightJoy = new ControllerJoystickState();

        //public Vector2 LeftRaw => leftJoy.joystick;
        //public Vector2 RightRaw => rightJoy.joystick;

        public ControllerTriggerState trigLeft = new ControllerTriggerState();
        public ControllerTriggerState trigRight = new ControllerTriggerState();

        public BlueprintXbox(InputSubsCallbacks subs = null) : base(subs)
        { }

        protected ControllerJoystickState getJoystick(InputJoystickSide side)
        {
            switch (side)
            {
                case InputJoystickSide.LEFT: return leftJoy;
                case InputJoystickSide.RIGHT: return rightJoy;
            }
            return null;
        }

        protected ControllerTriggerState getTrigger(InputTriggers side)
        {
            switch (side)
            {
                case InputTriggers.LT: return trigLeft;
                case InputTriggers.RT: return trigRight;
            }
            return null;
        }

        /// <summary>
        /// triggers
        /// </summary>
        public void inject(InputTriggers side, float value)
        {
            var tar = getTrigger(side);
            if (tar.inject(value)) subs.onTriggerPerformed?.Invoke(side, value);
        }

        public void inject(InputJoystickSide side, Vector2 intention, bool snap = true)
        {
            var tar = getJoystick(side);

            log("inject " + side + " " + intention + " snap?" + snap);

            if (tar.injectIntention(intention, snap))
            {
                log("intention : " + intention + " & " + tar.joystick);
                //subs.onJoystickPerformed?.Invoke(side, tar.joystick);
                if (snap) solveCallbacks(side, tar);
            }
        }

        /// <summary>
        /// snap = not keyboard
        /// </summary>
        public void mimic(InputJoystickSide side, Vector2 value, bool snap)
        {
            switch (side)
            {
                case InputJoystickSide.LEFT: leftJoy.injectIntention(value, snap); break;
                case InputJoystickSide.RIGHT: rightJoy.injectIntention(value, snap); break;
            }
        }
        public void mimicDirection(InputJoystickSide side, Vector2 dir)
        {
            switch (side)
            {
                case InputJoystickSide.LEFT: leftJoy.injectDirection(dir); break;
                case InputJoystickSide.RIGHT: rightJoy.injectDirection(dir); break;
            }
        }

        /// <summary>
        /// some button have timings
        /// </summary>
        override public void update(float dt)
        {
            base.update(dt);

            updateJoystick(InputJoystickSide.LEFT, leftJoy, dt);
            updateJoystick(InputJoystickSide.RIGHT, rightJoy, dt);
        }

        void updateJoystick(InputJoystickSide side, ControllerJoystickState j, float dt)
        {

            j.updateTimer(dt);

            if (j.updateIntention(dt) && subs != null)
            {
                solveCallbacks(side, j);
            }

        }

        void solveCallbacks(InputJoystickSide side, ControllerJoystickState j)
        {
            
            if (j.joystick.sqrMagnitude == 0f)
            {
                subs?.onJoystickReleased(side);
                log("punched ? " + j.Punch + " @ " + j.PunchTimer);
                if (j.Punch)
                {
                    log("punched !");
                    subs.onJoystickPunchDirection?.Invoke(side, j.joystickDirection);
                }
            }
            else
            {
                // positive magnitude
                subs?.onJoystickPerformed(side, j.joystick);
            }

            if (j.injectDirection(j.joystick))
            {
                log("direction : " + j.joystickDirection);
                subs.onJoystickDirection?.Invoke(side, j.joystickDirection);
            }
        }

    }

}

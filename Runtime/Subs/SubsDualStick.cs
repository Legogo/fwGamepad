using UnityEngine;
using System;

namespace fwp.inputeer
{
    public class SubsDualStick : SubsController
    {
        protected Vector2 _joyLeft;
        protected Vector2 _joyRight;

        public Action<InputJoystickSide, Vector2> onJoystickPerformed;
        public Action<InputJoystickSide> onJoystickReleased;

        /// <summary>
        /// pour les kappa de move
        /// performed = EACH FRAME until neutral
        /// release = when stick go back to neutral
        /// </summary>
        public void subJoysticks(Action<InputJoystickSide, Vector2> performed, Action<InputJoystickSide> release = null)
        {
            if (performed != null)
            {
                onJoystickPerformed += (InputJoystickSide side, Vector2 joy) =>
                {
                    switch (side)
                    {
                        case InputJoystickSide.LEFT:
                            _joyLeft = joy;
                            break;
                        case InputJoystickSide.RIGHT:
                            _joyRight = joy;
                            break;
                    }

                    performed?.Invoke(side, joy);
                };
            }

            if (release != null)
            {
                onJoystickReleased += (InputJoystickSide side) =>
                {
                    switch (side)
                    {
                        case InputJoystickSide.LEFT:
                            _joyLeft = Vector2.zero;
                            break;
                        case InputJoystickSide.RIGHT:
                            _joyRight = Vector2.zero;
                            break;
                    }

                    release?.Invoke(side);
                };
            }
        }

        public Vector2 getJoystickState(InputJoystickSide side)
        {
            switch (side)
            {
                case InputJoystickSide.LEFT: return _joyLeft;
                case InputJoystickSide.RIGHT: return _joyRight;
            }
            return Vector2.zero;
        }

        protected float _triggerLeft;
        protected float _triggerRight;

        public Action<InputTriggers, float> onTriggerPerformed;

        public void subTriggers(Action<InputTriggers, float> performed)
        {
            onTriggerPerformed += (InputTriggers trig, float val) =>
            {
                switch (trig)
                {
                    case InputTriggers.LEFT:
                        _triggerLeft = val;
                        break;
                    case InputTriggers.RIGHT:
                        _triggerRight = val;
                        break;
                }

                performed.Invoke(trig, val);
            };
        }

        public float getTriggerState(InputTriggers trigger)
        {
            switch (trigger)
            {
                case InputTriggers.LEFT: return _triggerLeft;
                case InputTriggers.RIGHT: return _triggerRight;
            }
            return 0f;
        }

    }
}
using UnityEngine;
using System;

namespace fwp.inputeer
{
    public class SubsDualStick : SubsController
    {
        public SubsDualStick():base()
        {
            onJoystickPerformed += (InputJoystickSide side, Vector2 joy) =>
            {
                //Debug.Log(side + " performed? " + joy);

                switch (side)
                {
                    case InputJoystickSide.LEFT:
                        _joyLeft = joy;
                        break;
                    case InputJoystickSide.RIGHT:
                        _joyRight = joy;
                        break;
                }
            };

            onJoystickReleased += (InputJoystickSide side) =>
            {
                //Debug.Log(side + " release");

                switch (side)
                {
                    case InputJoystickSide.LEFT:
                        _joyLeft = Vector2.zero;
                        break;
                    case InputJoystickSide.RIGHT:
                        _joyRight = Vector2.zero;
                        break;
                }

            };

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

            };
        }

        protected Vector2 _joyLeft;
        protected Vector2 _joyRight;

        public Action<InputJoystickSide, Vector2> onJoystickPerformed;
        public Action<InputJoystickSide> onJoystickReleased;

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

        public float getTriggerState(InputTriggers trigger)
        {
            switch (trigger)
            {
                case InputTriggers.LEFT: return _triggerLeft;
                case InputTriggers.RIGHT: return _triggerRight;
            }
            return 0f;
        }

        public override string stringify()
        {
            string output = base.stringify();

            output += $"\n TRIGGER  : L?{_triggerLeft} R?{_triggerRight}";
            output += $"\n JOYL     : {_joyLeft.x}x{_joyLeft.y}";
            output += $"\n JOYR     : {_joyRight.x}x{_joyRight.y}";

            return output;
        }
    }

}
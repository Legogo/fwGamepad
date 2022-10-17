using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace fwp.inputeer
{
    public class SubsController
    {
        public SubsController()
        {

            onDPadPerformed += (InputDPad dpadDirection, bool state) =>
            {

                switch (dpadDirection)
                {
                    case InputDPad.DPAD_WEST:
                        _dpadWest = state;
                        break;
                    case InputDPad.DPAD_EAST:
                        _dpadEast = state;
                        break;
                    case InputDPad.DPAD_NORTH:
                        _dpadNorth = state;
                        break;
                    case InputDPad.DPAD_SOUTH:
                        _dpadSouth = state;
                        break;
                }

            };

            onButtonPerformed += (InputButtons button, bool state) =>
            {
                switch (button)
                {
                    case InputButtons.PAD_WEST:
                        _buttonWest = state;
                        break;
                    case InputButtons.PAD_EAST:
                        _buttonEast = state;
                        break;
                    case InputButtons.PAD_NORTH:
                        _buttonNorth = state;
                        break;
                    case InputButtons.PAD_SOUTH:
                        _buttonSouth = state;
                        break;
                    case InputButtons.START:
                        _buttonStart = state;
                        break;
                    case InputButtons.RETURN:
                        _buttonBack = state;
                        break;
                    case InputButtons.BL:
                        _bumperLeft = state;
                        break;
                    case InputButtons.BR:
                        _bumperRight = state;
                        break;
                }
            };
        }

        protected bool _dpadNorth;
        protected bool _dpadSouth;
        protected bool _dpadEast;
        protected bool _dpadWest;

        public Action<InputDPad, bool> onDPadPerformed;

        public bool isDpadPressed(InputDPad button)
        {
            switch (button)
            {
                case InputDPad.DPAD_WEST: return _dpadWest;
                case InputDPad.DPAD_EAST: return _dpadEast;
                case InputDPad.DPAD_NORTH: return _dpadNorth;
                case InputDPad.DPAD_SOUTH: return _dpadSouth;
            }

            return false;
        }

        
        protected bool _buttonNorth;
        protected bool _buttonSouth;

        protected bool _buttonWest;
        protected bool _buttonEast;
        
        protected bool _buttonStart;
        protected bool _buttonBack;

        protected bool _bumperLeft;
        protected bool _bumperRight;

        public Action<InputButtons, bool> onButtonPerformed;

        public bool isButtonPressed(InputButtons button)
        {
            switch (button)
            {
                case InputButtons.PAD_WEST: return _buttonWest;
                case InputButtons.PAD_EAST: return _buttonEast;
                case InputButtons.PAD_NORTH: return _buttonNorth;
                case InputButtons.PAD_SOUTH: return _buttonSouth;
                case InputButtons.START: return _buttonStart;
                case InputButtons.RETURN: return _buttonBack;
                case InputButtons.BL: return _bumperLeft;
                case InputButtons.BR: return _bumperRight;
            }

            return false;
        }
        
        virtual public string stringify()
        {
            string output = "pad";
            
            output += $"\n DPAD   : N?{_dpadNorth} S?{_dpadSouth} W?{_dpadWest} E?{_dpadEast}";
            output += $"\n BUTTON : N?{_buttonNorth} S?{_buttonSouth} W?{_buttonWest} E?{_buttonEast}";
            output += $"\n BUMPER : L?{_bumperLeft} R?{_bumperRight}";

            return output;
        }
    }
}

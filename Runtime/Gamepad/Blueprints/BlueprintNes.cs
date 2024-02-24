using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.inputeer.blueprint
{
    using fwp.inputeer.state;

    [System.Serializable]
    public class BlueprintNes
    {

        public ControllerButtonState dpad_north;
        public ControllerButtonState dpad_south;
        public ControllerButtonState dpad_east;
        public ControllerButtonState dpad_west;

        public Vector2 dpad => new Vector2(
            dpad_west.state ? -1 : dpad_east.state ? 1 : 0f,
            dpad_south.state ? -1 : dpad_north.state ? 1 : 0f);

        public ControllerButtonState start;
        public ControllerButtonState back;

        public ControllerButtonState pad_south;  // A
        public ControllerButtonState pad_east;   // B

        public void mimic(InputDPad type, bool state)
        {
            switch (type)
            {
                case InputDPad.NONE:
                    break;
                case InputDPad.DPAD_WEST:
                    dpad_west.inject(state);
                    break;
                case InputDPad.DPAD_EAST:
                    dpad_east.inject(state);
                    break;
                case InputDPad.DPAD_NORTH:
                    dpad_north.inject(state);
                    break;
                case InputDPad.DPAD_SOUTH:
                    dpad_south.inject(state);
                    break;
                default:
                    break;
            }
        }

        virtual public void mimic(InputButtons type, bool state)
        {
            switch (type)
            {
                case InputButtons.PAD_EAST:
                    pad_east.inject(state);
                    break;
                case InputButtons.PAD_SOUTH:
                    pad_south.inject(state);
                    break;
                case InputButtons.START:
                    start.inject(state);
                    break;
                case InputButtons.RETURN:
                    back.inject(state);
                    break;
            }
        }

    }

}

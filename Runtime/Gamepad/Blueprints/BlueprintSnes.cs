using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.gamepad.blueprint
{
    using state;

    [System.Serializable]
    public class BlueprintSnes : BlueprintNes
    {
        public ControllerButtonState bumpLeft;
        public ControllerButtonState bumpRight;

        public ControllerButtonState pad_north;  // Y
        public ControllerButtonState pad_west;   // X

        override public void mimic(InputButtons type, bool state)
        {
            base.mimic(type, state);

            switch (type)
            {
                case InputButtons.BL:
                    bumpLeft.inject(state);
                    break;
                case InputButtons.BR:
                    bumpRight.inject(state);
                    break;
            }
        }

    }
}

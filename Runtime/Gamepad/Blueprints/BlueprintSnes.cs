using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.gamepad.blueprint
{
    using state;

    [System.Serializable]
    public class BlueprintSnes : BlueprintNes
    {
        public ControllerButtonState shoulderLeft = new ControllerButtonState();
        public ControllerButtonState shoulderRight = new ControllerButtonState();

        public ControllerButtonState pad_north = new ControllerButtonState();  // Y
        public ControllerButtonState pad_west = new ControllerButtonState();   // X

        public BlueprintSnes(InputSubsCallbacks subs = null) : base(subs)
        { }

        override protected ControllerButtonState getButton(InputButtons type)
        {
            switch (type)
            {
                case InputButtons.PAD_NORTH: return pad_north;
                case InputButtons.PAD_WEST: return pad_west;
                case InputButtons.BL: return shoulderLeft;
                case InputButtons.BR: return shoulderRight;
                default: return base.getButton(type);
            }
        }

    }
}

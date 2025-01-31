using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.gamepad.state
{
    [System.Serializable]
    public class ControllerButtonState :ControllerState
    {
        int frame;
        public bool state;
        public bool pressed => state & frame == Time.frameCount;
        public bool released => !state & frame == Time.frameCount;

        public bool inject(bool newState)
        {
            frame = Time.frameCount;
            if(state != newState)
            {
                state = newState;
                return true;
            }
            return false;
        }
    }
}

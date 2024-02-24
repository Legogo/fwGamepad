using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.inputeer.state
{
    [System.Serializable]
    public struct ControllerButtonState
    {
        int frame;
        public bool state;
        public bool pressed => state & frame == Time.frameCount;
        public bool released => !state & frame == Time.frameCount;

        public void inject(bool newState)
        {
            frame = Time.frameCount;
            state = newState;
        }
    }
}

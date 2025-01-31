using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace fwp.gamepad.state
{
    public class ControllerTriggerState
    {
        public float pression;

        public bool state => pression > 0f;

        public bool inject(float value)
        {
            if(pression != value)
            {
                pression = value;
                return true;
            }
            return false;
        }
    }
}
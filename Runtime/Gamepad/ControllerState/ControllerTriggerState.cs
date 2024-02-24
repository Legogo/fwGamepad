using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace fwp.inputeer.state
{
    public struct ControllerTriggerState
    {
        public float pression;

        public bool state => pression > 0f;

        public void inject(float value)
        {
            pression = value;
        }
    }
}
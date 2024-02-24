using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.inputeer
{
    public class InputKeyTouch : InputKeySchematic
    {
        public bool touch() { return false; }
        public bool release() { return false; }
    }
}
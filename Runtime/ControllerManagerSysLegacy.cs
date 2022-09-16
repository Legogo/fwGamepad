using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.inputeer
{
    public class ControllerManagerSysLegacy : ControllerManagerSys
    {
        protected override int countSystemConnected()
        {
            return Input.GetJoystickNames().Length;
        }

        protected override int solveSignatures(int sysCount)
        {
            throw new System.NotImplementedException();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.inputeer
{
    using UnityEngine.InputSystem;

    public class ControllerManagerSysInput : ControllerManagerSys
    {
        override protected int countSystemConnected()
        {
            return Gamepad.all.Count;
        }

        override protected int solveSignatures(int sysCount)
        {
            int cnt = 0;

            Debug.Log("solving signatures /w syscount @ " + sysCount);

            while(signatures.Count < sysCount)
            {
                signatures.Add(new ControllerSignature());
            }

            for (int i = 0; i < signatures.Count; i++)
            {
                var sign = signatures[i];

                if (i < sysCount)
                {
                    sign.state = GamepadWatcher.WatcherState.PLUGGED;
                    cnt++;
                }
                else
                {
                    sign.state = GamepadWatcher.WatcherState.UNPLUGGED;
                }

                signatures[i] = sign;
            }

            return cnt;
        }
    }
}

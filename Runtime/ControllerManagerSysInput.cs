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

            int safe = 999;
            while (signatures.Count < sysCount && safe > 0)
            {
                signatures.Add(new ControllerSignature());
                safe--;
            }
            Debug.Assert(safe > 0);

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

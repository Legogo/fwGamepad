using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using fwp.inputeer;

public class InputeerTest : MonoBehaviour
{
    public InputeerGamepadTarget targetA;
    public InputeerGamepadTarget targetB;
    public InputeerGamepadTarget targetC;

    void Start()
    {
        var cm = ControllerManager.create();

        cm.onWatcherStateChanged += onWatcher;

        cm.boot();
    }

    void onWatcher(GamepadWatcher watcher)
    {
        if(watcher.state == GamepadWatcher.WatcherState.PLUGGED)
        {
            switch(watcher.index)
            {
                case 0: watcher.setFocus(targetA); break;
                case 1: watcher.setFocus(targetB); break;
                case 2: watcher.setFocus(targetC); break;
            }
        }
    }

}
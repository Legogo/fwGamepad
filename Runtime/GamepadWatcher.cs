using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace fwp.inputeer
{
    /// <summary>
    /// an object that can select compatible element
    /// and will bubble inputs to that object
    /// </summary>
    public class GamepadWatcher
    {

        public enum WatcherState
        {
            UNKNOWN,
            UNPLUGGED,
            PLUGGED
        }

        SubsController subs;

        GamepadSelection currentSelection;

        public int index;
        WatcherState _state = WatcherState.UNKNOWN;
        public WatcherState state
        {
            set
            {
                if (value == _state) return;

                if(_state == WatcherState.UNKNOWN && value != WatcherState.UNKNOWN)
                {
                    init(new ReactorDualStick());
                }

                _state = value;
            }
            get
            {
                return _state;
            }
        }

        public GamepadWatcher(int index)
        {
            this.index = index;
        }

        public bool isConnected()
        {
            return state == WatcherState.PLUGGED;
        }

        void init(ReactorController reactor)
        {
            subs = reactor.setup();

            SubsDualStick sds = subs as SubsDualStick;
            if (sds != null)
            {
                sds.subJoysticks(true, onJoystick, (InputJoystickSide side) => onJoystick(side, Vector2.zero));
            }

            Debug.Log("init a gamepad");
        }

        void onJoystick(InputJoystickSide side, Vector2 vec)
        {
            GamepadDualStick gds = currentSelection as GamepadDualStick;
            if (gds != null) gds.onStick(side, vec);
        }

        public void setFocus(GamepadSelection newTarget)
        {
            if (newTarget == currentSelection) return;

            if(currentSelection != null)
            {
                currentSelection.onUnselected();
                currentSelection = null;
            }

            currentSelection = newTarget;
            currentSelection.onSelected();
        }
    }

}

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

        List<GamepadSelection> focused = new List<GamepadSelection>();

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

            Debug.Log("init gamepad #"+index);
        }

        void onJoystick(InputJoystickSide side, Vector2 vec)
        {
            for (int i = 0; i < focused.Count; i++)
            {
                GamepadDualStick gds = focused[i] as GamepadDualStick;
                if (gds != null) gds.onStick(side, vec);
            }
            
        }

        public void addFocus(GamepadSelection target)
        {
            if (focused.IndexOf(target) < 0)
            {
                focused.Add(target);
                target.onSelected();
            }
        }

        public void loseFocus(GamepadSelection target)
        {

            if (focused.IndexOf(target) > 0)
            {
                focused.Remove(target);
                target.onUnselected();
            }
        }

        public void swapFocus(GamepadSelection target)
        {
            if(focused.Count > 0)
            {
                for (int i = 0; i < focused.Count; i++)
                {
                    if (focused[i] != target) focused[i].onUnselected();
                }

                focused.Clear();
            }

            addFocus(target);
        }
    }

}

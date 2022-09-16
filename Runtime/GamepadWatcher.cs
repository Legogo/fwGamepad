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

        public SubsController controllerState;

        List<GamepadSelection> focused = new List<GamepadSelection>();

        public int index;

        WatcherState _plugState = WatcherState.UNKNOWN;
        public WatcherState plugState
        {
            set
            {
                if (value == _plugState) return;

                //...

                _plugState = value;
            }
            get
            {
                return _plugState;
            }
        }

        public GamepadWatcher(int index)
        {
            this.index = index;

            //reactor will create input system object and plug it to subs
            //and return that subs
            controllerState = new ReactorDualStick().setup();

            Debug.Log("init gamepad #" + index);
        }

        public bool isConnected()
        {
            return plugState == WatcherState.PLUGGED;
        }

        /// <summary>
        /// to activate capacity to transfert input to a selection
        /// </summary>
        /// <param name="reactor"></param>
        public void primeTransfertLayer(ReactorController reactor)
        {
            //controllerState.subButtons((InputButtons button, bool state) => { });

            SubsDualStick sds = controllerState as SubsDualStick;
            if (sds != null)
            {
                sds.onJoystickPerformed += onJoystick;
                sds.onJoystickReleased += (InputJoystickSide side) => onJoystick(side, Vector2.zero);
            }

            //... OTHER SUBS

            Debug.Log("gamepad#"+index+" primed transfert layer");
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
                target.onSelected(this);
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

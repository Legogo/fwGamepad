using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.gamepad.blueprint
{
    using state;

    [System.Serializable]
    public class BlueprintNes
    {

        /// <summary>
        /// dpads are buttons and not joystick
        /// because on some controller you can press 2+ at the same time
        /// </summary>
        public ControllerButtonState dpad_north = new ControllerButtonState();
        public ControllerButtonState dpad_south = new ControllerButtonState();
        public ControllerButtonState dpad_east = new ControllerButtonState();
        public ControllerButtonState dpad_west = new ControllerButtonState();

        /// <summary>
        /// to vector2D
        /// </summary>
        public Vector2 dpad => new Vector2(
            dpad_west.state ? -1 : dpad_east.state ? 1 : 0f,
            dpad_south.state ? -1 : dpad_north.state ? 1 : 0f);

        public ControllerButtonState start = new ControllerButtonState();
        public ControllerButtonState back = new ControllerButtonState();

        public ControllerButtonState pad_south = new ControllerButtonState();  // A
        public ControllerButtonState pad_east = new ControllerButtonState();   // B

        public InputSubsCallbacks subs = null; // reactor

        public BlueprintNes(InputSubsCallbacks callbacks = null)
        {
            subs = callbacks;
        }

        /// <summary>
        /// might wanna add diagonals ?
        /// </summary>
        virtual protected ControllerButtonState getDpad(InputDPad type)
        {
            switch (type)
            {
                case InputDPad.DPAD_NORTH: return dpad_north;
                case InputDPad.DPAD_EAST: return dpad_east;
                case InputDPad.DPAD_SOUTH: return dpad_south;
                case InputDPad.DPAD_WEST: return dpad_west;
            }
            return null;
        }

        /// <summary>
        /// can add more buttons
        /// </summary>
        virtual protected ControllerButtonState getButton(InputButtons type)
        {
            switch (type)
            {
                case InputButtons.PAD_SOUTH: return pad_south;
                case InputButtons.PAD_EAST: return pad_east;
            }
            return null;
        }

        public void inject(InputDPad type, bool state)
        {
            var tar = getDpad(type);
            if (tar.inject(state))
            {
                log("dpad       " + type + "=" + state);
                subs.onDPadPerformed?.Invoke(type, state);
            }
        }

        public void inject(InputButtons type, bool state)
        {
            var tar = getButton(type);
            if (tar.inject(state))
            {
                log("button         " + type + "=" + state);
                subs.onButtonPerformed?.Invoke(type, state);
            }
        }

        public void mimic(InputButtons type, bool state)
        {
            getButton(type).state = state;
        }
        public void mimic(InputDPad type, bool state)
        {
            getDpad(type).state = state;
        }

        virtual public void update(float dt)
        { }

        protected void log(string content) => GamepadVerbosity.sLog(GetType() + " > " + content, this);
    }

}

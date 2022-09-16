using UnityEngine;
using System;

namespace fwp.inputeer
{
    public class SubsDualStick : SubsController
    {
        public Action<InputJoystickSide, Vector2> onJoystickPerformed;
        public Action<InputJoystickSide> onJoystickReleased;
        public Action<InputButtons, bool> onButtonPerformed;
        public Action<InputTriggers, float> onTriggerPerformed;
        public Action<InputDPad, bool> onDPadPerformed;

        public void subTriggers(bool sub, Action<InputTriggers, float> performed)
        {
            if (sub)
            {
                if (performed != null) onTriggerPerformed += performed;
            }
            else
            {
                if (performed != null) onTriggerPerformed -= performed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void subButtons(bool sub, Action<InputButtons, bool> performed)
        {
            if (sub)
            {
                if (performed != null) onButtonPerformed += performed;
            }
            else
            {
                if (performed != null) onButtonPerformed -= performed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void subDPad(bool sub, Action<InputDPad, bool> performed)
        {
            if (sub)
            {
                if (performed != null) onDPadPerformed += performed;
            }
            else
            {
                if (performed != null) onDPadPerformed -= performed;
            }
        }

        /// <summary>
        /// pour les kappa de move
        /// performed = EACH FRAME until neutral
        /// release = when stick go back to neutral
        /// </summary>
        public void subJoysticks(bool sub, Action<InputJoystickSide, Vector2> performed, Action<InputJoystickSide> release = null)
        {
            if (sub)
            {
                if (performed != null) onJoystickPerformed += performed;
                if (release != null) onJoystickReleased += release;
            }
            else
            {
                if (performed != null) onJoystickPerformed -= performed;
                if (release != null) onJoystickReleased -= release;
            }

        }
    }
}

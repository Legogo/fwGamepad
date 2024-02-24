using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace fwp.gamepad
{
    /// <summary>
    /// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.PlayerInput.html
    /// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Devices.html
    /// 
    /// this is where InputSubs is sub to InputSystem
    /// any external object can use the ref of the subs to sub to it
    /// </summary>
    public class InputSysGamepad : MonoBehaviour
    {
        static public bool verbose = false;

        public InputDevice sysDevice;
        public PlayerInput sysPlayerInput; // link with unity InputSystem

        /// <summary>
        /// when InputSystem triggers it will activates the callbacks
        /// within this InputSubs
        /// </summary>
        public InputSubs subs;

        /// <summary>
        /// this controls what device to target
        /// </summary>
        public InputController controller;

        public enum InputController
        {
            any,
            keyboard,
            xbox_pone,
            xbox_ptwo,
        }

        /// <summary>
        /// names setup in InputActionAsset mapping
        /// </summary>
        public enum MappingActions
        {
            joyLeft, joyRight,
            bumperL, bumperR,
            triggerL, triggerR,
            buttonSouth, buttonNorth, buttonWest, buttonEast,
            buttonStart, buttonBack,
            dpadNorth, dpadSouth, dpadWest, dpadEast
        }

        InputAction action(MappingActions act)
        {
            Debug.Assert(sysPlayerInput != null, "PlayerInput not fed");

            try
            {
                return sysPlayerInput.actions[act.ToString()];
            }
            catch
            {
                Debug.LogError("no action : " + act, this);
            }

            return null;
        }

        private void Awake()
        {
            subs = new InputSubs();
            log("created subs");

            var localPi = GetComponent<PlayerInput>();
            if (localPi == null)
            {
                Debug.LogError("no PlayerInput ??", this);
                return;
            }

            feed(localPi);
        }

        [ContextMenu("log devices")]
        void logDevices()
        {
            Debug.Log("x" + InputSystem.devices.Count);
            for (int i = 0; i < InputSystem.devices.Count; i++)
            {
                Debug.Log(InputSystem.devices[i]);
            }
        }

        InputDevice getDeviceIndex(InputController device)
        {
            if (device == InputController.keyboard)
            {
                return Keyboard.current;
            }
            else
            {
                int cnt = Gamepad.all.Count;
                if (device == InputController.xbox_pone && cnt > 0)
                    return Gamepad.all[0];

                if (device == InputController.xbox_ptwo && cnt > 1)
                    return Gamepad.all[1];
            }

            Debug.LogWarning("issue fetching device # <b>" + device.ToString() + "</b>");
            Debug.Log("Keyboard :   " + Keyboard.current);
            Debug.Log("Gamepads     x" + Gamepad.all.Count);

            return null;
        }

        public void feed(PlayerInput linkedPlayerInput)
        {
            Debug.Assert(linkedPlayerInput != null);
            sysPlayerInput = linkedPlayerInput;

            log("feed(" + sysPlayerInput.name + ")");

            sysDevice = getDeviceIndex(controller);

            if (sysDevice == null)
            {
                Debug.LogWarning("no device # " + controller.ToString());
                return;
            }

            /*
            Debug.Log("switching controller to : " + _controller);
            Debug.Log("to " + pi.name, pi);

            if (_controller != null)
            {
                if(!pi.SwitchCurrentControlScheme(_controller))
                {
                    Debug.LogError(_controller + " is not supported by InputSystem context ??");
                    return;
                }
            }
            */

            //var map = pi.actions.FindActionMap("mapping");
            //pi.GetDevice()
            // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.1/api/UnityEngine.InputSystem.PlayerInputManager.html
            //var map = pi.actions.FindActionMap("mapping");


            log("controls generated");

            setupJoysticks();
            setupButtons();
            setupDpad();
        }

        void setupButtons()
        {
            // bumpers L/R

            action(MappingActions.bumperL).performed += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(this, InputButtons.BL, true);
            action(MappingActions.bumperL).canceled += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(this, InputButtons.BL, false);

            action(MappingActions.bumperR).performed += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(this, InputButtons.BR, true);
            action(MappingActions.bumperR).canceled += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(this, InputButtons.BR, false);

            // trigger L/R

            action(MappingActions.triggerL).performed += (InputAction.CallbackContext ctx)
                => subs.onTriggerPerformed?.Invoke(this, InputTriggers.LT, ctx.ReadValue<float>());
            action(MappingActions.triggerL).canceled += (InputAction.CallbackContext ctx)
                => subs.onTriggerPerformed?.Invoke(this, InputTriggers.LT, 0f);

            action(MappingActions.triggerR).performed += (InputAction.CallbackContext ctx)
                => subs.onTriggerPerformed?.Invoke(this, InputTriggers.RT, ctx.ReadValue<float>());
            action(MappingActions.triggerR).canceled += (InputAction.CallbackContext ctx)
                => subs.onTriggerPerformed?.Invoke(this, InputTriggers.RT, 0f);

            // pad buttons

            action(MappingActions.buttonStart).performed += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(this, InputButtons.START, true);
            action(MappingActions.buttonStart).canceled += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(this, InputButtons.START, false);

            // le bouton du nord parmis les XYBA
            action(MappingActions.buttonNorth).performed += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(this, InputButtons.PAD_NORTH, true);
            action(MappingActions.buttonNorth).canceled += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(this, InputButtons.PAD_NORTH, false);

            // le bouton du sud parmis les XYBA
            action(MappingActions.buttonSouth).performed += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(this, InputButtons.PAD_SOUTH, true);
            action(MappingActions.buttonSouth).canceled += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(this, InputButtons.PAD_SOUTH, false);

            // le bouton de gauche parmis les XYBA
            action(MappingActions.buttonWest).performed += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(this, InputButtons.PAD_WEST, true);
            action(MappingActions.buttonWest).canceled += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(this, InputButtons.PAD_WEST, false);

            // le bouton de droite parmis les XYBA
            action(MappingActions.buttonEast).performed += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(this, InputButtons.PAD_EAST, true);
            action(MappingActions.buttonEast).canceled += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(this, InputButtons.PAD_EAST, false);

            // le bouton start
            action(MappingActions.buttonStart).performed += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(this, InputButtons.START, true);
            action(MappingActions.buttonStart).canceled += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(this, InputButtons.START, false);
        }

        void setupDpad()
        {
            action(MappingActions.dpadSouth).performed += (InputAction.CallbackContext ctx)
                => subs.onDPadPerformed?.Invoke(this, InputDPad.DPAD_SOUTH, true);
            action(MappingActions.dpadSouth).canceled += (InputAction.CallbackContext ctx)
                => subs.onDPadPerformed?.Invoke(this, InputDPad.DPAD_SOUTH, false);

            action(MappingActions.dpadWest).performed += (InputAction.CallbackContext ctx)
                => subs.onDPadPerformed?.Invoke(this, InputDPad.DPAD_WEST, true);
            action(MappingActions.dpadWest).canceled += (InputAction.CallbackContext ctx)
                => subs.onDPadPerformed?.Invoke(this, InputDPad.DPAD_WEST, false);

            action(MappingActions.dpadEast).performed += (InputAction.CallbackContext ctx)
                => subs.onDPadPerformed?.Invoke(this, InputDPad.DPAD_EAST, true);
            action(MappingActions.dpadEast).canceled += (InputAction.CallbackContext ctx)
                => subs.onDPadPerformed?.Invoke(this, InputDPad.DPAD_EAST, false);

            action(MappingActions.dpadNorth).performed += (InputAction.CallbackContext ctx)
                => subs.onDPadPerformed?.Invoke(this, InputDPad.DPAD_NORTH, true);
            action(MappingActions.dpadNorth).canceled += (InputAction.CallbackContext ctx)
                => subs.onDPadPerformed?.Invoke(this, InputDPad.DPAD_NORTH, false);

        }

        void setupJoysticks()
        {
            //Joysticks
            //left

            action(MappingActions.joyLeft).performed += (InputAction.CallbackContext ctx) =>
            {
                Vector2 joyLeft = ctx.ReadValue<Vector2>();

                log("motion ? " + joyLeft.x + " x " + joyLeft.y + " , " + joyLeft.sqrMagnitude);

                //Debug.Assert(InputSubber.subs != null, "no sub ?");
                subs.onJoystickPerformed?.Invoke(this, InputJoystickSide.LEFT, joyLeft);
            };

            action(MappingActions.joyLeft).canceled += (InputAction.CallbackContext ctx) =>
            {
                Vector2 joyLeft = ctx.ReadValue<Vector2>();

                log("motion stopped");

                subs.onJoystickReleased?.Invoke(this, InputJoystickSide.LEFT);
            };

            //right

            action(MappingActions.joyRight).performed += (InputAction.CallbackContext ctx) =>
            {
                Vector2 joyRight = ctx.ReadValue<Vector2>();

                log("motion ? " + joyRight.x + " x " + joyRight.y + " , " + joyRight.sqrMagnitude);

                subs.onJoystickPerformed?.Invoke(this, InputJoystickSide.RIGHT, joyRight);
            };

            action(MappingActions.joyRight).canceled += (InputAction.CallbackContext ctx) =>
            {
                Vector2 joyRight = ctx.ReadValue<Vector2>();

                log("motion stopped");

                subs.onJoystickReleased?.Invoke(this, InputJoystickSide.RIGHT);
            };

        }

        void log(string content)
        {
            if (!verbose)
                return;

            Debug.Log(GetType() + "&" + name + " >>> " + content, this);
        }

    }


    /// <summary>
    /// used by various elements
    /// </summary>
    public class InputSubs
    {
        public Action<InputSysGamepad, InputJoystickSide, Vector2> onJoystickPerformed;
        public Action<InputSysGamepad, InputJoystickSide> onJoystickReleased;
        public Action<InputSysGamepad, InputButtons, bool> onButtonPerformed;
        public Action<InputSysGamepad, InputTriggers, float> onTriggerPerformed;
        public Action<InputSysGamepad, InputDPad, bool> onDPadPerformed;
    }

}

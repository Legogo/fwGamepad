using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace fwp.gamepad
{
    /// <summary>
    /// 
    /// mono to select type of controller
    /// and provide entry points for inputs
    /// inherit and use subs
    /// 
    /// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/api/UnityEngine.InputSystem.PlayerInput.html
    /// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Devices.html
    /// 
    /// this is where InputSubs is sub to InputSystem
    /// any external object can use the ref of the subs to sub to it
    /// </summary>
    public class InputSysGamepad : MonoBehaviour
    {
        const float joy_threshold = 0.5f; // magnitude
        const float joy_threshold_angle = 40f; // °

        Vector2 joyLeft;
        Vector2 joyLeftDir;

        Vector2 joyRight;
        Vector2 joyRightDir;

        /// <summary>
        /// when InputSystem triggers it will activates the callbacks
        /// within this InputSubs
        /// </summary>
        public InputSubsCallbacks subs;

        public enum InputController
        {
            any,
            keyboard,
            mouse,
            gamepad_0,
            gamepad_1,
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


        /// <summary>
        /// shortcut to call callbacks from 
        /// </summary>
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

        [Header("setup")]

        /// <summary>
        /// this controls what device to target
        /// </summary>
        public InputController controllerType;

        public InputDevice sysDevice;
        public PlayerInput sysPlayerInput; // unity.PlayerInput > link with unity InputSystem

        private void Awake()
        {
            subs = new InputSubsCallbacks();

            log("created subs");

            if (sysPlayerInput == null)
            {
                sysPlayerInput = GetComponent<PlayerInput>();
            }

            Debug.Assert(sysPlayerInput != null, "need component PlayerInput");

            log(sysPlayerInput.name + " feed()");

            sysDevice = getDeviceIndex(controllerType);

            // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.1/api/UnityEngine.InputSystem.PlayerInputManager.html
            //var map = pi.actions.FindActionMap("mapping");

            if (sysDevice == null) Debug.LogWarning("controller <color=red>failed to bind</color> @" + controllerType);
            else
            {
                log("controls generated");

                setupJoysticks();

                setupPad();
                setupBumpers();
                setupTriggers();

                setupDpad();

                //sysDevice.enabled = true;
            }
        }

        [ContextMenu("log devices")]
        void logDevices()
        {
            Debug.Log("devices x" + InputSystem.devices.Count);
            for (int i = 0; i < InputSystem.devices.Count; i++)
            {
                Debug.Log($"#{i} {InputSystem.devices[i]}");
            }
        }

        InputDevice getDeviceIndex(InputController device)
        {

            if (Gamepad.all.Count <= 0)
            {
                Debug.LogWarning(name + " no gamepad connected", this);
            }

            switch (device)
            {
                case InputController.gamepad_0:
                    if (Gamepad.all.Count > 0) return Gamepad.all[0];

                    device = InputController.keyboard;
                    Debug.LogWarning("no gamepad for #0 : <color=orange>using keyboard instead</color>");

                    break;
                case InputController.gamepad_1:
                    if (Gamepad.all.Count > 1) return Gamepad.all[1];
                    break;
            }

            if (device == InputController.keyboard)
            {
                return Keyboard.current;
            }

            Debug.LogWarning("could not solve device @ " + device, this);
            //Debug.LogWarning("gamepad x" + Gamepad.all.Count);
            logDevices();

            return null;
        }

        void setupBumpers()
        {
            // bumpers L/R

            action(MappingActions.bumperL).performed += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.BL, true);
            action(MappingActions.bumperL).canceled += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.BL, false);

            action(MappingActions.bumperR).performed += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.BR, true);
            action(MappingActions.bumperR).canceled += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.BR, false);

        }

        void setupTriggers()
        {

            // trigger L/R

            action(MappingActions.triggerL).performed += (InputAction.CallbackContext ctx) => subs.onTriggerPerformed?.Invoke(InputTriggers.LT, ctx.ReadValue<float>());
            action(MappingActions.triggerL).canceled += (InputAction.CallbackContext ctx) => subs.onTriggerPerformed?.Invoke(InputTriggers.LT, 0f);

            action(MappingActions.triggerR).performed += (InputAction.CallbackContext ctx) => subs.onTriggerPerformed?.Invoke(InputTriggers.RT, ctx.ReadValue<float>());
            action(MappingActions.triggerR).canceled += (InputAction.CallbackContext ctx) => subs.onTriggerPerformed?.Invoke(InputTriggers.RT, 0f);

        }

        void setupPad()
        {
            // pad buttons

            action(MappingActions.buttonStart).performed += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(InputButtons.START, true);
            action(MappingActions.buttonStart).canceled += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(InputButtons.START, false);

            // le bouton du nord parmis les XYBA
            action(MappingActions.buttonNorth).performed += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(InputButtons.PAD_NORTH, true);
            action(MappingActions.buttonNorth).canceled += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(InputButtons.PAD_NORTH, false);

            // le bouton du sud parmis les XYBA
            action(MappingActions.buttonSouth).performed += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(InputButtons.PAD_SOUTH, true);
            action(MappingActions.buttonSouth).canceled += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(InputButtons.PAD_SOUTH, false);

            // le bouton de gauche parmis les XYBA
            action(MappingActions.buttonWest).performed += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(InputButtons.PAD_WEST, true);
            action(MappingActions.buttonWest).canceled += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(InputButtons.PAD_WEST, false);

            // le bouton de droite parmis les XYBA
            action(MappingActions.buttonEast).performed += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(InputButtons.PAD_EAST, true);
            action(MappingActions.buttonEast).canceled += (InputAction.CallbackContext ctx)
                => subs.onButtonPerformed?.Invoke(InputButtons.PAD_EAST, false);

        }

        void setupDpad()
        {
            action(MappingActions.dpadSouth).performed += (InputAction.CallbackContext ctx) => subs.onDPadPerformed?.Invoke(InputDPad.DPAD_SOUTH, true);
            action(MappingActions.dpadSouth).canceled += (InputAction.CallbackContext ctx) => subs.onDPadPerformed?.Invoke(InputDPad.DPAD_SOUTH, false);

            action(MappingActions.dpadWest).performed += (InputAction.CallbackContext ctx) => subs.onDPadPerformed?.Invoke(InputDPad.DPAD_WEST, true);
            action(MappingActions.dpadWest).canceled += (InputAction.CallbackContext ctx) => subs.onDPadPerformed?.Invoke(InputDPad.DPAD_WEST, false);

            action(MappingActions.dpadEast).performed += (InputAction.CallbackContext ctx) => subs.onDPadPerformed?.Invoke(InputDPad.DPAD_EAST, true);
            action(MappingActions.dpadEast).canceled += (InputAction.CallbackContext ctx) => subs.onDPadPerformed?.Invoke(InputDPad.DPAD_EAST, false);

            action(MappingActions.dpadNorth).performed += (InputAction.CallbackContext ctx) => subs.onDPadPerformed?.Invoke(InputDPad.DPAD_NORTH, true);
            action(MappingActions.dpadNorth).canceled += (InputAction.CallbackContext ctx) => subs.onDPadPerformed?.Invoke(InputDPad.DPAD_NORTH, false);

        }

        void setupJoysticks()
        {
            //Joysticks
            //left

            action(MappingActions.joyLeft).performed += (InputAction.CallbackContext ctx) =>
            {
                Vector2 _joy = ctx.ReadValue<Vector2>();

                Vector2 direction = get6D(_joy);
                if (direction.x != joyLeftDir.x || direction.y != joyLeftDir.y) // any changes ?
                {
                    log("LS.DIR motion ? " + direction);
                    subs.onJoystickDirection?.Invoke(InputJoystickSide.LEFT, direction);
                    joyLeftDir = direction; // buff
                }

                joyLeft = _joy;

                log("RAW motion ? " + joyLeft.x + " x " + joyLeft.y + " , " + joyLeft.sqrMagnitude);

                subs.onJoystickPerformed?.Invoke(InputJoystickSide.LEFT, joyLeft);
            };

            action(MappingActions.joyLeft).canceled += (InputAction.CallbackContext ctx) =>
            {
                log("RAW motion stopped");

                // reset buffs
                joyLeft = Vector2.zero;
                joyLeftDir = Vector2.zero;

                subs.onJoystickReleased?.Invoke(InputJoystickSide.LEFT);
            };

            //right

            action(MappingActions.joyRight).performed += (InputAction.CallbackContext ctx) =>
            {
                Vector2 _joy = ctx.ReadValue<Vector2>();

                Vector2 direction = get6D(_joy);
                if (direction.x != joyRightDir.x || direction.y != joyRightDir.y) // any changes ?
                {
                    log("RS.DIR motion ? " + direction);
                    subs.onJoystickDirection?.Invoke(InputJoystickSide.RIGHT, direction);
                    joyRightDir = direction; // buff
                }

                joyRight = _joy;

                log("RAW motion ? " + joyRight.x + " x " + joyRight.y + " , " + joyRight.sqrMagnitude);

                subs.onJoystickPerformed?.Invoke(InputJoystickSide.RIGHT, joyRight);
            };

            action(MappingActions.joyRight).canceled += (InputAction.CallbackContext ctx) =>
            {
                Vector2 joyRight = ctx.ReadValue<Vector2>();

                log("RAW motion stopped");

                subs.onJoystickReleased?.Invoke(InputJoystickSide.RIGHT);
            };

        }

        void log(string content) => GamepadVerbosity.sLog(GetType() + "&" + name + " >>> " + content, this);

        /// <summary>
        /// [X,Y] normalized ?
        /// 
        /// DIRECTION
        ///      0
        ///  -90   90
        ///     180
        /// </summary>
        static public Vector2 get6D(Vector2 joyRaw, float deadZone = 0.5f, bool verbose = false)
        {
            if (joyRaw.magnitude < deadZone) return Vector2.zero;


            Vector2 ret = Vector2.zero;

            float aSection = 360f / 8f;

            //Debug.Log("---");
            if (verbose) Debug.Log("?" + joyRaw);
            //Debug.Log("section?" + aSection);

            joyRaw = joyRaw.normalized;

            float hSection = aSection * 0.5f;

            //Vector2 origin = rotate(Vector2.right, hSection);

            // CW +
            // CCW -
            float angle = -Vector2.SignedAngle(joyRaw, Vector2.right);
            float sCenter;

            //ret.y = angle != 180f && angle != 0f ? 1f : 0f;
            int cnt = 0;

            float bAngle = angle;

            // -180,0
            if (angle < 0f)
            {
                while (bAngle < -hSection)
                {
                    bAngle += aSection;
                    cnt++;
                    //if (bAngle < -hSection) cnt++;
                }
            }
            else if (angle > 0f)
            {
                while (bAngle > hSection)
                {
                    bAngle -= aSection;
                    cnt++;
                    //if (bAngle > hSection) cnt++;
                }
            }

            sCenter = cnt * aSection * Mathf.Sign(angle);

            if (verbose) Debug.Log("angle ? " + angle + " , cnt?" + cnt + " , center?" + sCenter);

            sCenter *= Mathf.Deg2Rad;

            float c = Mathf.Cos(sCenter);
            float s = Mathf.Sin(sCenter);
            //if (Mathf.Abs(c) <= Mathf.Epsilon) c = 0f;
            //if (Mathf.Abs(s) <= Mathf.Epsilon) s = 0f;
            if (verbose) Debug.Log(c + "x" + s);
            joyRaw.x = Mathf.Round(c);
            joyRaw.y = Mathf.Round(s);

            return joyRaw;
        }

        public static Vector2 rotate(Vector2 v, float delta)
        {
            return new Vector2(
                v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
                v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
            );
        }

    }



}

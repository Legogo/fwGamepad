using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.inputeer
{
    using UnityEngine.InputSystem;
    
    public class ReactorDualStick : ReactorController
    {
        SubsDualStick subs;
        XboxMapping controls;

        public ReactorDualStick()
        { }

        public override SubsController setup()
        {
            subs = new SubsDualStick();

            controls = new XboxMapping();

            setupDpad();
            setupButtons();
            setupJoysticks();
            setupBumpers();
            setupGlobals();

            controls.Enable();

            return subs;
        }

        void setupBumpers()
        {

            // bumpers L/R

            controls.controller.bumperL.performed += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.BL, true);
            controls.controller.bumperL.canceled += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.BL, false);

            controls.controller.bumperR.performed += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.BR, true);
            controls.controller.bumperR.canceled += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.BR, false);

        }

        void setupGlobals()
        {

            //release

            controls.controller.pause.performed += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.START, true);
            controls.controller.pause.canceled += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.START, false);

        }

        void setupJoysticks()
        {

            //Joysticks
            //left
            controls.controller.joyLeft.performed += (InputAction.CallbackContext ctx) =>
            {
                Vector2 joy = ctx.ReadValue<Vector2>();
                //if (joy.sqrMagnitude < Mathf.Epsilon) return;
                subs.onJoystickPerformed?.Invoke(InputJoystickSide.LEFT, joy);
            };

            controls.controller.joyLeft.canceled += (InputAction.CallbackContext ctx) =>
            {
                subs.onJoystickReleased?.Invoke(InputJoystickSide.LEFT);
            };

            //right

            controls.controller.joyRight.performed += (InputAction.CallbackContext ctx) =>
            {
                Vector2 joy = ctx.ReadValue<Vector2>();
                //if (joy.sqrMagnitude < Mathf.Epsilon) return;
                subs.onJoystickPerformed?.Invoke(InputJoystickSide.RIGHT, joy);
            };

            controls.controller.joyRight.canceled += (InputAction.CallbackContext ctx) =>
            {
                subs.onJoystickReleased?.Invoke(InputJoystickSide.RIGHT);
            };

        }


        void setupButtons()
        {

            // le bouton du nord parmis les XYBA
            controls.controller.buttonNorth.performed += (InputAction.CallbackContext ctx) =>
            {
                //Debug.Log("performed:north:true");
                subs.onButtonPerformed?.Invoke(InputButtons.PAD_NORTH, true);
            };

            controls.controller.buttonNorth.canceled += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.PAD_NORTH, false);

            // le bouton du sud parmis les XYBA
            controls.controller.buttonSouth.performed += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.PAD_SOUTH, true);
            controls.controller.buttonSouth.canceled += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.PAD_SOUTH, false);

            // le bouton de gauche parmis les XYBA
            controls.controller.buttonWest.performed += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.PAD_WEST, true);
            controls.controller.buttonWest.canceled += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.PAD_WEST, false);

            // le bouton de droite parmis les XYBA
            controls.controller.buttonEast.performed += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.PAD_EAST, true);
            controls.controller.buttonEast.canceled += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.PAD_EAST, false);

            // le bouton start
            controls.controller.buttonStart.performed += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.START, true);
            controls.controller.buttonStart.canceled += (InputAction.CallbackContext ctx) => subs.onButtonPerformed?.Invoke(InputButtons.START, false);
        }

        void setupDpad()
        {

            controls.controller.dPadSouth.performed += (InputAction.CallbackContext ctx) => subs.onDPadPerformed?.Invoke(InputDPad.DPAD_SOUTH, true);
            controls.controller.dPadSouth.canceled += (InputAction.CallbackContext ctx) => subs.onDPadPerformed?.Invoke(InputDPad.DPAD_SOUTH, false);

            controls.controller.dPadWest.performed += (InputAction.CallbackContext ctx) => subs.onDPadPerformed?.Invoke(InputDPad.DPAD_WEST, true);
            controls.controller.dPadWest.canceled += (InputAction.CallbackContext ctx) => subs.onDPadPerformed?.Invoke(InputDPad.DPAD_WEST, false);

            controls.controller.dPadEast.performed += (InputAction.CallbackContext ctx) => subs.onDPadPerformed?.Invoke(InputDPad.DPAD_EAST, true);
            controls.controller.dPadEast.canceled += (InputAction.CallbackContext ctx) => subs.onDPadPerformed?.Invoke(InputDPad.DPAD_EAST, false);

            controls.controller.dPadNorth.performed += (InputAction.CallbackContext ctx) => subs.onDPadPerformed?.Invoke(InputDPad.DPAD_NORTH, true);
            controls.controller.dPadNorth.canceled += (InputAction.CallbackContext ctx) => subs.onDPadPerformed?.Invoke(InputDPad.DPAD_NORTH, false);

        }

    }

}

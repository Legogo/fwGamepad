using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.gamepad
{
    using blueprint;

    /// <summary>
    /// this is a wrapper to which you give a target
    /// and that target will receive inputs
    /// </summary>
    public class GamepadWatcher : MonoBehaviour
    {
        [Header("refs")]

        public InputSysGamepad playerInputSys; // the bridge that will sub to InputSystem (specific to each player)

        /// <summary>
        /// available for kappas to sub to this controller and react
        /// this localSubs will transfert input to iTarget (if compat)
        /// </summary>
        GamepadSubber localSubs;

        /// <summary>
        /// provide some bridge to react to this controller
        /// ex : menus will sub to this to react to controller activity
        /// </summary>
        public GamepadSubber subber => localSubs;

        /// <summary>
        /// aka listener
        /// </summary>
        ISelectable iTarget;

        [Header("read only")]
        /// <summary>
        /// READ ONLY
        /// a virtualisation of the controller state
        /// </summary>
        [SerializeField]
        protected BlueprintXbox controllerState;

        System.Guid _guid;
        public int guid
        {
            get
            {
                if (_guid == null) _guid = System.Guid.NewGuid();
                return _guid.GetHashCode();
            }
        }

        public int pluggedIndex
        {
            get
            {
                return GamepadsWatcher.instance.getControllerIndex(this);
            }
        }

        //public bool isPlugged => pluggedIndex > -1;
        public bool isPlugged
        {
            get
            {
                if (playerInputSys.sysDevice == null)
                    return false;

                return playerInputSys.sysDevice.enabled;
            }
        }

        public bool isPrimary => playerInputSys.controller == InputSysGamepad.InputController.xbox_pone;

        private void Awake()
        {
            controllerState = new BlueprintXbox();
        }

        private void Start()
        {
            // must happened after init of inputsys
            // create a local "subs" for this player
            localSubs = new GamepadSubber(this);

            localSubs.subJoysticks(true, onJoystick, onJoystickRelease);
            localSubs.subTriggers(true, onTrigger);
            localSubs.subButtons(true, onButton);
            localSubs.subDPad(true, onDPad);

            Debug.Log(name + " is not setup to react to inputs");
        }

        public void feedTarget(ISelectable target)
        {
            if (iTarget != null)
            {
                releaseTarget(target);
            }

            iTarget = target;

            Debug.Log(name + " <b>HAS</b> control   : " + iTarget, iTarget as Object);

            iTarget.onSelected();
        }

        /// <summary>
        /// true : actually released it
        /// </summary>
        public bool releaseTarget(ISelectable target)
        {
            // not concerned
            if (target != iTarget)
            {
                return false;
            }

            if (iTarget == null)
                return false;

            iTarget.onUnselected();

            Debug.Log(name + " <b>LOSE</b> control  : <b>" + iTarget + "</b>", iTarget as Object);

            iTarget = null;

            return true;
        }

        private void OnDestroy()
        {
            localSubs.subJoysticks(false, onJoystick, onJoystickRelease);
            localSubs.subTriggers(false, onTrigger);
            localSubs.subButtons(false, onButton);
            localSubs.subDPad(false, onDPad);
        }

        public bool hasSelection() => iTarget != null;

        void onTrigger(InputSysGamepad controller, InputTriggers side, float value)
        {
            controllerState.mimic(side, value);

            var trig = iTarget as ISelectableTrigger;
            if (trig != null)
            {
                switch (side)
                {
                    case InputTriggers.LT:
                        trig.onTrigLeft(this, value);
                        break;
                    case InputTriggers.RT:
                        trig.onTrigRight(this, value);
                        break;
                }
            }
        }

        void onJoystickRelease(InputSysGamepad controller, InputJoystickSide side) => onJoystick(controller, side, Vector2.zero);

        void onJoystick(InputSysGamepad controller, InputJoystickSide side, Vector2 value)
        {
            controllerState.mimic(side, value);

            //Debug.Log(side + " = " + value, this);

            var candidate = iTarget as ISelectableJoy;

            //Debug.Log(iTarget);
            //Debug.Log(candidate);

            if (candidate != null)
            {
                switch (side)
                {
                    case InputJoystickSide.LEFT:
                        candidate.onJoyLeft(this, value);
                        break;
                    case InputJoystickSide.RIGHT:
                        candidate.onJoyRight(this, value);
                        break;
                }
            }
        }

        private void onButton(InputSysGamepad controller, InputButtons type, bool status)
        {
            controllerState?.mimic(type, status);

            var candidate = iTarget as ISelectableButton;
            if (candidate != null)
            {
                candidate.onButton(this, type, status);
            }
        }

        private void onDPad(InputSysGamepad controller, InputDPad type, bool status)
        {
            controllerState.mimic(type, status);

            var candidate = iTarget as ISelectableDpad;
            if (candidate != null)
            {
                candidate.onDPad(this, type, status);
            }
        }

    }
}

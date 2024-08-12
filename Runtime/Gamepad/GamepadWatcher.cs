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
        /// aka listener
        /// </summary>
        ISelectable iTarget;

        [Header("read only")]
        /// <summary>
        /// READ ONLY
        /// a virtualisation of the controller state
        /// </summary>
        [SerializeField]
        protected BlueprintXbox controllerState; // != null after init

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

        public bool isPrimary => playerInputSys.controllerType == InputSysGamepad.InputController.gamepad_0;

        private void Start()
        {
            if (playerInputSys != null)
            {
                setupCallbacks();
            }
        }

        void setupCallbacks()
        {
            // this field is serialize, this should never happen but jic
            if(controllerState == null)
            {
                controllerState = new BlueprintXbox();
            }
            
            
            InputSubsCallbacks subs = playerInputSys.subs;
            subs.onJoystickDirection += onJoyDirection;
            subs.onJoystickPerformed += onJoystick;
            subs.onJoystickReleased += onJoystickRelease;
            subs.onTriggerPerformed += onTrigger;
            subs.onButtonPerformed += onButton;
            subs.onDPadPerformed += onDPad;

            Debug.Log("watcher:ON");
        }

        public void clearCallbacks()
        {
            InputSubsCallbacks subs = playerInputSys.subs;
            subs.onJoystickPerformed -= onJoystick;
            subs.onJoystickReleased -= onJoystickRelease;
            subs.onTriggerPerformed -= onTrigger;
            subs.onButtonPerformed -= onButton;
            subs.onDPadPerformed -= onDPad;
        }

        public bool hasSelection() => iTarget != null;

        public void select(ISelectable target)
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

        void onJoyDirection(InputJoystickSide side, Vector2 value)
        {
            var c = iTarget as ISelectableJoyDirection;
            if (c == null) return;
            switch (side)
            {
                case InputJoystickSide.LEFT:
                    c.onJoyLeftDir(value);
                    break;
                case InputJoystickSide.RIGHT:
                    c.onJoyRightDir(value);
                    break;
            }
        }



        void onTrigger(InputTriggers side, float value)
        {
            controllerState.mimic(side, value);

            var trig = iTarget as ISelectableTrigger;
            if (trig != null)
            {
                switch (side)
                {
                    case InputTriggers.LT:
                        trig.onTrigLeft(value);
                        break;
                    case InputTriggers.RT:
                        trig.onTrigRight(value);
                        break;
                }
            }
        }

        void onJoystickRelease(InputJoystickSide side) => onJoystick(side, Vector2.zero);

        void onJoystick(InputJoystickSide side, Vector2 value)
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
                        candidate.onJoyLeft(value);
                        break;
                    case InputJoystickSide.RIGHT:
                        candidate.onJoyRight(value);
                        break;
                }
            }
        }

        private void onButton(InputButtons type, bool status)
        {
            controllerState?.mimic(type, status);

            var candidate = iTarget as ISelectableButton;
            if (candidate != null)
            {
                candidate.onButton(type, status);
            }
        }

        private void onDPad(InputDPad type, bool status)
        {
            controllerState.mimic(type, status);

            var candidate = iTarget as ISelectableDpad;
            if (candidate != null)
            {
                candidate.onDPad(type, status);
            }
        }

    }
}

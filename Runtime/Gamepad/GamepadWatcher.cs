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
        WatcherInput<ISelectable> targets; // normal object
        WatcherInput<ISelectableAbsorb> absorbs; // object that can absorb

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

                // keyboard doesn't support enabled state
                return true;
                //return playerInputSys.sysDevice.enabled;
            }
        }

        [ContextMenu("check plugged ?")]
        void cmLogPlugged()
        {
            Debug.Log(name + " ? " + isPlugged);
            Debug.Log(playerInputSys.sysDevice);
        }

        public bool isPrimary => playerInputSys.controllerType == InputSysGamepad.InputController.gamepad_0;

        private void Awake()
        {
            create();
        }

        virtual protected void create() { }

        private void Start()
        {
            targets = new WatcherInput<ISelectable>();
            absorbs = new WatcherInput<ISelectableAbsorb>();

            if (playerInputSys != null)
            {
                setupCallbacks();
            }

            setup();
        }

        virtual protected void setup()
        { }

        void setupCallbacks()
        {
            InputSubsCallbacks subs = playerInputSys.subs;

            subs.onJoystickPerformed += onJoystick;
            subs.onJoystickReleased += onJoystickRelease;
            subs.onJoystickDirection += onJoyDirection;
            subs.onJoystickPunchDirection += onJoyPunch;

            subs.onTriggerPerformed += onTrigger;
            subs.onButtonPerformed += onButton;
            subs.onDPadPerformed += onDPad;

            //Debug.Log("watcher:ON");
        }

        public void clearCallbacks()
        {
            InputSubsCallbacks subs = playerInputSys.subs;

            subs.onJoystickDirection -= onJoyDirection;
            subs.onJoystickPerformed -= onJoystick;

            subs.onJoystickReleased -= onJoystickRelease;
            subs.onTriggerPerformed -= onTrigger;
            subs.onButtonPerformed -= onButton;
            subs.onDPadPerformed -= onDPad;
        }

        public bool hasSelection() => targets.hasSomething();

        /// <summary>
        /// force all selected element to reset inputs
        /// 
        /// force tiny to reset joystick state (ie : when input block starts)
        /// </summary>
        public void bubbleNeutral()
        {
            // force a neutral (for motion)
            onJoystickRelease(InputJoystickSide.LEFT);

            // force a release
            onButton(InputButtons.PAD_SOUTH, false);
        }

        public void queueReplace(ISelectable target)
        {
            targets.deselectAll();
            queueSelection(target);
        }

        /// <summary>
        /// par default le queue selection va cancel les inputs du previous
        /// </summary>
        public void queueSelection(ISelectable target, bool forceInputRelease = true)
        {
            if (target == null)
            {
                Debug.LogError("don't use queue for clearing, use deselect");
                return;
            }

            if (forceInputRelease)
            {
                bubbleNeutral();
            }

            if(target is ISelectableAbsorb)
            {
                if(absorbs.queueSelection(target as ISelectableAbsorb))
                {
                    reactQueue(target);
                }
            }
            else
            {
                if (targets.queueSelection(target))
                {
                    reactQueue(target);
                }
            }

        }

        virtual protected void reactQueue(ISelectable target)
        { }

        public void unqueueSelection(ISelectable target)
        {
            if (target == null)
            {
                Debug.LogWarning("nothing given to unqeue ??");
                return;
            }

            if(target is ISelectableAbsorb)
            {
                if (absorbs.unqueueSelection(target as ISelectableAbsorb))
                {
                    reactUnqueue(target);
                }
            }
            else
            {
                if (targets.unqueueSelection(target))
                {
                    reactUnqueue(target);
                }
            }
        }

        /// <summary>
        /// from targets[]
        /// </summary>
        virtual protected void reactUnqueue(ISelectable target)
        { }

        void onJoystickRelease(InputJoystickSide side)
        {
            // feed selection(s)
            onJoystick(side, Vector2.zero);

            // no need, managed by sys gamepad
            //onJoyDirection(side, Vector2.zero); // joy release
        }

        void onJoystick(InputJoystickSide side, Vector2 value)
        {
            if (absorbs.onJoystick(side, value))
            {
                return;
            }

            targets.onJoystick(side, value);
        }

        void onJoyDirection(InputJoystickSide side, Vector2 value)
        {
            if (absorbs.onJoystickDirection(side, value))
            {
                return;
            }

            targets.onJoystickDirection(side, value);
        }

        void onJoyPunch(InputJoystickSide side, Vector2 value)
        {
            if (absorbs.onJoystickPunch(side, value)) return;
            targets.onJoystickPunch(side, value);
        }

        void onTrigger(InputTriggers side, float value)
        {
            if (absorbs.onTrigger(side, value))
            {
                return;
            }

            targets.onTrigger(side, value);
        }

        private void onButton(InputButtons type, bool status)
        {
            if (absorbs.onButton(type, status))
            {
                return;
            }

            targets.onButton(type, status);
        }

        private void onDPad(InputDPad type, bool status)
        {
            if (absorbs.onDPad(type, status))
            {
                return;
            }

            targets.onDPad(type, status);
        }

        [ContextMenu("log")]
        void cmLog()
        {
            Debug.Log(name, this);
            if (targets != null) Debug.Log(targets.stringify());
            if (absorbs != null) Debug.Log(absorbs.stringify());
        }
    }
}

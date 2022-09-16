using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace fwp.inputeer
{

    /// <summary>
    /// meant to manage plug/unplug of controllers
    /// 
    /// 2012.12.23
    /// Les ids des manettes correspondent à l'ordre de branchement
    /// Il faut attendre un input de la manette pour être certains de l'id
    /// http://forum.unity3d.com/threads/114993-Joystick-detection-and-direct-axis-detection
    ///
    /// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/HowDoI.html#find-all-connected-gamepads
    /// </summary>
    public class ControllerManager
    {
        enum ControllerManagerInputSystem
        {
            LEGACY, INPUT_SYSTEM
        };

        ControllerManagerSys sysBridge;

        public int connectedCount
        {
            get
            {
                return sysBridge.countConnected();
            }
        }

        public int maxControllers
        {
            get
            {
                return sysBridge.countSystemMaxControllers();
            }
        }

        protected List<GamepadWatcher> watchers = new List<GamepadWatcher>();

        public System.Action<GamepadWatcher> onWatcherStateChanged;

        public ControllerManager()
        {
            _mgr = this;

            sysBridge = new ControllerManagerSysInput();
            sysBridge.onControllerCountChanged += solveWatchers;

        }

        /// <summary>
        /// when ready trigger a controller check
        /// </summary>
        public void boot()
        {
            Debug.Log("~ControllerManager:boot()");

            sysBridge.solveChanges();
        }

        virtual public void solveWatchers()
        {
            var signs = sysBridge.signatures;

            while(watchers.Count < signs.Count)
            {
                watchers.Add(new GamepadWatcher(watchers.Count));
            }

            for (int i = 0; i < signs.Count; i++)
            {
                if (signs[i].state != watchers[i].plugState)
                {
                    watchers[i].plugState = signs[i].state;

                    if(watchers[i].plugState == GamepadWatcher.WatcherState.PLUGGED)
                        onWatcherStateChanged?.Invoke(watchers[i]);
                    else if(watchers[i].plugState == GamepadWatcher.WatcherState.UNPLUGGED)
                        onWatcherStateChanged?.Invoke(watchers[i]);
                }
            }
        }

        virtual public GamepadWatcher getController(int idx)
        {
            if (!watchers[idx].isConnected()) return null;
            return watchers[idx];
        }

        public GamepadWatcher[] getControllers() => watchers.ToArray();

        public GamepadWatcher getControllerById(int idx) => getController(idx);

        static protected ControllerManager _mgr;
        static public ControllerManager create() => _mgr = new ControllerManager();
        static public ControllerManager mgr
        {
            get
            {
                if (_mgr == null) _mgr = new ControllerManager();
                return _mgr;
            }
        }
    }

}

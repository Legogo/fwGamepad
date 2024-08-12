using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Collections.ObjectModel;

namespace fwp.gamepad
{
    /// <summary>
    /// bridge to provide acces to any controller
    /// </summary>
    public class GamepadsWatcher : MonoBehaviour
    {
        static GamepadsWatcher _instance;
        static public GamepadsWatcher instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                var obj = Resources.Load("System/[INPUTS]");
                var go = GameObject.Instantiate(obj) as GameObject;
                _instance = go.GetComponent<GamepadsWatcher>();
                return _instance;
            }
        }

        List<GamepadWatcher> controllers = new List<GamepadWatcher>();

        public ReadOnlyCollection<GamepadWatcher> pluggedControllers
        {
            get
            {
                return new ReadOnlyCollection<GamepadWatcher>(controllers);
            }
        }

        /// <summary>
        /// int is controller UID
        /// </summary>
        public Action<int> onControllerPlugged;
        public Action<int> onControllerUnplugged;

        public bool hasOnlyOne
        {
            get
            {
                return countPlugged() == 1;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Debug.LogError("another input element ?", this);
                return;
            }

            DontDestroyOnLoad(this);

            controllers.AddRange(GetComponentsInChildren<GamepadWatcher>());
            Debug.Assert(controllers.Count > 0, "no controllers ?");
        }

        private void Start()
        {
            fetchControllers();
        }

        public void fetchControllers()
        {
            controllers.Clear();
            var cs = GetComponentsInChildren<GamepadWatcher>();
            foreach (var c in cs)
            {
                if (c.isPlugged)
                {
                    controllers.Add(c);
                }
            }

            Debug.Log(name + " has fetched x" + controllers.Count + " controllers");
        }

        public int countPlugged()
        {
            int cnt = 0;
            foreach (var c in controllers)
            {
                if (c.isPlugged) cnt++;
            }
            return cnt;
        }

        public int getControllerIndex(GamepadWatcher controller)
        {
            for (int i = 0; i < controllers.Count; i++)
            {
                if (controllers[i] == controller)
                    return i;
            }
            return -1;
        }

        public GamepadWatcher getPlayerController(InputSysGamepad.InputController controller)
        {
            foreach (var c in controllers)
            {
                if (c.playerInputSys.controllerType == controller)
                {
                    return c;
                }
            }

            Debug.LogError("no controller " + controller);

            return null;
        }
    }


}

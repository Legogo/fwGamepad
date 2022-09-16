using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace fwp.inputeer
{
    /// <summary>
    /// meant to provide data from input system
    /// </summary>
    abstract public class ControllerManagerSys
    {
        protected int _cache_count_plugged = 0;
        
        public List<ControllerSignature> signatures = new List<ControllerSignature>();

        public Action onControllerCountChanged;

        public ControllerManagerSys()
        { }

        abstract protected int countSystemConnected();
        virtual public int countSystemMaxControllers()
        {
            return 4;
        }

        public int countConnected()
        {
            return _cache_count_plugged;
        }

        /// <summary>
        /// check for changes
        /// updates count plugged
        /// </summary>
        public bool solveChanges()
        {
            int newCount = solveSignatures(countSystemConnected());

            if(newCount != _cache_count_plugged)
            {
                onControllerCountChanged?.Invoke();
            }

            bool changed = newCount != _cache_count_plugged;
            
            _cache_count_plugged = newCount;

            return changed;
        }

        abstract protected int solveSignatures(int sysCount);
    }

    public struct ControllerSignature
    {
        public int index;
        public GamepadWatcher.WatcherState state;
    }

}
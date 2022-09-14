using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.inputeer
{
    /// <summary>
    /// core class to store action data
    /// </summary>
    abstract public class InputAction
    {
        public string description;
        public bool? checkState; // false = keydown ; true = key up
        bool _buffState;

        bool _raw = false; // actual raw state
        bool _acted = false; // this frame (must have checkState)

        public System.Action<bool> onActionStateChange;
        public System.Action onAction;

        public InputAction(bool? tarState, string description)
        {
            this.checkState = tarState;
            this.description = description;
        }

        abstract protected bool actionState();

        public bool justActed() => _acted;
        public bool getRaw() => _raw;

        public void update()
        {
            _acted = false;

            _raw = actionState();

            if (_buffState != _raw)
            {
                _buffState = _raw;

                onActionStateChange?.Invoke(_raw);

                if (checkState != null)
                {
                    if (_raw && _raw == checkState.Value)
                    {
                        _acted = true;
                        onAction?.Invoke();
                    }
                }
            }
        }

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.inputeer
{

    /// <summary>
    /// key wrapper
    /// </summary>
    public class InputActorKey : InputActor
    {
        public KeyCode[] tarKeys;

        public InputActorKey(KeyCode key, bool? tarState, string descr) : base(tarState, descr)
        {
            tarKeys = new KeyCode[] { key };
        }
        public InputActorKey(KeyCode[] keys, bool? tarState, string descr) : base(tarState, descr)
        {
            tarKeys = keys;
        }

        protected override bool actionState()
        {
            for (int i = 0; i < tarKeys.Length; i++)
            {
                if (Input.GetKey(tarKeys[i])) return true;
            }
            return false;
        }
    }

}
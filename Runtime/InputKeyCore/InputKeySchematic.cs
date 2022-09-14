using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Common base class for all InputKeyBridge bridge logic
/// 
/// container of multiple keys
/// 
/// </summary>

namespace fwp.inputeer
{
    abstract public class InputKeySchematic
    {
        protected List<InputActionKey> all = new List<InputActionKey>();

        public InputKeySchematic()
        { }

        protected void assign(InputActionKey[] keys)
        {
            all.AddRange(keys);
        }

        public void update()
        {
            for (int i = 0; i < all.Count; i++)
            {
                all[i].update();
            }
        }
    }

}
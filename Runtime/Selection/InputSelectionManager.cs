using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.inputeer
{
    public class InputSelectionManager
    {
        static public InputSelectionManager manager;

        List<HelperInputObject> selection = new List<HelperInputObject>();

        public InputSelectionManager()
        {
            manager = this;
        }

        public void add(HelperInputObject obj)
        {
            selection.Add(obj);

            //Debug.Log("added : " + io.owner.name);
        }

        public void remove(HelperInputObject obj)
        {
            if (selection.IndexOf(obj) < 0) return;

            selection.Remove(obj);
            //Debug.Log("removed : " + io.owner.name);
        }

        public HelperInputObject getSelection()
        {
            if (selection.Count <= 0) return null;
            if (selection[0] == null) return null;

            return selection[0];
        }

        public bool hasSelection()
        {
            return selection.Count > 0;
        }

        public string toString()
        {
            string ct = "[SELECTION]";
            for (int i = 0; i < selection.Count; i++)
            {
                ct += "\n   L #" + i + " -> " + selection[i];
            }
            return ct;
        }
    }

}

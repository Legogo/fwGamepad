using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.inputeer
{
    abstract public class ReactorController
    {
        /// <summary>
        /// creates whatever is needed to perform inputs
        /// and returns the bridge object
        /// </summary>
        abstract public SubsController setup();
    }

}

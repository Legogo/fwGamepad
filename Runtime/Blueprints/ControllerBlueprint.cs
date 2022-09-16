using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ControllerBlueprint
{
    public bool pressedThisFrame(int buttonUID);
    public bool releasedThisFrame(int buttonUID);
}

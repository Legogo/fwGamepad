using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BlueprintDualSticks : BlueprintSNES
{
    public Vector2 rawLeftJoystick();
    public Vector2 rawRightJoystick();

    public float rawTriggerLeft();
    public float rawTriggerRight();

    public bool rawDpadNorth();
    public bool rawDpadSouth();
    public bool rawDpadEast();
    public bool rawDpadWest();
}

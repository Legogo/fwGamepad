using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BlueprintSNES : BlueprintNES
{
    public bool rawBumperLeft();
    public bool rawBumperRight();

    public bool rawButtonWest();
    public bool rawButtonNorth();
}

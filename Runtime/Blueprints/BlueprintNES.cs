using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BlueprintNES : ControllerBlueprint
{

    public bool rawButtonStart();
    public bool rawButtonBack();

    public bool rawButtonEast();
    public bool rawButtonSouth();

}

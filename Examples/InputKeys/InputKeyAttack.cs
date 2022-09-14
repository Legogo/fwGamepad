using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.inputeer
{
  public class InputKeyAttack : InputKeySchematic
    {
    public bool pressed_attack()
    {
      return Input.GetKeyDown(KeyCode.LeftShift);
    }

  }
}
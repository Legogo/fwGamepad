using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace inputeer
{
  public class InputKeyAim : InputKeySchematic
  {

    public bool pressed_aim()
    {
      return Input.GetKeyDown(KeyCode.A);
    }
    public bool released_aim()
    {
      return Input.GetKeyUp(KeyCode.A);
    }

    /* to implem with arrow keys ?*/
    public Vector2 getDirectionVector() { return Vector2.zero; }
  }
}
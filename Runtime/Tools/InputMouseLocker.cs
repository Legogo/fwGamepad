using UnityEngine;
using System.Collections;

/// <summary>
/// 2018-10-05
/// This is meant to facilitate de capture / release of the mouse cursor
/// Just call lockMouse everyframe (or whenever useful) by giving the current state of locking you need
/// </summary>

namespace inputeer
{
  public class InputMouseLocker
  {
    static public bool isLocked()
    {
      return !Cursor.visible;
    }

    static public void lockMouse(bool lockState)
    {
      bool changed = false;

      if (isLocked() && !lockState)
      {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        changed = true;
        //Debug.Log("<InputFreeMouse>\tswitched to none");
      }
      else if (!isLocked() && lockState)
      {
        //Cursor.lockState = CursorLockMode.Confined;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        changed = true;
        //Debug.Log("<InputFreeMouse>\tswitched to locked");
      }

      if (changed)
      {
        Debug.Log("<InputFreeMouse> is now locked ? " + lockState + " (cursor.lockState = " + Cursor.lockState + ", Cursor.visible = " + Cursor.visible + ")");
      }
    }

  }
}
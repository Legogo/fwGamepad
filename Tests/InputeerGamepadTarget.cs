using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using fwp.inputeer;

public class InputeerGamepadTarget : MonoBehaviour, GamepadDualStick
{
    public void onSelected()
    {
        Debug.Log($"{name} selected", transform);
    }

    public void onUnselected()
    {
        Debug.Log($"{name} un-selected", transform);
    }

    public void onStick(InputJoystickSide side, Vector2 vec)
    {
        Debug.Log(side);
        Debug.Log(vec+" , "+vec.sqrMagnitude);
    }

}

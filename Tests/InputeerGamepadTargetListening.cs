using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using fwp.inputeer;

public class InputeerGamepadTargetListening : MonoBehaviour, GamepadDualStick
{

    SubsDualStick _looker;

    public void onSelected(GamepadWatcher watcher)
    {
        Debug.Log($"{name} selected", transform);
        _looker = watcher.controllerState as SubsDualStick;
    }

    public void onUnselected()
    {
        Debug.Log($"{name} un-selected", transform);
        _looker = null;
    }

    public void onStick(InputJoystickSide side, Vector2 vec)
    {
        Debug.Log(side);
        Debug.Log(vec + " , " + vec.sqrMagnitude);

        //transform.Translate(vec * Time.deltaTime * 10f);
    }

}

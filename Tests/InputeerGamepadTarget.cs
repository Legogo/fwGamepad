using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using fwp.inputeer;

public class InputeerGamepadTarget : MonoBehaviour, GamepadSelection
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

    void Update()
    {

        if(_looker != null)
        {
            Vector2 _joy = _looker.getJoystickState(InputJoystickSide.LEFT);

            transform.Translate(_joy * Time.deltaTime * 20f);

            if(_looker.isButtonPressed(InputButtons.PAD_EAST))
            {
                //Debug.Log("EAST");
            }
        }

    }

}

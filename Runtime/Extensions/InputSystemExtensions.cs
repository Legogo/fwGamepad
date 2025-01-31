using UnityEngine;

using UnityEngine.InputSystem;

static public class InputSystemExtensions
{

    const string device_keyboard = "Keyboard";

    static public bool IsKeyboard(this InputAction.CallbackContext device)
    {
        return device.control.device.displayName == device_keyboard;
    }
}

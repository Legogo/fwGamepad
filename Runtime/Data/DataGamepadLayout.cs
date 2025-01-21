using UnityEngine;

namespace fwp.gamepad.layout
{
    abstract public class DataGamepadLayout : ScriptableObject
    {
        [Header("output (readonly)")]

        [SerializeField]
        LayoutInputJoystick[] joysticks;

        [SerializeField]
        LayoutInputAction[] actions;      // anything press/release

        [SerializeField]
        LayoutInputDpad[] dpads;    // dpad

        abstract public LayoutInputJoystick[] getSticks();
        abstract public LayoutInputAction[] getActions();
        abstract public LayoutInputDpad[] getDpads();
    }

    [System.Serializable]
    public class LayoutInputJoystick : LayoutIcon
    {
        public InputJoystickSide input;
    }

    [System.Serializable]
    public class LayoutInputAction : LayoutIcon
    {
        public InputButtons input;
    }

    [System.Serializable]
    public class LayoutInputDpad : LayoutIcon
    {
        public InputDPad input;
    }

    [System.Serializable]
    public class LayoutIcon
    {
        public Sprite icon;
    }

    [System.Serializable]
    public class LayoutIconLabel
    {
        public Sprite icon;
    }

    [System.Serializable]
    public class LayoutLabel
    {
        public string label;
    }

}

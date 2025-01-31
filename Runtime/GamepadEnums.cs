namespace fwp.gamepad
{
    public enum InputType
    {
        JOYL, JOYR,     // [X,Y]
        ACTIONS,        // [press,release]
        TRIGGER,        // [0,1]
        DPAD,           // directional, NESW
    }

    /// <summary>
    /// anything press/release
    /// </summary>
    public enum InputButtons
    {
        NONE,
        PAD_NORTH, PAD_EAST, PAD_SOUTH, PAD_WEST, // actions
        START, RETURN,
        BL, BR // bumper aka shoulder (top)
    }

    /// <summary>
    /// NESW must match PadSectionDirection
    /// </summary>
    public enum InputDPad
    {
        NONE,
        DPAD_NORTH, DPAD_EAST, DPAD_SOUTH, DPAD_WEST,
    }

    /// <summary>
    /// [0,1]
    /// </summary>
    public enum InputTriggers
    {
        NONE,
        LT, RT,
    }

    public enum InputJoystickSide
    {
        NONE,
        LEFT, RIGHT,
    }

    /// <summary>
    /// ↑ → ↓ ←
    /// </summary>
    public enum PadDirection
    {
        NONE, 
        NORTH, EAST, SOUTH, WEST,
    };
}

namespace fwp.inputeer
{
    /// <summary>
    /// NESW must match PadSectionDirection
    /// </summary>
    public enum InputButtons
    {
        NONE,
        PAD_NORTH, PAD_EAST, PAD_SOUTH, PAD_WEST, // ABXY
        START, RETURN,
        BL, BR // bumper (top)
    }

    /// <summary>
    /// NESW must match PadSectionDirection
    /// </summary>
    public enum InputDPad
    {
        NONE,
        DPAD_NORTH, DPAD_EAST, DPAD_SOUTH, DPAD_WEST //DPad
    }

    public enum InputTriggers
    {
        LT, RT
    }

    public enum InputJoystickSide
    {
        LEFT, RIGHT
    }

    public enum InputType
    {
        JOYL, JOYR,
        BUTTON,
        TRIGGER,
        DPAD
    }

    public enum PadSection
    {
        none, buttons, dpads,
    };

    /// <summary>
    /// ↑ → ↓ ←
    /// </summary>
    public enum PadSectionDirection
    {
        none, north, east, south, west,
    };
}

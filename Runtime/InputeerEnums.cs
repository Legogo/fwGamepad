namespace fwp.inputeer
{
    public enum InputType
    {
        JOYL, JOYR,
        BUTTON,
        TRIGGER,
        DPAD
    }

    public enum InputTriggers
    {
        LEFT, RIGHT
    }

    public enum InputDPad
    {
        DPAD_WEST, DPAD_EAST, DPAD_NORTH, DPAD_SOUTH //DPad
    }

    public enum InputJoystickSide
    {
        LEFT, RIGHT
    }

    public enum InputButtons
    {
        PAD_WEST, PAD_EAST, PAD_NORTH, PAD_SOUTH, // ABXY
        START, RETURN,
        BL, BR // bumper (top)
    }

}

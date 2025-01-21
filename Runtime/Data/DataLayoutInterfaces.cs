namespace fwp.gamepad.layout
{
    public interface LayoutJoystickDual
    {
        public LayoutInputJoystick getLeftStick();
        public LayoutInputJoystick getRightStick();
    }

    /// <summary>
    /// YBAX
    /// </summary>
    public interface LayoutJoystickActionsNESW
    {
        public LayoutInputAction getActionNorth();
        public LayoutInputAction getActionEast();
        public LayoutInputAction getActionSouth();
        public LayoutInputAction getActionWest();
    }

    public interface LayoutJoystickActionsMenus
    {
        public LayoutInputAction getActionStart();
        public LayoutInputAction getActionSelect();
    }

    public interface LayoutJoystickDirectionalPad
    {
        public LayoutInputDpad getDpadNorth();
        public LayoutInputDpad getDpadEast();
        public LayoutInputDpad getDpadSouth();
        public LayoutInputDpad getDpadWest();
    }
}
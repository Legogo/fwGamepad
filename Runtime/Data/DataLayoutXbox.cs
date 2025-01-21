using UnityEngine;

namespace fwp.gamepad.layout
{
    /// <summary>
    /// any controller with 2x sticks
    /// directional pad
    /// action buttons
    /// </summary>

    [CreateAssetMenu(menuName = "gamepad/layout/xbox", order = 100)]
    public class DataLayoutXbox : DataGamepadLayout,
        LayoutJoystickDual, LayoutJoystickActionsNESW,
        LayoutJoystickActionsMenus, LayoutJoystickDirectionalPad
    {
        [Header("dual sticks")]

        public LayoutIcon joystickLeft;
        public LayoutIcon joystickRight;

        public LayoutInputJoystick getLeftStick() => new LayoutInputJoystick()
        {
            input = InputJoystickSide.LEFT,
            icon = joystickLeft.icon
        };

        public LayoutInputJoystick getRightStick() => new LayoutInputJoystick()
        {
            input = InputJoystickSide.RIGHT,
            icon = joystickLeft.icon
        };

        [Header("actions")]
        public LayoutIcon action_N;
        public LayoutIcon action_E;
        public LayoutIcon action_S;
        public LayoutIcon action_W;

        public LayoutInputAction getActionNorth() => new LayoutInputAction() { input = InputButtons.PAD_NORTH, icon = action_N.icon };
        public LayoutInputAction getActionEast() => new LayoutInputAction() { input = InputButtons.PAD_EAST, icon = action_E.icon };
        public LayoutInputAction getActionSouth() => new LayoutInputAction() { input = InputButtons.PAD_SOUTH, icon = action_S.icon };
        public LayoutInputAction getActionWest() => new LayoutInputAction() { input = InputButtons.PAD_WEST, icon = action_W.icon };

        public LayoutIcon action_START;
        public LayoutIcon action_SELECT;

        public LayoutInputAction getActionStart() => new LayoutInputAction() { input = InputButtons.START, icon = action_START.icon };
        public LayoutInputAction getActionSelect() => new LayoutInputAction() { input = InputButtons.RETURN, icon = action_SELECT.icon };

        [Header("dpad")]
        public LayoutIcon dpad_N;
        public LayoutIcon dpad_E;
        public LayoutIcon dpad_S;
        public LayoutIcon dpad_W;

        public LayoutInputDpad getDpadNorth() => new LayoutInputDpad() { input = InputDPad.DPAD_NORTH, icon = dpad_N.icon };
        public LayoutInputDpad getDpadEast() => new LayoutInputDpad() { input = InputDPad.DPAD_EAST, icon = dpad_E.icon };
        public LayoutInputDpad getDpadSouth() => new LayoutInputDpad() { input = InputDPad.DPAD_SOUTH, icon = dpad_S.icon };
        public LayoutInputDpad getDpadWest() => new LayoutInputDpad() { input = InputDPad.DPAD_WEST, icon = dpad_W.icon };

        public override LayoutInputJoystick[] getSticks() => new LayoutInputJoystick[] { getLeftStick(), getRightStick() };

        public override LayoutInputAction[] getActions() => new LayoutInputAction[]
        {
            getActionNorth(), getActionEast(), getActionSouth(), getActionWest()
        };

        public override LayoutInputDpad[] getDpads() => new LayoutInputDpad[]
        {
            getDpadNorth(), getDpadEast(), getDpadSouth(), getDpadWest()
        };
    }
}

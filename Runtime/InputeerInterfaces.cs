using UnityEngine;

namespace fwp.inputeer
{

    public interface GamepadSelection
    {
        public void onSelected(GamepadWatcher watcher);
        public void onUnselected();
    }

    public interface GamepadDualStick : GamepadSelection
    {
        public void onStick(InputJoystickSide side, Vector2 vec);
    }

}

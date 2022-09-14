using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace inputeer
{
    public class InputKeyTopDown : InputKeySchematic
    {
        public InputKeyTopDown()
        {
            assign(new InputActionKey[]
            {
                new InputActionKey(new KeyCode[]{ KeyCode.DownArrow, KeyCode.S }, false, "down"),
                new InputActionKey(new KeyCode[]{ KeyCode.UpArrow, KeyCode.Z }, false, "up"),
                new InputActionKey(new KeyCode[]{ KeyCode.LeftArrow, KeyCode.Q }, false, "left"),
                new InputActionKey(new KeyCode[]{ KeyCode.RightArrow, KeyCode.D }, false, "right")
            });
        }

        public bool pressing_down() => all[0].getRaw();
        public bool pressed_down() => all[0].justActed();

        public bool pressing_up() => all[1].getRaw();
        public bool pressed_up() => all[1].justActed();

        public bool pressing_left() => all[2].getRaw();
        public bool pressed_left() => all[2].justActed();

        public bool pressing_right() => all[3].getRaw();
        public bool pressed_right() => all[3].justActed();

        public bool pressingAnyHorizontalKey() => pressing_left() || pressing_right();

        public int getHorizontalDirection()
        {
            if (pressing_left()) return -1;
            else if (pressing_right()) return 1;
            return 0;
        }

    }
}
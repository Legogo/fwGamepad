using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using fwp.gamepad;

public class WatcherInput<SelectorType> where SelectorType : ISelectable
{
    public List<SelectorType> queue = new List<SelectorType>();

    public SelectorType previous;
    public SelectorType current;

    public bool hasSomething() => queue.Count > 0;


    /// <summary>
    /// deselect next frame
    /// </summary>
    public void deselectAll()
    {
        while (queue.Count > 0)
        {
            unqueueSelection(queue[0]);
        }

        //queue.Clear();
    }

    public ISelectable getLast()
    {
        if (queue.Count <= 0) return null;
        return queue[queue.Count - 1];
    }

    public void queueSelection(SelectorType target)
    {
        if (target == null) return;

        if (!queue.Contains(target))
        {
            queue.Add(target);

            target.onSelected();

            log($"queue:{target}  ↑{queue.Count}", target);
            // ↓{_unqueue.Count}
        }
    }

    public void unqueueSelection(SelectorType target)
    {
        if (target == null) return;

        if (!queue.Contains(target)) Debug.LogWarning(" ? selector queue doesn't contains " + target);
        else
        {
            queue.Remove(target);
            target.onUnselected();

            log($"un-queue:{target}  ↓{queue.Count}", target);
        }
    }

    public T extractFromQueue<T>() where T : class, ISelectable
    {
        for (int i = 0; i < queue.Count; i++)
        {
            T current = queue[i] as T;
            if (current != null)
                return current;
        }
        return null;
    }

    public string stringify()
    {
        string output = typeof(SelectorType).ToString() + " x" + queue.Count;

        for (int i = 0; i < queue.Count; i++)
        {
            var mono = queue[i] as MonoBehaviour;
            if (mono != null) output += $"\n #{i} [MONO] {mono.name}";
            else output += $"\n #{i} [interf] {queue[i]}";
        }

        return output;
    }


    // INPUTS

    public bool onButton(InputButtons type, bool status)
    {

        for (int i = queue.Count - 1; i >= 0; i--)
        {
            ISelectableButton elmt = queue[i] as ISelectableButton;
            if (elmt != null)
            {
                //Debug.Log("button @ " + elmt);

                if (elmt.onButton(type, status))
                {
                    return true;
                }
            }

            Debug.Assert(i < queue.Count, elmt + " : didn't return true after pressing button ?",
                elmt as Component);

            ISelectableAbsorb abs = queue[i] as ISelectableAbsorb;
            if (abs != null && abs.isAbsorb()) return true;
        }

        return false;
    }

    public bool onJoystickDirection(InputJoystickSide side, Vector2 value)
    {

        for (int i = queue.Count - 1; i >= 0; i--)
        {
            ISelectableJoyDirection elmt = queue[i] as ISelectableJoyDirection;
            if (elmt != null)
            {
                switch (side)
                {
                    case InputJoystickSide.LEFT:
                        elmt.onJoyLeftDir(value);
                        break;
                    case InputJoystickSide.RIGHT:
                        elmt.onJoyRightDir(value);
                        break;
                }
            }

            ISelectableAbsorb abs = queue[i] as ISelectableAbsorb;
            if (abs != null && abs.isAbsorb()) return true;
        }

        return false;
    }

    public bool onJoystick(InputJoystickSide side, Vector2 value)
    {

        for (int i = queue.Count - 1; i >= 0; i--)
        {
            ISelectableJoy elmt = queue[i] as ISelectableJoy;
            if (elmt != null)
            {
                switch (side)
                {
                    case InputJoystickSide.LEFT:
                        elmt.onJoyLeft(value);
                        break;
                    case InputJoystickSide.RIGHT:
                        elmt.onJoyRight(value);
                        break;
                }
            }

            ISelectableAbsorb abs = queue[i] as ISelectableAbsorb;
            if (abs != null && abs.isAbsorb()) return true;
        }

        return false;
    }

    public bool onTrigger(InputTriggers side, float value)
    {

        for (int i = queue.Count - 1; i >= 0; i--)
        {
            ISelectableTrigger elmt = queue[i] as ISelectableTrigger;
            if (elmt != null)
            {
                switch (side)
                {
                    case InputTriggers.LT:
                        elmt.onTrigLeft(value);
                        break;
                    case InputTriggers.RT:
                        elmt.onTrigRight(value);
                        break;
                }
            }

            ISelectableAbsorb abs = queue[i] as ISelectableAbsorb;
            if (abs != null && abs.isAbsorb()) return true;
        }

        return false;
    }

    public bool onDPad(InputDPad type, bool status)
    {
        for (int i = queue.Count - 1; i >= 0; i--)
        {
            ISelectableDpad elmt = queue[i] as ISelectableDpad;
            if (elmt?.onDPad(type, status) ?? false)
            {
                return true;
            }

            ISelectableAbsorb abs = queue[i] as ISelectableAbsorb;
            if (abs != null && abs.isAbsorb())
            {
                return true;
            }
        }
        return false;
    }


    void log(string msg, object tar = null) => GamepadVerbosity.sLog(msg, tar);
}

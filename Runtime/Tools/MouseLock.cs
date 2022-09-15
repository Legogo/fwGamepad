using UnityEngine;
using System.Collections;

public class MouseLock : MonoBehaviour
{

    public bool lockedAtStart = false;

    void Start()
    {
        //can't lock in editor
        if (Application.isEditor) return;

        if (lockedAtStart) toggle(true);
    }

    void Update()
    {

        //capture mouse
        if (Cursor.lockState == CursorLockMode.None && Input.GetMouseButtonUp(0))
        {
            toggle(false);
        }

        //release mouse
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            toggle(true);
        }
    }

    void toggle(bool flag)
    {
        if (flag) lockCursor();
        else unlockCursor();
    }

    static public void lockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    static public void unlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}

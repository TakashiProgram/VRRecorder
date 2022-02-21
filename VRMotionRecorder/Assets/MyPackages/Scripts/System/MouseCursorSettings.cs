using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorSettings : VTuberSystem.SingletonMonoBehaviour<MouseCursorSettings>
{
    void Start()
    {
        //Hide();
    }

    public void Hide()
    {
#if !UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
#endif
    }

    public void UnHide()
    {
#if !UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
#endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CS_Shortcut : MonoBehaviour
{
    void Update()
    {
       Cursor.lockState = CursorLockMode.None;
        if (Input.GetKeyDown(KeyCode.Mouse2)) { Application.Quit(); }
    }
}
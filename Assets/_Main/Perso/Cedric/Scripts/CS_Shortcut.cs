using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CS_Shortcut : MonoBehaviour
{
    [Space][SerializeField][Scene] private String _sceneToLoad;

    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        if (Input.GetKeyDown(KeyCode.Mouse2)) { Application.Quit(); }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            NetworkManager.Singleton.StartHost();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            //NetworkManager.Singleton.SceneManager.LoadScene(_sceneToLoad, LoadSceneMode.Single);
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}
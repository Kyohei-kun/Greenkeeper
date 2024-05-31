using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class CS_UI_Role : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI roleUI;

    void Start()
    {
        if (IsHost)
        {
            roleUI.color = new Color(1,0.5f,0);
            roleUI.text = "Role : HOST";
        }
        else
        {
            roleUI.color = Color.cyan;
            roleUI.text = "Role : CLIENT";
        }
    }
}

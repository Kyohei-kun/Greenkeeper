using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CS_RandomColor : NetworkBehaviour
{
    private NetworkVariable<Color> myColor = new NetworkVariable<Color>(Color.magenta, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

       private void Update()
    {
        if (!IsOwner) return;
        if (Input.GetKeyDown(KeyCode.Space)) myColor.Value = GenerateRandomColor();
    }

    public override void OnNetworkSpawn()
    {
        myColor.OnValueChanged += OnChangeColor;
        if (IsOwner)
        {
            myColor.Value = GenerateRandomColor();
        }
        else
        {
            ApplyColor();
        }
    }

    private void OnChangeColor(Color previousColor, Color newColor)
    {
        ApplyColor();
    }

    private void ApplyColor()
    {
        GetComponent<Renderer>().material = new Material(GetComponent<Renderer>().material);
        GetComponent<Renderer>().material.color = myColor.Value;
    }

    private Color GenerateRandomColor()
    {
        return new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }
}

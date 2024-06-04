using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class CS_DandelionPilot : MonoBehaviour
{
    [SerializeField] VisualEffect vfx;
    Transform tr_player;

    void Start()
    {
        tr_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        vfx.SetVector3("Player", tr_player.position);
    }
}

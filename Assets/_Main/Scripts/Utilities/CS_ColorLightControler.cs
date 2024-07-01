using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class CS_ColorLightControler : MonoBehaviour
{
    [SerializeField] private float _speed = 1;

    [SerializeField] AnimationCurve low;
    [SerializeField] AnimationCurve high;

    //[SerializeField][Range(0f, 1f)] float low = 0.5f;
    //[SerializeField] [Range(0f,1f)] private float high = 0.5f;

    private void Update()
    {
        Gamepad gamepad = Gamepad.current;
        if (gamepad == null || !(gamepad is DualSenseGamepadHID))
        { return; }
        SetLightColor((DualSenseGamepadHID)gamepad, GetColorByTimeAndSpeed());

        //DualSenseGamepadHID currentGamePad;
        //try { currentGamePad = (DualSenseGamepadHID)Gamepad.current; }
        //catch (System.Exception) { return; }

        //currentGamePad.SetMotorSpeeds(low.Evaluate(Mathf.Repeat(Time.time, 3)), high.Evaluate(Mathf.Repeat(Time.time, 3)));
    }

    private void SetLightColor(DualSenseGamepadHID dualSense, Color color)
    {
        dualSense.SetLightBarColor(color);
    }

    private Color GetColorByTimeAndSpeed()
    {
        float normalizedTime = (Time.time * _speed) % 1.0f;
        float r = Mathf.Sin(2 * Mathf.PI * normalizedTime) * 0.5f + 0.5f;
        float g = Mathf.Sin(2 * Mathf.PI * normalizedTime + 2 * Mathf.PI / 3) * 0.5f + 0.5f;
        float b = Mathf.Sin(2 * Mathf.PI * normalizedTime + 4 * Mathf.PI / 3) * 0.5f + 0.5f;
        return new Color(r, g, b);
    }
}

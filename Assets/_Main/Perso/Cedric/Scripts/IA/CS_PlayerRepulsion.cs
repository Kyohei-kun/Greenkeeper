using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.VFX;

public class CS_PlayerRepulsion : MonoBehaviour
{
    [SerializeField] private float _radiusRepulsion = 10;
    [SerializeField] private AnimationCurve _strenghtByTime;

    [SerializeField] private GameObject _vfxLoading;
    [SerializeField] private Transform _prefab_VFX_Wave;
    [SerializeField][MinMaxSlider(0.5f, 5f)] Vector2 _minMaxScaleWave_VFX;

    private CS_PouicIA_Manager _manager;
    private bool _isPressed;
    private float _timer = 0;

    public void OnRepulsion(InputValue value)
    {
        if (_manager == null)
        {
            try{_manager = GameObject.FindGameObjectWithTag("PouicIA_Manager").GetComponent<CS_PouicIA_Manager>();}
            catch (System.Exception) { return; }
        }

        if (value.isPressed)
        {
            _isPressed = true;
        }
        else
        {
            MakeRepulsion();   
        }
    }

    private void Update()
    {
        if (_isPressed)
        {
            _timer += Time.deltaTime;
            _vfxLoading.SetActive(true);
            if (_timer > _strenghtByTime.keys[_strenghtByTime.length - 1].time)
            {
                _vfxLoading.SetActive(false);
            }
        }
        else
        {
            _timer = 0;
            _vfxLoading.SetActive(false);
        }
    }

    private void MakeRepulsion()
    {
        _isPressed = false;
        _manager.AddPlayerForce(transform, _radiusRepulsion, _strenghtByTime.Evaluate(_timer));
        //VFX Wave
        Transform temp = Instantiate(_prefab_VFX_Wave);
        temp.position = transform.position;
        float scale = _strenghtByTime.Evaluate(_timer).Remap(_strenghtByTime.keys[0].value, _strenghtByTime.keys[_strenghtByTime.length - 1].value, _minMaxScaleWave_VFX.x, _minMaxScaleWave_VFX.y);
        temp.GetComponent<VisualEffect>().SetFloat("Scale", scale);
        
    }
}


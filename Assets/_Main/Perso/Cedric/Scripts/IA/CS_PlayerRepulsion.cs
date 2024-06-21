using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.VFX;

public class CS_PlayerRepulsion : NetworkBehaviour
{
    [SerializeField] private float _radiusRepulsion = 10;
    [SerializeField] private AnimationCurve _strenghtByTime;

    [SerializeField] private GameObject _vfxLoading;
    [SerializeField] private Transform _prefab_VFX_Wave;
    [SerializeField][MinMaxSlider(0.5f, 5f)] Vector2 _minMaxScaleWave_VFX;

    private CS_PouicIA_Manager _manager;
    private bool _isPressed;
    private float _timer = 0;

    private NetworkVariable<bool> _drawLoadFX = new NetworkVariable<bool>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        _drawLoadFX.OnValueChanged += OnDrawLoadingFXChanged;
    }

    public override void OnNetworkDespawn()
    {
        _drawLoadFX.OnValueChanged -= OnDrawLoadingFXChanged;
    }

    public void OnDrawLoadingFXChanged(bool previous, bool current)
    {
        if (_drawLoadFX.Value)
        {
            _vfxLoading.SetActive(true);
        }
        else
        {
            _vfxLoading.SetActive(false);
        }
    }


    /// <summary>
    /// Input receiver
    /// </summary>
    /// <param name="value"></param>
    public void OnRepulsion(InputValue value)
    {
        if (!IsOwner) return;
        if (value.isPressed)
        {
            _isPressed = true;
        }
        else
        {
            _isPressed = false;
            MakeRepulsionServerRpc(_timer);
        }
    }

    private void Update()
    {
        if (IsOwner)
        {
            if (_isPressed)
            {
                _timer += Time.deltaTime;
                _drawLoadFX.Value = true;
                if (_timer > _strenghtByTime.keys[_strenghtByTime.length - 1].time)
                {
                    _drawLoadFX.Value = false;
                }
            }
            else
            {
                _timer = 0;
                _drawLoadFX.Value = false;    
                _vfxLoading.SetActive(false);
            }
        }
        _vfxLoading.SetActive(_drawLoadFX.Value);
    }

    [ServerRpc]
    private void MakeRepulsionServerRpc(float timer)
    {
        if (_manager == null)
        {
            try { _manager = GameObject.FindGameObjectWithTag("PouicIA_Manager").GetComponent<CS_PouicIA_Manager>(); }
            catch (System.Exception) { return; }
        }
        _manager.AddPlayerForce(transform, _radiusRepulsion, _strenghtByTime.Evaluate(timer));
     
        //VFX Wave
        Transform temp = Instantiate(_prefab_VFX_Wave);
        temp.position = transform.position;
        float fxScale = _strenghtByTime.Evaluate(timer).Remap(_strenghtByTime.keys[0].value, _strenghtByTime.keys[_strenghtByTime.length - 1].value, _minMaxScaleWave_VFX.x, _minMaxScaleWave_VFX.y);
        temp.GetComponent<VisualEffect>().SetFloat("Scale", fxScale);

        WaveFXClientRpc(timer);
    }

    [ClientRpc]
    private void WaveFXClientRpc(float timer)
    {
        //VFX Wave
        Transform temp = Instantiate(_prefab_VFX_Wave);
        temp.position = transform.position;
        float fxScale = _strenghtByTime.Evaluate(timer).Remap(_strenghtByTime.keys[0].value, _strenghtByTime.keys[_strenghtByTime.length - 1].value, _minMaxScaleWave_VFX.x, _minMaxScaleWave_VFX.y);
        temp.GetComponent<VisualEffect>().SetFloat("Scale", fxScale);
    }

}
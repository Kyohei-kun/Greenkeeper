using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CS_PlayerRepulsion : MonoBehaviour
{
    [SerializeField] private AnimationCurve _strenghtByTime;
    [SerializeField] private GameObject _vfxLoading;
    [SerializeField] private Transform _prefab_VFX_Wave;

    private CS_PouicIA_Manager _manager;
    private bool _isPressed;
    private float _timer = 0;

    public void OnRepulsion(InputValue value)
    {
        if (_manager == null)
        {
            _manager = GameObject.FindGameObjectWithTag("PouicIA_Manager").GetComponent<CS_PouicIA_Manager>();
            if (_manager == null) { return; }
        }

        if (value.isPressed)
        {
            _isPressed = true;
        }
        else
        {
            _isPressed = false;
            _manager.AddPlayerForce(transform, 10, _strenghtByTime.Evaluate(_timer));
         
            //VFX Wave
            Transform temp = Instantiate(_prefab_VFX_Wave);
            temp.position = transform.position;
        }
    }


    private void Update()
    {
        if(_isPressed)
        {
            _timer += Time.deltaTime;
            _vfxLoading.SetActive(true);
        }
        else
        {
            _timer = 0;
            _vfxLoading.SetActive(false);
        }
    }
}


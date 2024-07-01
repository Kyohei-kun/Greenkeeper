using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public abstract class CS_IABehaviour: MonoBehaviour
{
    [SerializeField] private bool _activate = true;
    [Space]
    [SerializeField] protected float _weight = 1f;
    protected float _sqrWeight;

    public float Weight { get => _weight; set => _weight = value; }
    public bool Activate { get => _activate; set => _activate = value; }
    public bool ShowGizmo { get => _showGizmo; set => _showGizmo = value; }
    public Color ColorGizmo { get => _colorGizmo; set => _colorGizmo = value; }

    [Space]
    [SerializeField] private bool _showGizmo;
    [SerializeField] private Color _colorGizmo;

    public abstract Vector3 OnMove(List<Transform> neighborsAgents, List<Transform> neighborObstacles, Transform agent);

    public virtual void Start()
    {
        _sqrWeight = _weight * _weight;
    }
}

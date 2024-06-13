using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.AI;

public class CS_PouicIA : MonoBehaviour
{
    [Range(0.5f, 8f)][SerializeField] private float _radiusVision = 3f;
    [Range(0.1f, 7.5f)][SerializeField] private float _radiusRepulsion = 1f;
    [Range(0.1f, 7.5f)][SerializeField] private float _repulsionStrenght = 1f;
    [Range(0.1f, 7.5f)][SerializeField] private float _speed = 1f;

    [SerializeField][Range(1, 4)] float _weightAlignement;
    [SerializeField][Range(1, 4)] float _weightRepulsion;
    [SerializeField][Range(1, 4)] float _weightCentrage;

    [SerializeField] private LayerMask _layerMask;

    private NavMeshAgent _agent;
    private List<CS_PouicIA> _neighbor;
    Vector3 currentvelocity;

    private Vector3 _alignement;
    private Vector3 _repulsion;
    private Vector3 _centrage;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        UpdateNeighbor();
        UpdateAlignement();
        UpdateRepulsion();
        UpdateCentrage();

        Vector3 move = Vector3.zero;
        Vector3 partialMove = Vector3.zero;

        partialMove = _alignement * _weightAlignement;
        if (partialMove.sqrMagnitude > _weightAlignement * _weightAlignement)
        {
            partialMove.Normalize();
            partialMove *= _weightAlignement;
        }
        move += partialMove;

        partialMove = _repulsion* _weightRepulsion;
        if (partialMove.sqrMagnitude > _weightRepulsion * _weightRepulsion)
        {
            partialMove.Normalize();
            partialMove *= _weightRepulsion;
        }
        move += partialMove;

        partialMove = _centrage* _weightCentrage;
        if (partialMove.sqrMagnitude > _weightCentrage * _weightCentrage)
        {
            partialMove.Normalize();
            partialMove *= _weightCentrage;
        }
        move += partialMove;

        _agent.velocity = move * _speed;
    }

    private void UpdateNeighbor()
    {
        _neighbor = new List<CS_PouicIA>();
        List<Collider> col = Physics.OverlapSphere(transform.position, _radiusVision).ToList();

        foreach (Collider c in col)
        {
            CS_PouicIA ia = c.GetComponentInParent<CS_PouicIA>();
            if (ia != null)
            {
                if (ia != this)
                    _neighbor.Add(ia);
                ia = null;
            }
        }
    }


    private void UpdateAlignement()
    {
        if (_neighbor.Count == 0)
        {
            _alignement = _agent.transform.forward;
            return;
        }

        Vector3 result = Vector3.zero;
        foreach (var neighbor in _neighbor)
        {
            result += neighbor.transform.forward;
        }
        result /= _neighbor.Count;
        _alignement = result;
    }

    private void UpdateRepulsion()
    {
        if (_neighbor.Count == 0)
        {
            _repulsion = Vector3.zero;
            return;
        }

        Vector3 result = Vector3.zero;
        int nNear = 0;
        foreach (var neighbor in _neighbor)
        {
            if (Vector3.SqrMagnitude(neighbor.transform.position - _agent.transform.position) < _radiusRepulsion * _radiusRepulsion)
            {
                nNear++;
                result += _agent.transform.position - neighbor.transform.position;
            }
        }

        if (nNear > 0)
        {
            result /= nNear;
        }

        _repulsion = result;
    }

    private void UpdateCentrage()
    {
        if (_neighbor.Count == 0)
        {
            _centrage = Vector3.zero;
            return;
        }

        Vector3 result = Vector3.zero;
        foreach (var neighbor in _neighbor)
        {
            result += neighbor.transform.position;
        }

        result /= _neighbor.Count;
        result -= transform.position;
        result = Vector3.SmoothDamp(_agent.transform.forward, result, ref currentvelocity, 0.5f);

        _centrage = result;
    }


    //private void OnDrawGizmosSelected()
    //{
    //    //Draw Vision field (Green)
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, _radiusVision);

    //    //Draw Repulsion field (Red)
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, _radiusRepulsion);


    //    //Draw Neighbor (Yellow)
    //    UpdateNeighbor();
    //    if (_neighbor.Count > 0)
    //    {
    //        Gizmos.color = Color.yellow;
    //        foreach (var near in _neighbor)
    //        {
    //            Gizmos.DrawSphere(near.transform.position, 0.2f);
    //        }

    //        //Draw Alignment (Blue)
    //        UpdateAlignement();
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawLine(transform.position, transform.position + _alignement);
    //        Gizmos.DrawSphere(transform.position + _alignement, 0.1f);


    //        //Draw Repulsion (Red)
    //        UpdateRepulsion();
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawLine(transform.position, transform.position + _repulsion);
    //        Gizmos.DrawCube(transform.position + _repulsion, Vector3.one * 0.1f);

    //        //Draw centrage point (Green)
    //        UpdateCentrage();
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawCube(_centrage, Vector3.one * 0.3f);
    //        foreach (var near in _neighbor)
    //        {
    //            Gizmos.DrawLine(_centrage, near.transform.position);
    //        }
    //    }
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_EnclosureBehaviour : CS_IABehaviour
{
    [SerializeField] private Vector3 _center = Vector3.zero;
    [SerializeField] [Range(30,200)] private float _radius = 30;
    Vector3 _currentVelocity;

    public override Vector3 OnMove(List<Transform> neighborsAgent, List<Transform> neighborsObstacles, Transform agent)
    {
        Vector3 result = Vector3.zero;
        Vector3 centerOffset = _center - agent.position;
        float t = centerOffset.magnitude/_radius;
        if(t < 0.9f) 
        {
            return Vector3.zero;
        }
        result = centerOffset * t*t;

        result *= _weight;
               
        return result;
    }
}

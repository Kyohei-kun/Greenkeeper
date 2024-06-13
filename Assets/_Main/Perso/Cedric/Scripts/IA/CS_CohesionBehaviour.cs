using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_CohesionBehaviour : CS_IABehaviour
{
    private Vector3 _currentVelocity;

    public override Vector3 OnMove(List<Transform> neighborsAgent, List<Transform> neighborsObstacles, Transform agent)
    {
        if (neighborsAgent.Count == 0 || !Activate) return Vector3.zero;

        Vector3 result = Vector3.zero;
        foreach (var neighbor in neighborsAgent)
        {
            result += neighbor.position;
        }

        result /= neighborsAgent.Count;
        result = result - agent.transform.position;

        result = Vector3.SmoothDamp(agent.forward, result, ref _currentVelocity, 0.1f);

        result *= _weight;

        if (Vector3.SqrMagnitude(result) > _sqrWeight)
        {
            result.Normalize();
            result *= _weight;
        }

        return result;
    }
}

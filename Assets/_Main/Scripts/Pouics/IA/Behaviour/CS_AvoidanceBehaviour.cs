using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CS_AvoidanceBehaviour : CS_IABehaviour
{
    [Range(0.1f, 7.5f)][SerializeField] private float _radiusAvoidance = 1f;
    Vector3 _currentVelocity;

    public override Vector3 OnMove(List<Transform> neighborsAgent, List<Transform> neighborsObstacles, Transform agent)
    {
        if (neighborsAgent.Count == 0 || !Activate) return Vector3.zero;

        Vector3 result = Vector3.zero;
        int nNear = 0;
        foreach (var neighbor in neighborsAgent)
        {
            if ((neighbor.transform.position - agent.position).magnitude < _radiusAvoidance)
            {
                nNear++;
                Vector3 rawAvoidance = agent.position - neighbor.transform.position;
                if (rawAvoidance != Vector3.zero)
                    rawAvoidance = rawAvoidance.normalized / rawAvoidance.magnitude;
                result += rawAvoidance;

            }
        }

        if (nNear > 0)
        {
            result /= nNear;
        }

        result *= _weight;

        return result;
    }
}

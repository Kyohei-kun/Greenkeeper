using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Searcher.SearcherWindow;

public class CS_AlignementBehaviour : CS_IABehaviour
{
    public override Vector3 OnMove(List<Transform> neighborsAgent, List<Transform> neighborsObstacles, Transform agent)
    {
        if (neighborsAgent.Count == 0 || !Activate) return agent.forward;

        Vector3 result = Vector3.zero;
        foreach (var neighbor in neighborsAgent)
        {
            result += neighbor.transform.forward;
        }
        result /= neighborsAgent.Count;

        result *= _weight;

        if (Vector3.SqrMagnitude(result) > _sqrWeight)
        {
            result.Normalize();
            result *= _weight;
        }

        return result;
    }
}

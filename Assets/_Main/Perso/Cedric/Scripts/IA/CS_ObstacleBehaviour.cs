using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_ObstacleBehaviour : CS_IABehaviour
{
    private float _A;
    private float _B;

    [SerializeField] private float _maxStrengh = 2;

    float _obstacleRadius = 5;

    public override void Start()
    {
        base.Start();
        _B = 1 / (1 / _maxStrengh - (GetComponent<CS_PouicIA_Manager>().RadiusVision + _obstacleRadius)); 
        _A = _maxStrengh - 1 / _B;
    }

    public override Vector3 OnMove(List<Transform> neighborsAgents, List<Transform> neighborObstacles, Transform agent)
    {
        if (neighborObstacles.Count == 0 || !Activate || Vector3.Dot((neighborObstacles[0].position - agent.position).normalized, agent.forward) < -0.1f) return Vector3.zero;

        Vector3 endWorldPositionDeviantForce = (agent.position + Vector3.Project(neighborObstacles[0].position - agent.position, agent.forward));

        Vector3 deviantDirection = (endWorldPositionDeviantForce - neighborObstacles[0].position).normalized;
     
        //if the move of agent not enter in zone (with 0.2f u of security) 
        if (Vector3.Distance(endWorldPositionDeviantForce, neighborObstacles[0].position) < _obstacleRadius + 1f) 
        {
            float norme = (1 / Vector3.Distance(agent.position, neighborObstacles[0].position) + _B) + _A;
            return deviantDirection * norme;
        }
        else
            return Vector3.zero;
    }
}
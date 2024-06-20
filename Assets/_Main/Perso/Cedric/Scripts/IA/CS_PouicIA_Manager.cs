using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class CS_PouicIA_Manager : NetworkBehaviour
{
    [SerializeField][Range(1, 9)] private float _speed = 1;
    [SerializeField][Range(1, 10)] private float _maxSpeed = 3;
    [SerializeField][Range(1, 5)] private float _radiusVision = 1;

    private List<CS_IABehaviour> _behaviours;
    private Dictionary<NavMeshAgent, AgentData> _listAgents = new Dictionary<NavMeshAgent, AgentData>();
    private int _layerMaskVision;
    private List<GizmoData> _listGizmosData;
    public float RadiusVision { get => _radiusVision; set => _radiusVision = value; }

    public void Init(List<NavMeshAgent> _agent)
    {
        foreach (var agent in _agent)
        {
            _listAgents.Add(agent, new AgentData());
        }
        this.enabled = true;
        _layerMaskVision = LayerMask.GetMask("Pouic") | LayerMask.GetMask("Obstacle");
        _behaviours = GetComponents<CS_IABehaviour>().ToList();
        _listGizmosData = new List<GizmoData>();
        this.enabled = true;
    }

    private void Update()
    {
        _listGizmosData.Clear();
        foreach (KeyValuePair<NavMeshAgent, AgentData> agent in _listAgents.ToList()) //Agents
        {
            Vector3 move = Vector3.zero;
            List<Transform> neighborObstacles = null;
            List<Transform> neighborAgent = null;
            Transform tr_agent = agent.Key!.transform;
            GetNeighbors(tr_agent, _radiusVision, out neighborAgent, out neighborObstacles);
            foreach (var behaviour in _behaviours)// Behaviours
            {
                Vector3 partialMove = behaviour.OnMove(neighborAgent, neighborObstacles, tr_agent);
                move += partialMove;
                if (behaviour.ShowGizmo)
                {
                    GizmoData currentGizmoData = new GizmoData();
                    currentGizmoData.agentPosition = tr_agent.position;
                    currentGizmoData.gizmoColor = behaviour.ColorGizmo;
                    currentGizmoData.gizmoVector = partialMove;
                    _listGizmosData.Add(currentGizmoData);
                } //Visualization
            }
            move *= _speed;
            if (Vector3.SqrMagnitude(move) > _maxSpeed * _maxSpeed) //Clamp
            {
                move.Normalize();
                move *= _maxSpeed;
            }
            agent.Key.velocity = (move + agent.Value.playerForce) /2f;
            //Reduce player order in time
            AgentData agentData = new AgentData();
            agentData.playerForce = Vector3.Lerp(agent.Value.playerForce, Vector3.zero, 0.05f);
            _listAgents[agent.Key] = agentData;
        }
    }


    private void GetNeighbors(Transform agent, float radius, out List<Transform> neighborAgents, out List<Transform> neighborObstacles)
    {
        neighborObstacles = new List<Transform>();
        neighborAgents = new List<Transform>();
        Collider[] hitColliders = Physics.OverlapSphere(agent.position, radius + 5, _layerMaskVision);
        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                neighborObstacles.Add(hitCollider.transform);
                continue;
            }
            if (hitCollider.transform != agent)
            {
                neighborAgents.Add(hitCollider.transform);
            }
        }
        if (neighborObstacles.Count > 1)
        {
            neighborObstacles = neighborObstacles.OrderBy(o => Vector3.Distance(agent.position, o.position)).ToList();
        }
    }

    private void OnDrawGizmos()
    {
        if (_listGizmosData != null)
        {
            foreach (var item in _listGizmosData)
            {
                Gizmos.color = item.gizmoColor;
                Gizmos.DrawLine(item.agentPosition, item.agentPosition + item.gizmoVector);
                Gizmos.DrawCube(item.agentPosition + item.gizmoVector, Vector3.one * 0.2f);
            }
        }
    }

    struct GizmoData
    {
        public Vector3 agentPosition;
        public Color gizmoColor;
        public Vector3 gizmoVector;
    }

    struct AgentData
    {
        public float currentSpeed;
        public Vector3 playerForce;
    }

    public void AddPlayerForce(Transform trPlayer, float radius, float strenght)
    {
        foreach (var col in Physics.OverlapSphere(trPlayer.position, radius, LayerMask.GetMask("Pouic")).ToList())
        {
            Vector3 newForce = Vector3.zero;
            NavMeshAgent currentAgent = col.GetComponent<NavMeshAgent>();
            AgentData currentAgentData;
            currentAgentData = _listAgents[currentAgent];
            newForce = currentAgent.transform.position - trPlayer.position;
            newForce = Vector3.Lerp(newForce, currentAgent.transform.forward, Vector3.Dot(newForce.normalized, currentAgent.transform.forward));
            currentAgentData.playerForce = ((_listAgents[currentAgent].playerForce + (newForce)) / 2f).normalized * strenght;
            _listAgents[currentAgent] = currentAgentData;
        }
    }
}
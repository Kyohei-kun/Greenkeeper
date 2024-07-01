using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.AI;

public class CS_ContinousSpawnPouic : NetworkBehaviour
{
    [Header("Setup")]
    [SerializeField] Transform _pouicPrefab;

    [Space]
    [Header("Parameter")]
    [SerializeField] int _pouicMaxQuantity = 100;
    //[SerializeField][TooltipAttribute("Spawn X pouics at each refresh")]  float _refreshRate = 5;
    [SerializeField][TooltipAttribute("Refresh Spawn every X second")] int _spawnRate = 5;
    [SerializeField] List<Transform> _spawnPosition = new List<Transform>();
    
    CS_PouicIA_Manager _pouicIA_Manager;
    private List<NavMeshAgent> _newPouicIAs;

    void Start()
    {
        if (IsHost)
        {
            _pouicIA_Manager = GetComponent<CS_PouicIA_Manager>();
            SpawnPouicServerRPC();
            _pouicIA_Manager.Init(_newPouicIAs);
        }
    }

    private void Update()
    {
        if (IsHost)
        {
            SpawnPouicServerRPC();
            _pouicIA_Manager.AddAgents(_newPouicIAs);
        }
    }

    [ServerRpc]
    public void SpawnPouicServerRPC()
    {
        _newPouicIAs = new List<NavMeshAgent>();
        int pouicDelta = _pouicMaxQuantity - _pouicIA_Manager.ListAgents.Count;
        pouicDelta = Mathf.Clamp(pouicDelta, 0, _spawnRate);
        if (pouicDelta > 0)
        {
            for (int i = 0; i < pouicDelta; i++)
            {
                Transform currentPouic = GameObject.Instantiate(_pouicPrefab);
                currentPouic.transform.position = _spawnPosition[Random.Range(0, _spawnPosition.Count)].position;
                currentPouic.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                currentPouic.GetComponent<NetworkObject>().Spawn();
                currentPouic.transform.parent = transform;
                currentPouic.transform.name = "_IntancePouic_"+Random.Range(0,800);
                _newPouicIAs.Add(currentPouic.GetComponent<NavMeshAgent>());
                currentPouic.GetComponent<NavMeshAgent>().enabled = true;
            }
        }
    }
}

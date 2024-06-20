using Cinemachine.Utility;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class CS_SpawnPouic : NetworkBehaviour
{
    [Header("Setup")]
    [SerializeField] NavMeshSurface _navMeshSurface;
    [SerializeField] Transform _pouicPrefab;

    [Space]
    [Header("Parameter")]
    [SerializeField] int _nbPouic = 200;
    [SerializeField][Range(10, 100)] float _radiusSpawn = 30;

    private List<NavMeshAgent> _pouicIAs;

    void Start()
    {
        if (IsHost)
        {

        _pouicIAs = new List<NavMeshAgent>();

            SpawnPouicServerRPC();

        GetComponent<CS_PouicIA_Manager>().Init(_pouicIAs);
        }
    }

    [ServerRpc]
    public void SpawnPouicServerRPC()
    {
        for (int i = _nbPouic; i >= 1; i--)
        {
            Vector3 randomPoint = (transform.position + Random.insideUnitSphere * _radiusSpawn).ProjectOntoPlane(Vector3.up);
            Transform currentPouic = GameObject.Instantiate(_pouicPrefab);
            currentPouic.transform.position = randomPoint;
            currentPouic.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            currentPouic.GetComponent<NetworkObject>().Spawn();
            currentPouic.transform.parent = transform;
            currentPouic.transform.name = "_IntancePouic(" + i + ")";
            _pouicIAs.Add(currentPouic.GetComponent<NavMeshAgent>());
            currentPouic.GetComponent<NavMeshAgent>().enabled = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _radiusSpawn);
        }
    }
}

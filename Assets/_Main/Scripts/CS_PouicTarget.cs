using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class CS_PouicTarget : NetworkBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    private CS_PouicIA_Manager _pouicIA_Manager;
    [SerializeField] Transform _fxPrefab;

    private void Start()
    {
        if (IsHost)
        {
            _pouicIA_Manager = GameObject.FindGameObjectWithTag("PouicIA_Manager").GetComponent<CS_PouicIA_Manager>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsHost)
        {
            if ((_layerMask.value & (1 << other.gameObject.layer)) > 0)
            {
                _pouicIA_Manager.OnAgentDestroy(other.GetComponent<NavMeshAgent>());
                Transform fx = Instantiate(_fxPrefab);
                fx.position = other.transform.position;
                fx.GetComponent<NetworkObject>().Spawn();
                other.GetComponent<NetworkObject>().Despawn(true);
            }
        }
    }
}
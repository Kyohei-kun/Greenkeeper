using Cinemachine.Utility;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class CS_SpawnPouic : MonoBehaviour
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
        _pouicIAs = new List<NavMeshAgent>();

        for (int i = _nbPouic; i >= 1; i--)
        {
            Vector3 randomPoint = (transform.position + Random.insideUnitSphere * _radiusSpawn).ProjectOntoPlane(Vector3.up);
            Transform currentPouic = GameObject.Instantiate(_pouicPrefab);
            currentPouic.transform.position = randomPoint;
            currentPouic.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            currentPouic.transform.parent = transform;
            currentPouic.transform.name = "_IntancePouic(" + i + ")";
            _pouicIAs.Add(currentPouic.GetComponent<NavMeshAgent>());
            currentPouic.GetComponent<NavMeshAgent>().enabled = true;
        }

        GetComponent<CS_PouicIA_Manager>().Init(_pouicIAs);
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

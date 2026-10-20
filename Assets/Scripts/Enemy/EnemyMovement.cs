using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        var target = FindAnyObjectByType<EnemyTarget>();
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
        }
    }
}
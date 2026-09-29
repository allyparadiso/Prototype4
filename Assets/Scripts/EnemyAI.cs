using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public static EnemyAI Instance { get; private set; }

    public enum AIState { Patrolling, Chasing, Searching }

    public AIState currentState = AIState.Patrolling;

    [Header("Detection Settings")]
    public Transform player;
    public float viewDistance = 15f;
    public float viewAngle = 50f;
    public LayerMask wallsMask;
    public LayerMask playerMask;

    [Header("Patrol Settings")]
    public Transform[] patrolWaypoints;
    public float patrolSpeed = 2f;
    private int currentWaypointIndex = 0;

    [Header("Chase Settings")]
    public float chaseSpeed = 4.5f;

    [Header("Search Settings")]
    public float searchDuration = 5f;
    public float searchSpeed = 1.5f;
    private Vector3 lastKnownPosition;
    private float searchTimer = 0f;

    private NavMeshAgent agent;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (patrolWaypoints.Length > 0)
        {
            GoToNextWaypoint();
        }
    }

    private void Update()
    {
        bool canSeePlayer = CheckLineOfSight();

        switch (currentState)
        {
            case AIState.Patrolling:
                HandlePatrolling(canSeePlayer);
                break;
            case AIState.Chasing:
                HandleChasing(canSeePlayer);
                break;
            case AIState.Searching:
                HandleSearching(canSeePlayer);
                break;
        }
    }

    private void HandlePatrolling(bool canSeePlayer)
    {
        agent.speed = patrolSpeed;

        if (canSeePlayer)
        {
            TransitionToState(AIState.Chasing);
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < .5f)
        {
            GoToNextWaypoint();
        }
    }

    private void HandleChasing(bool canSeePlayer)
    {
        agent.speed = chaseSpeed;

        if (canSeePlayer)
        {
            lastKnownPosition = player.position;
            agent.SetDestination(lastKnownPosition);
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance < .5f)
            {
                TransitionToState(AIState.Searching);
            }
        }
    }

    private void HandleSearching(bool canSeePlayer)
    {
        agent.speed = searchSpeed;

        if (canSeePlayer)
        {
            TransitionToState(AIState.Chasing);
            return;
        }

        searchTimer += Time.deltaTime;
        if (searchTimer >= searchDuration)
        {
            TransitionToState(AIState.Patrolling);
        }
    }

    private void TransitionToState(AIState newState)
    {
        currentState = newState;

        if (newState == AIState.Searching)
        {
            searchTimer = 0;
        }
        else if (newState == AIState.Patrolling)
        {
            GoToNextWaypoint();
        }
    }

    private void GoToNextWaypoint()
    {
        if (patrolWaypoints.Length == 0) return;

        agent.SetDestination(patrolWaypoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % patrolWaypoints.Length;
    }

    private bool CheckLineOfSight()
    {
        if (player == null) return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= viewDistance)
        {
            if (Vector3.Angle(transform.forward, directionToPlayer) < viewAngle / 2f)
            {
                if (!Physics.Raycast(transform.position + Vector3.up, directionToPlayer, distanceToPlayer, wallsMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle /2f, 0) *transform.forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + Vector3.up, leftBoundary * viewDistance);
        Gizmos.DrawRay(transform.position + Vector3.up, rightBoundary * viewDistance);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : MonoBehaviour
{
    public Transform player;           // Reference to the player
    public Transform[] waypoints;      // Array of waypoints for the enemy to follow
    public float waypointTolerance = 1f; // Distance at which the enemy considers it has reached a waypoint
    public float stoppingDistance = 1f; // Distance at which the enemy stops moving towards the player
    public float chaseDistance = 10f;   // Distance within which the enemy starts chasing the player

    private NavMeshAgent navAgent;      // Reference to the NavMeshAgent component
    private int currentWaypointIndex; // Current waypoint index
    public bool isChasing = false;     // To track if the enemy is currently chasing the player

    private void Start()
    {
        // Initialize the NavMeshAgent
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (isChasing)
        {
            
            if (navAgent.remainingDistance <= waypointTolerance && currentWaypointIndex +1 < waypoints.Length)
            {
                // Move to the next waypoint
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                navAgent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player playerS = player.GetComponent<Player>();
            playerS.Killed();
        }
        else if(other.CompareTag("ChaseObstacle"))
        {
            other.gameObject.SetActive(false);
        }
    }
}

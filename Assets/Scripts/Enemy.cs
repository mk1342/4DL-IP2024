using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;          // Reference to the player's transform
    public float detectionRadius = 10f;  // Radius within which the enemy will start chasing the player

    private EnemyState currentState = EnemyState.Idle;
    private bool isMoving = false;
    /// <summary>
    /// I googled for examples and they seem to use this so
    /// </summary>
    public enum EnemyState
    {
        Idle,
        Chasing,
        Stopped
    }

    /// <summary>
    /// Handles State Switching
    /// </summary>
    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                IdleState();
                break;
            case EnemyState.Chasing:
                ChasingState();
                break;
            case EnemyState.Stopped:
                StoppedState();
                break;
        }
    }

    private void IdleState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            ChangeState(EnemyState.Chasing);
        }
    }

    private void ChasingState()
    {
        if (agent != null && player != null)
        {
            if (isMoving)
            {
                // Continue chasing the player
                agent.SetDestination(player.position);
            }
            else
            {
                // Stop movement
                agent.SetDestination(transform.position);
            }

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer > detectionRadius)
            {
                ChangeState(EnemyState.Idle);
            }
        }
    }

    private void StoppedState()
    {
        agent.SetDestination(transform.position);
    }

    public void ChangeState(EnemyState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case EnemyState.Idle:
                isMoving = false;
                break;
            case EnemyState.Chasing:
                isMoving = true;
                break;
            case EnemyState.Stopped:
                isMoving = false;
                break;
        }
    }
    /// <summary>
    /// Called by Player.cs
    /// </summary>
    /// <param name="value"></param>
    public void SetMovement(bool value)
    {
        if (value)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            if (distanceToPlayer <= detectionRadius)
            {
                ChangeState(EnemyState.Chasing);
            }
            else
            {
                ChangeState(EnemyState.Idle);
            }
        }
        else
        {
            ChangeState(EnemyState.Stopped);
        }
    }

        public void OnTriggerEnter(Collider other)
    {
        Player playerScript = player.gameObject.GetComponent<Player>();
        playerScript.Killed();
    }
}
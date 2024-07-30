using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;
    private bool isMoving = true;

    private void Update()
    {
        if (isMoving)
        {
            if(agent != null && target != null)
            {
                agent.SetDestination(target.position);
            }
        }   
        else
        {
            agent.SetDestination(gameObject.transform.position);
        }
    }

    public void SetMovement(bool value)
    {
        isMoving = value;
        Debug.Log(isMoving);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Operator))]
public class BasicNPCMovement : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent navAgent;
    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateUpAxis = false;
        navAgent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Set the NPC's destination
            navAgent.SetDestination(target.position);
        }
    }
}

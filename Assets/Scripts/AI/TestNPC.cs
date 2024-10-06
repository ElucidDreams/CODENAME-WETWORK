using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNPC : MonoBehaviour
{
    public Transform navTarget;
    NavMeshAgent agent;
    public Operator parentOperator;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        parentOperator = GetComponent<Operator>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        parentOperator.rotTarget = navTarget;
    }

    // Update is called once per frame
    void Update()
    {
        navTarget.position = agent.nextPosition;
        parentOperator.motionVec = agent.velocity;
        if (navTarget != null)
        {
            agent.SetDestination(navTarget.position);
        }
    }
}

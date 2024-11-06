using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
[System.Serializable]
public class TestNPC : MonoBehaviour
{
    public Transform navTarget;//TODO: Nav Target is currently public and set to reticule, needs actual implementation.
    NavMeshAgent agent;
    public Operator parentOperator;
    LineRenderer line;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        parentOperator = GetComponent<Operator>();
        line = GetComponent<LineRenderer>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (navTarget != null)
        {
            if (agent.hasPath)
            {
                parentOperator.rotTarget.position = agent.path.corners[1];
            }
            else
            {
                parentOperator.rotTarget.position = parentOperator.transform.position;
            }
            parentOperator.motionVec = agent.velocity;
            agent.SetDestination(navTarget.position);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal.Internal;
[System.Serializable]
public class TestNPC : MonoBehaviour
{
    public Transform navTarget;//TODO: Nav Target is currently public and set to reticule, needs actual implementation.
    NavMeshAgent agent;
    public Operator parentOperator;
    LineRenderer line;

    public float outOfSightTime = 1f;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            navTarget = other.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            Transform lastKnownPosition = new GameObject("LastKnownPosition").transform;
            lastKnownPosition.position = other.transform.position;
            lastKnownPosition.rotation = other.transform.rotation;
            lastKnownPosition.localScale = other.transform.localScale;
            StartCoroutine(MustHaveBeenTheWind(lastKnownPosition));
        }
    }

    IEnumerator MustHaveBeenTheWind(Transform lkp)
    {
        yield return new WaitForSeconds(outOfSightTime);
        navTarget = null;
        Destroy(lkp.gameObject);
    }
}

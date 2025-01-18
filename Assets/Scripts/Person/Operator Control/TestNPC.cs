using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.Splines;
[System.Serializable]
public class TestNPC : MonoBehaviour
{
    public Transform navTarget;//TODO: Nav Target is currently public and set to reticule, needs actual implementation.
    NavMeshAgent agent;
    public Operator parentOperator;
    LineRenderer line;
    public SplineContainer patrollingPath;
    public enum NPCStateEnum
    {
        Idle,
        Patrolling,
        Chasing,
        Attacking,
        Searching
    }
    public NPCStateEnum NPCState = NPCStateEnum.Idle;
    public float outOfSightTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        parentOperator = GetComponent<Operator>();
        line = GetComponent<LineRenderer>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        //Transform ClosestPoint = null;
        //float closestDistance = Mathf.Infinity;
        //patrollingPath.Spline.Knots.
        //navTarget = ClosestPoint;
    }

    // Update is called once per frame
    void Update()
    {
        switch (NPCState)
        {
            case NPCStateEnum.Patrolling:
                Patrol();
                break;
            case NPCStateEnum.Chasing:
                Chase();
                break;
            case NPCStateEnum.Attacking:
                Attack();
                break;
        }
    }

    void Chase()
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
    void Attack()
    {
        if (parentOperator.activeWeapon != null)
        {
            if (!parentOperator.activeWeapon.isAttacking)
            {
                parentOperator.activeWeapon.Attack();
            }
        }
    }
    void Patrol()
    {
        if (transform.position == navTarget.position)
        {
            //index++;
            //if (index >= patrolPoints.Count)
            //{
            //    index = 0;
            //}
            //navTarget = patrolPoints[index];
        }
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
            NPCState = NPCStateEnum.Chasing;
            navTarget = other.transform;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            NPCState = NPCStateEnum.Searching;
            Transform lastKnownPosition = new GameObject("LastKnownPosition").transform;
            lastKnownPosition.position = other.transform.position;
            lastKnownPosition.rotation = other.transform.rotation;
            lastKnownPosition.localScale = other.transform.localScale;
            navTarget = lastKnownPosition;
            MustHaveBeenTheWind(lastKnownPosition);
        }
    }
    public void MustHaveBeenTheWind(Transform lkp)
    {
        float timer = 0;
        while (timer < outOfSightTime)
        {
            timer += Time.deltaTime;
            if (navTarget != lkp)
            {
                Destroy(lkp.gameObject);
                return;
            }
        }
        NPCState = NPCStateEnum.Patrolling;
        Destroy(lkp.gameObject);
    }
}

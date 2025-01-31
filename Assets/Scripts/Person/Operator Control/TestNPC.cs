using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.Mathematics;
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
    public List<Vector3> patrolVectors;
    public List<GameObject> patrolPoints;
    int index;
    public enum NPCStateEnum
    {
        Idle,
        Patrolling,
        Chasing,
        Attacking,
        Searching
    }
    public NPCStateEnum NPCState = NPCStateEnum.Idle;
    public NPCStateEnum NPCStatePrevious = NPCStateEnum.Idle;
    public float outOfSightTime = 1f;
    public bool inAttackRange = false;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        parentOperator = GetComponent<Operator>();
        line = GetComponent<LineRenderer>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        foreach (BezierKnot knot in patrollingPath.Spline.Knots)
        {
            Vector3 tempPos = new Vector3(knot.Position.x, knot.Position.y, knot.Position.z) + patrollingPath.transform.position;
            patrolVectors.Add(tempPos);
        }
        for (int i = 0; i < patrolVectors.Count; i++)
        {
            GameObject temp = new("PatrolPoint" + i);
            temp.transform.position = patrolVectors[i];
            patrolPoints.Add(temp);
        }
        ClosestPatrolPoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //agent.updatePosition = parentOperator.takingDamage;
        //agent.updateRotation = parentOperator.takingDamage;
        //Debug.Log("Distance is : " + agent.remainingDistance.ToString() + " and the NPC following state is: " + agent.hasPath.ToString());
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
            Vector3 targetPos = navTarget.position;
            targetPos.z = agent.transform.position.z;
            agent.SetDestination(targetPos);
            if (agent.hasPath)
            {
                parentOperator.rotTarget.position = agent.path.corners[1];
            }
            else
            {
                parentOperator.rotTarget.position = parentOperator.transform.position;
            }
            parentOperator.motionVec = agent.velocity;
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
        if (agent.remainingDistance < 0.1f)
        {
            Debug.Log("Reached Patrol Point");
            index++;
            if (index >= patrolPoints.Count)
            {
                index = 0;
            }
            navTarget = patrolPoints[index].transform;
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
        if (other.gameObject.CompareTag("Player"))
        {
            NPCState = NPCStateEnum.Chasing;
            navTarget = other.transform;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            NPCState = NPCStateEnum.Searching;
            Transform lastKnownPosition = new GameObject("LastKnownPosition").transform;
            lastKnownPosition.position = other.transform.position;
            lastKnownPosition.rotation = other.transform.rotation;
            lastKnownPosition.localScale = other.transform.localScale;
            navTarget = lastKnownPosition;
            StartCoroutine(MustHaveBeenTheWind(lastKnownPosition));
        }
    }
    public IEnumerator MustHaveBeenTheWind(Transform lkp)
    {
        agent.SetDestination(lkp.position);
        yield return new WaitForSeconds(outOfSightTime);
        if (NPCState == NPCStateEnum.Searching)
        {
            NPCState = NPCStateEnum.Patrolling;
            ClosestPatrolPoint();
        }
        Destroy(lkp.gameObject);
    }

    public void ClosestPatrolPoint()
    {
        float lowestDistance = Mathf.Infinity;
        for (int i = 0; i < patrolVectors.Count; i++)
        {
            float distance = Vector3.Distance(patrolPoints[i].transform.position, parentOperator.transform.position);
            if (distance < lowestDistance)
            {
                index = i;
                lowestDistance = distance;
            }
        }
        navTarget = patrolPoints[index].transform;
    }
}

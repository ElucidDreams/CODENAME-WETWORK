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
    public float outOfSightTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        NPCState = NPCStateEnum.Patrolling;
        agent = GetComponent<NavMeshAgent>();
        parentOperator = GetComponent<Operator>();
        line = GetComponent<LineRenderer>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        float lowestDistance = Mathf.Infinity;
     
        foreach (BezierKnot knot in patrollingPath.Spline.Knots)
        {
            Vector3 tempPos = new Vector3(knot.Position.x,knot.Position.y,knot.Position.z) + patrollingPath.transform.position;
            patrolVectors.Add(tempPos);
        }
        for (int i = 0; i < patrolVectors.Count; i++)
        {
            GameObject temp = new GameObject("PatrolPoint" + i);
            temp.transform.position = patrolVectors[i];
            patrolPoints.Add(temp);
            float distance = Vector3.Distance(patrolPoints[i].transform.position, parentOperator.transform.position);
            if (distance < lowestDistance)
            {
                index = i;
                lowestDistance = distance;
            }
        }
        navTarget = patrolPoints[index].transform;
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

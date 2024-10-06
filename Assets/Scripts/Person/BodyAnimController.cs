using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAnimController : MonoBehaviour
{
    Transform target;
    Operator parentOperator;
    bool isPlayer;
    public Transform headTransform;
    public GameObject armsObject;
    Transform armsTransform;
    Animator armsAnimator;
    public GameObject legObject;
    Transform legTransform;
    Animator legAnimator;
    public float rotDeadZone = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        parentOperator = GetComponentInParent<Operator>();
        armsTransform = armsObject.GetComponent<Transform>();
        armsAnimator = armsObject.GetComponent<Animator>();
        legTransform = legObject.GetComponent<Transform>();
        legAnimator = legObject.GetComponent<Animator>();
        target = parentOperator.rotTarget;
    }

    // Update is called once per frame
    void Update()
    {
        
        headTransform.rotation = RotateToPoint(headTransform, target.position);
        armsTransform.rotation = RotateToPoint(armsTransform, target.position);
        parentOperator.transformComp.rotation = armsTransform.rotation;
        if (parentOperator.motionVec.magnitude > rotDeadZone)
        {
            legTransform.rotation = Quaternion.Euler(0, 0, (-1 * (Mathf.Atan2(parentOperator.motionVec.x, parentOperator.motionVec.y) * Mathf.Rad2Deg)-90));
        }
        if (parentOperator.motionVec != Vector2.zero)
        {
            legAnimator.SetFloat("motion", parentOperator.motionVec.magnitude / parentOperator.maxSpeed * 2);
        }
    }

    public Quaternion RotateToPoint(Transform t, Vector2 point)//Rotate 't' to face towards 'point'
    {
        Vector2 difference = (Vector3)point - t.position;//get a vector of the difference of the two points
        float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;// Calculate the angle between the two points using the differnce and convert it to degrees from radians
        return Quaternion.Euler(0, 0, angle);//sets the rotation of the transformation to the new angle.
    }
}

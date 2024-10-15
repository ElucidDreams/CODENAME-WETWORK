using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEditor.UIElements;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Operator : Person
{
    #region Operator Properties
    [Header("Operator Properties")]
    public int level = 1; 
    public bool inMission = true;
    public float maxSpeed = 10f;
    public float rating = 0;

    #region Operator Stats
    public float baseHealth;
    //public float baseArmour;
    public float baseAcceleration;
    public float baseStrength;
    public float baseAccuracy;
    [HideInInspector] public float effectiveHealth;
    [HideInInspector] public float currentHealth;
    //[HideInInspector] public float effectiveArmour;
    [HideInInspector] public float effectiveSpeed;
    [HideInInspector]public float effectiveStrength;
    [HideInInspector] public float effectiveAccuracy;
    #endregion
    [SerializeReference] public OperatorSkill[] skills;
    public Weapon activeWeapon;
    Weapon defaultWeapon; //Ensure this is set to the unarmed weapon 
    public Transform armTransform;
    public Transform rotTarget;
    [HideInInspector] public Vector2 motionVec;
    [HideInInspector] public Rigidbody2D rbComp;
    #endregion
    #region Body Controller
    #region Body part variables
    public GameObject headObject;
    Transform headTransform;
    SpriteRenderer headSpriteRenderer;
    public GameObject armsObject;
    Transform armsTransform;
    Animator armsAnimator;
    public GameObject legObject;
    Transform legTransform;
    Animator legAnimator;
    #endregion
    public float rotDeadZone = 0.1f;

    #endregion

    // Start is called before the first frame update
    public void Start()
    {
        if (UICard != null)
        {
            SetMissionUI();
        }
        InitEffectiveValues();
        InitSkills();
        InitComponents();
        currentHealth = effectiveHealth;
        SetHead();
        SetArms();
        //StartCoroutine(DebugTick());
    }
    void Update()
    {
        BodyUpdate();
    }
    public void InitEffectiveValues()//set all of the effective values to the base values
    {
        //Initialize effective values
        effectiveHealth = baseHealth;
        //effectiveArmour = baseArmour;
        effectiveSpeed = baseAcceleration;
        effectiveStrength = baseStrength;
        effectiveAccuracy = baseAccuracy;
    }
    public void InitSkills()//run all of the skills the operator has that are not on demand skills
    {
        //Initialize all skills
        foreach(OperatorSkill skill in skills)
        {
            skill.skillUser = this;
            if (!skill.onDemand)
            {
                skill.RunSkill();
            }
        }
    }
    public void InitComponents()//Get all of the components of the operator and assign them to variables
    {
        rbComp = GetComponent<Rigidbody2D>();
        if (activeWeapon != null)
        {
            activeWeapon.gameObject.transform.SetParent(armTransform);
            activeWeapon.gameObject.transform.position = Vector3.zero;
        }
        headTransform = headObject.GetComponent<Transform>();
        headSpriteRenderer = headObject.GetComponent<SpriteRenderer>();
        armsTransform = armsObject.GetComponent<Transform>();
        armsAnimator = armsObject.GetComponent<Animator>();
        legTransform = legObject.GetComponent<Transform>();
        legAnimator = legObject.GetComponent<Animator>();
    }
    public void WeaponThrow()
    {
        if (activeWeapon.weaponName != "Unarmed")
        {
            activeWeapon.transform.SetParent(null);//un-parents the weapon
            activeWeapon.weaponCollider.enabled = true;//Enable the weapons collider
            activeWeapon.weaponSpriteRenderer.sortingOrder = 1;
            float wielderFacing = armTransform.eulerAngles.z;//get the facing of the arms
            Vector2 throwDirection = new(Mathf.Cos(wielderFacing * Mathf.Deg2Rad), Mathf.Sin(wielderFacing * Mathf.Deg2Rad));//generate a vector from the facing
            activeWeapon.weaponRB = activeWeapon.AddComponent<Rigidbody2D>();
            activeWeapon.weaponRB.gravityScale = 0;
            activeWeapon.weaponRB.mass = activeWeapon.weaponMass;//set the rigidbody mass to be the preset mass of the weapon;
            activeWeapon.weaponRB.AddForce(throwDirection * effectiveStrength, ForceMode2D.Impulse);//add the force to the object, taking the operators strength into effect
            activeWeapon.weaponRB.AddTorque(UnityEngine.Random.Range(0f,1f));//add a slight spin to the item
            activeWeapon.inAir = true;//set the object to be in the air
            StartCoroutine(activeWeapon.CheckForStop());//start the timer to disable the collider again
            StartCoroutine(activeWeapon.ThrowFrictionCalc());//start the friction function
            activeWeapon = defaultWeapon;//set the active weapon to none
            SetArms();
        }
        else
        {
            Debug.Log("Cannot throw unarmed");
        }
    }
    public void SetMissionUI()//TODO: needs further functionality and reworking
    {
        SpawnFace();//generate the face in the UI
        UICard.nameText.text = givenName + " " + familyName;//set the name text
        UICard.factionText.text = faction.factionID.ToString();//set the faction text
    }
    IEnumerator DebugTick()
    {
        Debug.Log("Rot target facing: " + rotTarget.eulerAngles.z);
        Debug.Log("Arms facing:" + armTransform.eulerAngles.z);
        yield return new WaitForSeconds(1f);
        StartCoroutine(DebugTick());
    }
    public void BodyUpdate()
    {
        RotateToFacePoint(headTransform, rotTarget.position);
        RotateToFacePoint(armsTransform, rotTarget.position);
        if (motionVec.magnitude > rotDeadZone)
        {
            legTransform.rotation = Quaternion.Euler(0, 0, (-1 * (Mathf.Atan2(motionVec.x, motionVec.y) * Mathf.Rad2Deg) - 90));
        }
        if (motionVec.magnitude > 0.03f)
        {
            legAnimator.SetFloat("motion", (motionVec.magnitude / maxSpeed) * 2);
            armsAnimator.SetFloat("ArmsMotion", (motionVec.magnitude / maxSpeed) * 2);
        }
        
        armsAnimator.SetBool("Armed", activeWeapon.name.CompareTo("Unnamed") == 0);
    }
    public void RotateToFacePoint(Transform t, Vector2 point)//Rotate 't' to face towards 'point'
    {
        Vector2 difference = (Vector3)point - t.position;//get a vector of the difference of the two points
        if (difference.magnitude > rotDeadZone)
        {
            float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;// Calculate the angle between the two points using the difference and convert it to degrees from radians
            t.rotation = Quaternion.Euler(0, 0, angle);//sets the rotation of the transformation to the new angle.
        }
    }
    void SetHead()
    {
        MissionHeadSet[] g = Resources.LoadAll<MissionHeadSet>("Scriptable Objects/Head Sets");
        foreach (MissionHeadSet set in g)
        {
            if (faction.IdCheck(set.setFaction))
            {
                for (int index = 0; index < personFace.distinctiveComponent.Length; index++)
                {
                    if(personFace.distinctiveComponent[index] == true)
                    {
                        Sprite headSprite = set.heads[personFace.faceIndices[index]];
                        Color headColor = set.colors[personFace.colorIndices[index]];
                        if (personFace.toColorArr[index])
                        {
                            headSpriteRenderer.color = headColor;
                        }
                        headSpriteRenderer.sprite = headSprite;
                    }
                }
            }
        }
    }
    void SetArms()
    {
        MissionArmsSet armsSet = activeWeapon.armsSet;
        foreach(MissionArmsSet.FactionalArms factionArm in armsSet.arms)
        {
            if(faction.IdCheck(factionArm.faction))
            {
                armsAnimator.runtimeAnimatorController = factionArm.animatorController;
            }
        }
    }
}

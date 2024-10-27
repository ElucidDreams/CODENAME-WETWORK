using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEditor.EditorTools;
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
    [Tooltip("Ensure this is set to the unarmed weapon")]
    public Weapon defaultWeapon;
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
    public Animator armsAnimator;
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
            Debug.Log("Weapon not null");
            activeWeapon.gameObject.transform.SetParent(armTransform);//set the weapon object to be a child of the arms transform
            Debug.Log("pre move: "+activeWeapon.gameObject.transform.position);
            activeWeapon.gameObject.transform.position = Vector3.zero;//set the position to be directly on top of the arms transform
            Debug.Log("post move: "+activeWeapon.gameObject.transform.position);
            activeWeapon.wielder = this;//set the weapons wielder to be this operator
        }
        else{
            Weapon unarmed = Instantiate(defaultWeapon,armsTransform);
            activeWeapon = unarmed;
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
        if (activeWeapon.weaponName != "Unarmed")//Check if the player has the unarmed weapon
        {
            activeWeapon.transform.SetParent(null);//un-parents the weapon
            activeWeapon.weaponCollider.enabled = true;//Enable the weapons collider
            activeWeapon.weaponSpriteRenderer.sortingOrder = 1;
            float wielderFacing = armTransform.eulerAngles.z;//get the facing of the arms
            Vector2 throwDirection = new(Mathf.Cos(wielderFacing * Mathf.Deg2Rad), Mathf.Sin(wielderFacing * Mathf.Deg2Rad));//generate a vector from the facing
            activeWeapon.weaponRB = activeWeapon.AddComponent<Rigidbody2D>();//add a rigid body here to ensure that the weapon can have physics but not interfere with the player (Joints where not plausible with the current implementation)
            activeWeapon.weaponRB.gravityScale = 0;//Deactivate gravity on the weapon
            activeWeapon.weaponRB.mass = activeWeapon.weaponMass;//set the rigidbody mass to be the preset mass of the weapon;
            activeWeapon.weaponRB.AddForce(throwDirection * effectiveStrength, ForceMode2D.Impulse);//add the force to the object, taking the operators strength into effect
            activeWeapon.weaponRB.AddTorque(UnityEngine.Random.Range(0f,1f));//add a slight spin to the item
            activeWeapon.inAir = true;//set the object to be in the air
            StartCoroutine(activeWeapon.CheckForStop());//start the timer to disable the collider again
            StartCoroutine(activeWeapon.ThrowFrictionCalc());//start the friction function
            activeWeapon.wielder = null;
            Weapon unarmed = Instantiate(defaultWeapon,armsTransform);
            activeWeapon = unarmed;//set the active weapon to none
            
            SetArms();//Make the arms match the weapon
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
        RotateToFacePoint(headTransform, rotTarget.position);//rotate the head to face the rotTarget
        RotateToFacePoint(armsTransform, rotTarget.position);//rotate the arms to face the rotTarget
        if (motionVec.magnitude > rotDeadZone)//check if the operator is moving enough to to keep updating the leg rotation (avoids glitching at low speed)
        {
            legTransform.rotation = Quaternion.Euler(0, 0, (-1 * (Mathf.Atan2(motionVec.x, motionVec.y) * Mathf.Rad2Deg) - 90)); //Set the leg rotation to match the motion direction of the operator
        }
        if (motionVec.magnitude > 0.05f)//check if the operator is moving enough to keep playing the leg and arm walking animations, else stop them
        {
            legAnimator.SetFloat("Motion", (motionVec.magnitude / maxSpeed) * 2);
            armsAnimator.SetFloat("Motion", (motionVec.magnitude / maxSpeed) * 2);
        }
        else
        {
            legAnimator.SetFloat("Motion",0);
            armsAnimator.SetFloat("Motion",0);
        }
    }
    public void RotateToFacePoint(Transform t, Vector2 point)//Rotate 't' to face towards 'point'
    {
        Vector2 difference = (Vector3)point - t.position;//get a vector of the difference of the two points
        if (difference.magnitude > rotDeadZone)// Deadzone for avoiding rotation glitching
        {
            float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;// Calculate the angle between the two points using the difference and convert it to degrees from radians
            t.rotation = Quaternion.Euler(0, 0, angle);//sets the rotation of the transformation to the new angle.
        }
    }
    void SetHead()
    {
        MissionHeadSet[] g = Resources.LoadAll<MissionHeadSet>("Scriptable Objects/Head Sets");//load all of the heads sets
        foreach (MissionHeadSet set in g)
        {
            if (faction.IdCheck(set.setFaction))//check for the set that matches the player faction
            {
                //TODO implement a system for if there are multiple face sets for the same faction to ensure indexing is consistent. 
                for (int index = 0; index < personFace.distinctiveComponent.Length; index++)//create an index that is the length of distinctiveComponent and then iterate through until you reach a value that is true
                {
                    if(personFace.distinctiveComponent[index] == true)
                    {
                        Sprite headSprite = set.heads[personFace.faceIndices[index]];//use the index to get which index of faceIndices is for the distinctive component, then use that new index to select to the set.heads sprite
                        Color headColor = set.colors[personFace.colorIndices[index]];//Same as above but for colors
                        if (personFace.toColorArr[index])//check if the component should be colored or not, and if so colors it
                        {
                            headSpriteRenderer.color = headColor;
                        }
                        headSpriteRenderer.sprite = headSprite;//Set the head sprite renderer sprite to be the new sprite.  
                    }
                }
            }
        }
    }
    void SetArms()
    {
        MissionArmsSet armsSet = activeWeapon.armsSet;//get the arm set from the weapon
        foreach(MissionArmsSet.FactionalArms factionArm in armsSet.arms)//check through all of the faction-AnimatorController pairs for a faction that matches the operator and apply the corresponding animator to the arms animator
        {
            if(faction.IdCheck(factionArm.faction))
            {
                armsAnimator.runtimeAnimatorController = factionArm.animatorController;
            }
        }
    }
}

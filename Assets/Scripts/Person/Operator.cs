using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Newtonsoft.Json;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEditor.EditorTools;
using UnityEditor.UIElements;
using UnityEngine;
using static GameConstants;
[System.Serializable]
[RequireComponent(typeof(Rigidbody2D))]
public class Operator : Person
{
    [Header("Operator Properties")]
    public int level = 1;
    public bool inMission = true;
    public float maxSpeed = 10f;
    public float rating = 0;
    public float baseHealth;
    //public float baseArmour;
    public float baseAcceleration;
    public float baseStrength;
    public float baseAccuracy;
    [HideInInspector] public float effectiveHealth;
    public float currentHealth;
    //[HideInInspector] public float effectiveArmour;
    [HideInInspector] public float effectiveSpeed;
    [HideInInspector] public float effectiveStrength;
    [HideInInspector] public float effectiveAccuracy;
    [SerializeReference] public OperatorSkill[] skills;
    public Weapon activeWeapon;
    public GameObject unarmedWeapon;
    public MeleeCollider meleeCollider;
    public Transform armTransform;
    public Transform rotTarget;
    [HideInInspector] public Vector2 motionVec;
    [HideInInspector] public Rigidbody2D rbComp;
    public InteractSensor sensor;
    public GameObject headObject;
    Transform headTransform;
    SpriteRenderer headSpriteRenderer;
    public GameObject armsObject;
    Transform armsTransform;
    public Animator armsAnimator;
    public GameObject legObject;
    Transform legTransform;
    Animator legAnimator;
    public ParticleSystem hitParticles;
    public float rotDeadZone = 0.1f;
    public float rotationSpeed = 5f; 
    public float knockbackForce = 10f;
    public bool takingDamage = false;

    #region Unity Methods
    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        InitEffectiveValues();
        InitSkills();
        InitComponents();
        currentHealth = effectiveHealth;
        StartCoroutine(SetHead());
        //StartCoroutine(DebugTick());
    }
    public void Awake()
    {

    }
    void Update()
    {
        BodyUpdate();
    }
    #endregion
    #region Initialization Methods 
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
        foreach (OperatorSkill skill in skills)
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
        headTransform = headObject.GetComponent<Transform>();
        headSpriteRenderer = headObject.GetComponent<SpriteRenderer>();
        armsTransform = armsObject.GetComponent<Transform>();
        armsAnimator = armsObject.GetComponent<Animator>();
        legTransform = legObject.GetComponent<Transform>();
        legAnimator = legObject.GetComponent<Animator>();
        rbComp = GetComponent<Rigidbody2D>();
        if (activeWeapon == null)
        {
            activeWeapon = Instantiate(unarmedWeapon, armsTransform).GetComponent<Weapon>();
        }
        PickupWeapon(activeWeapon);
    }
    #endregion
    #region Weapon Methods
    public void WeaponThrow()
    {
        if (activeWeapon.weaponID != WeaponType.Unarmed)//Check if the player has the unarmed weapon
        {
            activeWeapon.ThrowWeapon();
            activeWeapon = Instantiate(unarmedWeapon, armsTransform).GetComponent<Weapon>();
            PickupWeapon(activeWeapon);

        }
        else
        {
            Debug.Log("Cannot throw unarmed");
        }
    }
    public void PickupWeapon(Weapon weapon)
    {
        //Debug.Log(activeWeapon);
        if (activeWeapon == null) { activeWeapon = weapon; }
        weapon.Initialize();
        weapon.SetTag(gameObject.tag);
        if (weapon.worldRB != null) { Destroy(weapon.worldRB); }
        weapon.transform.SetParent(transform);
        weapon.wielder = this;
        weapon.throwCollider.isTrigger = false;
        weapon.throwCollider.enabled = false;
        weapon.worldSpriteRenderer.enabled = false;
        weapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
        SetArms();
        meleeCollider.parentWeapon = weapon;
    }
    #endregion
    #region Body Methods
    public void BodyUpdate()
    {
        if (gameObject.tag == "Player")
        {
            RotateToFacePoint(headTransform, rotTarget.position);//rotate the head to face the rotTarget
            RotateToFacePoint(armsTransform, rotTarget.position);//rotate the arms to face the rotTarget
        }
        else if (gameObject.tag == "Enemy")
        {
            StartCoroutine(SmoothRotateToFacePoint(headTransform, rotTarget.position));//rotate the head to face the rotTarget
            StartCoroutine(SmoothRotateToFacePoint(armsTransform, rotTarget.position));//rotate the arms to face the rotTarget
        }
        
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
            legAnimator.SetFloat("Motion", 0);
            armsAnimator.SetFloat("Motion", 0);
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
    public IEnumerator SmoothRotateToFacePoint(Transform t, Vector2 point)//Rotate 't' to face towards 'point'
    {
        
        Vector2 difference = (Vector3)point - t.position; // Get a vector of the difference of the two points
        if (difference.magnitude > rotDeadZone)// Deadzone for avoiding rotation glitching
        {
            float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; // Calculate the angle between the two points using the difference and convert it to degrees from radians
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle); // Create a target rotation based on the calculated angle
            t.rotation = Quaternion.Slerp(t.rotation, targetRotation, Time.deltaTime * rotationSpeed); // Smoothly interpolate between the current rotation and the target rotation
        }
        yield return new WaitForEndOfFrame();
    }
    IEnumerator SetHead()
    {
        yield return new WaitUntil(() => distinctiveComponent.Length != 0);
        MissionHeadSet[] g = Resources.LoadAll<MissionHeadSet>("Scriptable Objects/Head Sets");//load all of the heads sets
        foreach (MissionHeadSet set in g)
        {
            if (faction == set.setFaction)//check for the set that matches the player faction
            {
                //Debug.Log(gameObject.name + "'s Distinctive component length is " + distinctiveComponent.Length);
                //Debug.Log(gameObject.name + "'s color indicies length is " + colorIndices.Count);
                //TODO implement a system for if there are multiple face sets for the same faction to ensure indexing is consistent. 
                for (int index = 0; index < distinctiveComponent.Length; index++)//create an index that is the length of distinctiveComponent and then iterate through until you reach a value that is true
                {
                    //Debug.Log(gameObject.name + "'s current index is " + index);
                    if (distinctiveComponent[index] == true)
                    {
                        Sprite headSprite = set.heads[faceIndices[index]];//use the index to get which index of faceIndices is for the distinctive component, then use that new index to select to the set.heads sprite
                        if (colorIndices.Count > 0)//check if any colors have been saved
                        {
                            Color headColor = set.colors[colorIndices[index]];//Same as above but for colors
                            if (toColorArr[index])//check if the component should be colored or not, and if so colors it
                            {
                                headSpriteRenderer.color = headColor;
                            }
                        }
                        headSpriteRenderer.sprite = headSprite;//Set the head sprite renderer sprite to be the new sprite.  
                    }
                }
            }
        }
    }
    void SetArms()
    {
        armsAnimator.runtimeAnimatorController = activeWeapon.armsAnimators[faction];
        //Debug.Log("Set Arms Called. Active weapon is " + activeWeapon.weaponName);
    }
    #endregion
    #region Debug Methods
    IEnumerator DebugTick()
    {
        Debug.Log("Rot target facing: " + rotTarget.eulerAngles.z);
        Debug.Log("Arms facing:" + armTransform.eulerAngles.z);
        yield return new WaitForSeconds(1f);
        StartCoroutine(DebugTick());
    }
    #endregion
    
    public void TakeDamage(Weapon weapon, float damage)
    {
        takingDamage = true;
        Debug.Log(gameObject.name + " has taken " + damage + " damage");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        else{
            
            ParticleSystem tempParticles = Instantiate(hitParticles, transform.position, Quaternion.identity);
            tempParticles.Play();
            rbComp.AddForce((transform.position - weapon.transform.position).normalized * knockbackForce, ForceMode2D.Impulse);
        }
        takingDamage = false;
    }
}

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
    [Header("Operator Properties")]
    public int Level; 
    public bool inMission = true;
    public float maxSpeed = 10f;
    [NonSerialized] public Transform transformComp;
    [NonSerialized] public Rigidbody2D rbComp;
    [NonSerialized] public FixedJoint2D activeWeaponJoint;
    [Space(10)]
    public float baseHealth;
    public float baseArmour;
    public float baseSpeed;
    public float baseStrength;
    public float baseAccuracy;
    [Space(10)]
    public float effectiveHealth;
    public float effectiveArmour;
    public float effectiveSpeed;
    public float effectiveStrength;
    public float effectiveAccuracy;
    [Space(10)]
    [SerializeReference]
    public OperatorSkill[] skills;
    public Weapon activeWeapon;
    public Transform rotTarget;
    public Vector2 motionVec; 
    [Space(10)]
    public float rating;

    // Start is called before the first frame update
    void Start()
    {
        if (UICard != null)
        {
            SetMissionUI();
        }
        InitEffectiveValues();
        InitSkills();
        InitComponents();
    }

    public void InitEffectiveValues()//set all of the effective values to the base values
    {
        //Initialize effective values
        effectiveHealth = baseHealth;
        effectiveArmour = baseArmour;
        effectiveSpeed = baseSpeed;
        effectiveStrength = baseStrength;
        effectiveAccuracy = baseAccuracy;
    }
    public void InitSkills()//run all of the skills the operator has that are not on demand skills
    {
        //Initialize all skills
        foreach(OperatorSkill skill in skills)
        {
            skill.SkillUser = this;
            if (!skill.OnDemand)
            {
                skill.RunSkill();
            }
        }
    }
    public void InitComponents()//Get all of the components of the operator and assign them to variables
    {
        transformComp = GetComponent<Transform>();
        rbComp = GetComponent<Rigidbody2D>();
        activeWeaponJoint = GetComponentInChildren<FixedJoint2D>();
    }
    public void WeaponThrow()
    {
        activeWeapon.transform.SetParent(null);//un-parents the weapon
        activeWeapon.weaponCollider.enabled = true;//Enable the weapons collider
        activeWeaponJoint.connectedBody = rbComp;//sets the joint to be connected to the operators rigidbody
        float wielderFacing = transform.eulerAngles.z;//get the facing of the operator
        Vector2 throwDirection = new(Mathf.Cos(wielderFacing * Mathf.Deg2Rad), Mathf.Sin(wielderFacing * Mathf.Deg2Rad));//generate a vector from the facing
        activeWeapon.weaponRB.mass = activeWeapon.weaponMass;//set the rigidbody mass to be the preset mass of the weapon;
        activeWeapon.weaponRB.AddForce(throwDirection * effectiveStrength, ForceMode2D.Impulse);//add the force to the object, taking the operators strength into effect
        activeWeapon.weaponRB.AddTorque(UnityEngine.Random.Range(0f,1f));//add a slight spin to the item
        activeWeapon.inAir = true;//set the object to be in the air
        StartCoroutine(activeWeapon.CheckForStop());//start the timer to disable the collider again
        StartCoroutine(activeWeapon.ThrowFrictionCalc());//start the friction function
        activeWeapon = null;//set the active weapon to none
    }
    public void SetMissionUI()//TODO: needs further functionality and reworking
    {
        SpawnFace();//generate the face in the UI
        UICard.nameText.text = givenName + " " + familyName;//set the name text
        UICard.factionText.text = faction.factionID.ToString();//set the faction text
    }
}
